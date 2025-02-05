using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Serialization;
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
using iText.Signatures.Cms;
using Org.BouncyCastle.Utilities;

namespace iText.Samples.Sandbox.Signatures.TwoPhase
{
    /// <summary>Basic example of two phaser document signing with PaDES Baseline-LT Profile.</summary>
    public class TwoPhasePadesSigningExample
    {
        public static readonly String SRC = "../../../resources/pdfs/hello.pdf";
        public static readonly String PREP = "results/sandbox/signatures/twophase/twoPhasePadesSignatureLevelLT.temp";
        public static readonly String DEST = "results/sandbox/signatures/twophase/twoPhasePadesSignatureLevelLT.pdf";

        private static readonly String CHAIN = "../../../resources/cert/chain.pem";
        private static readonly String ROOT = "../../../resources/cert/root.pem";
        private static readonly String SIGN = "../../../resources/cert/sign.pem";
        private static readonly String KEYFILE = "../../../resources/cert/signkey.xml";
        private static readonly String TSA = "../../../resources/cert/tsaCert.pem";
        private static readonly char[] PASSWORD = "testpassphrase".ToCharArray();


        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();


            SignerProperties signerProperties =
                new TwoPhasePadesSigningExample().CreateSignerProperties("PADES_SIGNATURE");
            // prepare the document
            CMSContainer cmsContainer = new TwoPhasePadesSigningExample().PrepareDocument(SRC, PREP,
                GetCertificateChain(),
                "SHA512", signerProperties);

            // simulate getting the signature
            byte[] signature = GetRSASignatureForDigest(cmsContainer.GetSerializedSignedAttributes(),
                KEYFILE, PASSWORD);

            // add the signature and needed info for a valid pades signature
            new TwoPhasePadesSigningExample().SignSignatureWithBaselineLTProfile(PREP, DEST, signerProperties,
                signature,
                "SHA512", "sha512WithRSA", null);
        }

        /// <summary> Prepare the document and put the prepared cms container in it for later reuse.</summary>
        /// <param name="src">             the path to the document to be signed</param>
        /// <param name="prep">            the path to the prepared document</param> 
        /// <param name="certificateChain">the certificate chain </param>
        /// <param name="digestAlgoritm">  the name of the digest algorithm</param>
        /// <param name="signerProperties">the signer properties to use to prepare the document</param>
        /// <returns>A prepared CMS container.</returns> 
        private CMSContainer PrepareDocument(String src, String prep, IX509Certificate[] certificateChain,
            String digestAlgoritm, SignerProperties signerProperties)
        {
            CMSContainer cmsContainer;

            using (PdfReader reader = new PdfReader(src))
            using (FileStream outputStream = FileUtil.GetFileOutputStream(prep + ".working"))
            {
                PadesTwoPhaseSigningHelper helper = new PadesTwoPhaseSigningHelper();
                helper.SetTrustedCertificates(GetTrustedStore());
                cmsContainer = helper.CreateCMSContainerWithoutSignature(certificateChain, digestAlgoritm, reader,
                    outputStream, signerProperties);
            }

            // This is optional, the CMS container can be stored in any way. 
            // Adding the prepared CMS container to the prepared document
            using (PdfReader reader = new PdfReader(prep + ".working"))
            using (FileStream outputStream = FileUtil.GetFileOutputStream(prep))
            {
                PdfTwoPhaseSigner.AddSignatureToPreparedDocument(reader, signerProperties.GetFieldName(), outputStream,
                    cmsContainer);
            }

            FileUtil.DeleteFile(new FileInfo(prep + ".working"));
            return cmsContainer;
        }


