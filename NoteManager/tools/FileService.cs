using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace NoteManager.tools
{
    public class FileService
    {
        public static FileService Instance = new FileService();
        public string SaveFile(string directory, IFormFile? file)
        {
            string uniqueFileName = Guid.NewGuid().ToString();
            string realtivePath = Path.Combine(directory, uniqueFileName);
            directory = toAbsolutePath(directory);
            Directory.CreateDirectory(directory);
            string filePath = Path.Combine(directory, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            return realtivePath;
        }

        public async Task DeleteFileAsync(string relativePath)
        {
            await Task.Run(() => File.Delete(toAbsolutePath(relativePath)));
        }

        /*        public File ReadFile(string fileRelativePath)
                {
                    string filePath = toAbsolutePath(fileRelativePath);
                    IFormFile formFile = ;
                    string filePath = Path.Combine(folder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }*/

        public string toAbsolutePath(string subPath)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), subPath);
        }

    }
}
