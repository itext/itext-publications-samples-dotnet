using Amazon.KeyManagementService;
using Amazon.KeyManagementService.Model;
using iText.Signatures;
using System;
using System.Collections.Generic;
using System.IO;

namespace iText.SigningExamples.AwsKms
{
    public class AwsKmsSignature : IExternalSignature
    {
        public AwsKmsSignature(string keyId, Func<List<string>, string> selector)
        {
            this.keyId = keyId;
            using (var kmsClient = new AmazonKeyManagementServiceClient())
            {
                GetPublicKeyRequest getPublicKeyRequest = new GetPublicKeyRequest() { KeyId = keyId };
                GetPublicKeyResponse getPublicKeyResponse = kmsClient.GetPublicKeyAsync(getPublicKeyRequest).Result;
                List<string> signingAlgorithms = getPublicKeyResponse.SigningAlgorithms;
                signingAlgorithm = selector.Invoke(signingAlgorithms);
                switch(signingAlgorithm)
                {
                    case "ECDSA_SHA_256":
                    case "ECDSA_SHA_384":
                    case "ECDSA_SHA_512":
                    case "RSASSA_PKCS1_V1_5_SHA_256":
                    case "RSASSA_PKCS1_V1_5_SHA_384":
                    case "RSASSA_PKCS1_V1_5_SHA_512":
                        break;
                    case "RSASSA_PSS_SHA_256":
                    case "RSASSA_PSS_SHA_384":
                    case "RSASSA_PSS_SHA_512":
                        throw new ArgumentException(String.Format("Signing algorithm {0} not supported directly by iText", signingAlgorithm));
                    default:
                        throw new ArgumentException(String.Format("Unknown signing algorithm: {0}", signingAlgorithm));
                }
            }
        }

        public string GetSignatureAlgorithmName()
        {
            switch (signingAlgorithm)
            {
                case "ECDSA_SHA_256":
                case "ECDSA_SHA_384":
                case "ECDSA_SHA_512":
                    return "ECDSA";
                case "RSASSA_PKCS1_V1_5_SHA_256":
                case "RSASSA_PKCS1_V1_5_SHA_384":
                case "RSASSA_PKCS1_V1_5_SHA_512":
                    return "RSA";
                default:
                    return null;
            }
        }

        public string GetDigestAlgorithmName()
        {
            switch (signingAlgorithm)
            {
                case "ECDSA_SHA_256":
                case "RSASSA_PKCS1_V1_5_SHA_256":
                    return "SHA-256";
                case "ECDSA_SHA_384":
                case "RSASSA_PKCS1_V1_5_SHA_384":
                    return "SHA-384";
                case "ECDSA_SHA_512":
                case "RSASSA_PKCS1_V1_5_SHA_512":
                    return "SHA-512";
                default:
                    return null;
            }
        }

        public byte[] Sign(byte[] message)
        {
            using (var kmsClient = new AmazonKeyManagementServiceClient())
            {
                SignRequest signRequest = new SignRequest() {
                    SigningAlgorithm = signingAlgorithm,
                    KeyId=keyId,
                    MessageType=MessageType.RAW,
                    Message=new MemoryStream(message)
                };
                SignResponse signResponse = kmsClient.SignAsync(signRequest).Result;
                return signResponse.Signature.ToArray();
            }
        }

        string keyId;
        string signingAlgorithm;
    }
}
