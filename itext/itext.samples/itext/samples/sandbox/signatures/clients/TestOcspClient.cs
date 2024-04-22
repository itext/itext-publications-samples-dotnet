using System;
using System.Collections;
using System.Collections.Generic;
using iText.Bouncycastleconnector;
using iText.Commons.Bouncycastle;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Cert.Ocsp;
using iText.Commons.Bouncycastle.Crypto;
using iText.Commons.Utils;
using iText.Kernel.Pdf;
using iText.Samples.Sandbox.Signatures.Utils;
using iText.Signatures;

namespace iText.Samples.Sandbox.Signatures.Clients
{
    public class TestOcspClient : IOcspClient
    {
        private static readonly IBouncyCastleFactory BOUNCY_CASTLE_FACTORY = BouncyCastleFactoryCreator.GetFactory
            ();

        private readonly IDictionary<String, TestOcspResponseBuilder> certDNToResponseBuilder = new LinkedDictionary
            <String, TestOcspResponseBuilder>();

        public virtual TestOcspClient AddBuilderForCertificate(IX509Certificate cert, IPrivateKey privateKey)
        {
            certDNToResponseBuilder.Add(cert.GetSubjectDN().ToString(), new TestOcspResponseBuilder(cert, privateKey
            ));
            return this;
        }

        public virtual TestOcspClient AddBuilderForCertificate(IX509Certificate cert, TestOcspResponseBuilder builder)
        {
            certDNToResponseBuilder.Add(cert.GetSubjectDN().ToString(), builder);
            return this;
        }

        public virtual byte[] GetEncoded(IX509Certificate checkCert, IX509Certificate issuerCert, String url)
        {
            byte[] bytes = null;
            try
            {
                ICertID id = BOUNCY_CASTLE_FACTORY.CreateCertificateID(BOUNCY_CASTLE_FACTORY
                    .CreateCertificateID().GetHashSha1(), issuerCert, checkCert.GetSerialNumber());
                try
                {
                    TestOcspResponseBuilder builder = certDNToResponseBuilder[checkCert.GetSubjectDN().ToString()];
                    if (builder == null)
                    {
                        return null;
                    }

                    bytes = builder.MakeOcspResponse(GenerateOcspRequestWithNonce(id).GetEncoded());
                }
                catch (KeyNotFoundException)
                {
                    return null;
                }
            }
            catch (Exception ignored)
            {
                if (ignored != null)
                {
                    throw;
                }
            }

            return bytes;
        }

        private IOcspRequest GenerateOcspRequestWithNonce(ICertID id)
        {
            IOcspReqGenerator gen = BOUNCY_CASTLE_FACTORY.CreateOCSPReqBuilder();
            gen.AddRequest(id);

            // create details for nonce extension
            IDictionary extensions = new Hashtable();

            extensions[BOUNCY_CASTLE_FACTORY.CreateOCSPObjectIdentifiers().GetIdPkixOcspNonce()] =
                BOUNCY_CASTLE_FACTORY.CreateExtension(false,
                    BOUNCY_CASTLE_FACTORY.CreateDEROctetString(BOUNCY_CASTLE_FACTORY
                        .CreateDEROctetString(PdfEncryption.GenerateNewDocumentId()).GetEncoded()));

            gen.SetRequestExtensions(BOUNCY_CASTLE_FACTORY.CreateExtensions(extensions));
            return gen.Build();
        }
    }
}
