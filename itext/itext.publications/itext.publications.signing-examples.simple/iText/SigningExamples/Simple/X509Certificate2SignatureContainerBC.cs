using iText.Kernel.Pdf;
using iText.Signatures;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace iText.SigningExamples.Simple
{
    public class X509Certificate2SignatureContainerBC : IExternalSignatureContainer
    {
        private X509Certificate2 certificate;

        public X509Certificate2SignatureContainerBC(X509Certificate2 certificate)
        {
            this.certificate = certificate;
        }

        public void ModifySigningDictionary(PdfDictionary signDic)
        {
            signDic.Put(PdfName.Filter, new PdfName("MKLx_GENERIC_SIGNER"));
            signDic.Put(PdfName.SubFilter, PdfName.Adbe_pkcs7_detached);
        }

        public byte[] Sign(Stream data)
        {
            throw new System.NotImplementedException();
        }
    }
}
