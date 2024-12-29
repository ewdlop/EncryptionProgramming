// Why did the encryption algorithm go to therapy? It had too many issues to decrypt!

using System.Text;
using PrivateMessaging;
using 三十二位元整數 = System.Int32;
using 字串 = System.String;

public partial class Program
{
    [STAThread] // 确保Main方法在单线程单元中运行，这是使用Clipboard类所需的
    public static void Main()
    {
        // 设置控制台编码为UTF-8
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        do
        {
            Console.WriteLine(提示.请输入要加密的文本);
            string? 明文 = Console.ReadLine();

            if (明文 is null)
            {
                Console.WriteLine(提示.文本不能为空);
                return;
            }

            Console.WriteLine(提示.請選擇加密方式);
            for (三十二位元整數 i = 1; i < 加密算法對照表.Count; i++)
            {
                Console.WriteLine($"{i}. {加密算法對照表[i].加密算法名}");
            }
            Console.WriteLine($"0. {加密算法對照表[0].加密算法名}");

            bool 選擇有問題 = !三十二位元整數.TryParse(Console.ReadLine(), out 三十二位元整數 選擇);
            if (選擇有問題)
            {
                Console.WriteLine(提示.選擇必须是整数);
                return;
            }
            if (選擇 == 0)
            {
                Console.WriteLine(提示.退出成功);
                return;
            }
            string 密文 = 加密算法對照表[選擇].加密算法函數(明文);
            Clipboard.SetText(密文);
            Console.WriteLine($"{提示.加密后的文本已复制到剪贴板}{密文}");
        }
        while (true);
    }

    public static readonly Dictionary<三十二位元整數, (字串 加密算法名, Func<字串, 字串> 加密算法函數)> 加密算法對照表 = new()
    {
        {0, (提示.退出,s=>s)},
        {1, (nameof(替換式密碼), 替換式密碼.Run)},
        {2, (nameof(萬國碼密碼), 萬國碼密碼.Run)},
        {3, (nameof(凱撒密碼), 凱撒密碼.Run)},
    };
}
 
