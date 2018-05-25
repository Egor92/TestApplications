using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SendArchives
{
    public class InputDataConverter
    {
        public Result<List<UserFileInfo>> Convert(string inputFilePath, string tempDirectory)
        {
            string[] lines = File.ReadAllLines(inputFilePath);
            if (lines.Length < 2)
            {
                return new Result<List<UserFileInfo>>
                {
                    IsSuccess = false,
                    ErrorMessage = "Input file must have at least 2 lines",
                };
            }

            int index = 1;
            Dictionary<int, Result<UserData>> userDataResultsByLineIndex = lines.ToDictionary(_ => index++)
                                                                                .Skip(1)
                                                                                .Where(x => !string.IsNullOrWhiteSpace(x.Value))
                                                                                .ToDictionary(x => x.Key, x => UserData.Create(x.Value));

            bool hasFails = userDataResultsByLineIndex.Any(x => !x.Value.IsSuccess);
            if (hasFails)
            {
                string errorMessage = userDataResultsByLineIndex.Select(x => string.Format("{0}. {1}\n\n", x.Key, x.Value.ErrorMessage))
                                                                .Aggregate(string.Empty, (s1, s2) => s1 + s2);

                return new Result<List<UserFileInfo>>
                {
                    IsSuccess = false,
                    ErrorMessage = errorMessage,
                };
            }

            var userDatas = userDataResultsByLineIndex.Select(x => x.Value.Data);
            var convertedFileInfos = userDatas.Select(x => SaveToFile(x, tempDirectory))
                                              .ToList();

            return new Result<List<UserFileInfo>>
            {
                IsSuccess = true,
                Data = convertedFileInfos,
            };
        }

        private static UserFileInfo SaveToFile(UserData userData, string tempDirectory)
        {
            var guid = Guid.NewGuid().ToString().Replace("-", "");
            var folderPath = string.Format(@"{0}\{1}_{2}", tempDirectory, userData.Login, guid);
            Directory.CreateDirectory(folderPath);
            var filePath = Path.Combine(folderPath, @"credentials.txt");
            var content = string.Format("Логин: {0}, Пароль: {1}", userData.Login, userData.UserPassword);
            File.WriteAllText(filePath, content);

            return new UserFileInfo
            {
                ArchivePassword = userData.ArchivePassword,
                Email = userData.Email,
                FilePath = filePath,
            };
        }
    }
}