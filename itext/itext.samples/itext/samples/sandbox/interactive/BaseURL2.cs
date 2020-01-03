/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Interactive
{
    public class BaseURL2
    {
        public static readonly String DEST = "results/sandbox/interactive/base_url2.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new BaseURL2().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PdfDictionary uri = new PdfDictionary();
            uri.Put(PdfName.Type, PdfName.URI);
            uri.Put(new PdfName("Base"), new PdfString("http://itextpdf.com/"));
            pdfDoc.GetCatalog().Put(PdfName.URI, uri);

            PdfAction action = PdfAction.CreateURI("index.php");
            Link link = new Link("Home page", action);
            doc.Add(new Paragraph(link));

            pdfDoc.Close();
        }
    }
}