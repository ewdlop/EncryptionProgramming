using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateMessaging;

public static class 替換式密碼
{
    public static string Run(string 明文)
    {
        if (明文 is null)
        {
            Console.WriteLine(提示.文本不能为空);
            return string.Empty;
        }

        Console.WriteLine(提示.请输入要使用的密钥);
        string? 密钥 = Console.ReadLine();

        if (密钥 is null)
        {
            Console.WriteLine(提示.密钥不能为空);
            return string.Empty;
        }

        Console.WriteLine($"{提示.原始文本}{明文}");

        string encryptedText = Encrypt(明文, 密钥);
        Console.WriteLine($"{提示.加密后的文本}{encryptedText}");

        string decryptedText = Decrypt(encryptedText, 密钥);
        Console.WriteLine($"{提示.解密后的文本}{decryptedText}");

        return encryptedText;
    }

    public static string Encrypt(string input, string key)
    {
        StringBuilder encrypted = new StringBuilder();
        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];
            char encryptedChar = EncryptChar(c, key[i % key.Length]);
            encrypted.Append(encryptedChar);
        }
        return encrypted.ToString();
    }

    public static string Decrypt(string input, string key)
    {
        StringBuilder decrypted = new StringBuilder();
        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];
            char decryptedChar = DecryptChar(c, key[i % key.Length]);
            decrypted.Append(decryptedChar);
        }
        return decrypted.ToString();
    }

    private static char EncryptChar(char c, char keyChar)
    {
        int charCode = c;
        int keyCharCode = keyChar;
        int encryptedCode = charCode + keyCharCode;
        return (char)encryptedCode;
    }

    private static char DecryptChar(char c, char keyChar)
    {
        int encryptedCode = c;
        int keyCharCode = keyChar;
        int decryptedCode = encryptedCode - keyCharCode;
        return (char)decryptedCode;
    }
}
