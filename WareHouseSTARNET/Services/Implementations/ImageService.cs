using WareHouseSTARNET.Services.Interfaces;

namespace WareHouseSTARNET.Services.Implementations
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;

        public ImageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SaveImageAsync(IFormFile file)
        {
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var folder = "images/Materials";
            var path = Path.Combine(_environment.WebRootPath, folder, fileName);
            if (!Directory.Exists(Path.Combine(_environment.WebRootPath, folder)))
            {
                Directory.CreateDirectory(Path.Combine(_environment.WebRootPath, folder));
            }
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return $"/{folder}/{fileName}";
        }
    }
}
