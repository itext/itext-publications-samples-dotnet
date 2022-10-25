using System;
using System.IO;
using iText.Bouncycastle.Cert;
using iText.Bouncycastle.Crypto;
using iText.Commons.Bouncycastle.Cert;
using iText.IO.Font;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Signatures;
using Org.BouncyCastle.Pkcs;

namespace iText.Samples.Signatures.Chapter02
{
    public class C2_06_SignatureAppearance
    {
        public static readonly string DEST = "results/signatures/chapter02/";

        public static readonly string SRC = "../../../resources/pdfs/hello_to_sign.pdf";
        public static readonly string KEYSTORE = "../../../resources/encryption/ks";
        public static readonly string IMG = "../../../resources/img/1t3xt.gif";

        public static readonly char[] PASSWORD = "password".ToCharArray();

        public static readonly String[] RESULT_FILES =
        {
            "signature_appearance1.pdf",
            "signature_appearance2.pdf",
            "signature_appearance3.pdf",
            "signature_appearance4.pdf"
        };

        public void Sign1(String src, String name, String dest, X509Certificate[] chain,
            ICipherParameters pk, String digestAlgorithm, PdfSigner.CryptoStandard subfilter,
            String reason, String location)
        {
            PdfReader reader = new PdfReader(src);
            PdfSigner signer = new PdfSigner(reader, new FileStream(dest, FileMode.Create), new StampingProperties());

            // Create the signature appearance
            PdfSignatureAppearance appearance = signer.GetSignatureAppearance();
            appearance
                .SetReason(reason)
                .SetLocation(location);

            // This name corresponds to the name of the field that already exists in the document.
            signer.SetFieldName(name);

            // Set the custom text and a custom font
            appearance.SetLayer2Text("This document was signed by Bruno Specimen");
            appearance.SetLayer2Font(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN));

            IExternalSignature pks = new PrivateKeySignature(new PrivateKeyBC(pk), digestAlgorithm);

            IX509Certificate[] certificateWrappers = new IX509Certificate[chain.Length];
            for (int i = 0; i < certificateWrappers.Length; ++i) {
                certificateWrappers[i] = new X509CertificateBC(chain[i]);
            }
            // Sign the document using the detached mode, CMS or CAdES equivalent.
            signer.SignDetached(pks, certificateWrappers, null, null, null, 0, subfilter);
        }

        public void Sign2(String src, String name, String dest, X509Certificate[] chain,
            ICipherParameters pk, String digestAlgorithm, PdfSigner.CryptoStandard subfilter,
            String reason, String location)
        {
            PdfReader reader = new PdfReader(src);
            PdfSigner signer = new PdfSigner(reader, new FileStream(dest, FileMode.Create), new StampingProperties());

            PdfSignatureAppearance appearance = signer.GetSignatureAppearance();
            appearance.SetReason(reason);
            appearance.SetLocation(location);
            signer.SetFieldName(name);

            // Creating the appearance for layer 2
            PdfFormXObject n2 = appearance.GetLayer2();

            // Custom text, custom font, and right-to-left writing
            // Characters: لورانس العرب
            Text text = new Text("\u0644\u0648\u0631\u0627\u0646\u0633 \u0627\u0644\u0639\u0631\u0628");
            text.SetFont(PdfFontFactory.CreateFont("../../../resources/font/NotoNaskhArabic-Regular.ttf",
                PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED));
            text.SetBaseDirection(BaseDirection.RIGHT_TO_LEFT);
            new Canvas(n2, signer.GetDocument()).Add(new Paragraph(text).SetTextAlignment(TextAlignment.RIGHT));

            IExternalSignature pks = new PrivateKeySignature(new PrivateKeyBC(pk), digestAlgorithm);
            IX509Certificate[] certificateWrappers = new IX509Certificate[chain.Length];
            for (int i = 0; i < certificateWrappers.Length; ++i) {
                certificateWrappers[i] = new X509CertificateBC(chain[i]);
            }
            signer.SignDetached(pks, certificateWrappers, null, null, null, 0, subfilter);
        }

        public void Sign3(String src, String name, String dest, X509Certificate[] chain,
            ICipherParameters pk, String digestAlgorithm, PdfSigner.CryptoStandard subfilter,
            String reason, String location)
        {
            PdfReader reader = new PdfReader(src);
            PdfSigner signer = new PdfSigner(reader, new FileStream(dest, FileMode.Create), new StampingProperties());

            PdfSignatureAppearance appearance = signer.GetSignatureAppearance();
            appearance.SetReason(reason);
            appearance.SetLocation(location);
            signer.SetFieldName(name);

            // Set a custom text and background image
            appearance.SetLayer2Text("This document was signed by Bruno Specimen");
            appearance.SetImage(ImageDataFactory.Create(IMG));
            appearance.SetImageScale(1);

            PrivateKeySignature pks = new PrivateKeySignature(new PrivateKeyBC(pk), digestAlgorithm);
            IX509Certificate[] certificateWrappers = new IX509Certificate[chain.Length];
            for (int i = 0; i < certificateWrappers.Length; ++i) {
                certificateWrappers[i] = new X509CertificateBC(chain[i]);
            }
            signer.SignDetached(pks, certificateWrappers, null, null, null, 0, subfilter);
        }

        public void Sign4(String src, String name, String dest, X509Certificate[] chain,
            ICipherParameters pk, String digestAlgorithm, PdfSigner.CryptoStandard subfilter,
            String reason, String location)
        {
            PdfReader reader = new PdfReader(src);
            PdfSigner signer = new PdfSigner(reader, new FileStream(dest, FileMode.Create), new StampingProperties());

            PdfSignatureAppearance appearance = signer.GetSignatureAppearance();
            appearance.SetReason(reason);
            appearance.SetLocation(location);
            signer.SetFieldName(name);

            // Set a custom text and a scaled background image
            appearance.SetLayer2Text("This document was signed by Bruno Specimen");
            appearance.SetImage(ImageDataFactory.Create(IMG));
            appearance.SetImageScale(-1);

            PrivateKeySignature pks = new PrivateKeySignature(new PrivateKeyBC(pk), digestAlgorithm);
            IX509Certificate[] certificateWrappers = new IX509Certificate[chain.Length];
            for (int i = 0; i < certificateWrappers.Length; ++i) {
                certificateWrappers[i] = new X509CertificateBC(chain[i]);
            }
            signer.SignDetached(pks, certificateWrappers, null, null, null, 0, subfilter);
        }

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

            C2_06_SignatureAppearance app = new C2_06_SignatureAppearance();
            app.Sign1(SRC, "Signature1", DEST + RESULT_FILES[0], chain, pk,
                DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CMS,
                "Custom appearance example", "Ghent");

            app.Sign2(SRC, "Signature1", DEST + RESULT_FILES[1], chain, pk,
                DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CMS,
                "Custom appearance example", "Ghent");

            app.Sign3(SRC, "Signature1", DEST + RESULT_FILES[2], chain, pk,
                DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CMS,
                "Custom appearance example", "Ghent");

            app.Sign4(SRC, "Signature1", DEST + RESULT_FILES[3], chain, pk,
                DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CMS,
                "Custom appearance example", "Ghent");
        }
    }
}