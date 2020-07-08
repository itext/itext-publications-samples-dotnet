using System;
using System.Collections.Generic;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Font.Constants;
using iText.IO.Util;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Acroforms.Reporting
{
    public class FillFlattenMerge3
    {
        public static readonly String DEST = "results/sandbox/acroforms/reporting/fill_flatten_merge3.pdf";

        public static readonly String DATA = "../../../resources/data/united_states.csv";
        public static readonly String SRC = "../../../resources/pdfs/state.pdf";

        public static readonly String[] FIELDS =
        {
            "name", "abbr", "capital", "city", "population", "surface", "timezone1", "timezone2", "dst"
        };

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new FillFlattenMerge3().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument srcDoc = new PdfDocument(new PdfReader(SRC));
            PdfAcroForm form = PdfAcroForm.GetAcroForm(srcDoc, true);

            // Create a map with fields from the acroform and their names
            Dictionary<String, Rectangle> positions = new Dictionary<String, Rectangle>();
            IDictionary<String, PdfFormField> fields = form.GetFormFields();
            foreach (PdfFormField field in fields.Values)
            {
                positions.Add(field.GetFieldName().GetValue(), field.GetWidgets()[0].GetRectangle().ToRectangle());
            }

            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            // Event handler copies content of the source pdf file on every page
            // of the result pdf file as template to fill in.
            pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE,
                new PaginationEventHandler(srcDoc.GetFirstPage().CopyAsFormXObject(pdfDoc)));
            srcDoc.Close();

            using (StreamReader streamReader = new StreamReader(DATA))
            {
                // Read first line with headers,
                // do nothing with current text line, because headers are already filled in form
                String line = streamReader.ReadLine();

                while ((line = streamReader.ReadLine()) != null)
                {
                    int i = 0;
                    StringTokenizer tokenizer = new StringTokenizer(line, ";");

                    pdfDoc.AddNewPage();

                    while (tokenizer.HasMoreTokens())
                    {
                        // Fill in current form field, got by the name from FIELDS[],
                        // with content, read from the current token
                        Process(doc, FIELDS[i++], tokenizer.NextToken(), font, positions);
                    }
                }
            }

            doc.Close();
        }

        protected void Process(Document doc, String name, String value, PdfFont font,
            Dictionary<String, Rectangle> positions)
        {
            Rectangle rect = positions[name];
            Paragraph p = new Paragraph(value).SetFont(font).SetFontSize(10);

            doc.ShowTextAligned(p, rect.GetLeft() + 2, rect.GetBottom() + 2, doc.GetPdfDocument().GetNumberOfPages(),
                TextAlignment.LEFT, VerticalAlignment.BOTTOM, 0);
        }

        protected class PaginationEventHandler : IEventHandler
        {
            PdfFormXObject background;

            public PaginationEventHandler(PdfFormXObject background)
            {
                this.background = background;
            }

            public void HandleEvent(Event currentEvent)
            {
                PdfDocument pdfDoc = ((PdfDocumentEvent) currentEvent).GetDocument();
                int pageNum = pdfDoc.GetPageNumber(((PdfDocumentEvent) currentEvent).GetPage());

                // Add the background
                PdfCanvas canvas = new PdfCanvas(pdfDoc.GetPage(pageNum).NewContentStreamBefore(),
                        ((PdfDocumentEvent) currentEvent).GetPage().GetResources(), pdfDoc)
                    .AddXObject(background, 0, 0);

                // Add the page number
                new Canvas(canvas, ((PdfDocumentEvent) currentEvent).GetPage().GetPageSize())
                    .ShowTextAligned("page " + pageNum, 550, 800, TextAlignment.RIGHT);
            }
        }
    }
}