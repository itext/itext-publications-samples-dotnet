using System;
using System.Collections.Generic;
using System.IO;
using iText.Bouncycastle.X509;
using iText.Bouncycastle.Crypto;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Utils;
using iText.Kernel.Crypto;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Signatures;
using Org.BouncyCastle.Pkcs;

namespace iText.Samples.Signatures.Chapter03
{
    public class C3_12_SignWithEstimatedSize
    {
        public static readonly string DEST = "results/signatures/chapter03/";

        public static readonly string SRC = "../../../resources/pdfs/hello.pdf";

        public static readonly string[] RESULT_FILES =
        {
            "hello_estimated.pdf"
        };

        public static void Main(string[] args)
        {
            DirectoryInfo directory = new DirectoryInfo(DEST);
            directory.Create();

            Properties properties = new Properties();

            // Specify the correct path to the certificate
            properties.Load(new FileStream("c:/home/blowagie/key.properties", FileMode.Open, FileAccess.Read));
            string path = properties.GetProperty("PRIVATE");
            char[] pass = properties.GetProperty("PASSWORD").ToCharArray();
            string tsaUrl = properties.GetProperty("TSAURL");
            string tsaUser = properties.GetProperty("TSAUSERNAME");
            string tsaPass = properties.GetProperty("TSAPASSWORD");

            Pkcs12Store pk12 = new Pkcs12StoreBuilder().Build();
            pk12.Load(new FileStream(path, FileMode.Open, FileAccess.Read), pass);
            string alias = null;
            foreach (var a in pk12.Aliases)
            {
                alias = ((string)a);
                if (pk12.IsKeyEntry(alias))
                    break;
            }

            ICipherParameters pk = pk12.GetKey(alias).Key;
            X509CertificateEntry[] ce = pk12.GetCertificateChain(alias);
            X509Certificate[] chain = new X509Certificate[ce.Length];
            for (int k = 0; k < ce.Length; ++k)
            {
                chain[k] = ce[k].Certificate;
            }

            IOcspClient ocspClient = new OcspClientBouncyCastle();
            ITSAClient tsaClient = new TSAClientBouncyCastle(tsaUrl, tsaUser, tsaPass);
            C3_12_SignWithEstimatedSize app = new C3_12_SignWithEstimatedSize();

            bool succeeded = false;
            int estimatedSize = 1000;
            while (!succeeded)
            {
                try
                {
                    Console.WriteLine("Attempt: " + estimatedSize + " bytes");

                    app.Sign(SRC, DEST, chain, pk, DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CMS,
                        "Test", "Ghent", null, ocspClient, tsaClient, estimatedSize);

                    succeeded = true;
                    Console.WriteLine("Succeeded!");
                }
                catch (IOException ioe)
                {
                    Console.WriteLine("Not succeeded: " + ioe.Message);
                    estimatedSize += 50;
                }
            }
        }

        public void Sign(string src, string dest, X509Certificate[] chain, ICipherParameters pk,
            string digestAlgorithm, PdfSigner.CryptoStandard subfilter, string reason, string location,
            ICollection<ICrlClient> crlList, IOcspClient ocspClient, ITSAClient tsaClient, int estimatedSize)
        {
            PdfReader reader = new PdfReader(src);
            PdfSigner signer = new PdfSigner(reader, new FileStream(dest, FileMode.Create), new StampingProperties());

            // Create the signature appearance
            Rectangle rect = new Rectangle(36, 648, 200, 100);
            SignerProperties signerProperties = new SignerProperties()
                .SetReason(reason)
                .SetLocation(location)
                .SetFieldName("sig")
                .SetPageRect(rect)
                .SetPageNumber(1);
            signer.SetSignerProperties(signerProperties);

            IExternalSignature pks = new PrivateKeySignature(new PrivateKeyBC(pk), digestAlgorithm);

            IX509Certificate[] certificateWrappers = new IX509Certificate[chain.Length];
            for (int i = 0; i < certificateWrappers.Length; ++i)
            {
                certificateWrappers[i] = new X509CertificateBC(chain[i]);
            }

            // Sign the document using the detached mode, CMS or CAdES equivalent.
            signer.SignDetached(pks, certificateWrappers, crlList, ocspClient, tsaClient, estimatedSize, subfilter);
        }
    }
}