/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Annotations
{
    public class AddLinkAnnotation4
    {
        public static readonly String DEST = "results/sandbox/annotations/add_link_annotation4.pdf";

        public static readonly String SRC = "../../../resources/pdfs/hello.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new AddLinkAnnotation4().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

            Link link = new Link("The Best iText Questions on StackOverflow",
                PdfAction.CreateURI("https://pages.itextpdf.com/ebook-stackoverflow-questions.html"));
            link.SetFont(bold);
            Paragraph p = new Paragraph("Download ")
                .Add(link)
                .Add(" and discover more than 200 questions and answers.");

            // Rotate the paragraph on 90 degrees and add it to the document.
            doc.ShowTextAligned(p, 30, 100, 1, TextAlignment.LEFT,
                VerticalAlignment.TOP, (float) Math.PI / 2);

            doc.Close();
        }
    }
}