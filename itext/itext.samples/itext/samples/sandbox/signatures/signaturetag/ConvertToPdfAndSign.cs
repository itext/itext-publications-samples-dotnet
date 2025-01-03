using System;
using System.IO;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Crypto;
using iText.Commons.Utils;
using iText.Forms.Form.Element;
using iText.Html2pdf;
using iText.Html2pdf.Attach.Impl;
using iText.Kernel.Colors;
using iText.Kernel.Exceptions;
using iText.Kernel.Pdf;
using iText.Samples.Sandbox.Signatures.Utils;
using iText.Signatures;

namespace iText.Samples.Sandbox.Signatures.Signaturetag
{
    public class ConvertToPdfAndSign
    {
        public static readonly string SRC = "../../../resources/signatures/signaturetag/ConvertToPdfAndSign/htmlWithSignatureTag.html";
        public static readonly string DEST = "results/sandbox/signatures/signaturetag/ConvertToPdfAndSign/signedPdfAfterConvert.pdf";
        
        private static readonly String PDF_TO_SIGN = "results/sandbox/signatures/signaturetag/ConvertToPdfAndSign/htmlToSign.pdf";

        
        private static readonly String CHAIN = "../../../resources/cert/chain.pem";
        private static readonly String SIGN = "../../../resources/cert/sign.pem";
        private static readonly char[] PASSWORD = "testpassphrase".ToCharArray();


        
        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new ConvertToPdfAndSign().ConvertHtmlToPdfAndSign(PDF_TO_SIGN, SRC);
        }

        private void ConvertHtmlToPdfAndSign(String dest, String src)
        {
            ConverterProperties converterProperties = new ConverterProperties();
            DefaultTagWorkerFactory tagWorkerFactory = new SignatureTagWorkerFactory();
            converterProperties.SetTagWorkerFactory(tagWorkerFactory);
            
            PdfWriter pdfWriter = new PdfWriter(dest);
            PdfDocument pdfDocument = new PdfDocument(pdfWriter);
            
            HtmlConverter.ConvertToPdf(new FileStream(src, FileMode.Open), pdfDocument, converterProperties);

            PdfPadesSigner padesSigner = new PdfPadesSigner(new PdfReader(FileUtil.GetInputStreamForFile(dest)),
                FileUtil.GetFileOutputStream(DEST));
            // We can pass the appearance through the signer properties.
            SignerProperties signerProperties = CreateSignerProperties("signature_id");
            padesSigner.SignWithBaselineBProfile(signerProperties, GetCertificateChain(), GetPrivateKey());
        }
        
        private static SignerProperties CreateSignerProperties(String signatureName)
        {
            SignerProperties signerProperties = new SignerProperties().SetFieldName(signatureName);
            
            // Create the appearance instance and set the signature content to be shown and different appearance properties.
            SignatureFieldAppearance appearance = new SignatureFieldAppearance(signatureName)
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
        
        private static IX509Certificate[] GetCertificateChain()
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
        private static IPrivateKey GetPrivateKey()
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
