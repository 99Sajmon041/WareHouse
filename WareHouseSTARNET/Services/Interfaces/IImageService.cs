namespace WareHouseSTARNET.Services.Interfaces
{
    public interface IImageService
    {
        Task<String> SaveImageAsync(IFormFile file);
    }
}
