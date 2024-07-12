using iText.Kernel.Pdf;
using iText.Signatures;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Text;
using iText.Bouncycastle.Cert;
using iText.Bouncycastle.X509;
using iText.Commons.Bouncycastle.Cert;
using static iText.Signatures.PdfSigner;

namespace iText.SigningExamples.Pkcs11
{
  
    class TestSignSimple
    {
                [Test]
        public void TestPkcs11SignSimple()
        {
            string testFileName = @"..\..\..\resources\circles.pdf";

            using (Pkcs11Signature signature = new Pkcs11Signature(@"PKCS11LIBRARY"))
            // Belgian eID
            // using (Pkcs11Signature signature = new Pkcs11Signature(@"C:\Program Files (x86)\Belgium Identity Card\FireFox Plugin Manifests\beid_ff_pkcs11_64.dll"))
            // Thales SafeNet
            // using (Pkcs11Signature signature = new Pkcs11Signature(@"C:\Windows\System32\eTPKCS11.dll"))
            // Entrust SAS
            // using (Pkcs11Signature signature = new Pkcs11Signature(@"c:\Program Files\Entrust\SigningClient\P11SigningClient64.dll"))//                
            // Utimaco HSM
            // using (Pkcs11Signature signature = new Pkcs11Signature(@"C:\Program Files\Utimaco\CryptoServer\Lib\cs_pkcs11_R2.dll") { UsePssForRsaSsa = true })
            using (PdfReader pdfReader = new PdfReader(testFileName))
            using (FileStream result = File.Create("circles-pkcs11-signed-simple.pdf"))
            {
                // first lists the slots 
                var slots = signature.GetAvailbaleSlots();
                // select the needed slot/token by slot description or token model or label
                var slot = slots.Where(s => s.SlotDescription.StartsWith("SLOT_DESCRIPTION")).FirstOrDefault();
                if (slot != null)
                {                    
                    // only use a pin code when no other means are available
                    // passing user secrets should be avoided where possible
                    // certainly avoid using strings to store them in memory
                    //
                    // Sometimes the pin is needed to query the token, sometimes only for the signing.
                    // use the pin only for one token as tokens can become locked after severeal failed login attempts.
                    char[] pin = { 'P', 'I', 'N' };
                    var pinAsByteArray = Encoding.GetEncoding("UTF-8").GetBytes(pin);
                    signature.Pin = pinAsByteArray;
                    Array.Clear(pinAsByteArray, 0, pinAsByteArray.Length);
                    Array.Clear(pin, 0, pin.Length);

                    // lists the certificates and select the needed one
                    var key = signature.GetCertificatesWithPrivateKeys(slot).Where(k => k.keyLabel.Equals("KEYALIAS")).FirstOrDefault();

                    // Entrust SAS
                    //var key = signature.GetCertificatesWithPrivateKeys(slot).Where(k => k.certificate.SubjectDN.ToString().Equals("CN=Entrust Limited,OU=ECS,O=Entrust Limited,L=Kanata,ST=Ontario,C=CA")).FirstOrDefault();
                    // Utimaco HSM
                    //var key = signature.GetCertificatesWithPrivateKeys(slot).FirstOrDefault();


                    if (key != null)
                    {
                        signature.SelectSigningKeyAndCertificate(key).SetDigestAlgorithmName("SHA256");

                        //sometimes wo only need the pin here
                        //char[] pin = { 'P', 'I', 'N' };
                        //var pinAsByteArray = Encoding.GetEncoding("UTF-8").GetBytes(pin);
                        //signature.Pin = pinAsByteArray;
                        //Array.Clear(pinAsByteArray, 0, pinAsByteArray.Length);
                        //Array.Clear(pin, 0, pin.Length);

                        PdfSigner pdfSigner = new PdfSigner(pdfReader, result, new StampingProperties().UseAppendMode());
                        ITSAClient tsaClient = new TSAClientBouncyCastle("RFC3161_SERVER_URL");

                        IX509Certificate[] certificateWrappers = new IX509Certificate[signature.GetChain().Length];
                        for (int i = 0; i < certificateWrappers.Length; ++i)
                        {
                            certificateWrappers[i] = new X509CertificateBC(signature.GetChain()[i]);
                        }
                        pdfSigner.SignDetached(signature, certificateWrappers, null, null, tsaClient, 0, CryptoStandard.CMS);
                        return;
                    }
                }
                throw new Exception("No valid token or key found.");
            }
        }
    }
}