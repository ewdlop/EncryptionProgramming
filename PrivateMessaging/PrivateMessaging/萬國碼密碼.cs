using System.Text;

namespace PrivateMessaging;

public static class 萬國碼密碼
{
    private static readonly char[] ChineseCharacters = GetChineseCharacterRange();

    public static string Run(string 明文)
    {
        if (明文 is null)
        {
            Console.WriteLine(提示.文本不能为空);
            return string.Empty;
        }

        Console.WriteLine(提示.请输入偏移量);
        bool 偏移量有問題 = !int.TryParse(Console.ReadLine(), out int 偏移量);
        if (偏移量有問題)
        {
            Console.WriteLine(提示.偏移量必须是整数);
            return string.Empty;
        }


        Console.WriteLine($"{提示.原始文本}{明文}");

        string encryptedText = Encrypt(明文, 偏移量);
        Console.WriteLine($"{提示.加密后的文本}{encryptedText}");

        string decryptedText = Decrypt(encryptedText, 偏移量);
        Console.WriteLine($"{提示.解密后的文本}{decryptedText}");

        return encryptedText;
    }

    public static string Encrypt(string input, int offset)
    {
        StringBuilder encrypted = new StringBuilder();
        foreach (char c in input)
        {
            int charCode = c;
            int encryptedCode = EncryptCharCode(charCode, offset);
            encrypted.Append((char)encryptedCode);
        }
        return encrypted.ToString();
    }

    public static string Decrypt(string input, int offset)
    {
        StringBuilder decrypted = new StringBuilder();
        foreach (char c in input)
        {
            int encryptedCode = c;
            int decryptedCode = DecryptCharCode(encryptedCode, offset);
            decrypted.Append((char)decryptedCode);
        }
        return decrypted.ToString();
    }

    private static int EncryptCharCode(int charCode, int offset)
    {
        // 简单的偏移量加密方法
        return charCode + offset;
    }

    private static int DecryptCharCode(int encryptedCode, int offset)
    {
        // 与加密时相同的偏移量
        return encryptedCode - offset;
    }

    private static char[] GetChineseCharacterRange()
    {
        // 定义一个包含常用中文字符的范围
        int start = 0x4E00; // '一' 的 Unicode 编码
        int end = 0x9FFF;   // '龥' 的 Unicode 编码
        char[] chars = new char[end - start + 1];
        for (int i = start; i <= end; i++)
        {
            chars[i - start] = (char)i;
        }
        return chars;
    }
}