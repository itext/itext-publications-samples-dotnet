using System;
using iText.Kernel.Crypto;

namespace iText.Signatures.Pqc {
    /// <summary>Example of document signing using Picnic3-L1 post-quantum algorithm.</summary>
    public class Picnic : PqcSignatureExample {
        public const String DEST = "results/sandbox/signatures/pqc/signPicnic.pdf";

        private const String SIGNATURE_ALGO = "Picnic3-L1";

        private const String CERT_PATH = "../../../resources/cert/pqc/cert_" + SIGNATURE_ALGO + ".pem";

        private const String DIGEST_ALGO = DigestAlgorithms.SHAKE256;

        public static void Main(String[] args) {
            new Picnic().ManipulatePdf();
        }

        public override String GetDestination() {
            return DEST;
        }

        /// <summary>This certificate was generated using bouncycastle.</summary>
        /// <returns>Picnic3-L1 certificate</returns>
        public override String GetCertPath() {
            return CERT_PATH;
        }

        /// <summary>
        /// For Picnic possible parameters are:
        /// "Picnic3-L1", "Picnic3-L3", "Picnic3-L5",
        /// "Picnic-L1-FS", "Picnic-L1-Full", "Picnic-L1-UR",
        /// "Picnic-L3-FS", "Picnic-L3-Full", "Picnic-L3-UR",
        /// "Picnic-L5-FS", "Picnic-L5-Full", "Picnic-L5-UR".
        /// </summary>
        /// <remarks>
        /// For Picnic possible parameters are:
        /// "Picnic3-L1", "Picnic3-L3", "Picnic3-L5",
        /// "Picnic-L1-FS", "Picnic-L1-Full", "Picnic-L1-UR",
        /// "Picnic-L3-FS", "Picnic-L3-Full", "Picnic-L3-UR",
        /// "Picnic-L5-FS", "Picnic-L5-Full", "Picnic-L5-UR".
        /// These can be used for certificate generation. For all of them signature algorithm will be "Picnic".
        /// </remarks>
        /// <returns>"Picnic" signature algorithm.</returns>
        public override String GetSignatureAlgo() {
            return "Picnic";
        }

        public override String GetSignatureAlgoOid() {
            return "1.3.6.1.4.1.22554.2.6.2.2";
        }

        public override String GetDigestAlgo() {
            return DIGEST_ALGO;
        }

        public override int GetEstimatedSize() {
            return 30000;
        }
    }
}
