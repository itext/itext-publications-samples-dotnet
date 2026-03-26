using System;
using iText.Kernel.Crypto;

namespace iText.Signatures.Pqc {
    /// <summary>Example of document signing using FN-DSA-512 post-quantum algorithm.</summary>
    public class FNDSA : PqcSignatureExample {
        public const String DEST = "results/sandbox/signatures/pqc/signFNDSA.pdf";

        private const String SIGNATURE_ALGO = "Falcon-512";

        private const String CERT_PATH = "../../../resources/cert/pqc/cert_" + SIGNATURE_ALGO + ".pem";

        private const String DIGEST_ALGO = DigestAlgorithms.SHAKE256;

        public static void Main(String[] args) {
            new FNDSA().ManipulatePdf();
        }

        public override String GetDestination() {
            return DEST;
        }

        /// <summary>This certificate was generated using bouncycastle.</summary>
        /// <returns>FN-DSA-512 certificate</returns>
        public override String GetCertPath() {
            return CERT_PATH;
        }

        /// <summary>Possible FN-DSA algorithms values are Falcon-512 and Falcon-1024 for now.</summary>
        /// <returns>Falcon-512 signature algorithm</returns>
        public override String GetSignatureAlgo() {
            return SIGNATURE_ALGO;
        }

        /// <summary>This sample uses "1.3.9999.3.11" which is Falcon-512 OID, use "1.3.9999.3.14" for Falcon-1024.</summary>
        /// <returns>"1.3.9999.3.11" Falcon-512 signature algorithm OID</returns>
        public override String GetSignatureAlgoOid() {
            return "1.3.9999.3.11";
        }

        public override String GetDigestAlgo() {
            return DIGEST_ALGO;
        }

        public override int GetEstimatedSize() {
            return 5000;
        }
    }
}
