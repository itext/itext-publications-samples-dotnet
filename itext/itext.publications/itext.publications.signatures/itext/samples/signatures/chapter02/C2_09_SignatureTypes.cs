using System;
using System.IO;
using iText.Bouncycastle.Cert;
using iText.Bouncycastle.Crypto;
using iText.Commons.Bouncycastle.Cert;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Layout;
using iText.Layout.Properties;
using iText.Signatures;
using Org.BouncyCastle.Pkcs;

namespace iText.Samples.Signatures.Chapter02
{
    public class C2_09_SignatureTypes
    {
        public static readonly string DEST = "results/signatures/chapter02/";

        public static readonly string KEYSTORE = "../../../resources/encryption/ks";
        public static readonly string SRC = "../../../resources/pdfs/hello.pdf";

        public static readonly char[] PASSWORD = "password".ToCharArray();

        public static readonly String[] RESULT_FILES =
        {
            "hello_level_1.pdf", "hello_level_2.pdf",
            "hello_level_3.pdf", "hello_level_4.pdf",
            "hello_level_1_annotated.pdf", "hello_level_2_annotated.pdf",
            "hello_level_3_annotated.pdf", "hello_level_4_annotated.pdf",
            "hello_level_1_annotated_wrong.pdf", "hello_level_1_text.pdf",
            "hello_level_1_double.pdf", "hello_level_2_double.pdf",
            "hello_level_3_double.pdf", "hello_level_4_double.pdf",
        };

        public void Sign(String src, String dest, X509Certificate[] chain, ICipherParameters pk,
            String digestAlgorithm, PdfSigner.CryptoStandard subfilter,
            int certificationLevel, String reason, String location)
        {
            PdfReader reader = new PdfReader(src);
            PdfSigner signer = new PdfSigner(reader, new FileStream(dest, FileMode.Create), new StampingProperties());

            // Create the signature appearance
            PdfSignatureAppearance appearance = signer.GetSignatureAppearance();
            appearance.SetReason(reason);
            appearance.SetLocation(location);

            Rectangle rect = new Rectangle(36, 648, 200, 100);
            appearance.SetPageRect(rect).SetPageNumber(1);
            signer.SetFieldName("sig");

            /* Set the document's certification level. This parameter defines if changes are allowed
             * after the applying of the signature.
             */
            signer.SetCertificationLevel(certificationLevel);

            PrivateKeySignature pks = new PrivateKeySignature(new PrivateKeyBC(pk), digestAlgorithm);

            IX509Certificate[] certificateWrappers = new IX509Certificate[chain.Length];
            for (int i = 0; i < certificateWrappers.Length; ++i) {
                certificateWrappers[i] = new X509CertificateBC(chain[i]);
            }
            // Sign the document using the detached mode, CMS or CAdES equivalent.
            signer.SignDetached(pks, certificateWrappers, null, null, null, 0, subfilter);
        }

        public void AddText(String src, String dest)
        {
            PdfReader reader = new PdfReader(src);
            PdfDocument pdfDoc = new PdfDocument(reader, new PdfWriter(dest),
                new StampingProperties().UseAppendMode());
            PdfPage firstPage = pdfDoc.GetFirstPage();

            new Canvas(firstPage, firstPage.GetPageSize()).ShowTextAligned("TOP SECRET",
                36, 820, TextAlignment.LEFT);

            pdfDoc.Close();
        }

        public void AddAnnotation(String src, String dest)
        {
            PdfReader reader = new PdfReader(src);
            PdfDocument pdfDoc = new PdfDocument(reader, new PdfWriter(dest),
                new StampingProperties().UseAppendMode());

            PdfAnnotation comment = new PdfTextAnnotation(new Rectangle(200, 800, 50, 20))
                .SetOpen(true)
                .SetIconName(new PdfName("Comment"))
                .SetTitle(new PdfString("Finally Signed!"))
                .SetContents("Bruno Specimen has finally signed the document");
            pdfDoc.GetFirstPage().AddAnnotation(comment);

            pdfDoc.Close();
        }

