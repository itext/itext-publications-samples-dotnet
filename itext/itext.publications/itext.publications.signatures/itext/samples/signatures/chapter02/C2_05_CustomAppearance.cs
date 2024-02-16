using System.IO;
using iText.Bouncycastle.X509;
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
using Rectangle = iText.Kernel.Geom.Rectangle;

namespace iText.Samples.Signatures.Chapter02
{
    public class C2_05_CustomAppearance
    {
        public static readonly string DEST = "results/signatures/chapter02/";

        public static readonly string KEYSTORE = "../../../resources/encryption/ks";
        public static readonly string SRC = "../../../resources/pdfs/hello_to_sign.pdf";

        public static readonly char[] PASSWORD = "password".ToCharArray();

        public static readonly string[] RESULT_FILES =
        {
            "signature_custom.pdf"
        };

        private void Sign(string src, string name, string dest, X509Certificate[] chain,
            ICipherParameters pk, string digestAlgorithm, PdfSigner.CryptoStandard subfilter,
            string reason, string location)
        {
            PdfReader reader = new PdfReader(src);
            PdfSigner signer = new PdfSigner(reader, new FileStream(dest, FileMode.Create), new StampingProperties());

            // This name corresponds to the name of the field that already exists in the document.
            signer.SetFieldName(name);
            
            // Create the signature appearance
            signer
                .SetReason(reason)
                .SetLocation(location);
            
            var widget = signer.GetSignatureField().GetFirstFormAnnotation().GetWidget().GetRectangle()
                .ToRectangle();
            var SignatureRect = new Rectangle(0, 0, widget.GetWidth(), widget.GetHeight());
            
            // Get the background layer and draw a gray rectangle as a background.
            var backgroundLayer = new PdfFormXObject(SignatureRect);
            PdfCanvas canvas = new PdfCanvas(backgroundLayer, signer.GetDocument());
            canvas.SetFillColor(ColorConstants.LIGHT_GRAY);
            canvas.Rectangle(SignatureRect);
            canvas.Fill();

            var foregroundLayer = new PdfFormXObject(SignatureRect);
            new Canvas(foregroundLayer, signer.GetDocument()).Add(new Paragraph("This document was signed by Bruno Specimen."));

            signer.GetSignatureField().SetBackgroundLayer(backgroundLayer).SetSignatureAppearanceLayer(foregroundLayer);

            IExternalSignature pks = new PrivateKeySignature(new PrivateKeyBC(pk), digestAlgorithm);

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

            new C2_05_CustomAppearance().Sign(SRC, "Signature1", DEST + RESULT_FILES[0], chain, pk,
                DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CMS,
                "Custom appearance example", "Ghent");
        }
    }
}