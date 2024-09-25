using System;
using System.IO;
using System.Linq;
using iText.Bouncycastle.X509;
using iText.Commons.Bouncycastle.Cert;
using iText.Kernel.Pdf;
using iText.Kernel.Crypto;
using iText.Signatures;
using NUnit.Framework;

namespace iText.SigningExamples.Pkcs11
{
    
    /// <summary>
    /// This sample shows how to sign a pdf document using a Belgian eId card.
    /// Next to a card reader, the correct software needs to be installed.
    /// This software can be found here: https://eid.belgium.be/en
    /// 
    /// Note that for signing with an eId of any kind, it is prefered to let the container 
    /// ask for the PIN code instead of passing it trough application memory.
    /// </summary>
    public class TestBelgianEIdSigning
    {
        // update this path to the specific location on your device
        private const string LIBPATH = @"C:\Program Files (x86)\Belgium Identity Card\FireFox Plugin Manifests\beid_ff_pkcs11_64.dll";
        private const string testFileName = @"..\..\..\resources\circles.pdf";
        
        [Test]
        public void TestEIdSigning()
        {
            using (Pkcs11Signature signature = new Pkcs11Signature(LIBPATH))
            using (PdfReader pdfReader = new PdfReader(testFileName))
            using (FileStream result = File.Create("circles-pkcs11-b-eid-signed-simple.pdf"))
            {
                // list available slots
                var slots = signature.GetAvailbaleSlots();
                // select the slot containing a Belgian eId card
                var slot = slots.Where(s => "Belgium eID".Equals(s.TokenModel)).FirstOrDefault();
                if (slot != null)
                {
                    // setting the pin here is not needed, and it will be asked interactievely anyhow for signing

                    //list available keys
                    var keys = signature.GetCertificatesWithPrivateKeys(slot);

                    // On a Belgian eId card there are two keys available
                    // which both can produce a valid digital signature.
                    // But one is designated for authentication purposes and the other for digital signatures.  
                    // The keys and their certificate are labelled as such
                    //
                    // here we search for the key to sign with
                    var key = keys.FindLast(k => k.certificateLabel.Equals("Signature"));
                    if (key == null)
                    {
                        throw new Exception("No valid key found.");
                    }

                    // Select the key and certificate to be used
                    signature.SelectSigningKeyAndCertificate(key);

                    PdfSigner pdfSigner = new PdfSigner(pdfReader, result, new StampingProperties().UseAppendMode());

                    IX509Certificate[] certificateWrappers = signature.GetChain().Select(e => new X509CertificateBC(e)).ToArray();

                    signature.SetDigestAlgorithmName(DigestAlgorithms.SHA256);
                    pdfSigner.SignDetached(signature, certificateWrappers, null, null, null, 0, PdfSigner.CryptoStandard.CMS);
                }
                else
                {
                    throw new Exception("No eId card available.");
                }
            }
        }
    }
}