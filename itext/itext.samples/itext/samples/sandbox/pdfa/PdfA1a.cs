/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.IO.Font;
using iText.IO.Util;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Tagging;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Pdfa;

namespace iText.Samples.Sandbox.Pdfa
{
    public class PdfA1a
    {
        public static readonly string DEST = "results/sandbox//pdfa/pdf_a_1a.pdf";

        public static readonly String BOLD = "../../resources/font/OpenSans-Bold.ttf";

        public static readonly String DATA = "../../resources/data/united_states.csv";

        public static readonly String FONT = "../../resources/font/OpenSans-Regular.ttf";

        private PdfFormXObject template;

        private Image total;

        private PdfFont font;

        private PdfFont bold;

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new PdfA1a().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            font = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H);
            bold = PdfFontFactory.CreateFont(BOLD, PdfEncodings.IDENTITY_H);

            FileStream fileStream =
                new FileStream("../../resources/data/sRGB_CS_profile.icm", FileMode.Open, FileAccess.Read);

            PdfADocument pdfDoc = new PdfADocument(new PdfWriter(dest), PdfAConformanceLevel.PDF_A_1A,
                new PdfOutputIntent("Custom", "",
                    null, "sRGB IEC61966-2.1", fileStream));

            Document document = new Document(pdfDoc, PageSize.A4.Rotate());
            pdfDoc
                .SetTagged()
                .GetCatalog()
                .SetLang(new PdfString("en-us"));

            template = new PdfFormXObject(new Rectangle(795, 575, 30, 30));
            PdfCanvas canvas = new PdfCanvas(template, pdfDoc);

            total = new Image(template);
            total.GetAccessibilityProperties().SetRole(StandardRoles.ARTIFACT);

            // Creates a header for every page in the document
            pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE, new HeaderHandler(this));

            PdfDictionary parameters = new PdfDictionary();
            parameters.Put(PdfName.ModDate, new PdfDate().GetPdfObject());

            Table table = new Table(UnitValue.CreatePercentArray(
                new float[] {4, 1, 3, 4, 3, 3, 3, 3, 1})).UseAllAvailableWidth();

            StreamReader br = new StreamReader(DATA);

            // Reads content of csv file
            String line = br.ReadLine();
            Process(table, line, bold, 10, true);

            while ((line = br.ReadLine()) != null)
            {
                Process(table, line, font, 10, false);
            }

            br.Close();

            document.Add(table);

            canvas.BeginText();
            canvas.SetFontAndSize(bold, 12);
            canvas.MoveText(795, 575);
            canvas.ShowText(pdfDoc.GetNumberOfPages().ToString());
            canvas.EndText();
            canvas.Stroke();

            document.Close();
        }

        private void Process(Table table, String line, PdfFont currentFont, int fontSize, bool isHeader)
        {
            // Parses csv string line with specified delimiter
            StringTokenizer tokenizer = new StringTokenizer(line, ";");

            while (tokenizer.HasMoreTokens())
            {
                Paragraph content = new Paragraph(tokenizer.NextToken()).SetFont(currentFont).SetFontSize(fontSize);

                if (isHeader)
                {
                    table.AddHeaderCell(content);
                }
                else
                {
                    table.AddCell(content);
                }
            }
        }

        private class HeaderHandler : IEventHandler
        {
            private readonly PdfA1a enclosing;

            public HeaderHandler(PdfA1a enclosing)
            {
                this.enclosing = enclosing;
            }

            public virtual void HandleEvent(Event currentEvent)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent) currentEvent;
                PdfPage page = docEvent.GetPage();
                int pageNum = docEvent.GetDocument().GetPageNumber(page);

                PdfCanvas canvas = new PdfCanvas(page);

                // Creates header text content
                canvas.BeginText();
                canvas.SetFontAndSize(enclosing.font, 12);
                canvas.BeginMarkedContent(PdfName.Artifact);
                canvas.MoveText(34, 575);
                canvas.ShowText("Test");
                canvas.MoveText(703, 0);
                canvas.ShowText(String.Format("Page {0:d} of", pageNum));
                canvas.EndText();
                canvas.Stroke();
                canvas.AddXObject(enclosing.template, 0, 0);
                canvas.EndMarkedContent();
                canvas.Release();
            }
        }
    }
}