        /// <summary>Complete the signing process.</summary>
        ///<param name="src">             the path to the prepared document</param>
        ///<param name="dest">            the path to the signed document</param>
        ///<param name="signerProperties">the signerproperties used to sign</param>
        ///<param name="signature">       the signature</param>
        ///<param name="digestAlgorithm"> the used digest algorithm name</param>
        ///<param name="signingAlgoritm"> the used signing algorithm</param>
        ///<param name="signingParams">   the used signong algorithm properties</param>
        public void SignSignatureWithBaselineLTProfile(String src, String dest, SignerProperties signerProperties,
            byte[] signature, String digestAlgorithm, String signingAlgoritm,
            ISignatureMechanismParams signingParams)
        {
            // Extract the prepared cms container from the prepared document.
            CMSContainer cmsContainer;
            using (PdfReader reader = new PdfReader(src))
            using (PdfDocument doc = new PdfDocument(reader))
            {
                SignatureUtil su = new SignatureUtil(doc);
                PdfSignature preparedSignature = su.GetSignature(signerProperties.GetFieldName());
                cmsContainer = new CMSContainer(preparedSignature.GetContents().GetValueBytes());
            }

            // Second phase of signing.
            PadesTwoPhaseSigningHelper helper = new PadesTwoPhaseSigningHelper();
            helper.SetTrustedCertificates(GetTrustedStore());
            helper.SetTSAClient(GetTsaClient());
            helper.SetOcspClient(GetOcspClient());
            helper.SetCrlClient(GetCrlClient());

            using (PdfReader reader = new PdfReader(src))
            using (FileStream outputStream = FileUtil.GetFileOutputStream(dest))
            {
                // An external signature implementation that starts from an existing signature.
                IExternalSignature externalSignature = new SignatureProvider(signature, digestAlgorithm,
                    signingAlgoritm, signingParams);

                helper.SignCMSContainerWithBaselineTProfile(externalSignature, reader, outputStream,
                    signerProperties.GetFieldName(), cmsContainer);
            }
        }


        /// <summary> This method simulates signing outside this application.</summary>
        /// <param name="signedAttributes">    the data to be signed</param>
        /// <param name="pathToKeyFile">       the path to the file containing the private key</param>
        /// <param name="keyPass">             the password for the key file if any</param>µ
        /// <param name="signingAlgorithmName">the signing algorithm name, see  <a href="https://docs.oracle.com/en/java/javase/11/docs/specs/security/standard-names.html#signature-algorithms">...</a></param>
        /// <returns>The signature of the data.</returns>     
        private static byte[] GetRSASignatureForDigest(byte[] signedAttributes, String pathToKeyFile, char[] keyPass)
        {
            using (var fs = new StreamReader(pathToKeyFile))
            {
                RSA rsa = RSA.Create();
                XmlSerializer serializer =
                    new XmlSerializer(typeof(RSAParameters));
                var rsaParams = (RSAParameters)serializer.Deserialize(fs);

                rsa.ImportParameters(rsaParams);
                byte[] signaturedata =
                    rsa.SignData(signedAttributes, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
                return signaturedata;
            }
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

        /// <summary>Creates trusted certificate store for the sample.
        /// This store shouldn't be used for the real signing.</summary>
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
        protected SignerProperties CreateSignerProperties(String signaturefieldName)
        {
            SignerProperties signerProperties = new SignerProperties().SetFieldName(signaturefieldName);
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


        /// <summary>
        ///  An IExternalSignature implementation that adds a previously created signature.
        /// </summary>
        private class SignatureProvider : IExternalSignature
        {
            private byte[] signature;
            private String digestAlgorithm;
            private String signatureAlgorithm;
            private ISignatureMechanismParams signatureMechanismParameters;

            public SignatureProvider(byte[] signature, String digestAlgorithm, String signatureAlgorithm,
                ISignatureMechanismParams signatureMechanismParameters)
            {
                this.signature = Arrays.CopyOf(signature, signature.Length);
                this.digestAlgorithm = digestAlgorithm;
                this.signatureAlgorithm = signatureAlgorithm;
                this.signatureMechanismParameters = signatureMechanismParameters;
            }

            public String GetDigestAlgorithmName()
            {
                return digestAlgorithm;
            }

            public String GetSignatureAlgorithmName()
            {
                return signatureAlgorithm;
            }

            public ISignatureMechanismParams GetSignatureMechanismParameters()
            {
                return signatureMechanismParameters;
            }

            public byte[] Sign(byte[] message)
            {
                return signature;
            }
        }
    }
}
