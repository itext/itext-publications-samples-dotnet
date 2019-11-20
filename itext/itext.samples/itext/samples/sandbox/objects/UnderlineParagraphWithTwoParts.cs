/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System.IO;
using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Objects
{
    public class UnderlineParagraphWithTwoParts
    {
        public static readonly string DEST = "results/sandbox/objects/underline_paragraph_with_two_parts.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new UnderlineParagraphWithTwoParts().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            pdfDoc.AddNewPage();

            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.COURIER, PdfEncodings.WINANSI, false);
            float charWidth = font.GetWidth(" ");
            int charactersPerLine = 101;
            float pageWidth = pdfDoc.GetPage(1).GetPageSize().GetWidth() - doc.GetLeftMargin() - doc.GetRightMargin();
            float fontSize = (1000 * (pageWidth / (charWidth * charactersPerLine)));
            fontSize = ((int) (fontSize * 100)) / 100f;

            string string2 = "0123456789";
            for (int i = 0; i < 5; i++)
            {
                string2 = string2 + string2;
            }

            AddParagraphWithTwoParts1(doc, font, "0123", string2, fontSize);
            doc.Add(new Paragraph("Test 1"));
            AddParagraphWithTwoParts2(doc, font, "0123", string2, fontSize);
            doc.Add(new Paragraph("Test 1"));
            doc.Add(new Paragraph("Test 1"));

            AddParagraphWithTwoParts1(doc, font, "012345", string2, fontSize);
            doc.Add(new Paragraph("Test 2"));
            AddParagraphWithTwoParts2(doc, font, "012345", string2, fontSize);
            doc.Add(new Paragraph("Test 2"));

            AddParagraphWithTwoParts1(doc, font, "0123456789012345", string2, fontSize);
            doc.Add(new Paragraph("Test 3"));
            doc.Add(new Paragraph("Test 3"));
            AddParagraphWithTwoParts2(doc, font, "0123456789012345", string2, fontSize);
            doc.Add(new Paragraph("Test 3"));
            doc.Add(new Paragraph("Test 3"));

            AddParagraphWithTwoParts1(doc, font, "012", "0123456789", fontSize);
            doc.Add(new Paragraph("Test 4"));
            AddParagraphWithTwoParts2(doc, font, "012", "0123456789", fontSize);
            doc.Add(new Paragraph("Test 4"));

            AddParagraphWithTwoParts1(doc, font, "012345", "01234567890123456789", fontSize);
            doc.Add(new Paragraph("Test 5"));
            AddParagraphWithTwoParts2(doc, font, "012345", "01234567890123456789", fontSize);
            doc.Add(new Paragraph("Test 5"));

            AddParagraphWithTwoParts1(doc, font, "0", "01234567890123456789012345678901234567890123456789", fontSize);
            doc.Add(new Paragraph("Test 6"));
            AddParagraphWithTwoParts2(doc, font, "0", "01234567890123456789012345678901234567890123456789", fontSize);

            doc.Close();
        }

        private static void AddParagraphWithTwoParts1(Document doc, PdfFont font, string string1, string string2, float fontSize)
        {
            if (string1 == null)
            {
                string1 = "";
            }

            if (string1.Length > 10)
            {
                string1 = string1.Substring(0, 10);
            }

            Text chunk1 = new Text(string1).SetFont(font).SetFontSize(fontSize);

            if (string2 == null)
            {
                string2 = "";
            }

            if (string1.Length + string2.Length > 100)
            {
                string2 = string2.Substring(0, 100 - string1.Length);
            }

            Text chunk2 = new Text(string2).SetFont(font).SetFontSize(fontSize);

            Paragraph p = new Paragraph();
            p.Add(chunk1);
            p.AddTabStops(new TabStop(1000, TabAlignment.RIGHT));
            p.Add(new Tab());
            p.Add(chunk2);
            doc.Add(p);
            doc.Add(new LineSeparator(new SolidLine(1)).SetMarginTop(-6));
        }

        private static void AddParagraphWithTwoParts2(Document doc, PdfFont font, string string1, string string2, float fontSize)
        {
            if (string1 == null)
            {
                string1 = "";
            }

            if (string1.Length > 10)
            {
                string1 = string1.Substring(0, 10);
            }

            if (string2 == null)
            {
                string2 = "";
            }

            if (string1.Length + string2.Length > 100)
            {
                string2 = string2.Substring(0, 100 - string1.Length);
            }

            Paragraph p = new Paragraph(string1 + " " + string2).SetFont(font).SetFontSize(fontSize);
            doc.Add(p);
            doc.Add(new LineSeparator(new SolidLine(1)).SetMarginTop(-6));
        }
    }
}