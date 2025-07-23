namespace Yu.Infrastructure.Concretes;

public class StorageHelper
{
    public string FileRename(string fileName, string email = "", params string[] paths)
    {
        string path = Path.Combine(paths);
        string newFileName = DateTime.UtcNow.ToString("yyyyMMddHHmmss") + NewFileName(fileName, email);
        return newFileName;
    }
    string NewFileName(string fileName, string username)
    {
        string extension = Path.GetExtension(fileName);
        return string.Concat(username, Path.GetFileNameWithoutExtension(fileName)).ConcatWithDate(extension);
    }
}