        public void AddWrongAnnotation(String src, String dest)
        {
            PdfReader reader = new PdfReader(src);
            PdfDocument pdfDoc = new PdfDocument(reader, new PdfWriter(dest));

            PdfAnnotation comment = new PdfTextAnnotation(new Rectangle(200, 800, 50, 20))
                .SetOpen(true)
                .SetIconName(new PdfName("Comment"))
                .SetTitle(new PdfString("Finally Signed!"))
                .SetContents("Bruno Specimen has finally signed the document");
            pdfDoc.GetFirstPage().AddAnnotation(comment);

            pdfDoc.Close();
        }

        public void SignAgain(String src, String dest, X509Certificate[] chain, ICipherParameters pk,
            String digestAlgorithm, PdfSigner.CryptoStandard subfilter, String reason, String location)
        {
            PdfReader reader = new PdfReader(src);
            PdfSigner signer = new PdfSigner(reader, new FileStream(dest, FileMode.Create),
                new StampingProperties().UseAppendMode());

            PdfSignatureAppearance appearance = signer.GetSignatureAppearance();
            appearance.SetReason(reason);
            appearance.SetLocation(location);
            appearance.SetReuseAppearance(false);
            Rectangle rect = new Rectangle(36, 700, 200, 100);
            appearance.SetPageRect(rect).SetPageNumber(1);
            signer.SetFieldName("Signature2");

            IX509Certificate[] certificateWrappers = new IX509Certificate[chain.Length];
            for (int i = 0; i < certificateWrappers.Length; ++i) {
                certificateWrappers[i] = new X509CertificateBC(chain[i]);
            }
            PrivateKeySignature pks = new PrivateKeySignature(new PrivateKeyBC(pk), digestAlgorithm);
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

            C2_09_SignatureTypes app = new C2_09_SignatureTypes();
            app.Sign(SRC, DEST + RESULT_FILES[0], chain, pk, DigestAlgorithms.SHA256,
                PdfSigner.CryptoStandard.CMS, PdfSigner.NOT_CERTIFIED,
                "Test 1", "Ghent");
            app.Sign(SRC, DEST + RESULT_FILES[1], chain, pk, DigestAlgorithms.SHA256,
                PdfSigner.CryptoStandard.CMS, PdfSigner.CERTIFIED_FORM_FILLING_AND_ANNOTATIONS,
                "Test 1", "Ghent");
            app.Sign(SRC, DEST + RESULT_FILES[2], chain, pk, DigestAlgorithms.SHA256,
                PdfSigner.CryptoStandard.CMS, PdfSigner.CERTIFIED_FORM_FILLING,
                "Test 1", "Ghent");
            app.Sign(SRC, DEST + RESULT_FILES[3], chain, pk, DigestAlgorithms.SHA256,
                PdfSigner.CryptoStandard.CMS, PdfSigner.CERTIFIED_NO_CHANGES_ALLOWED,
                "Test 1", "Ghent");

            app.AddAnnotation(DEST + RESULT_FILES[0], DEST + RESULT_FILES[4]);
            app.AddAnnotation(DEST + RESULT_FILES[1], DEST + RESULT_FILES[5]);
            app.AddAnnotation(DEST + RESULT_FILES[2], DEST + RESULT_FILES[6]);
            app.AddAnnotation(DEST + RESULT_FILES[3], DEST + RESULT_FILES[7]);

            app.AddWrongAnnotation(DEST + RESULT_FILES[0], DEST + RESULT_FILES[8]);
            app.AddText(DEST + RESULT_FILES[0], DEST + RESULT_FILES[9]);

            app.SignAgain(DEST + RESULT_FILES[0], DEST + RESULT_FILES[10], chain, pk,
                DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CMS,
                "Second signature test", "Gent");
            app.SignAgain(DEST + RESULT_FILES[1], DEST + RESULT_FILES[11], chain, pk,
                DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CMS,
                "Second signature test", "Gent");
            app.SignAgain(DEST + RESULT_FILES[2], DEST + RESULT_FILES[12], chain, pk,
                DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CMS,
                "Second signature test", "Gent");
            app.SignAgain(DEST + RESULT_FILES[3], DEST + RESULT_FILES[13], chain, pk,
                DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CMS,
                "Second signature test", "Gent");
        }
    }
}