using System;
using System.IO;
using iText.Bouncycastle.X509;
using iText.Commons.Utils;
using Org.BouncyCastle.X509;
using iText.Signatures;
using Org.BouncyCastle.Pkcs;

namespace iText.Samples.Signatures.Chapter03
{
    public class C3_02_GetCrlUrl
    {
        public static void Main(String[] args)
        {
            Properties properties = new Properties();

            // Specify the correct path to the certificate
            properties.Load(new FileStream("c:/home/blowagie/key.properties", FileMode.Open, FileAccess.Read));
            string path = properties.GetProperty("PRIVATE");
            char[] pass = properties.GetProperty("PASSWORD").ToCharArray();

            Pkcs12Store pk12 = new Pkcs12StoreBuilder().Build();
            pk12.Load(new FileStream(path, FileMode.Open, FileAccess.Read), pass);
            string alias = null;
            foreach (var a in pk12.Aliases)
            {
                alias = ((string) a);
                if (pk12.IsKeyEntry(alias))
                    break;
            }

            X509CertificateEntry[] ce = pk12.GetCertificateChain(alias);
            X509Certificate[] chain = new X509Certificate[ce.Length];
            for (int k = 0; k < ce.Length; ++k)
            {
                chain[k] = ce[k].Certificate;
            }

            for (int i = 0; i < chain.Length; i++)
            {
                X509Certificate cert = chain[i];
                Console.WriteLine("[{0}] {1}", i, cert.SubjectDN);
                Console.WriteLine(CertificateUtil.GetCRLURL(new X509CertificateBC(cert)));
            }
        }
    }
}