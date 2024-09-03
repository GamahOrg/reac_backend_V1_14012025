using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Shared.Responses;
using Microsoft.AspNetCore.Http;
using REAC.Domain.Shared.Constants;
namespace REAC.Domain.Shared.Helpers
{
    public class FileHelper
    {
        public static async Task Move(List<Guid> names, string root)
        {
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            var files = new DirectoryInfo(SharedConstants.RootFolder).GetFiles();
            foreach (var file in files)
            {
                var name = Path.GetFileNameWithoutExtension(file.Name);
                if (names.Any(a => name.Contains(Path.GetFileNameWithoutExtension(a.ToString())!)))
                    file.MoveTo($"{root}/{file.Name}");
            }
        }
        public static async Task Move(Guid name, string root)
        {
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            var files = new DirectoryInfo(SharedConstants.RootFolder).GetFiles();
            foreach (var file in files)
            {
                var filename = Path.GetFileNameWithoutExtension(file.Name);
                if (filename.Contains(Path.GetFileNameWithoutExtension(name.ToString())!))
                    file.MoveTo($"{root}/{file.Name}");
            }
        }

        public static async Task CopyFiles(Guid name, string root)
        {
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            var files = new DirectoryInfo(SharedConstants.RootFolder).GetFiles();
            foreach (var file in files)
            {
                var filename = Path.GetFileNameWithoutExtension(file.Name);
                if (filename.Contains(Path.GetFileNameWithoutExtension(name.ToString())!))
                {
                    var destinationPath = Path.Combine(root, file.Name);
                    file.CopyTo(destinationPath, true);
                }
            }
            //public static async Task<string> SaveFileAsync(IFormFile file, string rootFolder)
            //{

            //    if (file == null || file.Length == 0)
            //    {
            //        throw new ExceptionResponse("No file uploaded.");
            //    }

            //    // Ensure the directory exists
            //    if (!Directory.Exists(rootFolder))
            //    {
            //        Directory.CreateDirectory(rootFolder);
            //    }

            //    string fileName = Path.GetFileName(file.FileName);
            //    string filePath = Path.Combine(rootFolder, fileName);

            //    using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            //    {
            //        await file.CopyToAsync(fileStream);
            //    }
            //    return filePath;
            //}
            //public static async Task<List<string>> SaveFileAsync(List<IFormFile> files, string rootFolder)
            //{
            //    List<string> filePathList = new List<string>();
            //    foreach (var file in files)
            //    {
            //        if (file == null || file.Length == 0)
            //        {
            //            throw new ExceptionResponse("No file uploaded.");
            //        }

            //        // Ensure the directory exists
            //        if (!Directory.Exists(rootFolder))
            //        {
            //            Directory.CreateDirectory(rootFolder);
            //        }

            //        string fileName = Path.GetFileName(file.FileName);
            //        string filePath = Path.Combine(rootFolder, fileName);

            //        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            //        {
            //            await file.CopyToAsync(fileStream);
            //        }
            //        filePathList.Add(filePath);
            //    }


            //    return filePathList;
            //}
        }
    }
}
