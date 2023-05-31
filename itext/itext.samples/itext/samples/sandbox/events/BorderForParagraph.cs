using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Events
{
    public class BorderForParagraph
    {
        public static readonly String DEST = "results/sandbox/events/border_for_paragraph.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new BorderForParagraph().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            doc.Add(new Paragraph("Hello,"));
            doc.Add(new Paragraph("In this doc, we'll add several paragraphs that will trigger page events. " +
                                  "As long as the event isn't activated, nothing special happens, " +
                                  "but let's make the event active and see what happens:"));

            Paragraph paragraphWithBorder = new Paragraph("This paragraph now has a border. Isn't that fantastic? " +
                                                        "By changing the event, we can even provide a background color, " +
                                                        "change the line width of the border and many other things. Now let's deactivate the event.");

            // There were no method that allows you to create a border for a Paragraph, since iText5 is EOL.
            // In iText a border for a Paragraph can be created by calling setBorder() method.
            paragraphWithBorder.SetBorder(new SolidBorder(1));
            doc.Add(paragraphWithBorder);

            doc.Add(new Paragraph("This paragraph no longer has a border."));

            doc.Close();
        }
    }
}