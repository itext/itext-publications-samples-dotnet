using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Signatures;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;

namespace iText.Samples.Signatures.Chapter04
{
    public class C4_09_DeferredSigning
    {
        public static readonly String DEST = "results/signatures/chapter04/";

        public static readonly String SRC = "../../../resources/pdfs/hello.pdf";
        public static readonly String TEMP = "results/signatures/chapter04/hello_empty_sig.pdf";
        public static readonly String KEYSTORE = "../../../resources/encryption/ks";

        public static readonly char[] PASSWORD = "password".ToCharArray();

        public static readonly String[] RESULT_FILES = new String[]
        {
            "hello_sig_ok.pdf"
        };

        public static void Main(String[] args)
        {
            DirectoryInfo directory = new DirectoryInfo(DEST);
            directory.Create();
            Pkcs12Store pk12 = new Pkcs12Store(new FileStream(KEYSTORE, FileMode.Open, FileAccess.Read), PASSWORD);
            string alias = null;
            foreach (var a in pk12.Aliases)
            {
                alias = ((string) a);
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

            C4_09_DeferredSigning app = new C4_09_DeferredSigning();
            app.EmptySignature(SRC, TEMP, "sig", chain);
            app.CreateSignature(TEMP, DEST + RESULT_FILES[0], "sig", pk, chain);
        }

        public void EmptySignature(String src, String dest, String fieldname, X509Certificate[] chain)
        {
            PdfReader reader = new PdfReader(src);
            PdfSigner signer = new PdfSigner(reader, new FileStream(dest, FileMode.Create), new StampingProperties());

            PdfSignatureAppearance appearance = signer.GetSignatureAppearance();
            appearance
                .SetPageRect(new Rectangle(36, 748, 200, 100))
                .SetPageNumber(1)
                .SetCertificate(chain[0]);
            signer.SetFieldName(fieldname);

            /* ExternalBlankSignatureContainer constructor will create the PdfDictionary for the signature
             * information and will insert the /Filter and /SubFilter values into this dictionary.
             * It will leave just a blank placeholder for the signature that is to be inserted later.
             */
            IExternalSignatureContainer external = new ExternalBlankSignatureContainer(PdfName.Adobe_PPKLite,
                PdfName.Adbe_pkcs7_detached);

            // Sign the document using an external container
            // 8192 is the size of the empty signature placeholder.
            signer.SignExternalContainer(external, 8192);
        }

        public void CreateSignature(String src, String dest, String fieldName,
            ICipherParameters pk, X509Certificate[] chain)
        {
            PdfReader reader = new PdfReader(src);
            using (FileStream os = new FileStream(dest, FileMode.Create))
            {
                PdfSigner signer = new PdfSigner(reader, os, new StampingProperties());

                IExternalSignatureContainer external = new MyExternalSignatureContainer(pk, chain);

                // Signs a PDF where space was already reserved. The field must cover the whole document.
                PdfSigner.SignDeferred(signer.GetDocument(), fieldName, os, external);
            }
        }

        class MyExternalSignatureContainer : IExternalSignatureContainer
        {
            protected ICipherParameters pk;
            protected X509Certificate[] chain;

            public MyExternalSignatureContainer(ICipherParameters pk, X509Certificate[] chain)
            {
                this.pk = pk;
                this.chain = chain;
            }

            public byte[] Sign(Stream inputStream)
            {
                try
                {
                    PrivateKeySignature signature = new PrivateKeySignature(pk, "SHA256");
                    String hashAlgorithm = signature.GetHashAlgorithm();

                    PdfPKCS7 sgn = new PdfPKCS7(null, chain, hashAlgorithm, false);
                    byte[] hash = DigestAlgorithms.Digest(inputStream, hashAlgorithm);
                    byte[] sh = sgn.GetAuthenticatedAttributeBytes(hash, PdfSigner.CryptoStandard.CMS,
                        null, null);
                    byte[] extSignature = signature.Sign(sh);
                    sgn.SetExternalDigest(extSignature, null, signature.GetEncryptionAlgorithm());

                    return sgn.GetEncodedPKCS7(hash, PdfSigner.CryptoStandard.CMS, null,
                        null, null);
                }
                catch (IOException ioe)
                {
                    throw new Exception(ioe.Message);
                }
            }

            public void ModifySigningDictionary(PdfDictionary signDic)
            {
            }
        }
    }
}