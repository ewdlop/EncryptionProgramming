using System;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Forms;

//萬國碼加密
public class Program
{
    [STAThread] // 确保Main方法在单线程单元中运行，这是使用Clipboard类所需的
    public static void Main()
    {
        // 设置控制台编码为UTF-8
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;


        Console.WriteLine("请输入要加密的文本：");
        string? 文本 = Console.ReadLine();
        Console.WriteLine("请输入偏移量：");
        bool 有問題 = !int.TryParse(Console.ReadLine(), out int 偏移量);
        if (有問題)
        {
            Console.WriteLine("偏移量必须是整数。");
            return;
        }
        
        string 加密后的文本 = UnicodeEncryption.Run(文本, 偏移量);
        Clipboard.SetText(加密后的文本);

        Console.WriteLine($"加密后的文本已复制到剪贴板：{加密后的文本}");
    }

    public static class UnicodeEncryption
    {
        private static readonly char[] ChineseCharacters = GetChineseCharacterRange();

        public static string Run(string? originalText, int offset)
        {
            if (string.IsNullOrWhiteSpace(originalText))
            {
                Console.WriteLine("文本不能为空。");
                return string.Empty;
            }
            Console.WriteLine($"Original Text: {originalText}");

            string encryptedText = Encrypt(originalText, offset);
            Console.WriteLine($"Encrypted Text: {encryptedText}");

            string decryptedText = Decrypt(encryptedText, offset);
            Console.WriteLine($"Decrypted Text: {decryptedText}");

            return encryptedText;
        }

        public static string Encrypt(string input, int offset)
        {
            StringBuilder encrypted = new StringBuilder();
            foreach (char c in input)
            {
                int charCode = (int)c;
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
                int encryptedCode = (int)c;
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
}
 