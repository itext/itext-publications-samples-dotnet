using iText.Signatures;
using iText.Kernel.Crypto;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Math;
using System;
using System.IO;
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
        private string digestAlgorithmName;
        /** The encryption algorithm (obtained from the private key) */
        private string signatureAlgorithmName;

        /// <summary>
        /// Creates a signature using a X509Certificate2. It supports smartcards without 
        /// exportable private keys.
        /// </summary>
        /// <param name="certificate">The certificate with the private key</param>
        /// <param name="digestAlgorithmName">The digest algorithm for the signature. As the Windows CAPI is used
        /// to do the signature the only hash guaranteed to exist is SHA-1</param>
        public X509Certificate2Signature(X509Certificate2 certificate, string digestAlgorithmName)
        {
            if (!certificate.HasPrivateKey)
                throw new ArgumentException("No private key.");
            this.certificate = certificate;
            this.digestAlgorithmName = DigestAlgorithms.GetDigest(DigestAlgorithms.GetAllowedDigest(digestAlgorithmName));
            if (certificate.GetRSAPrivateKey() != null)
                signatureAlgorithmName = "RSA";
            else if (certificate.GetDSAPrivateKey() != null)
                signatureAlgorithmName = "DSA";
            else if (certificate.GetECDsaPrivateKey() != null)
                signatureAlgorithmName = "ECDSA";
            else
                throw new ArgumentException("Unknown encryption algorithm " + certificate.GetKeyAlgorithm());
        }

        public bool UsePssForRsaSsa { get; set; }

        public string GetSignatureAlgorithmName()
        {
            return UsePssForRsaSsa && "RSA".Equals(signatureAlgorithmName) ? "RSASSA-PSS" : signatureAlgorithmName;
        }

        public ISignatureMechanismParams GetSignatureMechanismParameters()
        {
            return UsePssForRsaSsa && "RSA".Equals(signatureAlgorithmName) ? RSASSAPSSMechanismParams.CreateForDigestAlgorithm(digestAlgorithmName) : null;
        }

        public string GetDigestAlgorithmName()
        {
            return digestAlgorithmName;
        }

        /// <summary>
        /// Beware: This implementation is geared towards DER/Standard encoding of [EC]DSA
        /// signatures. For use with *-PLAIN-* algorithms you'll have to remove the calls of
        /// <see cref="PlainToDer(byte[])"/>  and maybe even introduce a DerToPlain method.
        /// </summary>
        public byte[] Sign(byte[] message)
        {
            switch(signatureAlgorithmName)
            {
                case "RSA":
                    return certificate.GetRSAPrivateKey().SignData(message, new HashAlgorithmName(digestAlgorithmName),
                        UsePssForRsaSsa ? RSASignaturePadding.Pss : RSASignaturePadding.Pkcs1);
                case "DSA":
                    return PlainToDer(certificate.GetDSAPrivateKey().SignData(message, new HashAlgorithmName(digestAlgorithmName)));
                case "ECDSA":
                    return PlainToDer(certificate.GetECDsaPrivateKey().SignData(message, new HashAlgorithmName(digestAlgorithmName)));
                default:
                    throw new ArgumentException("Unknown encryption algorithm " + signatureAlgorithmName);
            }
        }

        byte[] PlainToDer(byte[] plain)
        {
            if (IsDerEncoding(plain))
                return plain;

            int valueLength = plain.Length / 2;
            BigInteger r = new BigInteger(1, plain, 0, valueLength);
            BigInteger s = new BigInteger(1, plain, valueLength, valueLength);
            return new DerSequence(new DerInteger(r), new DerInteger(s)).GetEncoded(Asn1Encodable.Der);
        }

        bool IsDerEncoding(byte[] signatureBytes)
        {
            try
            {
                Asn1Object prim = Asn1Object.FromByteArray(signatureBytes);
                if (prim is Asn1Sequence)
                {
                    Asn1Sequence seq = (Asn1Sequence)prim;
                    if (seq.Count == 2)
                    {
                        return (seq[0] is DerInteger) && (seq[1] is DerInteger);
                    }
                }
            }
            catch (IOException ex)
            {
                // could not be parsed as DER
            }

            return false;
        }
    }
}
