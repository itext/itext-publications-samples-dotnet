using iText.Bouncycastleconnector;
using iText.Commons.Bouncycastle;
using iText.Commons.Bouncycastle.Asn1.X509;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Crypto;
using iText.Commons.Utils;
using iText.Kernel.Pdf;
using iText.Samples.Sandbox.Signatures.Utils;
using iText.Signatures;
using iText.Signatures.Cms;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Serialization;
using iText.Kernel.Crypto;

namespace iText.Samples.Sandbox.Signatures.TwoPhase
{
    /// <summary> Basic example of document two-phase signing.</summary>
    public class TwoPhaseSigningExample
    {

        private static readonly IBouncyCastleFactory BC_FACTORY = BouncyCastleFactoryCreator.GetFactory();
        private IDigest digest;
        public const string SRC = "../../../resources/pdfs/hello.pdf";
        public const string DEST = "results/sandbox/signatures/twophase/2phaseSigned.pdf";
        public const string PREPPED = "results/sandbox/signatures/twophase/2phasePrepared.tmp";
        
        // Digest oid
        private const String SHA512 = "2.16.840.1.101.3.4.2.3";

        private const String FIELD_NAME = "TEST_SIGNATURE";
        /// <summary> In this example the certificate chain is contained in one PEM file
        /// this can be split in multiple ways.
        ///
        /// Merging PEM files is easy, and can be done in a text editor if needed.</summary>
        private const String CHAIN = "../../../resources/cert/rsachain.pem";
        private const String KEYFILE = "../../../resources/cert/rsakey.xml";

        /// <summary> The example PEM with key has no password. </summary>
        private const char[] PASSWORD = null;

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            TwoPhaseSigningExample twoPhaseSigner = new TwoPhaseSigningExample();

            // Set the signature field name to a specific value, we need this to complete the signature later on.
            SignerProperties signerProperties = new SignerProperties().SetFieldName(FIELD_NAME);

            // Prepare the document, add the field, create a CMS container and return the data to sign.
            byte[] dataToSign = twoPhaseSigner.PrepareDocument(SRC, PREPPED, CHAIN, SHA512, signerProperties);

            // Sign the data to sign, in a two phase scenario this typically happens off device.
            byte[] signature = twoPhaseSigner.GetRSASignatureForDigest(dataToSign, KEYFILE, PASSWORD);

            // Add the signature to the document.
            twoPhaseSigner.CompletePreparedDocument(PREPPED, DEST, signerProperties.GetFieldName(), signature);
        }


        /// <summary>Prepares a pdf document to be signed externally.</summary>
        /// <param name="sourcePath">        the path to the document to be signed</param>
        /// <param name="targetPath">        the path to the prepared document</param>
        /// <param name="certificatePath">   the path to a pem file containing the certificate chain</param>
        /// <param name="digestAlgorithmOid">the Oid off the digest to use (a list of constants for this is added tyo this example)</param>
        /// <param name="signerProperties">  the signer properties to use to prepare the document</param>
        /// <returns>the data to be signed (not yet digested).</returns>
        public byte[] PrepareDocument(String sourcePath, String targetPath, String certificatePath,
                                      String digestAlgorithmOid, SignerProperties signerProperties)
        {
            CMSContainer cmsContainer;
            using (PdfReader reader = new PdfReader(FileUtil.GetInputStreamForFile(sourcePath)))
            using (FileStream outputStream = FileUtil.GetFileOutputStream(targetPath + ".temp"))
            {

                IX509Certificate[] certificateChain = loadCertificatesFromFile(certificatePath);

                String digestAlgorithm = DigestAlgorithms.GetDigest(digestAlgorithmOid);

                // 1. Preparing the container to get a size estimate
                cmsContainer = new CMSContainer();
                cmsContainer.AddCertificates(certificateChain);
                cmsContainer.GetSignerInfo().SetSigningCertificateAndAddToSignedAttributes(certificateChain[0],
                        digestAlgorithmOid);
                // In a later version the default algorithm is extracted from the certificate
                cmsContainer.GetSignerInfo().SetSignatureAlgorithm(getAlgorithmOidFromCertificate(certificateChain[0]));
                cmsContainer.GetSignerInfo().SetDigestAlgorithm(new AlgorithmIdentifier(digestAlgorithmOid));

                // Next to these required fields, we can add validation data and other signed or unsigned attributes with
                // the following methods:
                // cmsContainer.GetSignerInfo().SetCrlResponses();
                // cmsContainer.GetSignerInfo().SetOcspResponses();
                // cmsContainer.GetSignerInfo().AddSignedAttribute();
                // cmsContainer.GetSignerInfo().AddUnSignedAttribute();

                // Get the estimated size
                long estimatedSize = cmsContainer.GetSizeEstimation();
                
                digest = BC_FACTORY.CreateIDigest(digestAlgorithm);
                // Add enough space for the digest
                estimatedSize += digest.GetDigestLength() * 2L + 2;
                // Duplicate the size for conversion to hex
                estimatedSize *= 2;

                PdfTwoPhaseSigner signer = new PdfTwoPhaseSigner(reader, outputStream);
                signer.SetStampingProperties(new StampingProperties().UseAppendMode());

                // 2. Prepare the document by adding the signature field and getting the digest in return
                byte[] documentDigest = signer.PrepareDocumentForSignature(signerProperties, digestAlgorithm,
                        PdfName.Adobe_PPKLite, PdfName.Adbe_pkcs7_detached, (int)estimatedSize, false);

                // 3. Add the digest to the CMS container, because this will be part of the items to be signed
                cmsContainer.GetSignerInfo().SetMessageDigest(documentDigest);
            }
            // 4. This step is completely optional. Add the CMS container to the document
            // to avoid having to build it again, or storing it separately from the document
            using (PdfReader reader = new PdfReader(targetPath + ".temp"))
            using (FileStream outputStream = FileUtil.GetFileOutputStream(targetPath))
            {
                PdfTwoPhaseSigner.AddSignatureToPreparedDocument(reader, signerProperties.GetFieldName(), outputStream,
                        cmsContainer.Serialize());
            }

            // 5. The serialized signed attributes is what actually needs to be signed
            // sometimes we have to create a digest from it, sometimes this needs to be sent as is.
            return cmsContainer.GetSerializedSignedAttributes();
        }

