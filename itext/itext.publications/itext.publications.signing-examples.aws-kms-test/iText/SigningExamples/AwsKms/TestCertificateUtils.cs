using NUnit.Framework;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace iText.SigningExamples.AwsKms
{
    class TestCertificateUtils
    {
        [Test]
        public void testCreateSimpleRsa()
        {
            string keyId = "alias/SigningExamples-RSA_2048";
            X509Certificate2 certificate = CertificateUtils.generateSelfSignedCertificate(keyId, "CN=AWS KMS Certificate Test,OU=signing tests,O=iText", list => list[0]);
            Console.WriteLine(certificate);
            File.WriteAllBytes("AWS KMS Certificate Test RSA.cer", certificate.GetRawCertData());
        }

        [Test]
        public void testCreateSimpleRsaSsaPss()
        {
            string keyId = "alias/SigningExamples-RSA_2048";
            X509Certificate2 certificate = CertificateUtils.generateSelfSignedCertificate(keyId, "CN=AWS KMS Certificate Test,OU=signing tests,O=iText", list => list.Find(a => a.StartsWith("RSASSA_PSS")));
            Console.WriteLine(certificate);
            File.WriteAllBytes("AWS KMS Certificate Test RSAwithMGF1.cer", certificate.GetRawCertData());
        }

        [Test]
        public void testCreateSimpleEcdsa()
        {
            string keyId = "alias/SigningExamples-ECC_NIST_P256";
            X509Certificate2 certificate = CertificateUtils.generateSelfSignedCertificate(keyId, "CN=AWS KMS Certificate Test,OU=signing tests,O=iText", list => list[0]);
            Console.WriteLine(certificate);
            File.WriteAllBytes("AWS KMS Certificate Test ECDSA.cer", certificate.GetRawCertData());
        }
    }
}
