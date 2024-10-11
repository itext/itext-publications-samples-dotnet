using System;
using System.IO;
using iText.Bouncycastle.X509;
using iText.Bouncycastle.Crypto;
using iText.Commons.Bouncycastle.Cert;
using iText.Forms.Form.Element;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509;
using iText.IO.Image;
using iText.Kernel.Crypto;
using iText.Kernel.Pdf;
using iText.Signatures;
using Org.BouncyCastle.Pkcs;

namespace iText.Samples.Signatures.Chapter02
{
    public class C2_07_SignatureAppearances
    {
        public static readonly string DEST = "results/signatures/chapter02/";

        public static readonly string KEYSTORE = "../../../resources/encryption/certificate.p12";
        public static readonly string SRC = "../../../resources/pdfs/hello_to_sign.pdf";
        public static readonly string IMG = "../../../resources/img/1t3xt.gif";

        public static readonly char[] PASSWORD = "testpassphrase".ToCharArray();

        public static readonly string[] RESULT_FILES =
        {
            "signature_appearance_1.pdf",
            "signature_appearance_2.pdf",
            "signature_appearance_3.pdf",
            "signature_appearance_4.pdf"
        };

        private void Sign1(string src, string name, string dest, X509Certificate[] chain,
            ICipherParameters pk, string digestAlgorithm, PdfSigner.CryptoStandard subfilter,
            string reason, string location)
        {
            PdfReader reader = new PdfReader(src);
            PdfSigner signer = new PdfSigner(reader, new FileStream(dest, FileMode.Create), new StampingProperties());

            // Create the signature appearance
            SignerProperties signerProperties = new SignerProperties()
                .SetReason(reason)
                .SetLocation(location);

            // This name corresponds to the name of the field that already exists in the document.
            signerProperties.SetFieldName(name);

            // Only description is rendered
            SignatureFieldAppearance appearance = new SignatureFieldAppearance(SignerProperties.IGNORED_ID);
            appearance.SetContent("Signed by iText");
            signerProperties.SetSignatureAppearance(appearance);
            signer.SetSignerProperties(signerProperties);

            PrivateKeySignature pks = new PrivateKeySignature(new PrivateKeyBC(pk), digestAlgorithm);

            IX509Certificate[] certificateWrappers = new IX509Certificate[chain.Length];
            for (int i = 0; i < certificateWrappers.Length; ++i) {
                certificateWrappers[i] = new X509CertificateBC(chain[i]);
            }
            // Sign the document using the detached mode, CMS or CAdES equivalent.
            signer.SignDetached(pks, certificateWrappers, null, null, null, 0, subfilter);
        }
        
        private void Sign2(string src, string name, string dest, X509Certificate[] chain,
            ICipherParameters pk, string digestAlgorithm, PdfSigner.CryptoStandard subfilter,
            string reason, string location)
        {
            PdfReader reader = new PdfReader(src);
            PdfSigner signer = new PdfSigner(reader, new FileStream(dest, FileMode.Create), new StampingProperties());

            // Create the signature appearance
            SignerProperties signerProperties = new SignerProperties()
                .SetReason(reason)
                .SetLocation(location);

            // This name corresponds to the name of the field that already exists in the document.
            signerProperties.SetFieldName(name);

            // Name and description is rendered
            SignatureFieldAppearance appearance = new SignatureFieldAppearance(SignerProperties.IGNORED_ID);
            appearance.SetContent("", "Signed by iText");
            signerProperties.SetSignatureAppearance(appearance);
            signer.SetSignerProperties(signerProperties);

            PrivateKeySignature pks = new PrivateKeySignature(new PrivateKeyBC(pk), digestAlgorithm);

            IX509Certificate[] certificateWrappers = new IX509Certificate[chain.Length];
            for (int i = 0; i < certificateWrappers.Length; ++i) {
                certificateWrappers[i] = new X509CertificateBC(chain[i]);
            }
            // Sign the document using the detached mode, CMS or CAdES equivalent.
            signer.SignDetached(pks, certificateWrappers, null, null, null, 0, subfilter);
        }
        
