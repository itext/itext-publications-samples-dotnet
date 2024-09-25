using System;
using System.IO;
using iText.Bouncycastle.X509;
using iText.Bouncycastle.Crypto;
using iText.Commons.Bouncycastle.Cert;
using iText.Kernel.Crypto;
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

        public static readonly string KEYSTORE = "../../../resources/encryption/certificate.p12";
        public static readonly string SRC = "../../../resources/pdfs/hello.pdf";

        public static readonly char[] PASSWORD = "testpassphrase".ToCharArray();

        public static readonly string[] RESULT_FILES =
        {
            "hello_level_1.pdf", "hello_level_2.pdf",
            "hello_level_3.pdf", "hello_level_4.pdf",
            "hello_level_1_annotated.pdf", "hello_level_2_annotated.pdf",
            "hello_level_3_annotated.pdf", "hello_level_4_annotated.pdf",
            "hello_level_1_annotated_wrong.pdf", "hello_level_1_text.pdf",
            "hello_level_1_double.pdf", "hello_level_2_double.pdf",
            "hello_level_3_double.pdf", "hello_level_4_double.pdf",
        };

        public void Sign(string src, string dest, X509Certificate[] chain, ICipherParameters pk,
            string digestAlgorithm, PdfSigner.CryptoStandard subfilter,
            AccessPermissions certificationLevel, string reason, string location)
        {
            PdfReader reader = new PdfReader(src);
            PdfSigner signer = new PdfSigner(reader, new FileStream(dest, FileMode.Create), new StampingProperties());

            // Create the signature appearance
            SignerProperties signerProperties = new SignerProperties()
                .SetReason(reason)
                .SetLocation(location);

            Rectangle rect = new Rectangle(36, 648, 200, 100);
            signerProperties.SetPageRect(rect)
                .SetPageNumber(1)
                .SetFieldName("sig");

            // Set the document's certification level. This parameter defines if changes are allowed
            // after the applying of the signature.
            signerProperties.SetCertificationLevel(certificationLevel);
            signer.SetSignerProperties(signerProperties);

            PrivateKeySignature pks = new PrivateKeySignature(new PrivateKeyBC(pk), digestAlgorithm);

            IX509Certificate[] certificateWrappers = new IX509Certificate[chain.Length];
            for (int i = 0; i < certificateWrappers.Length; ++i) {
                certificateWrappers[i] = new X509CertificateBC(chain[i]);
            }
            // Sign the document using the detached mode, CMS or CAdES equivalent.
            signer.SignDetached(pks, certificateWrappers, null, null, null, 0, subfilter);
        }

        public void AddText(string src, string dest)
        {
            PdfReader reader = new PdfReader(src);
            PdfDocument pdfDoc = new PdfDocument(reader, new PdfWriter(dest),
                new StampingProperties().UseAppendMode());
            PdfPage firstPage = pdfDoc.GetFirstPage();

            new Canvas(firstPage, firstPage.GetPageSize()).ShowTextAligned("TOP SECRET",
                36, 820, TextAlignment.LEFT);

            pdfDoc.Close();
        }

        public void AddAnnotation(string src, string dest)
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

        public void SignAgain(string src, string dest, X509Certificate[] chain, ICipherParameters pk,
            string digestAlgorithm, PdfSigner.CryptoStandard subfilter, string reason, string location)
        {
            PdfReader reader = new PdfReader(src);
            PdfSigner signer = new PdfSigner(reader, new FileStream(dest, FileMode.Create),
                new StampingProperties().UseAppendMode());

            Rectangle rect = new Rectangle(36, 700, 200, 100);
            SignerProperties signerProperties = new SignerProperties()
                .SetFieldName("Signature2")
                .SetReason(reason)
                .SetLocation(location)
                .SetPageRect(rect)
                .SetPageNumber(1);
            signer.SetSignerProperties(signerProperties);

            IX509Certificate[] certificateWrappers = new IX509Certificate[chain.Length];
            for (int i = 0; i < certificateWrappers.Length; ++i) {
                certificateWrappers[i] = new X509CertificateBC(chain[i]);
            }
            PrivateKeySignature pks = new PrivateKeySignature(new PrivateKeyBC(pk), digestAlgorithm);
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

            C2_09_SignatureTypes app = new C2_09_SignatureTypes();
            app.Sign(SRC, DEST + RESULT_FILES[0], chain, pk, DigestAlgorithms.SHA256,
                PdfSigner.CryptoStandard.CMS, AccessPermissions.UNSPECIFIED,
                "Test 1", "Ghent");
            app.Sign(SRC, DEST + RESULT_FILES[1], chain, pk, DigestAlgorithms.SHA256,
                PdfSigner.CryptoStandard.CMS, AccessPermissions.ANNOTATION_MODIFICATION,
                "Test 1", "Ghent");
            app.Sign(SRC, DEST + RESULT_FILES[2], chain, pk, DigestAlgorithms.SHA256,
                PdfSigner.CryptoStandard.CMS, AccessPermissions.FORM_FIELDS_MODIFICATION,
                "Test 1", "Ghent");
            app.Sign(SRC, DEST + RESULT_FILES[3], chain, pk, DigestAlgorithms.SHA256,
                PdfSigner.CryptoStandard.CMS, AccessPermissions.NO_CHANGES_PERMITTED,
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