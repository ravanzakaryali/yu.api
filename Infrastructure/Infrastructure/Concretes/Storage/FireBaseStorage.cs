namespace Yu.Infrastructure.Concretes;

public class FireBaseStorage(IConfiguration configuration) : StorageHelper, IStorage
{
    public void Delete(string fileName, params string[] paths)
    {
        throw new NotImplementedException();
    }

    public List<string> GetFiles(params string[] paths)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> HasFile(string fileName, params string[] paths)
    {
        var storage = new FirebaseStorage(configuration["Storage:BucketName"]);
        string path = string.Join("/", paths);
        var fileReference = storage.Child(path).Child(fileName);

        var url = await fileReference.GetDownloadUrlAsync();
        return true;

    }

    public async Task<FileUploadDto> UploadFileAsync(IFormFile file, params string[] paths)
    {
        FirebaseStorage storage = new(configuration["Storage:BucketName"]);


        string fileName = FileRename(file.FileName);
        FirebaseStorageReference fileReference = storage.Child(fileName);

        await fileReference.PutAsync(file.OpenReadStream());

        var url = await fileReference.GetDownloadUrlAsync();
        var response = new FileUploadDto
        {
            FileName = fileName,
            PathName = url,
            Size = file.Length,
            Extension = Path.GetExtension(file.FileName),
            ContentType = file.ContentType,
        };

        return response;
    }

    public async Task<List<FileUploadDto>> UploadFilesAsync(IFormFileCollection files, params string[] paths)
    {
        FirebaseStorage storage = new FirebaseStorage(configuration["Storage:BucketName"]);

        List<FileUploadDto> datas = new();

        foreach (IFormFile file in files)
        {
            string fileName = FileRename(file.FileName);
            FirebaseStorageReference fileReference = storage.Child(fileName);

            await fileReference.PutAsync(file.OpenReadStream());

            var url = await fileReference.GetDownloadUrlAsync();
            datas.Add(new FileUploadDto
            {
                FileName = fileName,
                PathName = url,
                Size = file.Length,
                Extension = Path.GetExtension(file.FileName),
                ContentType = file.ContentType,
            });
        }

        return datas;
    }

}