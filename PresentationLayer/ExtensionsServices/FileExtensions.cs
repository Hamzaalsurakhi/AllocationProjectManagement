using System.Text.RegularExpressions;

namespace PresentationLayer.ExtensionsServices
{
    public static class FileExtensions
    {
        public static async Task<string> ConvertFileToStringAsync(IFormFile file, IWebHostEnvironment webHostEnvironment)
        {
            string uniqueFileName = null;
            try
            {
                string uniqueUpload = Path.Combine(webHostEnvironment.WebRootPath, "File");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uniqueUpload, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while copying the file.", ex);
            }

            return uniqueFileName;
        }

        public static void DeleteFileFromFileFolder(string url, IWebHostEnvironment webHostEnvironment)
        {
            string uniqueUpload = Path.Combine(webHostEnvironment.WebRootPath, "File");
            string filePath = Path.Combine(uniqueUpload, url);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public static string TypeOfFile(string FileName)
        {
            string type = null;
            string regex = @"^.+\.(jpg|png|jpeg|ppt|pptx|pps|ppsx|doc|docx|pdf)$";
            if (Regex.IsMatch(FileName.ToLower(), regex))
            {
                int indexDot = FileName.LastIndexOf(".");
                type = FileName.Substring(indexDot);
            }
            return type;
        }

        private static async Task<string> SaveEmployeePhoto(IFormFile photo)
        {
            var fileName = Path.GetFileName(photo.FileName);
            var filePath = Path.Combine("wwwroot/File", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            return $"~/Files/{fileName}";
        }
    }
}