        private void Sign3(string src, string name, string dest, X509Certificate[] chain,
            ICipherParameters pk, string digestAlgorithm, PdfSigner.CryptoStandard subfilter,
            string reason, string location, ImageData image)
        {
            PdfReader reader = new PdfReader(src);
            PdfSigner signer = new PdfSigner(reader, new FileStream(dest, FileMode.Create), new StampingProperties());

            // Create the signature appearance
            SignerProperties signerProperties = new SignerProperties()
                .SetReason(reason)
                .SetLocation(location);

            // This name corresponds to the name of the field that already exists in the document.
            signerProperties.SetFieldName(name);

            // Graphic and description is rendered
            SignatureFieldAppearance appearance = new SignatureFieldAppearance(SignerProperties.IGNORED_ID);
            appearance.SetContent("Signed by iText", image);
            signerProperties.SetSignatureAppearance(appearance);
            signer.SetSignerProperties(signerProperties);

            PrivateKeySignature pks = new PrivateKeySignature(new PrivateKeyBC(pk), digestAlgorithm);

            IX509Certificate[] certificateWrappers = new IX509Certificate[chain.Length];
            for (int i = 0; i < certificateWrappers.Length; ++i) {
                certificateWrappers[i] = new X509CertificateBC(chain[i]);
            }
            // Sign the document using the detached mode, CMS or CAdES equivalent.
            signer.SignDetached(pks, certificateWrappers, null, null, null, 0, subfilter);
        }
        
        private void Sign4(string src, string name, string dest, X509Certificate[] chain,
            ICipherParameters pk, string digestAlgorithm, PdfSigner.CryptoStandard subfilter,
            string reason, string location, ImageData image)
        {
            PdfReader reader = new PdfReader(src);
            PdfSigner signer = new PdfSigner(reader, new FileStream(dest, FileMode.Create), new StampingProperties());

            // Create the signature appearance
            SignerProperties signerProperties = new SignerProperties()
                .SetReason(reason)
                .SetLocation(location);

            // This name corresponds to the name of the field that already exists in the document.
            signerProperties.SetFieldName(name);

            // Graphic is rendered
            SignatureFieldAppearance appearance = new SignatureFieldAppearance(SignerProperties.IGNORED_ID);
            appearance.SetContent(image);
            signerProperties.SetSignatureAppearance(appearance);
            signer.SetSignerProperties(signerProperties);

            PrivateKeySignature pks = new PrivateKeySignature(new PrivateKeyBC(pk), digestAlgorithm);

            IX509Certificate[] certificateWrappers = new IX509Certificate[chain.Length];
            for (int i = 0; i < certificateWrappers.Length; ++i) {
                certificateWrappers[i] = new X509CertificateBC(chain[i]);
            }
            // Sign the document using the detached mode, CMS or CAdES equivalent.
            signer.SignDetached(pks, certificateWrappers, null, null, null, 0, subfilter);
        }


        public static void Main(string[] args)
        {
            DirectoryInfo directory = new DirectoryInfo(DEST);
            directory.Create();

            Pkcs12Store pk12 = new Pkcs12StoreBuilder().Build();
            pk12.Load(new FileStream(KEYSTORE, FileMode.Open, FileAccess.Read), PASSWORD);
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

            ImageData image = ImageDataFactory.Create(IMG);

            C2_07_SignatureAppearances app = new C2_07_SignatureAppearances();
            string signatureName = "Signature1";
            string location = "Ghent";
            app.Sign1(SRC, signatureName, DEST + RESULT_FILES[0], chain, pk,
                DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CMS,
                "Appearance 1", location);

            app.Sign2(SRC, signatureName, DEST + RESULT_FILES[1], chain, pk,
                DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CMS,
                "Appearance 2", location);
            
            app.Sign3(SRC, signatureName, DEST + RESULT_FILES[2], chain, pk,
                DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CMS,
                "Appearance 3", location, image);
            
            app.Sign4(SRC, signatureName, DEST + RESULT_FILES[3], chain, pk,
                DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CMS,
                "Appearance 4", location, image);
        }
    }
}