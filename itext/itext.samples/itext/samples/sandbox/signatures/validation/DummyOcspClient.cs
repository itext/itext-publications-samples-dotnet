using System;
using System.Collections.Generic;
using System.IO;
using iText.Bouncycastle.Asn1.Ocsp;
using iText.Commons.Bouncycastle.Asn1;
using iText.Commons.Bouncycastle.Asn1.Ocsp;
using iText.Commons.Bouncycastle.Asn1.X509;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Cert.Ocsp;
using iText.Signatures;

namespace iText.Samples.Sandbox.Signatures.Validation {
    public class DummyOcspClient : OcspClientBouncyCastle
    {
        public override IBasicOcspResponse GetBasicOCSPResp(IX509Certificate checkCert, IX509Certificate rootCert,
            string url)
        {
            return new DummyResponse();
        }

        class DummyResponse : IBasicOcspResponse
        {
            public IAsn1Object ToASN1Primitive()
            {
                return null;
            }

            public bool IsNull()
            {
                return false;
            }

            public DateTime GetProducedAtDate()
            {
                return DateTime.Now;
            }

            public bool Verify(IX509Certificate cert)
            {
                return false;
            }

            public IEnumerable<IX509Certificate> GetCerts()
            {
                return new List<IX509Certificate>();
            }

            public IX509Certificate[] GetOcspCerts()
            {
                return new IX509Certificate[0];
            }

            public byte[] GetEncoded()
            {
                return new byte[1];
            }

            public ISingleResponse[] GetResponses()
            {
                throw new NotImplementedException();
            }

            public DateTime GetProducedAt()
            {
                return DateTime.Now;
            }

            public IAsn1Encodable GetExtensionParsedValue(IDerObjectIdentifier getIdPkixOcspArchiveCutoff)
            {
                return null;
            }

            public IRespID GetResponderId()
            {
                return null;
            }

            public IAlgorithmIdentifier GetSignatureAlgorithmID()
            {
                return null;
            }
        }
    }
}