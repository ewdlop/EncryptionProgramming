// Why did the encryption algorithm go to therapy? It had too many issues to decrypt!

using System.Text;

namespace PrivateMessaging;

public static class 凱撒密碼
{

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

    // Encrypts the input sstring using a Caesar cipher with the specified shift
    public static string Encrypt(string input, int shift)
    {
        StringBuilder encrypted = new StringBuilder();
        foreach (char c in input)
        {
            char encryptedChar = EncryptChar(c, shift);
            encrypted.Append(encryptedChar);
        }
        return encrypted.ToString();
    }

    // Decrypts the input string using a Caesar cipher with the specified shift
    public static string Decrypt(string input, int shift)
    {
        StringBuilder decrypted = new StringBuilder();
        foreach (char c in input)
        {
            char decryptedChar = DecryptChar(c, shift);
            decrypted.Append(decryptedChar);
        }
        return decrypted.ToString();
    }

    private static char EncryptChar(char c, int shift)
    {
        if (!char.IsLetter(c))
        {
            return c; // Non-alphabetic characters are not encrypted
        }

        char offset = char.IsUpper(c) ? 'A' : 'a';
        int adjusted = (c - offset + shift) % 26;
        adjusted = adjusted < 0 ? 26 + adjusted : adjusted;
        return (char)( adjusted + offset);
    }

    // Decrypts a single character by shifting it back by the specified number of places
    private static char DecryptChar(char c, int shift)
    {
        if (!char.IsLetter(c))
        {
            return c; // Non-alphabetic characters are not decrypted
        }

        char offset = char.IsUpper(c) ? 'A' : 'a';
        int adjusted =( c - offset - shift) % 26;
        adjusted = adjusted < 0 ? 26 + adjusted : adjusted;
        return (char)(adjusted + offset);
    }
}
