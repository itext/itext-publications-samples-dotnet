using System;
using System.Collections.Generic;
using System.IO;
using iText.Bouncycastle.Cert;
using iText.Bouncycastle.X509;
using iText.Bouncycastle.Crypto;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Utils;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Signatures;
using Org.BouncyCastle.Pkcs;

namespace iText.Samples.Signatures.Chapter03
{
	public class C3_09_SignWithTSA
	{
		public static readonly string DEST = "results/signatures/chapter03/";
		
	    public static readonly string SRC = "../../../resources/pdfs/hello.pdf";
	    
	    public static readonly String[] RESULT_FILES =
	    {
		    "hello_cacert_ocsp_ts.pdf"
	    };

	    public static void Main(String[] args)
		{
			DirectoryInfo directory = new DirectoryInfo(DEST);
			directory.Create();
			
            Properties properties = new Properties();
            
            // Specify the correct path to the certificate
            properties.Load(new FileStream("c:/home/blowagie/key.properties", FileMode.Open, FileAccess.Read));
            String path = properties.GetProperty("PRIVATE");
            char[] pass = properties.GetProperty("PASSWORD").ToCharArray();
            String tsaUrl = properties.GetProperty("TSAURL");
            String tsaUser = properties.GetProperty("TSAUSERNAME");
            String tsaPass = properties.GetProperty("TSAPASSWORD");

            Pkcs12Store pk12 = new Pkcs12StoreBuilder().Build();
            pk12.Load(new FileStream(path, FileMode.Open, FileAccess.Read), pass);
            string alias = null;
            foreach (var a in pk12.Aliases)
            {
                alias = ((string)a);
                if (pk12.IsKeyEntry(alias))
                    break;
            }
            
            ICipherParameters pk = pk12.GetKey(alias).Key;
            X509CertificateEntry[] ce = pk12.GetCertificateChain(alias);
            X509Certificate[] chain = new X509Certificate[ce.Length];
            for (int k = 0; k < ce.Length; ++k)
            {
	            chain[k] = ce[k].Certificate;
            }

            IOcspClient ocspClient = new OcspClientBouncyCastle(null);
            
            /* Create an instance of TSAClientBouncyCastle, an implementation of TSAClient.
             * Pass the timestamp authority server url.
             * Note that not all TSA would require user credentials.
             */
			ITSAClient tsaClient = new TSAClientBouncyCastle(tsaUrl, tsaUser, tsaPass);

			new C3_09_SignWithTSA().Sign(SRC, DEST + RESULT_FILES[0], chain, pk, 
				DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CMS, 
				"Test", "Ghent", null, ocspClient, tsaClient, 0);
		}
	    
	    public void Sign(String src, String dest, X509Certificate[] chain, ICipherParameters pk,
		    String digestAlgorithm, PdfSigner.CryptoStandard subfilter, String reason, String location,
		    ICollection<ICrlClient> crlList, IOcspClient ocspClient, ITSAClient tsaClient, int estimatedSize)
	    {
		    PdfReader reader = new PdfReader(src);
		    PdfSigner signer = new PdfSigner(reader, new FileStream(dest, FileMode.Create), new StampingProperties());

		    // Create the signature appearance
		    Rectangle rect = new Rectangle(36, 648, 200, 100);
		    PdfSignatureAppearance appearance = signer.GetSignatureAppearance();
		    appearance
			    .SetReason(reason)
			    .SetLocation(location)

			    // Specify if the appearance before field is signed will be used
			    // as a background for the signed field. The "false" value is the default value.
			    .SetReuseAppearance(false)
			    .SetPageRect(rect)
			    .SetPageNumber(1);
		    signer.SetFieldName("sig");

		    IExternalSignature pks = new PrivateKeySignature(new PrivateKeyBC(pk), digestAlgorithm);

		    IX509Certificate[] certificateWrappers = new IX509Certificate[chain.Length];
		    for (int i = 0; i < certificateWrappers.Length; ++i) {
			    certificateWrappers[i] = new X509CertificateBC(chain[i]);
		    }
		    // Sign the document using the detached mode, CMS or CAdES equivalent.
		    // Pass the created TSAClient to the signing method.
		    signer.SignDetached(pks, certificateWrappers, crlList, ocspClient, tsaClient, estimatedSize, subfilter);
	    }
	}
}