        /// <summary>* This method simulates signing outside this application.</summary>
        /// <param name="signedAttributes">    the data to be signed</param>
        /// <param name="pathToKeyFile">       the path to the file containing the private key</param>
        /// <param name="keyPass">             the password for the key file if any</param>µ
        /// <param name="signingAlgorithmName">the signing algorithm name, see  <a href="https://docs.oracle.com/en/java/javase/11/docs/specs/security/standard-names.html#signature-algorithms">...</a></param>
        /// <returns>the signature of the data.</returns>     
        public byte[] GetRSASignatureForDigest(byte[] signedAttributes, String pathToKeyFile, char[] keyPass)
        {
            using (var fs = new StreamReader(pathToKeyFile)) {
                RSA rsa = RSA.Create();
                XmlSerializer serializer =
                    new XmlSerializer(typeof(RSAParameters));
                var rsaParams = (RSAParameters)serializer.Deserialize(fs);
                rsa.ImportParameters(rsaParams);
                byte[] signaturedata = rsa.SignData(signedAttributes, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
                return signaturedata;
            }           
        }

        ///<summary>Adds an existing signature to a PDF where space was already reserved.</summary>
        /// <param name="preparedDocumentPath"> the original prepared PDF</param>
        /// <param name="targetPath">           the output PDF</param>
        ///<param name="fieldName">            the name of the signature field to sign</param>
        ///<param name="signature">            the signed bytes for the signed data</param> 

        public void CompletePreparedDocument(String preparedDocumentPath, String targetPath, String fieldName,
                                             byte[] signature)
        {
            using (PdfReader reader = new PdfReader(preparedDocumentPath))
            using (PdfDocument document = new PdfDocument(reader))
            using (FileStream outputStream = FileUtil.GetFileOutputStream(targetPath))
            {
                // 1. Read the documents CMS container
                SignatureUtil su = new SignatureUtil(document);
                PdfSignature sig = su.GetSignature(fieldName);
                PdfString encodedCMS = sig.GetContents();
                byte[] encodedCMSdata = encodedCMS.GetValueBytes();
                CMSContainer cmsContainer = new CMSContainer(encodedCMSdata);

                // 2. Add the signatureValue to the CMS
                cmsContainer.GetSignerInfo().SetSignature(signature);

                PdfTwoPhaseSigner.AddSignatureToPreparedDocument(document, fieldName, outputStream,
                        cmsContainer.Serialize());
            }
        }

        ///<summary>Creates signing chain for the sample. This chain shouldn't be used for the real signing.</summary>

        /// <param name="certificatePath">path to the file with certificate chain</param>
        /// <returns>the chain of certificates to be used for the signing operation.</returns>
        /// 
        protected IX509Certificate[] loadCertificatesFromFile(String certificatePath)
        {            
            var asCollection = PemFileHelper.ReadFirstChain(certificatePath);

            var result = new IX509Certificate[asCollection.Length];
            asCollection.CopyTo(result, 0);
            return result;
        }

        ///<summary>Creates private key for the sample. This key shouldn't be used for the real signing.</summary>     
        /// <param name="pathToKeyFile">path to the file with private key</param>
        /// <param name="keyPass">      key password</param>
        /// <return>{@link PrivateKey} instance to be used for the main signing operation.</return> 

        protected IPrivateKey getKeyFromPemFile(String pathToKeyFile, char[] keyPass)
        {
            return PemFileHelper.ReadFirstKey(pathToKeyFile, keyPass);
        }

        private AlgorithmIdentifier getAlgorithmOidFromCertificate(IX509Certificate x509Certificate)
        {
            ITbsCertificateStructure tbsCert = BC_FACTORY.CreateTBSCertificate(x509Certificate.GetTbsCertificate());
            if (tbsCert.GetSubjectPublicKeyInfo().GetAlgorithm().GetParameters() != null)
            {
                if (tbsCert.GetSubjectPublicKeyInfo().GetAlgorithm().GetParameters().IsNull())
                {
                    return new AlgorithmIdentifier(tbsCert.GetSubjectPublicKeyInfo().GetAlgorithm().GetAlgorithm
                        ().GetId(), BC_FACTORY.CreateDERNull());
                }
                return new AlgorithmIdentifier(tbsCert.GetSubjectPublicKeyInfo().GetAlgorithm().GetAlgorithm
                    ().GetId(), tbsCert.GetSubjectPublicKeyInfo().GetAlgorithm().GetParameters().ToASN1Primitive());
            }
            return new AlgorithmIdentifier(tbsCert.GetSubjectPublicKeyInfo().GetAlgorithm().GetAlgorithm().GetId());
        }
    }

}