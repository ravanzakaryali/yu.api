namespace Yu.Application.Abstractions;

public interface IStorage
{
    Task<List<FileUploadDto>> UploadFilesAsync(IFormFileCollection files, params string[] paths);

    Task<FileUploadDto> UploadFileAsync(IFormFile file, params string[] paths);

    void Delete(string fileName, params string[] paths);

    List<string> GetFiles(params string[] paths);

    Task<bool> HasFile(string fileName, params string[] paths);

}