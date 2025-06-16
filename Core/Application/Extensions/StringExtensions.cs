namespace Yu.Application.Extensions;

public static class StringExtensions
{
    public static string CharacterRegulatory(this string name, int maxLenght = int.MaxValue)
    {
        int i = name.IndexOfAny(new char[] { 'ş', 'ç', 'ö', 'ğ', 'ü', 'ı', 'ə' });
        string newName = name.ToLower();
        if (i > -1)
        {
            StringBuilder outPut = new(newName);
            outPut.Replace('ö', 'o');
            outPut.Replace('ç', 'c');
            outPut.Replace('ş', 's');
            outPut.Replace('ı', 'i');
            outPut.Replace('ğ', 'g');
            outPut.Replace('ü', 'u');
            outPut.Replace('ə', 'e');
            newName = outPut.ToString();
        }
        newName = Regex.Replace(newName, @"[^a-z0-9\s-\.]", String.Empty);
        newName = Regex.Replace(newName, @"[\s-]+", "_").Trim(); // "  -" -> "_"
        newName = newName[..(newName.Length <= maxLenght ? newName.Length : maxLenght)].Trim();
        newName = Regex.Replace(newName, @"\s", "_"); // " " -> "_"
        return newName;
    }
    public static string RandomWithDate(this string word)
    {
        return $"{word}.{DateTime.Now:yyyy-dd-M--HH-mm-ss}";
    }
    public static string ConcatWithDate(this string startWord, string endWord)
    {
        return $"{startWord}.{DateTime.Now:yyyy-dd-M--HH-mm-ss}{endWord}";
    }
}
