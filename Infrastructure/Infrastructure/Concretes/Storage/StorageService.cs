namespace Yu.Infrastructure.Concretes;

public class StorageService(IStorage storage) : IStorageService
{
    public string StorageName { get => storage.GetType().Name; }

    public void Delete(string fileName, params string[] paths)
        => storage.Delete(fileName, paths);

    public List<string> GetFiles(params string[] paths)
        => storage.GetFiles(paths);

    public async Task<bool> HasFile(string fileName, params string[] paths)
        => await storage.HasFile(fileName, paths);

    public Task<List<FileUploadDto>> UploadFilesAsync(IFormFileCollection files, params string[] paths)
        => storage.UploadFilesAsync(files, paths);

    public Task<FileUploadDto> UploadFileAsync(IFormFile file, params string[] paths)
        => storage.UploadFileAsync(file, paths);
}