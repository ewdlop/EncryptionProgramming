// Why did the encryption algorithm go to therapy? It had too many issues to decrypt!

using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

public class CertificateGenerator
{
    public static void Run()
    {
        var cert = GenerateSelfSignedCertificate("CN=MyRSACertificate");
        File.WriteAllBytes("my_rsa_cert.pfx", cert.Export(X509ContentType.Pfx, "password"));
        File.WriteAllBytes("my_rsa_cert.cer", cert.Export(X509ContentType.Cert));
        Console.WriteLine("证书已生成并保存。");
    }

    public static X509Certificate2 GenerateSelfSignedCertificate(string subjectName)
    {
        using (var rsa = RSA.Create(2048))
        {
            var request = new CertificateRequest(
                subjectName,
                rsa,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1);

            // 设置证书用途为密钥交换
            request.CertificateExtensions.Add(
                new X509KeyUsageExtension(
                    X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DigitalSignature,
                    true));

            // 设置增强密钥用途
            request.CertificateExtensions.Add(
                new X509EnhancedKeyUsageExtension(
                    new OidCollection { new Oid("1.3.6.1.5.5.7.3.1") }, // 服务器身份验证
                    true));

            // 生成自签名证书
            var cert = request.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(1));
            return cert;
        }
    }

    public static void Deploy()
    {
        // 加载自己的证书
        var cert = new X509Certificate2("my_rsa_cert.pfx", "password");
        using (var rsa = cert.GetRSAPrivateKey())
        {
            // 加载对方的证书
            var peerCert = new X509Certificate2("peer_certificate.cer");
            var peerPublicKey = peerCert.GetRSAPublicKey();

            // 使用 ECDiffieHellman 生成共享密钥
            using (var ecdh = ECDiffieHellman.Create(ECCurve.NamedCurves.nistP256))
            {
                var publicKey = ecdh.PublicKey.ToByteArray();
                Console.WriteLine("Public Key: " + Convert.ToBase64String(publicKey));

                var peerECDH = ECDiffieHellman.Create(ECCurve.NamedCurves.nistP256);
                peerECDH.ImportParameters(ecdh.ExportParameters(false));

                var sharedKey = ecdh.DeriveKeyMaterial(peerECDH.PublicKey);
                Console.WriteLine("Shared Key: " + Convert.ToBase64String(sharedKey));

                // 假设要加密的消息
                string message = "Hello, secure world!";
                byte[] encryptedMessage = EncryptMessage(message, sharedKey);
                Console.WriteLine("Encrypted Message: " + Convert.ToBase64String(encryptedMessage));

                // 解密消息
                string decryptedMessage = DecryptMessage(encryptedMessage, sharedKey);
                Console.WriteLine("Decrypted Message: " + decryptedMessage);
            }
        }
    }

    public static byte[] EncryptMessage(string message, byte[] key)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = key;
            aes.GenerateIV();
            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            using (var ms = new MemoryStream())
            {
                ms.Write(aes.IV, 0, aes.IV.Length);
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(message);
                }
                return ms.ToArray();
            }
        }
    }

    public static string DecryptMessage(byte[] encryptedMessage, byte[] key)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = key;
            using (var ms = new MemoryStream(encryptedMessage))
            {
                byte[] iv = new byte[aes.BlockSize / 8];
                ms.Read(iv, 0, iv.Length);
                aes.IV = iv;
                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
