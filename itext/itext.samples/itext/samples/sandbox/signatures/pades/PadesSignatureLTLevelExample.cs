using System;
using System.Collections.Generic;
using System.IO;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Crypto;
using iText.Commons.Utils;
using iText.Forms.Form.Element;
using iText.Kernel.Exceptions;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Samples.Sandbox.Signatures.Clients;
using iText.Samples.Sandbox.Signatures.Utils;
using iText.Signatures;

namespace iText.Samples.Sandbox.Signatures.Pades
{
    /// <summary>Basic example of document signing with PaDES Baseline-LT Profile.</summary>
    public class PadesSignatureLTLevelExample
    {
        public static readonly String SRC = "../../../resources/pdfs/hello.pdf";
        public static readonly String DEST = "results/sandbox/signatures/pades/padesSignatureLevelLTTest.pdf";

        private static readonly String CHAIN = "../../../resources/cert/chain.pem";
        private static readonly String ROOT = "../../../resources/cert/root.pem";
        private static readonly String SIGN = "../../../resources/cert/sign.pem";
        private static readonly String TSA = "../../../resources/cert/tsaCert.pem";
        private static readonly char[] PASSWORD = "testpassphrase".ToCharArray();

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new PadesSignatureLTLevelExample().SignSignatureWithBaselineLTProfile(SRC, DEST);
        }

        /// <summary>Basic example of document signing with PaDES Baseline-LT Profile.</summary>
        /// <param name="src">source file</param>
        /// <param name="dest">destination file</param>
        public void SignSignatureWithBaselineLTProfile(String src, String dest)
        {
            PdfPadesSigner padesSigner = new PdfPadesSigner(new PdfReader(
                    FileUtil.GetInputStreamForFile(src)),

                FileUtil.GetFileOutputStream(dest));
            SignerProperties signerProperties = CreateSignerProperties();

            padesSigner.SetTrustedCertificates(GetTrustedStore());

            // Set revocation info clients to be used for LTV Verification.
            padesSigner.SetOcspClient(GetOcspClient()).SetCrlClient(GetCrlClient());

            padesSigner.SignWithBaselineLTProfile(signerProperties, GetCertificateChain(), GetPrivateKey(),
                GetTsaClient());
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

        /// <summary>Creates trusted certificate store for the sample. This store shouldn't be used for the real signing.</summary>
        /// <returns>
        /// The trusted certificate store to be used for the missing certificates in chain an CRL response
        /// issuer certificates retrieving during the signing operation.
        /// </returns>
        protected List<IX509Certificate> GetTrustedStore()
        {
            try
            {
                return new List<IX509Certificate>(PemFileHelper.ReadFirstChain(ROOT));
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

        /// <summary>
        /// Creates timestamp client for the sample which will be used for the timestamp creation.
        ///<p>
        ///     NOTE: for the real signing you should use real revocation data clients
        ///     (such as iText.Signatures.TSAClientBouncyCastle).
        /// </p>
        /// </summary>
        /// <returns>The TSA client to be used for the timestamp creation.</returns>
        protected ITSAClient GetTsaClient()
        {
            try
            {
                IX509Certificate[] tsaChain = PemFileHelper.ReadFirstChain(TSA);
                IPrivateKey tsaPrivateKey = PemFileHelper.ReadFirstKey(TSA, PASSWORD);
                return new TestTsaClient(new List<IX509Certificate>(tsaChain), tsaPrivateKey);
            }
            catch (Exception e)
            {
                throw new PdfException(e);
            }
        }

        /// <summary>
        /// Creates an OCSP client for the sample.
        ///<p>
        ///     NOTE: for the real signing you should use real revocation data clients
        ///     (such as iText.Signatures.OcspClientBouncyCastle).
        /// </p>
        /// </summary>
        /// <returns>The OCSP client to be used for the signing operation.</returns>
        protected IOcspClient GetOcspClient()
        {
            try
            {
                IX509Certificate[] caCert = PemFileHelper.ReadFirstChain(ROOT);
                IPrivateKey caPrivateKey = PemFileHelper.ReadFirstKey(ROOT, PASSWORD);
                return new TestOcspClient().AddBuilderForCertificate(caCert[0], caPrivateKey);
            }
            catch (Exception e)
            {
                throw new PdfException(e);
            }
        }

        /// <summary>
        /// Creates an CRL client for the sample.
        ///<p>
        ///     NOTE: for the real signing you should use real revocation data clients
        ///     (such as iText.Signatures.CrlClientOnline).
        /// </p>
        /// </summary>
        /// <returns>The CRL client to be used for the signing operation.</returns>
        protected ICrlClient GetCrlClient()
        {
            try
            {
                IX509Certificate[] signCert = PemFileHelper.ReadFirstChain(SIGN);
                IPrivateKey privateKey = PemFileHelper.ReadFirstKey(SIGN, PASSWORD);
                return new TestCrlClient().AddBuilderForCertIssuer(signCert[0], privateKey);
            }
            catch (Exception e)
            {
                throw new PdfException(e);
            }
        }

        /// <summary>Creates properties to be used in signing operations.</summary>
        /// <returns>Signer properties to be used for main signing operation.</returns>
        protected SignerProperties CreateSignerProperties()
        {
            SignerProperties signerProperties = new SignerProperties().SetFieldName("Signature1");
            SignatureFieldAppearance appearance = new SignatureFieldAppearance(SignerProperties.IGNORED_ID)
                .SetContent("Approval test signature.\nCreated by iText.");
            signerProperties
                .SetPageNumber(1)
                .SetPageRect(new Rectangle(50, 650, 200, 100))
                .SetSignatureAppearance(appearance)
                .SetReason("Reason")
                .SetLocation("Location");
            return signerProperties;
        }
    }
}