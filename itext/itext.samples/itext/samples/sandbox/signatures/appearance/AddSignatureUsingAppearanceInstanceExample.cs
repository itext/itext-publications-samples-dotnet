using System;
using System.IO;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Crypto;
using iText.Commons.Utils;
using iText.Forms.Form.Element;
using iText.Kernel.Colors;
using iText.Kernel.Exceptions;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Samples.Sandbox.Signatures.Utils;
using iText.Signatures;

namespace iText.Samples.Sandbox.Signatures.Appearance
{
    /// <summary>Basic example of creating the signature field via signature appearance layout element.</summary>
    public class AddSignatureUsingAppearanceInstanceExample
    {
        public static readonly String SRC = "../../../resources/pdfs/hello.pdf";
        public static readonly String DOC_TO_SIGN = "results/sandbox/signatures/appearance/signatureAddedUsingAppearance.pdf";
        public static readonly String DEST = "results/sandbox/signatures/appearance/signSignatureAddedUsingAppearance.pdf";

        private static readonly String SIGNATURE_NAME = "Signature1";
        private static readonly String CHAIN = "../../../resources/cert/chain.pem";
        private static readonly String SIGN = "../../../resources/cert/sign.pem";
        private static readonly char[] PASSWORD = "testpassphrase".ToCharArray();

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new AddSignatureUsingAppearanceInstanceExample().CreateAndSignSignature(SRC, DOC_TO_SIGN, DEST, SIGNATURE_NAME);
        }

        /// <summary>Basic example of creating the signature field via signature appearance layout element and signing it.</summary>
        /// <param name="src">source file</param>
        /// <param name="doc">destination for the file with added signature field</param>
        /// <param name="dest">final destination file</param>
        /// <param name="signatureName">the name of the signature field to be added and signed</param>
        public void CreateAndSignSignature(String src, String doc, String dest, String signatureName)
        {
            AddSignatureToTheDocument(src, doc, signatureName);
            PdfPadesSigner padesSigner = new PdfPadesSigner(new PdfReader(
                FileUtil.GetInputStreamForFile(doc)), FileUtil.GetFileOutputStream(dest));
            // We can pass the appearance through the signer properties.
            SignerProperties signerProperties = CreateSignerProperties(signatureName);
            padesSigner.SignWithBaselineBProfile(signerProperties, GetCertificateChain(), GetPrivateKey());
        }

        /// <summary>Basic example of creating the signature field via signature appearance layout element.</summary>
        /// <param name="src">source file</param>
        /// <param name="dest">destination for the file with added signature field</param>
        /// <param name="signatureName">the name of the signature field to be added</param>
        protected void AddSignatureToTheDocument(String src, String dest, String signatureName)
        {
            Document document = new Document(new PdfDocument(new PdfReader(src), new PdfWriter(dest)));
            Table table = new Table(2);
            Cell cell = new Cell(0, 2).Add(new Paragraph("Test signature").SetFontColor(ColorConstants.WHITE));
            cell.SetBackgroundColor(ColorConstants.GREEN);
            table.AddCell(cell);
            cell = new Cell().Add(new Paragraph("Signer"));
            cell.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
            table.AddCell(cell);

            // Add signature field to the table.
            cell = new Cell().Add(
                new SignatureFieldAppearance(signatureName)
                    .SetContent("Sign here")
                    .SetHeight(50)
                    .SetWidth(100)
                    .SetInteractive(true));
            table.AddCell(cell);

            document.Add(table);
            document.Close();
        }

        /// <summary>
        /// Creates properties to be used in signing operations. Also creates the appearance that will be passed to the
        /// PDF signer through the signer properties.
        /// </summary>
        /// <param name="signatureName">the name of the signature field to be added</param>
        /// <returns>Signer properties to be used for main signing operation.</returns>
        protected SignerProperties CreateSignerProperties(String signatureName)
        {
            SignerProperties signerProperties = new SignerProperties().SetFieldName(signatureName);

            // Create the appearance instance and set the signature content to be shown and different appearance properties.
            SignatureFieldAppearance appearance = new SignatureFieldAppearance(SignerProperties.IGNORED_ID)
                .SetContent("Signer", "Signature description. " +
                                      "Signer is replaced to the one from the certificate.")
                .SetBackgroundColor(ColorConstants.YELLOW);

            // Set created signature appearance and other signer properties.
            signerProperties
                .SetSignatureAppearance(appearance)
                .SetReason("Reason")
                .SetLocation("Location");
            return signerProperties;
        }

        /// <summary>Creates signing chain for the sample. This chain shouldn't be used for the real signing.</summary>
        /// <returns>The chain of certificates to be used for the signing operation.</returns>
        protected IX509Certificate[] GetCertificateChain()
        {
            try
            {
                return PemFileHelper.ReadFirstChain(CHAIN);
            }
            catch (Exception e)
            {
                throw new PdfException(e);
            }
        }

        /// <summary>Creates private key for the sample. This key shouldn't be used for the real signing.</summary>
        /// <returns>IPrivateKey instance to be used for the main signing operation.</returns>
        protected IPrivateKey GetPrivateKey()
        {
            try
            {
                return PemFileHelper.ReadFirstKey(SIGN, PASSWORD);
            }
            catch (Exception e)
            {
                throw new PdfException(e);
            }
        }
    }
}