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
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Annotations
{
    public class RemoteGoto
    {
        public static readonly String DEST = "results/sandbox/annotations/";

        public static readonly String[] DEST_NAMES =
        {
            "remote_goto.pdf",
            "subdir/xyz.pdf"
        };

        public static void Main(String[] args)
        {
            DirectoryInfo directory = new DirectoryInfo(DEST + "subdir/");
            directory.Create();

            new RemoteGoto().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            CreateLinkPdf(dest + DEST_NAMES[0]);
            CreateDestinationPdf(dest + DEST_NAMES[1]);
        }

        // This method creates a link destination pdf file.
        private void CreateDestinationPdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Paragraph anchor = new Paragraph("This is a destination");

            // Set string destination, to which the created in the another pdf file link will lead.
            anchor.SetProperty(Property.DESTINATION, "dest");
            doc.Add(anchor);

            doc.Close();
        }

        // This method creates a pdf file, which will contain a link
        // to the page with set string destination of another pdf file.
        private static void CreateLinkPdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            // Create a link action, which leads to the another pdf file's page.
            // The 1st argument is the relative destination pdf file's path;
            // the 2nd argument is the string destination in the destination pdf file,
            // to which the link will lead after a click on it.
            PdfAction action = PdfAction.CreateGoToR(DEST_NAMES[1], "dest");
            Paragraph chunk = new Paragraph(new Link("Link", action));
            doc.Add(chunk);

            doc.Close();
        }
    }
}