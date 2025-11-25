using System;
using System.IO;
using iText.Html2pdf;
using iText.StyledXmlParser.Resolver.Resource;

namespace iText.Samples.Sandbox.Pdfhtml.Resource.Retriever
{
   
    // FilterSizeByDefaultResourceRetriever.cs
    //
    // Example showing how to filter resources by size during HTML conversion.
    // Demonstrates setting resource size limit to exclude large files.
 
    public class FilterSizeByDefaultResourceRetriever
    {
        public static readonly string SRC = "../../../resources/pdfhtml/FilterSizeByDefaultResourceRetriever/";
        public static readonly string DEST = "results/sandbox/pdfhtml/FilterSizeByDefaultResourceRetriever.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            string htmlSource = SRC + "FilterSizeByDefaultResourceRetriever.html";

            new FilterSizeByDefaultResourceRetriever().ManipulatePdf(htmlSource, DEST);
        }

        protected void ManipulatePdf(String src, String dest)
        {
            // Specify that resources exceeding 100kb will be filtered out, i.e. data will not be extracted from them.
            IResourceRetriever retriever = new DefaultResourceRetriever().SetResourceSizeByteLimit(100_000);
            ConverterProperties converterProperties = new ConverterProperties();
            converterProperties.SetResourceRetriever(retriever);

            HtmlConverter.ConvertToPdf(new FileInfo(src), new FileInfo(dest), converterProperties);
        }
    }
}