using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SendArchives.Models
{
    public class CredentialsSender
    {
        #region Fields

        private readonly InputDataConverter _inputDataConverter = new InputDataConverter();
        private readonly Archiver _archiver = new Archiver();
         
        #endregion

        public Result<CredentialsSendingInfo> Send(string inputFilePath, EmailProperties emailProperties)
        {
            var tempDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
            Directory.CreateDirectory(tempDirectoryPath);
            var credentialsSendingInfo = new CredentialsSendingInfo();

            try
            {
                Result<List<UserFileInfo>> conversionResult = _inputDataConverter.Convert(inputFilePath, tempDirectoryPath);
                if (!conversionResult.IsSuccess)
                {
                    return new Result<CredentialsSendingInfo>
                    {
                        IsSuccess = false,
                        ErrorMessage = conversionResult.ErrorMessage,
                        Data = credentialsSendingInfo,
                    };
                }

                var convertedFileInfos = conversionResult.Data;
                var archiveFileInfos = convertedFileInfos.Select(MakeArchive).ToList();

                var emailSender = new EmailSender(emailProperties);
                bool areAllSendingSuccessful = true;
                foreach (var archiveFileInfo in archiveFileInfos)
                {
                    var sendResult = emailSender.Send(archiveFileInfo);
                    areAllSendingSuccessful &= sendResult.IsSuccess;
                    credentialsSendingInfo.IsSendingSuccessfulByEmail[archiveFileInfo.Email] = $"Result: {sendResult.IsSuccess}. {sendResult.ErrorMessage}";
                    File.Delete(archiveFileInfo.FilePath);
                }

                return new Result<CredentialsSendingInfo>
                {
                    IsSuccess = areAllSendingSuccessful,
                    Data = credentialsSendingInfo,
                };
            }
            catch (Exception e)
            {
                return new Result<CredentialsSendingInfo>
                {
                    IsSuccess = false,
                    ErrorMessage = e.ToString(),
                    Data = credentialsSendingInfo,
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