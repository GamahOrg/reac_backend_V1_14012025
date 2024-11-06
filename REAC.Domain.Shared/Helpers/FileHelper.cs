using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
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


        public static async Task<BaseResponse> CheckMaliciousFile(IFormFile file)
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    // Read the PDF as bytes, but don't convert directly to string
                    byte[] content = stream.ToArray();

                    // Check for malicious patterns
                    if (ContainsMaliciousPatterns(content))
                    {
                        return new BaseResponse() { IsDone = false, Message = "ملف غير سليم" };
                    }

                    return new BaseResponse() { IsDone = true, Message = "File Ok" };

                }
            }
            catch (Exception ex)
            {
                return new BaseResponse() { IsDone = false, Message = "خطأ داخلي" };

            }
        }


        public static async Task<BaseResponse> CheckExcel(IFormFile file)
        {
            try
            {
                using (var stream = new StreamReader(file.OpenReadStream()))
                {
                    while (!stream.EndOfStream)
                    {
                        var line = await stream.ReadLineAsync();
                        var columns = line.Split(','); // Adjust the delimiter as needed

                        // Check each column for SQL patterns
                        foreach (var column in columns)
                        {
                            // Check for malicious patterns
                            if (ContainsMaliciousPatterns(column))
                            {
                                return new BaseResponse() { IsDone = false, Message = "ملف غير سليم" };
                            }

                            return new BaseResponse() { IsDone = true, Message = "File Ok" };
                        }
                    }
                    return new BaseResponse() { IsDone = false, Message = "خطأ داخلي" };

                }
            }
            catch (Exception ex)
            {
                return new BaseResponse() { IsDone = false, Message = "خطأ داخلي" };

            }
        }

        private static bool ContainsMaliciousPatterns(string content)
        {

            // Example patterns to check for
            var patterns = new[]
            {
            @"<script>", // HTML script tag
            @"</script>", // Closing script tag
            @"eval\(",   // JavaScript eval function
            @"base64,",  // Base64 encoded data
            @"document.cookie", // Accessing cookies
            @"window.location", // Potential redirection
            @"onerror=", // Event handler attribute
            @"data:text/html;base64,", // Data URLs for HTML
            @"data:image/png;base64,", // Data URLs for images
            @"<iframe>", // Embedding sites
            @"javascript:", // URL scheme
            @"<img src=x onerror=", // Image tag exploit
            @"SELECT", // SQL command
            @"UNION", // SQL injection
            //@"\b--\b", // SQL comments
            //@"\b;\b", // Command separator
            //@"\b|\b", // Pipe operator
            @"<?php", // PHP code
            @"eval($_POST['file'])", // Executing uploaded files
            @"<%@ ", // ASP.NET directives
            @"shell_exec(", // PHP command execution
            @"system(", // PHP command execution
            @"os.system(", // Python command execution
            @"Process.Start" // C# process execution
            };

            foreach (var pattern in patterns)
            {
                if (Regex.IsMatch(content, pattern, RegexOptions.IgnoreCase))
                {
                    return true; // Return early if a match is found
                }
            }

            return false;
        }


        private static bool ContainsMaliciousPatterns(byte[] content)
        {

            string text = System.Text.Encoding.UTF8.GetString(content);

            // Example patterns to check for
            var patterns = new[]
            {
            @"<script>", // HTML script tag
            @"</script>", // Closing script tag
            @"eval(",   // JavaScript eval function
            @"base64",  // Base64 encoded data
            @"document.cookie", // Accessing cookies
            @"window.location", // Potential redirection
            @"onerror=", // Event handler attribute
            @"data:text/html;base64,", // Data URLs for HTML
            @"data:image/png;base64,", // Data URLs for images
            @"<iframe>", // Embedding sites
            @"javascript:", // URL scheme
            @"<img src=x onerror=", // Image tag exploit
            @"SELECT", // SQL command
            @"UNION", // SQL injection
           // @"\b--\b", // SQL comments
           // @"\b;\b", // Command separator
           // @"\b|\b", // Pipe operator
            @"<?php", // PHP code
            @"eval($_POST['file'])", // Executing uploaded files
            @"<%@ ", // ASP.NET directives
            @"shell_exec(", // PHP command execution
            @"system(", // PHP command execution
            @"os.system(", // Python command execution
            @"Process.Start" // C# process execution
        };

            foreach (var pattern in patterns)
            {
                if (Regex.IsMatch(text, Regex.Escape(pattern), RegexOptions.IgnoreCase))
                {
                    return true; // Return early if a match is found
                }
            }

            return false; // No patterns matched
        }
    }
}
