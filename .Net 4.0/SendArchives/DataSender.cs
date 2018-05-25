using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SendArchives
{
    public class DataSender
    {
        #region Fields

        private readonly InputDataConverter _inputDataConverter = new InputDataConverter();
        private readonly Archiver _archiver = new Archiver();

        #endregion

        public Result Send(string inputFilePath, EmailProperties emailProperties)
        {
            var tempDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
            Directory.CreateDirectory(tempDirectoryPath);

            try
            {
                Result<List<UserFileInfo>> conversionResult = _inputDataConverter.Convert(inputFilePath, tempDirectoryPath);
                if (!conversionResult.IsSuccess)
                    return conversionResult;

                var convertedFileInfos = conversionResult.Data;
                var archiveFileInfos = convertedFileInfos.Select(MakeArchive).ToList();

                var emailSender = new EmailSender(emailProperties);
                foreach (var archiveFileInfo in archiveFileInfos)
                {
                    emailSender.Send(archiveFileInfo);
                    File.Delete(archiveFileInfo.FilePath);
                }

                return new Result
                {
                    IsSuccess = true,
                };
            }
            catch (Exception e)
            {
                return new Result
                {
                    IsSuccess = false,
                    ErrorMessage = e.ToString(),
                };
            }
            finally
            {
                Directory.Delete(tempDirectoryPath, true);
            }
        }

        private UserFileInfo MakeArchive(UserFileInfo convertedFileInfo)
        {
            UserFileInfo archiveFileInfo = _archiver.MakeArchive(convertedFileInfo);
            File.Delete(convertedFileInfo.FilePath);
            return archiveFileInfo;
        }
    }
}