// Why did the encryption algorithm go to therapy? It had too many issues to decrypt!

using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Tsp;
using Org.BouncyCastle.X509;

namespace PrivateMessaging
{
    public class CodeSigningWithTimestamp
    {
        public static void Run(string[] args)
        {
            using(var rsa = RSA.Create(2048))
            {
                // 生成自签名证书
                var cert = GenerateSelfSignedCertificate(rsa, "CN=CodeSigningCert");

                // 导出证书和私钥到 PFX 文件
                byte[] pfxData = cert.Export(X509ContentType.Pfx, "password");
                File.WriteAllBytes("codesigning_certificate.pfx", pfxData);

                Console.WriteLine("证书和私钥已导出到 PFX 文件。");

                // 签名代码
                string dataToSign = "This is the data to be signed.";
                byte[] signature = SignData(dataToSign, rsa);
                File.WriteAllBytes("signature.bin", signature);

                Console.WriteLine("数据已签名。");

                // 验证签名
                bool isVerified = VerifySignature(dataToSign, signature, cert);
                Console.WriteLine("签名验证结果: " + isVerified);
            }

            if (args.Length < 4)
            {
                Console.WriteLine("Usage: CodeSigningWithTimestamp <file> <privateKey> <certificate> <timestampUrl>");
                Console.WriteLine("To verify: CodeSigningWithTimestamp verify <file> <signatureFile> <certificate>");
                return;
            }

            if (args[0] == "verify")
            {
                string filePath = args[1];
                string signatureFilePath = args[2];
                string certificatePath = args[3];

                bool isVerified = VerifySignature(filePath, signatureFilePath, certificatePath);
                Console.WriteLine("签名验证结果: " + (isVerified ? "通过" : "失败"));
            }
            else
            {
                string filePath = args[0];
                string privateKeyPath = args[1];
                string certificatePath = args[2];
                string timestampUrl = args[3];

                try
                {
                    // 读取私钥和证书
                    var privateKey = ReadPrivateKey(privateKeyPath);
                    var certificate = ReadCertificate(certificatePath);

                    // 签名文件
                    byte[] fileContent = File.ReadAllBytes(filePath);
                    byte[] signature = SignData(fileContent, privateKey);

                    // 添加时间戳
                    byte[] timestampedSignature = AddTimestamp(signature, timestampUrl);

                    // 保存签名和时间戳
                    string signatureFilePath = $"{filePath}.sig";
                    File.WriteAllBytes(signatureFilePath, timestampedSignature);

                    Console.WriteLine($"文件已签名并添加时间戳。签名文件: {signatureFilePath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        public static X509Certificate2 GenerateSelfSignedCertificate(RSA rsa, string subjectName)
        {
            var req = new CertificateRequest(
                new X500DistinguishedName(subjectName),
                rsa,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1);

            req.CertificateExtensions.Add(
                new X509KeyUsageExtension(
                    X509KeyUsageFlags.DigitalSignature,
                    critical: true));

            req.CertificateExtensions.Add(
                new X509EnhancedKeyUsageExtension(
                    new OidCollection { new Oid("1.3.6.1.5.5.7.3.3") }, // 代码签名 OID
                    critical: true));

            var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(1));
            return cert;
        }

        public static byte[] SignData(string data, RSA rsa)
        {
            byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(data);
            return rsa.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }

        public static bool VerifySignature(string data, byte[] signature, X509Certificate2 cert)
        {
            using (var rsa = cert.GetRSAPublicKey())
            {
                byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(data);
                return rsa.VerifyData(dataBytes, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }

        public static AsymmetricKeyParameter ReadPrivateKey(string path)
        {
            using (var reader = File.OpenText(path))
            {
                var pemReader = new PemReader(reader);
                return (AsymmetricKeyParameter)pemReader.ReadObject();
            }
        }

        public static Org.BouncyCastle.X509.X509Certificate ReadCertificate(string path)
        {
            using (var reader = File.OpenRead(path))
            {
                var parser = new X509CertificateParser();
                return parser.ReadCertificate(reader);
            }
        }

        public static byte[] SignData(byte[] data, AsymmetricKeyParameter privateKey)
        {
            var signer = SignerUtilities.GetSigner("SHA256withRSA");
            signer.Init(true, privateKey);
            signer.BlockUpdate(data, 0, data.Length);
            return signer.GenerateSignature();
        }

        public static byte[] AddTimestamp(byte[] signature, string tsaUrl)
        {
            var tspRequestGenerator = new TimeStampRequestGenerator();
            tspRequestGenerator.SetCertReq(true);
            var tspRequest = tspRequestGenerator.Generate(TspAlgorithms.Sha256, signature);

            var httpClient = new HttpClient();
            var requestContent = new ByteArrayContent(tspRequest.GetEncoded());
            requestContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/timestamp-query");

            var response = httpClient.PostAsync(tsaUrl, requestContent).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to get timestamp from TSA");
            }

            var responseBytes = response.Content.ReadAsByteArrayAsync().Result;
            var tspResponse = new TimeStampResponse(responseBytes);
            tspResponse.Validate(tspRequest);

            return tspResponse.GetEncoded();
        }

        public static bool VerifySignature(string filePath, string signatureFilePath, string certificatePath)
        {
            byte[] fileContent = File.ReadAllBytes(filePath);
            byte[] signature = File.ReadAllBytes(signatureFilePath);
            var certificate = ReadCertificate(certificatePath);

            var rsa = certificate.GetPublicKey();
            var verifier = SignerUtilities.GetSigner("SHA256withRSA");
            verifier.Init(false, rsa);
            verifier.BlockUpdate(fileContent, 0, fileContent.Length);
            return verifier.VerifySignature(signature);
        }
    }
}
