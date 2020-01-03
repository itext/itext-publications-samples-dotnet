/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System.IO;
using iText.Html2pdf;

namespace iText.Samples.Sandbox.Pdfhtml
{
    public class ParseHtmlSimple
    {
        public static readonly string SRC = "../../resources/pdfhtml/rainbow/";
        public static readonly string DEST = "results/sandbox/pdfhtml/rainbow_simple.pdf";

        public static void Main(string[] args)
        {
            string currentSrc = SRC + "rainbow.html";
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ParseHtmlSimple().ManipulatePdf(currentSrc, DEST, SRC);
        }

        public void ManipulatePdf(string htmlSource, string pdfDest, string resourceLoc)
        {
            // Base URI is required to resolve the path to source files
            ConverterProperties converterProperties = new ConverterProperties().SetBaseUri(resourceLoc);
            
            HtmlConverter.ConvertToPdf(
                new FileStream(htmlSource, FileMode.Open, FileAccess.Read, FileShare.Read), 
                new FileStream(pdfDest, FileMode.Create, FileAccess.Write), 
                converterProperties);
        }
    }
}