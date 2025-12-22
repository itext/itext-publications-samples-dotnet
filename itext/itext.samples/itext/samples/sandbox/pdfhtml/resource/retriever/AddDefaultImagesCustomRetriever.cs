using System;
using System.IO;
using iText.Html2pdf;
using iText.StyledXmlParser.Resolver.Resource;

namespace iText.Samples.Sandbox.Pdfhtml.Resource.Retriever
{
   
    // AddDefaultImagesCustomRetriever.cs
    //
    // Example showing how to replace images with default ones during retrieval.
    // Demonstrates custom IResourceRetriever for conditional image substitution.
 
    public class AddDefaultImagesCustomRetriever
    {
        public static readonly string SRC = "../../../resources/pdfhtml/";
        public static readonly string DEST = "results/sandbox/pdfhtml/AddDefaultImagesCustomRetriever.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            string htmlSource = SRC + "AddDefaultImagesCustomRetriever/AddDefaultImagesCustomRetriever.html";

            new AddDefaultImagesCustomRetriever().ManipulatePdf(htmlSource, DEST);
        }

        protected void ManipulatePdf(String src, String dest)
        {
            IResourceRetriever retriever = new CustomResourceRetriever(SRC);
            ConverterProperties converterProperties = new ConverterProperties();
            converterProperties.SetResourceRetriever(retriever);

            HtmlConverter.ConvertToPdf(new FileInfo(src), new FileInfo(dest), converterProperties);
        }

        private class CustomResourceRetriever : IResourceRetriever
        {
            private String baseUri;

            public CustomResourceRetriever(String baseUri)
            {
                this.baseUri = baseUri;
            }

            public Stream GetInputStreamByUrl(Uri url)
            {
                // The image with name 'imageToReplace.png' will be replaced by the default image.
                if (url.ToString().Contains("imageToReplace.png"))
                {
                    url = new UriResolver(this.baseUri).ResolveAgainstBaseUri("images/defaultImage.png");
                }

                return new FileStream(url.LocalPath, FileMode.Open, FileAccess.Read);
            }

            public byte[] GetByteArrayByUrl(Uri url)
            {
                byte[] result = null;
                using (Stream stream = GetInputStreamByUrl(url))
                {
                    if (stream == null)
                    {
                        return null;
                    }

                    result = InputStreamToArray(stream);
                }

                return result;
            }

            private static byte[] InputStreamToArray(Stream stream)
            {
                byte[] b = new byte[8192];
                MemoryStream output = new MemoryStream();
                while (true) {
                    int read = stream.Read(b, 0, b.Length);
                    if (read < 1) {
                        break;
                    }
                    
                    output.Write(b, 0, read);
                }
                
                output.Dispose();
                return output.ToArray();
            }
        }
    }
}