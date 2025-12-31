using System;
using iText.Kernel.Crypto;

namespace iText.Signatures.Pqc {
    /// <summary>Example of document signing using ML-DSA-44 post-quantum algorithm.</summary>
    public class MLDSA : PqcSignatureExample {
        public const String DEST = "results/sandbox/signatures/pqc/signMLDSA.pdf";

        private const String SIGNATURE_ALGO = "ML-DSA-44";

        private const String CERT_PATH = "../../../resources/cert/pqc/cert_" + SIGNATURE_ALGO + ".pem";

        private const String DIGEST_ALGO = DigestAlgorithms.SHA3_256;

        public static void Main(String[] args) {
            new MLDSA().ManipulatePdf();
        }

        public override String GetDestination() {
            return DEST;
        }

        /// <summary>
        /// This certificate was generated via openssl:
        /// openssl req -x509 -newkey mldsa44 -keyout key_ML-DSA-44.pem -out cert_ML-DSA-44.pem
        /// -subj "/CN=iText Test ML-DSA-44 Certificate" -days 365
        /// </summary>
        /// <returns>ML-DSA-44 certificate</returns>
        public override String GetCertPath() {
            return CERT_PATH;
        }

        /// <summary>Possible ML-DSA algorithms values are ML-DSA-44, ML-DSA-65 and ML-DSA-87.</summary>
        /// <returns>ML-DSA-44 signature algorithm</returns>
        public override String GetSignatureAlgo() {
            return SIGNATURE_ALGO;
        }

        /// <summary>
        /// Possible ML-DSA algorithms OID values are
        /// <see cref="iText.Kernel.Crypto.OID.ML_DSA_44"/>
        /// ,
        /// <see cref="iText.Kernel.Crypto.OID.ML_DSA_65"/>
        /// and
        /// <see cref="iText.Kernel.Crypto.OID.ML_DSA_87"/>.
        /// </summary>
        /// <returns>
        /// 
        /// <see cref="iText.Kernel.Crypto.OID.ML_DSA_44"/>
        /// OID value.
        /// </returns>
        public override String GetSignatureAlgoOid() {
            return OID.ML_DSA_44;
        }

        /// <summary>
        /// ML-DSA-44 with DigestAlgorithms.SHA3_256,
        /// ML-DSA-65 with DigestAlgorithms.SHA3_384,
        /// ML-DSA-87 with DigestAlgorithms.SHA3_512.
        /// </summary>
        /// <returns>SHA3-256 digest algorithm</returns>
        public override String GetDigestAlgo() {
            return DIGEST_ALGO;
        }

        public override int GetEstimatedSize() {
            return 10000;
        }
    }
}
