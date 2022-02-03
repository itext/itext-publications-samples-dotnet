using iText.Kernel.Pdf;
using iText.Signatures;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;
using System.Collections.Generic;
using System.IO;

namespace iText.SigningExamples.Simple
{
    public class PrivateKeySignatureContainer : IExternalSignatureContainer
    {
        public PrivateKeySignatureContainer(AsymmetricKeyParameter key, X509Certificate[] chain, string algorithm)
        {
            this.key = key;
            this.chain = chain;
            this.algorithm = algorithm;
        }

        public void ModifySigningDictionary(PdfDictionary signDic)
        {
            signDic.Put(PdfName.Filter, new PdfName("MKLx_GENERIC_SIGNER"));
            signDic.Put(PdfName.SubFilter, PdfName.Adbe_pkcs7_detached);
        }

        public byte[] Sign(Stream data)
        {
            CmsProcessable msg = new CmsProcessableInputStream(data);

            CmsSignedDataGenerator gen = new CmsSignedDataGenerator();
            SignerInfoGenerator signerInfoGenerator = new SignerInfoGeneratorBuilder()
                .WithSignedAttributeGenerator(new DefaultSignedAttributeTableGenerator())
                .Build(new Asn1SignatureFactory(algorithm, key), chain[0]);
            gen.AddSignerInfoGenerator(signerInfoGenerator);

            X509CollectionStoreParameters collectionStoreParameters = new X509CollectionStoreParameters(new List<X509Certificate>(chain));
            IX509Store collectionStore = X509StoreFactory.Create("CERTIFICATE/COLLECTION", collectionStoreParameters);
            gen.AddCertificates(collectionStore);

            CmsSignedData sigData = gen.Generate(msg, false);
            return sigData.GetEncoded();
        }

        AsymmetricKeyParameter key;
        X509Certificate[] chain;
        string algorithm;
    }
}
