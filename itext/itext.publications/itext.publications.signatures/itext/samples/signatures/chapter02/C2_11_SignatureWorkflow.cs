using System;
using System.IO;
using iText.Bouncycastle.Cert;
using iText.Bouncycastle.X509;
using iText.Bouncycastle.Crypto;
using iText.Commons.Bouncycastle.Cert;
using Org.BouncyCastle.Crypto;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Renderer;
using iText.Signatures;
using Org.BouncyCastle.Pkcs;

namespace iText.Samples.Signatures.Chapter02
{
    public class C2_11_SignatureWorkflow
    {
        public static readonly string DEST = "results/signatures/chapter02/";
        public static readonly string FORM = "results/signatures/chapter02/form.pdf";

        public static readonly string ALICE = "../../../resources/encryption/alice.p12";
        public static readonly string BOB = "../../../resources/encryption/bob.p12";
        public static readonly string CAROL = "../../../resources/encryption/carol.p12";
        public static readonly string DAVE = "../../../resources/encryption/dave.p12";

        public static readonly char[] PASSWORD = "testpassphrase".ToCharArray();

        public static readonly String[] RESULT_FILES =
        {
            "step1_signed_by_alice.pdf", "step2_signed_by_alice_and_filled_out_by_bob.pdf",
            "step3_signed_by_alice_and_bob.pdf", "step4_signed_by_alice_and_bob_filled_out_by_carol.pdf",
            "step5_signed_by_alice_bob_and_carol.pdf", "step6_signed_by_alice_bob_carol_and_dave.pdf"
        };

        public static void Main(String[] args)
        {
            DirectoryInfo directory = new DirectoryInfo(DEST);
            directory.Create();

            C2_11_SignatureWorkflow app = new C2_11_SignatureWorkflow();
            app.CreateForm();

            String aliceCertifiedFile = DEST + RESULT_FILES[0];
            app.Certify(ALICE, FORM, "sig1", aliceCertifiedFile);

            String bobFilledFile = DEST + RESULT_FILES[1];
            String bobSignedFile = DEST + RESULT_FILES[2];
            app.FillOut(aliceCertifiedFile, bobFilledFile, "approved_bob", "Read and Approved by Bob");
            app.Sign(BOB, bobFilledFile, "sig2", bobSignedFile);

            String carolFilledFile = DEST + RESULT_FILES[3];
            String carolSignedFile = DEST + RESULT_FILES[4];
            app.FillOut(bobSignedFile, carolFilledFile, "approved_carol", "Read and Approved by Carol");
            app.Sign(CAROL, carolFilledFile, "sig3", carolSignedFile);

            String daveFilledCertifiedFile = DEST + RESULT_FILES[5];
            app.FillOutAndSign(DAVE, carolSignedFile, "sig4", "approved_dave",
                "Read and Approved by Dave", daveFilledCertifiedFile);
        }

        public void CreateForm()
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(FORM));
            Document doc = new Document(pdfDoc);

            Table table = new Table(1).UseAllAvailableWidth();
            table.AddCell("Written by Alice");
            table.AddCell(CreateSignatureFieldCell("sig1"));
            table.AddCell("For approval by Bob");
            table.AddCell(CreateTextFieldCell("approved_bob"));
            table.AddCell(CreateSignatureFieldCell("sig2"));
            table.AddCell("For approval by Carol");
            table.AddCell(CreateTextFieldCell("approved_carol"));
            table.AddCell(CreateSignatureFieldCell("sig3"));
            table.AddCell("For approval by Dave");
            table.AddCell(CreateTextFieldCell("approved_dave"));
            table.AddCell(CreateSignatureFieldCell("sig4"));
            doc.Add(table);

