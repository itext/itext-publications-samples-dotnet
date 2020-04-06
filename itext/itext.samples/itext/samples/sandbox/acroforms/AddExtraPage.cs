/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Forms;
using iText.IO.Font.Constants;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Acroforms
{
    public class AddExtraPage
    {
        public static readonly String DEST = "results/sandbox/acroforms/add_extra_page.pdf";

        public static readonly String SRC = "../../../resources/pdfs/stationery.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new AddExtraPage().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument srcDoc = new PdfDocument(new PdfReader(SRC));
            PdfAcroForm form = PdfAcroForm.GetAcroForm(srcDoc, false);

            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));

            // Event handler copies content of the source pdf file on every page
            // of the result pdf file
            pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE,
                new PaginationEventHandler(srcDoc.GetFirstPage().CopyAsFormXObject(pdfDoc)));
            srcDoc.Close();

            Document doc = new Document(pdfDoc);
            Rectangle rect = form.GetField("body").GetWidgets()[0].GetRectangle().ToRectangle();

            // The renderer will place content in columns specified with the rectangles
            doc.SetRenderer(new ColumnDocumentRenderer(doc, new Rectangle[] {rect}));

            Paragraph p = new Paragraph();

            // The easiest way to add a Text object to Paragraph
            p.Add("Hello ");

            // Use add(Text) if you want to specify some Text characteristics, for example, font size
            p.Add(new Text("World").SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD)));

            for (int i = 1; i < 101; i++)
            {
                doc.Add(new Paragraph("Hello " + i));
                doc.Add(p);
            }

            doc.Close();
        }

        protected class PaginationEventHandler : IEventHandler
        {
            protected PdfFormXObject background;

            public PaginationEventHandler(PdfFormXObject background)
            {
                this.background = background;
            }

            public void HandleEvent(Event currentEvent)
            {
                PdfDocument pdfDoc = ((PdfDocumentEvent) currentEvent).GetDocument();
                PdfPage currentPage = ((PdfDocumentEvent) currentEvent).GetPage();

                // Add the background
                new PdfCanvas(currentPage.NewContentStreamBefore(), currentPage.GetResources(), pdfDoc)
                    .AddXObject(background, 0, 0);
            }
        }
    }
}