/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Objects
{
    public class LeftRight
    {
        public static readonly string DEST = "results/sandbox/objects/left_right.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new LeftRight().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Paragraph p = new Paragraph("Text to the left");
            p.Add(new Tab());
            p.AddTabStops(new TabStop(1000, TabAlignment.RIGHT));
            p.Add("Text to the right");
            doc.Add(p);

            Table table = new Table(UnitValue.CreatePercentArray(3)).UseAllAvailableWidth();
            table.AddCell(GetCell("Text to the left", TextAlignment.LEFT));
            table.AddCell(GetCell("Text in the middle", TextAlignment.CENTER));
            table.AddCell(GetCell("Text to the right", TextAlignment.RIGHT));
            doc.Add(table);

            doc.Close();
        }

        private static Cell GetCell(string text, TextAlignment alignment)
        {
            Cell cell = new Cell().Add(new Paragraph(text));
            cell.SetPadding(0);
            cell.SetTextAlignment(alignment);
            cell.SetBorder(Border.NO_BORDER);
            return cell;
        }
    }
}