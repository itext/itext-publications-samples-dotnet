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
using iText.Kernel.Pdf;
using iText.Layout.Font;
using iText.Samples.Pdfhtml.Accessiblepdf.Headertagging;

namespace iText.Samples.Sandbox.Pdfhtml
{
    public class CreateAccessiblePDF
    {
        public static readonly string SRC = "../../resources/pdfhtml/AccessiblePDF/";
        public static readonly string DEST = "results/sandbox/pdfhtml/Accessibility.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            string htmlSource = SRC + "Accessibility.html";

            new CreateAccessiblePDF().ManipulatePdf(htmlSource, DEST);
        }

        public void ManipulatePdf(string src, string dest)
        {
            WriterProperties writerProperties = new WriterProperties();
            writerProperties.AddXmpMetadata();

            PdfWriter pdfWriter = new PdfWriter(dest, writerProperties);
            PdfDocument pdfDoc = new PdfDocument(pdfWriter);
            pdfDoc.GetCatalog().SetLang(new PdfString("en-US"));

            pdfDoc.SetTagged();
            pdfDoc.GetCatalog().SetViewerPreferences(new PdfViewerPreferences().SetDisplayDocTitle(true));

            PdfDocumentInfo pdfMetaData = pdfDoc.GetDocumentInfo();
            pdfMetaData.SetAuthor("Samuel Huylebroeck");
            pdfMetaData.AddCreationDate();
            pdfMetaData.GetProducer();
            pdfMetaData.SetCreator("iText Software");
            pdfMetaData.SetKeywords("example, accessibility");
            pdfMetaData.SetSubject("PDF accessibility");

            // Title is derived from html

            // pdf conversion
            ConverterProperties props = new ConverterProperties();
            FontProvider fontProvider = new FontProvider();
            fontProvider.AddStandardPdfFonts();
            fontProvider.AddDirectory(SRC);

            // The noto-nashk font file (.ttf extension) is placed in the resources
            props.SetFontProvider(fontProvider);
            // Base URI is required to resolve the path to source files
            props.SetBaseUri(SRC);

            // Setup custom tagworker factory for better tagging of headers
            DefaultTagWorkerFactory tagWorkerFactory = new AccessibilityTagWorkerFactory();
            props.SetTagWorkerFactory(tagWorkerFactory);
            
            HtmlConverter.ConvertToPdf(new FileStream(src, FileMode.Open), pdfDoc, props);

            pdfDoc.Close();
        }
    }
}