using System;
using System.Collections.Generic;
using System.IO;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Crypto;
using iText.Commons.Utils;
using iText.Kernel.Exceptions;
using iText.Kernel.Pdf;
using iText.Samples.Sandbox.Signatures.Clients;
using iText.Samples.Sandbox.Signatures.Utils;
using iText.Signatures;

namespace iText.Samples.Sandbox.Signatures.Pades
{
    /// <summary>Basic example of document signature prolongation.</summary>
    public class PadesProlongSignatureExample
    {
        public static readonly String SRC = "../../../resources/pdfs/padesSignatureLTLevel.pdf";
        public static readonly String DEST = "results/sandbox/signatures/pades/padesProlongSignature.pdf";

        private static readonly String ROOT = "../../../resources/cert/root.pem";
        private static readonly String TSA = "../../../resources/cert/tsaCert.pem";
        private static readonly char[] PASSWORD = "testpassphrase".ToCharArray();

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new PadesProlongSignatureExample().ProlongSignature(SRC, DEST);
        }

        /// <summary>Basic example of document signature prolongation.</summary>
        /// <param name="src">source file</param>
        /// <param name="dest">destination file</param>
        public void ProlongSignature(String src, String dest)
        {
            PdfPadesSigner padesSigner = new PdfPadesSigner(new PdfReader(
                FileUtil.GetInputStreamForFile(src)), FileUtil.GetFileOutputStream(dest));
            // Set clients in order to add revocation information for all the signatures in the provided document.
            padesSigner.SetOcspClient(GetOcspClient()).SetCrlClient(GetCrlClient());
            padesSigner.SetTrustedCertificates(GetTrustedStore());
            padesSigner.SetTimestampSignatureName("TimestampSignature");

            padesSigner.ProlongSignatures(GetTsaClient());
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
            return new TestOcspClient();
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
                IX509Certificate[] signCert = PemFileHelper.ReadFirstChain(ROOT);
                IPrivateKey privateKey = PemFileHelper.ReadFirstKey(ROOT, PASSWORD);
                return new TestCrlClient().AddBuilderForCertIssuer(signCert[0], privateKey);
            }
            catch (Exception e)
            {
                throw new PdfException(e);
            }
        }

        /// <summary>Creates trusted certificate store for the sample. This store shouldn't be used for the real signing.</summary>
        /// <returns>
        /// The trusted certificate store to be used for the missing CRL response issuer certificates retrieving
        /// during the signing operation.
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
    }
}