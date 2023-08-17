using Amazon.KeyManagementService;
using Amazon.KeyManagementService.Model;
using iText.Kernel.Pdf;
using iText.Signatures;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Utilities.Collections;
using X509Certificate = Org.BouncyCastle.X509.X509Certificate;

namespace iText.SigningExamples.AwsKms
{
    public class AwsKmsSignatureContainer : IExternalSignatureContainer
    {
        public AwsKmsSignatureContainer(X509Certificate x509Certificate, string keyId, Func<List<string>, string> selector)
        {
            this.x509Certificate = x509Certificate;
            this.keyId = keyId;

            using (var kmsClient = new AmazonKeyManagementServiceClient())
            {
                GetPublicKeyRequest getPublicKeyRequest = new GetPublicKeyRequest() { KeyId = keyId };
                GetPublicKeyResponse getPublicKeyResponse = kmsClient.GetPublicKeyAsync(getPublicKeyRequest).Result;
                List<string> signingAlgorithms = getPublicKeyResponse.SigningAlgorithms;
                this.signingAlgorithm = selector.Invoke(signingAlgorithms);
                if (signingAlgorithm == null)
                    throw new ArgumentException("KMS key has no signing algorithms", nameof(keyId));
                signatureFactory = new AwsKmsSignatureFactory(keyId, signingAlgorithm);
            }
        }

        public void ModifySigningDictionary(PdfDictionary signDic)
        {
            signDic.Put(PdfName.Filter, new PdfName("MKLx_AWS_KMS_SIGNER"));
            signDic.Put(PdfName.SubFilter, PdfName.Adbe_pkcs7_detached);
        }

        public byte[] Sign(Stream data)
        {
            CmsProcessable msg = new CmsProcessableInputStream(data);

            CmsSignedDataGenerator gen = new CmsSignedDataGenerator();

            SignerInfoGenerator signerInfoGenerator = new SignerInfoGeneratorBuilder()
                .WithSignedAttributeGenerator(new DefaultSignedAttributeTableGenerator())
                .Build(signatureFactory, x509Certificate);
            gen.AddSignerInfoGenerator(signerInfoGenerator);
            
            IStore<X509Certificate> store =CollectionUtilities.CreateStore(new List<X509Certificate> { x509Certificate });
            gen.AddCertificates(store);

            CmsSignedData sigData = gen.Generate(msg, false);
            return sigData.GetEncoded();
        }

        X509Certificate x509Certificate;
        String keyId;
        string signingAlgorithm;
        ISignatureFactory signatureFactory;
    }

    class AwsKmsSignatureFactory : ISignatureFactory
    {
        private string keyId;
        private string signingAlgorithm;
        private AlgorithmIdentifier signatureAlgorithm;

        public AwsKmsSignatureFactory(string keyId, string signingAlgorithm)
        {
            this.keyId = keyId;
            this.signingAlgorithm = signingAlgorithm;
            string signatureAlgorithmName = signingAlgorithmNameBySpec[signingAlgorithm];
            if (signatureAlgorithmName == null)
                throw new ArgumentException("Unknown signature algorithm " + signingAlgorithm, nameof(signingAlgorithm));

            // Special treatment because of issue https://github.com/bcgit/bc-csharp/issues/250
            switch (signatureAlgorithmName.ToUpperInvariant())
            {
                case "SHA256WITHECDSA":
                    this.signatureAlgorithm = new AlgorithmIdentifier(X9ObjectIdentifiers.ECDsaWithSha256);
                    break;
                case "SHA512WITHECDSA":
                    this.signatureAlgorithm = new AlgorithmIdentifier(X9ObjectIdentifiers.ECDsaWithSha512);
                    break;
                default:
                    this.signatureAlgorithm = new DefaultSignatureAlgorithmIdentifierFinder().Find(signatureAlgorithmName);
                    break;
            }
        }

        public object AlgorithmDetails => signatureAlgorithm;

        public IStreamCalculator<IBlockResult> CreateCalculator()
        {
            return new AwsKmsStreamCalculator(keyId, signingAlgorithm);
        }

        static Dictionary<string, string> signingAlgorithmNameBySpec = new Dictionary<string, string>()
        {
            { "ECDSA_SHA_256", "SHA256withECDSA" },
            { "ECDSA_SHA_384", "SHA384withECDSA" },
            { "ECDSA_SHA_512", "SHA512withECDSA" },
            { "RSASSA_PKCS1_V1_5_SHA_256", "SHA256withRSA" },
            { "RSASSA_PKCS1_V1_5_SHA_384", "SHA384withRSA" },
            { "RSASSA_PKCS1_V1_5_SHA_512", "SHA512withRSA" },
            { "RSASSA_PSS_SHA_256", "SHA256withRSAandMGF1"},
            { "RSASSA_PSS_SHA_384", "SHA384withRSAandMGF1"},
            { "RSASSA_PSS_SHA_512", "SHA512withRSAandMGF1"}
        };
    }

    class AwsKmsStreamCalculator : IStreamCalculator<IBlockResult>
    {
        private string keyId;
        private string signingAlgorithm;
        private MemoryStream stream = new MemoryStream();

        public AwsKmsStreamCalculator(string keyId, string signingAlgorithm)
        {
            this.keyId = keyId;
            this.signingAlgorithm = signingAlgorithm;
        }

        public Stream Stream => stream;

        public IBlockResult GetResult()
        {
            try
            {
                using (var kmsClient = new AmazonKeyManagementServiceClient())
                {
                    SignRequest signRequest = new SignRequest()
                    {
                        SigningAlgorithm = signingAlgorithm,
                        KeyId = keyId,
                        MessageType = MessageType.RAW,
                        Message = new MemoryStream(stream.ToArray())
                    };
                    SignResponse signResponse = kmsClient.SignAsync(signRequest).Result;
                    return new SimpleBlockResult(signResponse.Signature.ToArray());
                }
            }
            finally
            {
                stream = new MemoryStream();
            }
        }
    }
}