            doc.Close();
        }

        public void Certify(String keystore, String src, String name, String dest)
        {
            Pkcs12Store pk12 = new Pkcs12StoreBuilder().Build();
            pk12.Load(new FileStream(keystore, FileMode.Open, FileAccess.Read), PASSWORD);
            string alias = null;
            foreach (var a in pk12.Aliases)
            {
                alias = ((string) a);
                if (pk12.IsKeyEntry(alias))
                    break;
            }

            ICipherParameters pk = pk12.GetKey(alias).Key;
            X509CertificateEntry[] ce = pk12.GetCertificateChain(alias);
            IX509Certificate[] chain = new IX509Certificate[ce.Length];
            for (int k = 0; k < ce.Length; ++k)
            {
                chain[k] = new X509CertificateBC(ce[k].Certificate);
            }

            PdfReader reader = new PdfReader(src);
            PdfSigner signer = new PdfSigner(reader, new FileStream(dest, FileMode.Create),
                new StampingProperties().UseAppendMode());

            // Set signer options
            SignerProperties signerProperties = new SignerProperties()
                .SetFieldName(name)
                .SetCertificationLevel(AccessPermissions.FORM_FIELDS_MODIFICATION);
            signer.SetSignerProperties(signerProperties);

            IExternalSignature pks = new PrivateKeySignature(new PrivateKeyBC(pk), DigestAlgorithms.SHA256);

            // Sign the document using the detached mode, CMS or CAdES equivalent.
            signer.SignDetached(pks, chain, null, null, null, 0, PdfSigner.CryptoStandard.CMS);
        }

        public void FillOut(String src, String dest, String name, String value)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(src), new PdfWriter(dest),
                new StampingProperties().UseAppendMode());

            PdfAcroForm form = PdfFormCreator.GetAcroForm(pdfDoc, true);
            form.GetField(name).SetValue(value);

            pdfDoc.Close();
        }

        public virtual void Sign(String keystore, String src, String name, String dest)
        {
            Pkcs12Store pk12 = new Pkcs12StoreBuilder().Build();
            pk12.Load(new FileStream(keystore, FileMode.Open, FileAccess.Read), PASSWORD);
            string alias = null;
            foreach (var a in pk12.Aliases)
            {
                alias = ((string) a);
                if (pk12.IsKeyEntry(alias))
                    break;
            }

            ICipherParameters pk = pk12.GetKey(alias).Key;
            X509CertificateEntry[] ce = pk12.GetCertificateChain(alias);
            IX509Certificate[] chain = new IX509Certificate[ce.Length];
            for (int k = 0; k < ce.Length; ++k)
            {
                chain[k] = new X509CertificateBC(ce[k].Certificate);
            }

            PdfReader reader = new PdfReader(src);
            PdfSigner signer = new PdfSigner(reader, new FileStream(dest, FileMode.Create),
                new StampingProperties().UseAppendMode());
            signer.SetSignerProperties(new SignerProperties().SetFieldName(name));

            IExternalSignature pks = new PrivateKeySignature(new PrivateKeyBC(pk), DigestAlgorithms.SHA256);
            signer.SignDetached(pks, chain, null, null, null, 0,
                PdfSigner.CryptoStandard.CMS);
        }

        public void FillOutAndSign(String keystore, String src, String name, String fname,
            String value, String dest)
        {
            Pkcs12Store pk12 = new Pkcs12StoreBuilder().Build();
            pk12.Load(new FileStream(keystore, FileMode.Open, FileAccess.Read), PASSWORD);
            string alias = null;
            foreach (var a in pk12.Aliases)
            {
                alias = ((string) a);
                if (pk12.IsKeyEntry(alias))
                    break;
            }

            ICipherParameters pk = pk12.GetKey(alias).Key;
            X509CertificateEntry[] ce = pk12.GetCertificateChain(alias);
            IX509Certificate[] chain = new IX509Certificate[ce.Length];
            for (int k = 0; k < ce.Length; ++k)
            {
                chain[k] = new X509CertificateBC(ce[k].Certificate);
            }

            PdfReader reader = new PdfReader(src);
            PdfSigner signer = new PdfSigner(reader, new FileStream(dest, FileMode.Create),
                new StampingProperties().UseAppendMode());
            signer.SetSignerProperties(new SignerProperties().SetFieldName(name));

            PdfAcroForm form = PdfFormCreator.GetAcroForm(signer.GetDocument(), true);
            form.GetField(fname).SetValue(value);

            IExternalSignature pks = new PrivateKeySignature(new PrivateKeyBC(pk), DigestAlgorithms.SHA256);
            signer.SignDetached(pks, chain, null, null, null, 0,
                PdfSigner.CryptoStandard.CMS);
        }

        protected static Cell CreateTextFieldCell(String name)
        {
            Cell cell = new Cell();
            cell.SetHeight(20);
            cell.SetNextRenderer(new TextFieldCellRenderer(cell, name));
            return cell;
        }

        protected static Cell CreateSignatureFieldCell(String name)
        {
            Cell cell = new Cell();
            cell.SetHeight(50);
            cell.SetNextRenderer(new SignatureFieldCellRenderer(cell, name));
            return cell;
        }

        private class TextFieldCellRenderer : CellRenderer
        {
            public String name;

            public TextFieldCellRenderer(Cell modelElement, String name)
                : base(modelElement)
            {
                this.name = name;
            }

            public override void Draw(DrawContext drawContext)
            {
                base.Draw(drawContext);
                PdfFormField field = new TextFormFieldBuilder(drawContext.GetDocument(), name)
                    .SetWidgetRectangle(GetOccupiedAreaBBox()).CreateText();
                PdfFormCreator.GetAcroForm(drawContext.GetDocument(), true).AddField(field);
            }
        }

        private class SignatureFieldCellRenderer : CellRenderer
        {
            public String name;

            public SignatureFieldCellRenderer(Cell modelElement, String name)
                : base(modelElement)
            {
                this.name = name;
            }

            public override void Draw(DrawContext drawContext)
            {
                base.Draw(drawContext);
                PdfFormField field = new SignatureFormFieldBuilder(drawContext.GetDocument(), name)
                    .SetWidgetRectangle(GetOccupiedAreaBBox()).CreateSignature();
                field.GetWidgets()[0].SetHighlightMode(PdfAnnotation.HIGHLIGHT_INVERT);
                field.GetWidgets()[0].SetFlags(PdfAnnotation.PRINT);
                PdfFormCreator.GetAcroForm(drawContext.GetDocument(), true).AddField(field);
            }
        }
    }
}