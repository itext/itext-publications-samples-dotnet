using iText.IO.Util;
using iText.Kernel.Pdf;
using iText.Signatures;
using System;
using System.IO;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;

namespace iText.SigningExamples.Simple
{
    public class X509Certificate2SignatureContainer : IExternalSignatureContainer
    {
        private X509Certificate2 certificate;
        private Action<CmsSigner> customization;

        public X509Certificate2SignatureContainer(X509Certificate2 certificate, Action<CmsSigner> customization = null)
        {
            this.certificate = certificate;
            this.customization = customization;
        }

        public void ModifySigningDictionary(PdfDictionary signDic)
        {
            signDic.Put(PdfName.Filter, new PdfName("MKLx_GENERIC_SIGNER"));
            signDic.Put(PdfName.SubFilter, PdfName.Adbe_pkcs7_detached);
        }

        public byte[] Sign(Stream data)
        {
            ContentInfo content = new ContentInfo(StreamUtil.InputStreamToArray(data));
            SignedCms signedCms = new SignedCms(content, true);
            CmsSigner signer = new CmsSigner(certificate);
            customization?.Invoke(signer);
            signer.IncludeOption = X509IncludeOption.WholeChain;
            signedCms.ComputeSignature(signer);
            return signedCms.Encode();
        }
    }
}
