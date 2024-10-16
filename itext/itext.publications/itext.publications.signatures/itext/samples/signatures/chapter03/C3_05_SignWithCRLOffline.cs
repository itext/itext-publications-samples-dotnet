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
    public class C3_05_SignWithCRLOffline
    {
        public static readonly string DEST = "results/signatures/chapter03/";

        public static readonly string CRLURL = "../../../resources/encryption/revoke.crl";
        public static readonly string SRC = "../../../resources/pdfs/hello.pdf";

        public static readonly string[] RESULT_FILES =
        {
            "hello_cacert_crl_offline.pdf"
        };

        public static void Main(string[] args)
        {
            DirectoryInfo directory = new DirectoryInfo(DEST);
            directory.Create();

            Properties properties = new Properties();

            // Specify the correct path to the certificate
            properties.Load(new FileStream("c:/home/blowagie/key.properties", FileMode.Open, FileAccess.Read));
            String path = properties.GetProperty("PRIVATE");
            char[] pass = properties.GetProperty("PASSWORD").ToCharArray();

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

            FileStream fileStream = new FileStream(CRLURL, FileMode.Open, FileAccess.Read);
            MemoryStream baos = new MemoryStream();
            byte[] buf = new byte[1024];
            while (fileStream.Read(buf, 0, buf.Length) != 0)
            {
                baos.Write(buf, 0, buf.Length);
            }

            /* Create a CrlClientOffline instance with the read CRL file's data.
             * Given CRL file is specific to the CAcert provider and was downloaded long time ago.
             * Make sure that you have the CRL specific for your certificate and CRL is up to date
             * (by checking NextUpdate properties as seen below).
             */
            ICrlClient crlClient = new CrlClientOffline(baos.ToArray());
            X509Crl crl = new X509CrlParser().ReadCrl(new FileStream(CRLURL, FileMode.Open, FileAccess.Read));
            Console.WriteLine("CRL valid until: " + crl.NextUpdate);
            Console.WriteLine("Certificate revoked: " + crl.IsRevoked(chain[0]));
            IList<ICrlClient> crlList = new List<ICrlClient>();
            crlList.Add(crlClient);

            new C3_05_SignWithCRLOffline().Sign(SRC, DEST + RESULT_FILES[0], chain, pk,
                DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CMS, "Test", "Ghent",
                crlList, null, null, 0);
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
            // Pass the created CRL to the signing method.
            signer.SignDetached(pks, certificateWrappers, crlList, ocspClient, tsaClient, estimatedSize, subfilter);
        }
    }
}