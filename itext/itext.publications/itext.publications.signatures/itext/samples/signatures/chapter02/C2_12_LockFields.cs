using System;
using System.IO;
using iText.Bouncycastle.Cert;
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
    public class C2_12_LockFields
    {
        public static readonly string DEST = "results/signatures/chapter02/";
        public static readonly string FORM = "results/signatures/chapter02/form_lock.pdf";

        public static readonly string ALICE = "../../../resources/encryption/alice";
        public static readonly string BOB = "../../../resources/encryption/bob";
        public static readonly string CAROL = "../../../resources/encryption/carol";
        public static readonly string DAVE = "../../../resources/encryption/dave";
        public static readonly string KEYSTORE = "../../../resources/encryption/ks";

        public static readonly char[] PASSWORD = "password".ToCharArray();

        public static readonly String[] RESULT_FILES =
        {
            "step_1_signed_by_alice.pdf", "step_2_signed_by_alice_and_bob.pdf",
            "step_3_signed_by_alice_bob_and_carol.pdf", "step_4_signed_by_alice_bob_carol_and_dave.pdf",
            "step_5_signed_by_alice_and_bob_broken_by_chuck.pdf", "step_6_signed_by_dave_broken_by_chuck.pdf"
        };

        public static void Main(String[] args)
        {
            DirectoryInfo directory = new DirectoryInfo(DEST);
            directory.Create();

            C2_12_LockFields app = new C2_12_LockFields();
            app.CreateForm();

            app.Certify(ALICE, FORM, "sig1", DEST + RESULT_FILES[0]);
            app.FillOutAndSign(BOB, DEST + RESULT_FILES[0], "sig2", "approved_bob",
                "Read and Approved by Bob", DEST + RESULT_FILES[1]);
            app.FillOutAndSign(CAROL, DEST + RESULT_FILES[1], "sig3", "approved_carol",
                "Read and Approved by Carol", DEST + RESULT_FILES[2]);
            app.FillOutAndSign(DAVE, DEST + RESULT_FILES[2], "sig4", "approved_dave",
                "Read and Approved by Dave", DEST + RESULT_FILES[3]);
            app.FillOut(DEST + RESULT_FILES[1], DEST + RESULT_FILES[4],
                "approved_bob", "Changed by Chuck");
            app.FillOut(DEST + RESULT_FILES[3], DEST + RESULT_FILES[5],
                "approved_carol", "Changed by Chuck");
        }

        public void CreateForm()
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(FORM));
            Document doc = new Document(pdfDoc);

            Table table = new Table(1).UseAllAvailableWidth();
            table.AddCell("Written by Alice");
            table.AddCell(CreateSignatureFieldCell("sig1", null));
            table.AddCell("For approval by Bob");
            table.AddCell(CreateTextFieldCell("approved_bob"));

            PdfSigFieldLock Lock = new PdfSigFieldLock().SetFieldLock(PdfSigFieldLock.LockAction.INCLUDE,
                "sig1", "approved_bob", "sig2");
            table.AddCell(CreateSignatureFieldCell("sig2", Lock));
            table.AddCell("For approval by Carol");
            table.AddCell(CreateTextFieldCell("approved_carol"));

            Lock = new PdfSigFieldLock().SetFieldLock(PdfSigFieldLock.LockAction.EXCLUDE,
                "approved_dave", "sig4");
            table.AddCell(CreateSignatureFieldCell("sig3", Lock));
            table.AddCell("For approval by Dave");
            table.AddCell(CreateTextFieldCell("approved_dave"));

            Lock = new PdfSigFieldLock().SetDocumentPermissions(PdfSigFieldLock.LockPermissions.NO_CHANGES_ALLOWED);
            table.AddCell(CreateSignatureFieldCell("sig4", Lock));
            doc.Add(table);

            doc.Close();
        }

        public void Certify(String keystore, String src, String name, String dest)
        {
            Pkcs12Store pk12 = new Pkcs12Store(new FileStream(keystore, FileMode.Open, FileAccess.Read), PASSWORD);
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
            signer.SetFieldName(name);
            signer.SetCertificationLevel(PdfSigner.CERTIFIED_FORM_FILLING);

            PdfAcroForm form = PdfAcroForm.GetAcroForm(signer.GetDocument(), true);
            form.GetField(name).SetReadOnly(true);

            PrivateKeySignature pks = new PrivateKeySignature(new PrivateKeyBC(pk), DigestAlgorithms.SHA256);

            // Sign the document using the detached mode, CMS or CAdES equivalent.
            signer.SignDetached(pks, chain, null, null, null,
                0, PdfSigner.CryptoStandard.CMS);
        }

        public void FillOutAndSign(String keystore, String src, String name, String fname, String value, String dest)
        {
            Pkcs12Store pk12 = new Pkcs12Store(new FileStream(keystore, FileMode.Open, FileAccess.Read), PASSWORD);
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
            signer.SetFieldName(name);

            PdfAcroForm form = PdfAcroForm.GetAcroForm(signer.GetDocument(), true);
            form.GetField(fname).SetValue(value);
            form.GetField(name).SetReadOnly(true);
            form.GetField(fname).SetReadOnly(true);

            PrivateKeySignature pks = new PrivateKeySignature(new PrivateKeyBC(pk), DigestAlgorithms.SHA256);
            signer.SignDetached(pks, chain, null, null, null,
                0, PdfSigner.CryptoStandard.CMS);
        }

        public void FillOut(String src, String dest, String name, String value)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(src), new PdfWriter(dest),
                new StampingProperties().UseAppendMode());

            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
            form.GetField(name).SetValue(value);

            pdfDoc.Close();
        }

        public void Sign(String keystore, String src, String name, String dest)
        {
            Pkcs12Store pk12 = new Pkcs12Store(new FileStream(keystore, FileMode.Open, FileAccess.Read), PASSWORD);
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
            signer.SetFieldName(name);

            PrivateKeySignature pks = new PrivateKeySignature(new PrivateKeyBC(pk), DigestAlgorithms.SHA256);
            signer.SignDetached(pks, chain, null, null, null,
                0, PdfSigner.CryptoStandard.CMS);
        }

        protected static Cell CreateTextFieldCell(String name)
        {
            Cell cell = new Cell();
            cell.SetHeight(20);
            cell.SetNextRenderer(new TextFieldCellRenderer(cell, name));
            return cell;
        }

        protected static Cell CreateSignatureFieldCell(String name, PdfSigFieldLock Lock)
        {
            Cell cell = new Cell();
            cell.SetHeight(50);
            cell.SetNextRenderer(new SignatureFieldCellRenderer(cell, name, Lock));
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
                PdfFormField field = PdfFormField.CreateText(drawContext.GetDocument(),
                    GetOccupiedAreaBBox(), name);
                PdfAcroForm.GetAcroForm(drawContext.GetDocument(), true).AddField(field);
            }
        }

        private class SignatureFieldCellRenderer : CellRenderer
        {
            public String name;
            public PdfSigFieldLock Lock;

            public SignatureFieldCellRenderer(Cell modelElement, String name, PdfSigFieldLock Lock)
                : base(modelElement)
            {
                this.name = name;
                this.Lock = Lock;
            }

            public override void Draw(DrawContext drawContext)
            {
                base.Draw(drawContext);
                PdfFormField field = PdfFormField.CreateSignature(drawContext.GetDocument(), GetOccupiedAreaBBox());
                field.SetFieldName(name);
                if (Lock != null)
                {
                    field.Put(PdfName.Lock, this.Lock.MakeIndirect(drawContext.GetDocument()).GetPdfObject());
                }

                field.GetWidgets()[0].SetFlag(PdfAnnotation.PRINT);
                field.GetWidgets()[0].SetHighlightMode(PdfAnnotation.HIGHLIGHT_INVERT);
                PdfAcroForm.GetAcroForm(drawContext.GetDocument(), true).AddField(field);
            }
        }
    }
}