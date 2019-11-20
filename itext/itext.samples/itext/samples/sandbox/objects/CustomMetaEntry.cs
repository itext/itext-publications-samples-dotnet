/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Objects
{
    public class CustomMetaEntry
    {
        public static readonly string DEST = "results/sandbox/objects/custom_meta_entry.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new CustomMetaEntry().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            pdfDoc.GetDocumentInfo().SetTitle("Some example");
            
            // Add metadata to pdf document
            pdfDoc.GetDocumentInfo().SetMoreInfo("MetadataName", "metadataValue");

            Paragraph p = new Paragraph("Hello World");
            doc.Add(p);

            doc.Close();
        }
    }
}