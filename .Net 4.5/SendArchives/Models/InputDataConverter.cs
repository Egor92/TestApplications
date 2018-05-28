using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SendArchives.Models
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
                string errorMessage = userDataResultsByLineIndex.Where(x => !x.Value.IsSuccess)
                                                                .Select(x => $"Line {x.Key}: {x.Value.ErrorMessage}\n")
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
            var guid = Guid.NewGuid()
                           .ToString()
                           .Replace("-", "");
            var folderPath = $@"{tempDirectory}\{userData.Login}_{guid}";
            Directory.CreateDirectory(folderPath);
            var filePath = Path.Combine(folderPath, $"{userData.Login}.txt");
            var content = $"Логин: {userData.Login}, Пароль: {userData.UserPassword}";
            File.WriteAllText(filePath, content);

            return new UserFileInfo
            {
                ArchivePassword = userData.ArchivePassword,
                Login = userData.Login,
                Email = userData.Email,
                FilePath = filePath,
            };
        }
    }
}
