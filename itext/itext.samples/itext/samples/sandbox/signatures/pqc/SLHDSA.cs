using System;
using iText.Kernel.Crypto;

namespace iText.Signatures.Pqc {
    /// <summary>Example of document signing using SLH-DSA-SHA2-128F post-quantum algorithm.</summary>
    public class SLHDSA : PqcSignatureExample {
        public const String DEST = "results/sandbox/signatures/pqc/signSLHDSA.pdf";

        private const String SIGNATURE_ALGO = "slh-dsa-sha2-128f";

        private const String CERT_PATH = "../../../resources/cert/pqc/cert_" + SIGNATURE_ALGO + ".pem";

        private const String DIGEST_ALGO = DigestAlgorithms.SHA256;

        public static void Main(String[] args) {
            new SLHDSA().ManipulatePdf();
        }

        public override String GetDestination() {
            return DEST;
        }

        /// <summary>
        /// This certificate was generated via openssl:
        /// openssl req -x509 -newkey slh-dsa-sha2-128f -keyout key_slh-dsa-sha2-128f.pem -out cert_slh-dsa-sha2-128f.pem
        /// -subj "/CN=iText Test slh-dsa-sha2-128f Certificate" -days 365
        /// </summary>
        /// <returns>SLH-DSA-SHA2-128F certificate</returns>
        public override String GetCertPath() {
            return CERT_PATH;
        }

        /// <summary>
        /// Possible SLH-DSA algorithms values depending on the parameters are:
        /// "slh-dsa-sha2-128s", "slh-dsa-sha2-128f", "slh-dsa-shake-128s", "slh-dsa-shake-128f",
        /// "slh-dsa-sha2-192s", "slh-dsa-sha2-192f", "slh-dsa-shake-192s", "slh-dsa-shake-192f",
        /// "slh-dsa-sha2-256s", "slh-dsa-sha2-256f", "slh-dsa-shake-256s", "slh-dsa-shake-256f".
        /// </summary>
        /// <returns>SLH-DSA-SHA2-128F signature algorithm</returns>
        public override String GetSignatureAlgo() {
            return SIGNATURE_ALGO;
        }

        /// <summary>
        /// Possible SLH-DSA algorithms OID values are:
        /// <see cref="iText.Kernel.Crypto.OID.SLH_DSA_SHA2_128S"/>
        /// ,
        /// <see cref="iText.Kernel.Crypto.OID.SLH_DSA_SHA2_128F"/>
        /// ,
        /// <see cref="iText.Kernel.Crypto.OID.SLH_DSA_SHAKE_128S"/>
        /// ,
        /// <see cref="iText.Kernel.Crypto.OID.SLH_DSA_SHAKE_128F"/>
        /// ,
        /// <see cref="iText.Kernel.Crypto.OID.SLH_DSA_SHA2_192S"/>
        /// ,
        /// <see cref="iText.Kernel.Crypto.OID.SLH_DSA_SHA2_192F"/>
        /// ,
        /// <see cref="iText.Kernel.Crypto.OID.SLH_DSA_SHAKE_192S"/>
        /// ,
        /// <see cref="iText.Kernel.Crypto.OID.SLH_DSA_SHAKE_192F"/>
        /// ,
        /// <see cref="iText.Kernel.Crypto.OID.SLH_DSA_SHA2_256S"/>
        /// ,
        /// <see cref="iText.Kernel.Crypto.OID.SLH_DSA_SHA2_256F"/>
        /// ,
        /// <see cref="iText.Kernel.Crypto.OID.SLH_DSA_SHAKE_256S"/>
        /// ,
        /// <see cref="iText.Kernel.Crypto.OID.SLH_DSA_SHAKE_256F"/>
        /// </summary>
        /// <returns>
        /// 
        /// <see cref="iText.Kernel.Crypto.OID.SLH_DSA_SHA2_128F"/>
        /// OID value.
        /// </returns>
        public override String GetSignatureAlgoOid() {
            return OID.SLH_DSA_SHA2_128F;
        }

        public override String GetDigestAlgo() {
            return DIGEST_ALGO;
        }

        public override int GetEstimatedSize() {
            return 50000;
        }
    }
}
