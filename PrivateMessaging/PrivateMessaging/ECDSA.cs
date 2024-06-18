// See https://aka.ms/new-console-template for more information
using CommunityToolkit.Diagnostics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using 等等 = System.Security.Cryptography.X509Certificates.X509Certificate2;

//need more work and testing
public static class ECDSA
{
    static void Run()
    {
        Console.WriteLine("Hello, World!");

        string subjectName = "CN=MyECDHCertificate";
        X509KeyUsageFlags x509KeyUsageFlags = X509KeyUsageFlags.KeyAgreement | X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.KeyEncipherment;
        X509Certificate2 cert = GenerateSelfSignedCertificate(subjectName, x509KeyUsageFlags);

        Console.WriteLine("请输入密码：");
        string? password = Console.ReadLine();
        Guard.IsNotNullOrWhiteSpace(password, nameof(password));

        string pfxFilePath = "my_ecdh_cert.pfx";
        File.WriteAllBytes(pfxFilePath, cert.Export(X509ContentType.Pfx, password));
        File.WriteAllBytes("my_ecdh_cert.cer", cert.Export(X509ContentType.Cert));

        Deploy(pfxFilePath, password);
    }

    static 等等 GenerateSelfSignedCertificate(string subjectName, X509KeyUsageFlags x509KeyUsageFlags)
    {
        using (ECDsa ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256))
        {
            CertificateRequest request = new CertificateRequest(
                subjectName,
                ecdsa,
                HashAlgorithmName.SHA256);

            // 设置证书用途为密钥交换

            request.CertificateExtensions.Add(
                new X509KeyUsageExtension(
                    x509KeyUsageFlags,
                    critical: true));

            // 设置增强密钥用途
            var enhancedKeyUsages = new OidCollection
            {
                new Oid("1.3.6.1.5.5.7.3.1"), // 服务器身份验证
                new Oid("1.3.6.1.5.5.7.3.2"),  // 客户端身份验证
                new Oid("1.3.6.1.5.5.7.3.3"), // 代码签名
                new Oid("1.3.6.1.5.5.7.3.4"), // 安全电子邮件
                new Oid("1.3.6.1.5.5.7.3.8"), // 时间戳
            };

            request.CertificateExtensions.Add(
                new X509EnhancedKeyUsageExtension(
                    enhancedKeyUsages,
                    critical: true));

            // 生成自签名证书
            X509Certificate2 cert = request.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(1));
            return cert;
        }
    }

    static void Deploy(string pfxFilePath, string password)
    {
        var cert = new X509Certificate2(pfxFilePath, password);
        var ecdsa = cert.GetECDsaPrivateKey();

        using (var ecdh = new ECDiffieHellmanCng())
        {
            ecdh.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
            ecdh.HashAlgorithm = CngAlgorithm.Sha256;

            // 导出自己的公钥
            var publicKey = ecdh.PublicKey.ToByteArray();
            Console.WriteLine("Public Key: " + Convert.ToBase64String(publicKey));

            // 加载对方的证书
            var peerCert = new X509Certificate2("peer_certificate.cer");
            var peerPublicKey = peerCert.GetECDsaPublicKey().ExportSubjectPublicKeyInfo();
            var otherPublicKey = peerCert.GetECDsaPublicKey().ExportSubjectPublicKeyInfo();
            // 从证书中提取对方的公钥参数
            //ECParameters peerKeyParameters = peerPublicKey.ExportParameters(false);
            //byte[] bytes = peerKeyParameters.Q.X.Concat(peerKeyParameters.Q.Y).ToArray();

            using (var peerECDH = new ECDiffieHellmanCng(CngKey.Import(peerPublicKey, CngKeyBlobFormat.EccPublicBlob)))
            {
                // 生成共享密钥
                var sharedKey = ecdh.DeriveKeyMaterial(peerECDH.PublicKey);
                Console.WriteLine("Shared Key: " + Convert.ToBase64String(sharedKey));

                // 加密消息
                string message = "Hello, secure world!";
                byte[] encryptedMessage = EncryptMessage(message, sharedKey);
                Console.WriteLine("Encrypted Message: " + Convert.ToBase64String(encryptedMessage));

                // 解密消息
                string decryptedMessage = DecryptMessage(encryptedMessage, sharedKey);
                Console.WriteLine("Decrypted Message: " + decryptedMessage);
            }
        }
    }


    static byte[] EncryptMessage(string message, byte[] key)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.GenerateIV();
            aes.Mode = CipherMode.CBC; // 使用CBC模式
            aes.Padding = PaddingMode.PKCS7; // 使用PKCS7填充
            using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(aes.IV, 0, aes.IV.Length);
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (StreamWriter sw = new StreamWriter(cs))
                {
                    sw.Write(message);
                }
                return ms.ToArray();
            }
        }
    }

    static string DecryptMessage(byte[] encryptedMessage, byte[] key)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;

            //aes.Mode = CipherMode.CBC; // 使用CBC模式
            //aes.Padding = PaddingMode.PKCS7; // 使用PKCS7填充

            using MemoryStream ms = new MemoryStream(encryptedMessage);
            {
                byte[] iv = new byte[aes.BlockSize / 8]; // 创建一个新的字节数组来存储 IV
                ms.Read(iv, 0, iv.Length); // 从加密数据中读取 IV
                aes.IV = iv; // 设置 AES 对象的 IV

                using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (StreamReader sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }

    //static CngKey ECDsaToECDH2(ECDsa ecdsa)
    //{
    //    //ECCurve eCCurve = ECCurve.CreateFromOid(new Oid("1.2.840.10045.3.1.7")); // NIST P-256 curve
    //    //eCCurve.CurveType = ECCurve.ECCurveType.Named;

    //    // Assuming you have the ECDsa key (ecdsa)
    //    var curve = ecdsa.ExportParameters(false).Curve; // Get the curve from ECDsa
    //    var cngKey = CngKey.Create(
    //        new CngAlgorithm(ecdsa.KeyExchangeAlgorithm), // Specify desired curve
    //        null,
    //        new CngKeyCreationParameters { ExportPolicy = CngExportPolicies.AllowPlaintextExport }
    //    );
    //    return cngKey;
    //}

    //static CngKey ECDsaToECDH(ECDsa ecdsa)
    //{
    //    var parameters = ecdsa.ExportParameters(false);
    //    int keySizeBytes = parameters.Q.X.Length;

    //    var keyBlob = new byte[8 + 2 * keySizeBytes]; // 8字节头部 + 2 * 坐标大小

    //    keyBlob[0] = 0x45; // BCRYPT_ECCPUBLIC_BLOB Magic number
    //    keyBlob[1] = 0x43;
    //    keyBlob[2] = 0x4B;
    //    keyBlob[3] = 0x45;
    //    keyBlob[4] = 0x20; // NIST P-256 curve
    //    keyBlob[5] = 0x00;

    //    Buffer.BlockCopy(parameters.Q.X, 0, keyBlob, 6, parameters.Q.X.Length);
    //    Buffer.BlockCopy(parameters.Q.Y, 0, keyBlob, 6 + parameters.Q.X.Length, parameters.Q.Y.Length);

    //    return CngKey.Import(keyBlob, CngKeyBlobFormat.EccPublicBlob);
    //}

    static byte[] GenerateRandomKey(int length)
    {
        using (var rng = new RNGCryptoServiceProvider())
        {
            byte[] key = new byte[length];
            rng.GetBytes(key);
            return key;
        }
    }

    static void EncryptFile(string inputFile, string outputFile, byte[] key, byte[] iv)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            using (var inputFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
            using (var outputFileStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            using (var cryptoStream = new CryptoStream(outputFileStream, encryptor, CryptoStreamMode.Write))
            {
                inputFileStream.CopyTo(cryptoStream);
            }
        }
    }

    static void DecryptFile(string inputFile, string outputFile, byte[] key, byte[] iv)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            using (var inputFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
            using (var outputFileStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            using (var cryptoStream = new CryptoStream(inputFileStream, decryptor, CryptoStreamMode.Read))
            {
                cryptoStream.CopyTo(outputFileStream);
            }
        }
    }

    static bool CheckEnhancedKeyUsage(X509Certificate2 certificate, string oid)
    {
        foreach (var extension in certificate.Extensions)
        {
            if (extension is X509EnhancedKeyUsageExtension ekuExtension)
            {
                foreach (var usageOid in ekuExtension.EnhancedKeyUsages)
                {
                    if (usageOid.Value == oid)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}