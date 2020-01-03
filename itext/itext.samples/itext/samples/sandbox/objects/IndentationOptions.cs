/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Objects
{
    public class IndentationOptions
    {
        public static readonly string CONTENT =
            "test A, test B, coconut, coconut, watermelons, apple, oranges, many more " +
            "fruites, carshow, monstertrucks thing, everything is startting on the same point in the line now";
        public static readonly string LABEL = "A list of stuff: ";
        public static readonly string DEST = "results/sandbox/objects/indentation_options.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new IndentationOptions().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            List list = new List()
                .SetListSymbol(LABEL)
                .Add(CONTENT);
            doc.Add(list);

            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            Paragraph paragraph = new Paragraph(LABEL + CONTENT).SetFont(font);
            float indentation = font.GetWidth(LABEL, 12);
            
            // Shift all lines except the first one to the width of the label
            paragraph.SetMarginLeft(indentation)
                .SetFirstLineIndent(-indentation);
            doc.Add(paragraph);

            // Add 4, because the default padding (left and right) of a cell equals 2
            Table table = new Table(new float[] {indentation + 4, 519 - indentation});
            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(LABEL)));
            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(CONTENT)));
            doc.Add(table);

            doc.Close();
        }
    }
}