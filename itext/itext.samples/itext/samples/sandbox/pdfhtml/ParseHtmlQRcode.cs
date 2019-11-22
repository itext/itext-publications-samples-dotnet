/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System.IO;
using iText.Html2pdf;
using iText.Html2pdf.Attach.Impl;
using iText.Html2pdf.Css.Apply.Impl;
using iText.Samples.Pdfhtml;

namespace iText.Samples.Sandbox.Pdfhtml
{
    public class ParseHtmlQRcode
    {
        public static readonly string SRC = "../../resources/pdfhtml/qrcode/";
        public static readonly string DEST = "results/sandbox/pdfhtml/qrcode.pdf";

        public static void Main(string[] args)
        {
            string currentSrc = SRC + "qrcode.html";
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ParseHtmlQRcode().ManipulatePdf(currentSrc, DEST, SRC);
        }

        public void ManipulatePdf(string htmlSource, string pdfDest, string resourceLoc)
        {
            // Create custom tagworker factory
            // The tag <qr> is mapped on a QRCode tagworker. Every other tag is mapped to the default.
            // The tagworker processes a <qr> tag using iText Barcode functionality
            DefaultTagWorkerFactory tagWorkerFactory = new QRCodeTagWorkerFactory();

            // Creates custom css applier factory
            // The tag <qr> is mapped on a BlockCssApplier. Every other tag is mapped to the default.
            DefaultCssApplierFactory cssApplierFactory = new QRCodeTagCssApplierFactory();

            ConverterProperties converterProperties = new ConverterProperties()
                // Base URI is required to resolve the path to source files
                .SetBaseUri(resourceLoc)
                .SetTagWorkerFactory(tagWorkerFactory)
                .SetCssApplierFactory(cssApplierFactory);
            
            HtmlConverter.ConvertToPdf(
                new FileStream(htmlSource, FileMode.Open), 
                new FileStream(pdfDest, FileMode.Create, FileAccess.Write), 
                converterProperties);
        }
    }
}