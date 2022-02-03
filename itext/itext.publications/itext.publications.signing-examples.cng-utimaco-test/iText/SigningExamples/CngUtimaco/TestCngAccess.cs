using NUnit.Framework;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace iText.SigningExamples.CngUtimaco
{
    public class TestCngAccess
    {
        [Test]
        public void TestAccessKey()
        {
            CngProvider provider = new CngProvider("Utimaco CryptoServer Key Storage Provider");
            Assert.AreEqual("Utimaco CryptoServer Key Storage Provider", provider.Provider);

            bool demoEcdsaExists = CngKey.Exists("DEMOecdsa", provider);
            Assert.IsTrue(demoEcdsaExists);
            
            CngKey key = CngKey.Open("DEMOecdsa", provider);
            Assert.AreEqual("DEMOecdsa", key.KeyName);
            Assert.AreEqual("ECDSA_P521", key.Algorithm.Algorithm);
            Assert.AreEqual(521, key.KeySize);

            byte[] publicBlob = key.Export(CngKeyBlobFormat.EccPublicBlob);
            Assert.AreEqual(140, publicBlob.Length);

            ECDsaCng ecdsaKey = new ECDsaCng(key);
            CertificateRequest request = new CertificateRequest("CN = Utimaco CNG Access Test", ecdsaKey, HashAlgorithmName.SHA512);
            X509Certificate2 certificate = request.CreateSelfSigned(System.DateTimeOffset.Now, System.DateTimeOffset.Now.AddYears(2));
            certificate.FriendlyName = "Utimaco CNG Access Test";
            System.Console.WriteLine("Certificate:\n****\n{0}\n****", certificate);

            using (X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser, OpenFlags.ReadWrite))
            {
                store.Add(certificate);
                Assert.IsTrue(store.Certificates.Contains(certificate));

                store.Remove(certificate);
                Assert.IsFalse(store.Certificates.Contains(certificate));
            }
        }
    }
}