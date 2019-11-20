/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System.IO;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Objects
{
    public class TableSpace
    {
        public static readonly string DEST = "results/sandbox/objects/table_space.pdf";
        public static readonly string FONT = "../../resources/font/PTM55FT.ttf";
        public static readonly string[][] DATA =
        {
            new string[] {"John Edward Jr.", "AAA"},
            new string[] {"Pascal Einstein W. Alfi", "BBB"},
            new string[] {"St. John", "CCC"}
        };

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new TableSpace().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.CP1250, true);

            doc.Add(CreateParagraphWithSpaces(font, string.Format("{0}: {1}", "Name", DATA[0][0]), DATA[0][1]));
            doc.Add(CreateParagraphWithSpaces(font, string.Format("{0}: {1}", "Surname", DATA[1][0]), DATA[1][1]));
            doc.Add(CreateParagraphWithSpaces(font, string.Format("{0}: {1}", "School", DATA[2][0]), DATA[2][1]));

            doc.Close();
        }

        private static Paragraph CreateParagraphWithSpaces(PdfFont font, string value1, string value2)
        {
            Paragraph p = new Paragraph();
            p.SetFont(font);
            p.Add(string.Format("{0, -35}", value1));
            p.Add(value2);
            return p;
        }
    }
}