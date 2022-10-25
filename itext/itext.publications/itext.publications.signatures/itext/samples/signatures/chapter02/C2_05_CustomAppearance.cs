using System;
using System.IO;
using iText.Bouncycastle.Cert;
using iText.Bouncycastle.Crypto;
using iText.Commons.Bouncycastle.Cert;
using iText.Kernel.Colors;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using iText.Signatures;
using Org.BouncyCastle.Pkcs;

namespace iText.Samples.Signatures.Chapter02
{
    public class C2_05_CustomAppearance
    {
        public static readonly string DEST = "results/signatures/chapter02/";

        public static readonly string KEYSTORE = "../../../resources/encryption/ks";
        public static readonly string SRC = "../../../resources/pdfs/hello_to_sign.pdf";

        public static readonly char[] PASSWORD = "password".ToCharArray();

        public static readonly String[] RESULT_FILES =
        {
            "signature_custom.pdf"
        };

        public void Sign(String src, String name, String dest, X509Certificate[] chain,
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

            // Get the background layer and draw a gray rectangle as a background.
            PdfFormXObject n0 = appearance.GetLayer0();
            float x = n0.GetBBox().ToRectangle().GetLeft();
            float y = n0.GetBBox().ToRectangle().GetBottom();
            float width = n0.GetBBox().ToRectangle().GetWidth();
            float height = n0.GetBBox().ToRectangle().GetHeight();
            PdfCanvas canvas = new PdfCanvas(n0, signer.GetDocument());
            canvas.SetFillColor(ColorConstants.LIGHT_GRAY);
            canvas.Rectangle(x, y, width, height);
            canvas.Fill();

            // Set the signature information on layer 2
            PdfFormXObject n2 = appearance.GetLayer2();
            Paragraph p = new Paragraph("This document was signed by Bruno Specimen.");
            new Canvas(n2, signer.GetDocument()).Add(p);

            IExternalSignature pks = new PrivateKeySignature(new PrivateKeyBC(pk), digestAlgorithm);

            IX509Certificate[] certificateWrappers = new IX509Certificate[chain.Length];
            for (int i = 0; i < certificateWrappers.Length; ++i) {
                certificateWrappers[i] = new X509CertificateBC(chain[i]);
            }
            // Sign the document using the detached mode, CMS or CAdES equivalent.
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

            new C2_05_CustomAppearance().Sign(SRC, "Signature1", DEST + RESULT_FILES[0], chain, pk,
                DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CMS,
                "Custom appearance example", "Ghent");
        }
    }
}