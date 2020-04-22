using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class ColoredBorder
    {
        public static readonly string DEST = "results/sandbox/tables/colored_border.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ColoredBorder().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            
            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            
            Cell cell = new Cell().Add(new Paragraph("Cell 1"));
            cell.SetBorderTop(new SolidBorder(ColorConstants.RED, 1));
            cell.SetBorderBottom(new SolidBorder(ColorConstants.BLUE, 1));
            table.AddCell(cell);

            cell = new Cell().Add(new Paragraph("Cell 2"));
            cell.SetBorderLeft(new SolidBorder(ColorConstants.GREEN, 5));
            cell.SetBorderTop(new SolidBorder(ColorConstants.YELLOW, 8));
            table.AddCell(cell);

            cell = new Cell().Add(new Paragraph("Cell 3"));
            cell.SetBorderLeft(new SolidBorder(ColorConstants.RED, 1));
            cell.SetBorderBottom(new SolidBorder(ColorConstants.BLUE, 1));
            table.AddCell(cell);

            cell = new Cell().Add(new Paragraph("Cell 4"));
            cell.SetBorderLeft(new SolidBorder(ColorConstants.GREEN, 5));
            cell.SetBorderTop(new SolidBorder(ColorConstants.YELLOW, 8));
            table.AddCell(cell);

            doc.Add(table);

            doc.Close();
        }
    }
}