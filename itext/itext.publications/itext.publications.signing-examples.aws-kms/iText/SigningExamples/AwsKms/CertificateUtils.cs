using Amazon.KeyManagementService;
using Amazon.KeyManagementService.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace iText.SigningExamples.AwsKms
{
    public class CertificateUtils
    {
        public static X509Certificate2 generateSelfSignedCertificate(string keyId, string subjectDN, Func<List<string>, string> selector)
        {
            string signingAlgorithm = null;
            using (var kmsClient = new AmazonKeyManagementServiceClient())
            {
                GetPublicKeyRequest getPublicKeyRequest = new GetPublicKeyRequest() { KeyId = keyId };
                GetPublicKeyResponse getPublicKeyResponse = kmsClient.GetPublicKeyAsync(getPublicKeyRequest).Result;
                List<string> signingAlgorithms = getPublicKeyResponse.SigningAlgorithms;
                signingAlgorithm = selector.Invoke(signingAlgorithms);
                byte[] spkiBytes = getPublicKeyResponse.PublicKey.ToArray();

                CertificateRequest certificateRequest = null;
                X509SignatureGenerator simpleGenerator = null;
                string keySpecString = getPublicKeyResponse.CustomerMasterKeySpec.ToString();
                if (keySpecString.StartsWith("ECC"))
                {
                    ECDsa ecdsa = ECDsa.Create();
                    int bytesRead = 0;
                    ecdsa.ImportSubjectPublicKeyInfo(new ReadOnlySpan<byte>(spkiBytes), out bytesRead);
                    certificateRequest = new CertificateRequest(subjectDN, ecdsa, getHashAlgorithmName(signingAlgorithm));
                    simpleGenerator = X509SignatureGenerator.CreateForECDsa(ecdsa);
                }
                else if (keySpecString.StartsWith("RSA"))
                {
                    RSA rsa = RSA.Create();
                    int bytesRead = 0;
                    rsa.ImportSubjectPublicKeyInfo(new ReadOnlySpan<byte>(spkiBytes), out bytesRead);
                    RSASignaturePadding rsaSignaturePadding = getSignaturePadding(signingAlgorithm);
                    certificateRequest = new CertificateRequest(subjectDN, rsa, getHashAlgorithmName(signingAlgorithm), rsaSignaturePadding);
                    simpleGenerator = X509SignatureGenerator.CreateForRSA(rsa, rsaSignaturePadding);
                }
                else
                {
                    throw new ArgumentException("Cannot determine encryption algorithm for " + keySpecString, nameof(keyId));
                }

                X509SignatureGenerator generator = new SignatureGenerator(keyId, signingAlgorithm, simpleGenerator);
                X509Certificate2 certificate = certificateRequest.Create(new X500DistinguishedName(subjectDN), generator, System.DateTimeOffset.Now, System.DateTimeOffset.Now.AddYears(2), new byte[] { 17 });
                return certificate;
            }
        }

        public static HashAlgorithmName getHashAlgorithmName(string signingAlgorithm)
        {
            if (signingAlgorithm.Contains("SHA_256"))
            {
                return HashAlgorithmName.SHA256;
            }
            else if (signingAlgorithm.Contains("SHA_384"))
            {
                return HashAlgorithmName.SHA384;
            }
            else if (signingAlgorithm.Contains("SHA_512"))
            {
                return HashAlgorithmName.SHA512;
            }
            else
            {
                throw new ArgumentException("Cannot determine hash algorithm for " + signingAlgorithm, nameof(signingAlgorithm));
            }
        }

        public static RSASignaturePadding getSignaturePadding(string signingAlgorithm)
        {
            if (signingAlgorithm.StartsWith("RSASSA_PKCS1_V1_5"))
            {
                return RSASignaturePadding.Pkcs1;
            }
            else if (signingAlgorithm.StartsWith("RSASSA_PSS"))
            {
                return RSASignaturePadding.Pss;
            }
            else
            {
                return null;
            }
        }

        class SignatureGenerator : X509SignatureGenerator
        {
            public SignatureGenerator(string keyId, string signingAlgorithm, X509SignatureGenerator simpleGenerator)
            {
                this.keyId = keyId;
                this.signingAlgorithm = signingAlgorithm;
                this.simpleGenerator = simpleGenerator;
            }

            public override byte[] GetSignatureAlgorithmIdentifier(HashAlgorithmName hashAlgorithm)
            {
                HashAlgorithmName hashAlgorithmHere = getHashAlgorithmName(signingAlgorithm);
                if (hashAlgorithm != hashAlgorithmHere)
                {
                    throw new ArgumentException("Hash algorithm " + hashAlgorithm + "does not match signing algorithm " + signingAlgorithm, nameof(hashAlgorithm));
                }
                return simpleGenerator.GetSignatureAlgorithmIdentifier(hashAlgorithm);
            }

            public override byte[] SignData(byte[] data, HashAlgorithmName hashAlgorithm)
            {
                HashAlgorithmName hashAlgorithmHere = getHashAlgorithmName(signingAlgorithm);
                if (hashAlgorithm != hashAlgorithmHere)
                {
                    throw new ArgumentException("Hash algorithm " + hashAlgorithm + "does not match signing algorithm " + signingAlgorithm, nameof(hashAlgorithm));
                }

                using (var kmsClient = new AmazonKeyManagementServiceClient())
                {
                    SignRequest signRequest = new SignRequest()
                    {
                        SigningAlgorithm = signingAlgorithm,
                        KeyId = keyId,
                        MessageType = MessageType.RAW,
                        Message = new MemoryStream(data)
                    };
                    SignResponse signResponse = kmsClient.SignAsync(signRequest).Result;
                    return signResponse.Signature.ToArray();
                }
            }

            protected override PublicKey BuildPublicKey()
            {
                return simpleGenerator.PublicKey;
            }

            string keyId;
            string signingAlgorithm;
            X509SignatureGenerator simpleGenerator;
        }
    }
}
