using System;
using System.Collections.Generic;
using System.IO;
using iText.Bouncycastle.X509;
using iText.Bouncycastle.Crypto;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Utils;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Signatures;
using Org.BouncyCastle.Pkcs;

namespace iText.Samples.Signatures.Chapter03
{
    public class C3_01_SignWithCAcert
    {
        public static readonly string DEST = "results/signatures/chapter03/";

        public static readonly string SRC = "../../../resources/pdfs/hello.pdf";

        public static readonly string[] RESULT_FILES =
        {
            "hello_cacert.pdf"
        };

        public void Sign(string src, string dest, X509Certificate[] chain, ICipherParameters pk,
            string digestAlgorithm, PdfSigner.CryptoStandard subfilter, string reason, string location,
            ICollection<ICrlClient> crlList, IOcspClient ocspClient, ITSAClient tsaClient, int estimatedSize)
        {
            PdfReader reader = new PdfReader(src);
            PdfSigner signer = new PdfSigner(reader, new FileStream(dest, FileMode.Create), new StampingProperties());

            // Create the signature appearance
            Rectangle rect = new Rectangle(36, 648, 200, 100);
            signer
                .SetReason(reason)
                .SetLocation(location)
                .SetPageRect(rect)
                .SetPageNumber(1)
                .SetFieldName("sig");
            

            IExternalSignature pks = new PrivateKeySignature(new PrivateKeyBC(pk), digestAlgorithm);

            IX509Certificate[] certificateWrappers = new IX509Certificate[chain.Length];
            for (int i = 0; i < certificateWrappers.Length; ++i)
            {
                certificateWrappers[i] = new X509CertificateBC(chain[i]);
            }

            // Sign the document using the detached mode, CMS or CAdES equivalent.
            signer.SignDetached(pks, certificateWrappers, crlList, ocspClient, tsaClient, estimatedSize, subfilter);
        }

        public static void Main(String[] args)
        {
            DirectoryInfo directory = new DirectoryInfo(DEST);
            directory.Create();

            Properties properties = new Properties();

            /* This properties file should contain a CAcert certificate that belongs to the user,
             * according to the original sample purpose. However right now it contains a simple
             * self-signed certificate in p12 format, which serves as a stub.
             */
            properties.Load(new FileStream("../../../resources/encryption/signkey.properties",
                FileMode.Open, FileAccess.Read));

            // Get path to the p12 file
            string path = properties.GetProperty("PRIVATE");

            // Get a password
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

            new C3_01_SignWithCAcert().Sign(SRC, DEST + RESULT_FILES[0], chain, pk,
                DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CMS,
                "Test", "Ghent", null, null, null, 0);
        }
    }
}