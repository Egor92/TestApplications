using System.IO;
using Ionic.Zip;

namespace SendArchives
{
    public class Archiver
    {
        public UserFileInfo MakeArchive(UserFileInfo userFileInfo)
        {
            var directoryPath = Path.GetDirectoryName(userFileInfo.FilePath);
            var archiveFilePath = Path.Combine(directoryPath, "credentials.zip");

            using (var zip = new ZipFile())
            {
                zip.Password = userFileInfo.ArchivePassword;
                zip.AddItem(userFileInfo.FilePath, string.Empty);
                zip.Save(archiveFilePath);
            }

            return new UserFileInfo
            {
                ArchivePassword = userFileInfo.ArchivePassword,
                Email = userFileInfo.Email,
                FilePath = archiveFilePath,
            };
        }
    }
}