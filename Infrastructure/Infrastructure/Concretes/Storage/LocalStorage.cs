namespace Yu.Infrastructure.Concretes;

public class LocalStorage(IWebHostEnvironment webHostEnvironment) : StorageHelper, ILocalStorage
{
    public void Delete(string fileName, params string[] paths)
    {
        string path = Path.Combine(paths);
        System.IO.File.Delete(Path.Combine(webHostEnvironment.ContentRootPath, path, fileName));
    }

    public List<string> GetFiles(params string[] paths)
    {
        string path = Path.Combine(paths);
        DirectoryInfo directory = new(path);
        return directory.GetFiles().Select(f => f.Name).ToList();
    }

    public bool HasFile(string fileName, params string[] paths)
    {
        string path = Path.Combine(paths);
        return System.IO.File.Exists(Path.Combine(webHostEnvironment.ContentRootPath, path, fileName));
    }
    public async Task<List<FileUploadDto>> UploadFilesAsync(IFormFileCollection files, params string[] paths)
    {
        string path = Path.Combine(paths);
        string uploadPath = Path.Combine(webHostEnvironment.ContentRootPath, path);
        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        List<FileUploadDto> datas = [];

        foreach (IFormFile file in files)
        {
            //Todo: File Name
            string fileNewName = FileRename(file.FileName);
            using (FileStream fileStream = System.IO.File.Create(Path.Combine(uploadPath, fileNewName.CharacterRegulatory(int.MaxValue))))
            {
                await file.CopyToAsync(fileStream);
            }

            FileUploadDto fileResponse = new()
            {
                FileName = fileNewName.CharacterRegulatory(int.MaxValue),
                PathName = uploadPath,

                Size = file.Length,
                Extension = Path.GetExtension(fileNewName.CharacterRegulatory(int.MaxValue)),
                ContentType = file.ContentType,

            };

            // if (Helper.IsImageFile(fileNewName.CharacterRegulatory(int.MaxValue)))
            // {
            //     Image? img = Image.FromFile(Path.Combine(uploadPath, fileNewName.CharacterRegulatory(int.MaxValue)));
            //     if (img != null)
            //     {
            //         fileResponse.Height = img.Height;
            //         fileResponse.Width = img.Width;
            //     }
            // }
            datas.Add(fileResponse);
        }

        return datas;
    }

    public async Task<FileUploadDto> UploadFileAsync(IFormFile file, params string[] paths)
    {
        string path = Path.Combine(paths);
        string uploadPath = Path.Combine(webHostEnvironment.ContentRootPath, path);
        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        string fileNewName = FileRename(file.FileName);

        using (FileStream fileStream = System.IO.File.Create(Path.Combine(uploadPath, fileNewName.CharacterRegulatory(int.MaxValue))))
        {
            await file.CopyToAsync(fileStream);
        }

        return new FileUploadDto()
        {
            FileName = fileNewName.CharacterRegulatory(int.MaxValue),
            PathName = uploadPath,
            Size = file.Length,
            Extension = Path.GetExtension(fileNewName),
            ContentType = file.ContentType,
        };
    }
}
