using iText.Signatures;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Math;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace iText.SigningExamples.Simple
{
    /// <summary>
    /// Creates a signature using a X509Certificate2. It supports smartcards without 
    /// exportable private keys.
    /// </summary>
    public class X509Certificate2Signature : IExternalSignature
    {
        /// <summary>
        /// The certificate with the private key
        /// </summary>
        private X509Certificate2 certificate;
        /** The hash algorithm. */
        private string hashAlgorithm;
        /** The encryption algorithm (obtained from the private key) */
        private string encryptionAlgorithm;

        /// <summary>
        /// Creates a signature using a X509Certificate2. It supports smartcards without 
        /// exportable private keys.
        /// </summary>
        /// <param name="certificate">The certificate with the private key</param>
        /// <param name="hashAlgorithm">The hash algorithm for the signature. As the Windows CAPI is used
        /// to do the signature the only hash guaranteed to exist is SHA-1</param>
        public X509Certificate2Signature(X509Certificate2 certificate, string hashAlgorithm)
        {
            if (!certificate.HasPrivateKey)
                throw new ArgumentException("No private key.");
            this.certificate = certificate;
            this.hashAlgorithm = DigestAlgorithms.GetDigest(DigestAlgorithms.GetAllowedDigest(hashAlgorithm));
            if (certificate.GetRSAPrivateKey() != null)
                encryptionAlgorithm = "RSA";
            else if (certificate.GetDSAPrivateKey() != null)
                encryptionAlgorithm = "DSA";
            else if (certificate.GetECDsaPrivateKey() != null)
                encryptionAlgorithm = "ECDSA";
            else
                throw new ArgumentException("Unknown encryption algorithm " + certificate.GetKeyAlgorithm());
        }

        public string GetEncryptionAlgorithm()
        {
            return encryptionAlgorithm;
        }

        public string GetHashAlgorithm()
        {
            return hashAlgorithm;
        }

        public byte[] Sign(byte[] message)
        {
            switch(encryptionAlgorithm)
            {
                case "RSA":
                    return certificate.GetRSAPrivateKey().SignData(message, new HashAlgorithmName(hashAlgorithm), RSASignaturePadding.Pkcs1);
                case "DSA":
                    return PlainToDer(certificate.GetDSAPrivateKey().SignData(message, new HashAlgorithmName(hashAlgorithm)));
                case "ECDSA":
                    return certificate.GetECDsaPrivateKey().SignData(message, new HashAlgorithmName(hashAlgorithm));
                default:
                    throw new ArgumentException("Unknown encryption algorithm " + encryptionAlgorithm);
            }
        }

        byte[] PlainToDer(byte[] plain)
        {
            int valueLength = plain.Length / 2;
            BigInteger r = new BigInteger(1, plain, 0, valueLength);
            BigInteger s = new BigInteger(1, plain, valueLength, valueLength);
            return new DerSequence(new DerInteger(r), new DerInteger(s)).GetEncoded(Asn1Encodable.Der);
        }
    }
}
