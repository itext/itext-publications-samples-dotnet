using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Objects
{
    public class RectangleInCell
    {
        public static readonly string DEST = "results/sandbox/objects/rectangle_in_cell.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new RectangleInCell().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PdfFormXObject formXObject = new PdfFormXObject(new Rectangle(120, 80));
            new PdfCanvas(formXObject, pdfDoc).SetFillColor(ColorConstants.RED)
                .Rectangle(0, 0, formXObject.GetWidth(), formXObject.GetHeight())
                .Fill();

            doc.Add(new Paragraph("Option 1:"));
            Table table = new Table(UnitValue.CreatePercentArray(3)).UseAllAvailableWidth();
            table.AddCell("A rectangle:");
            table.AddCell(new Cell().Add(new Image(formXObject).SetAutoScale(true)).SetPadding(10));
            table.AddCell("The rectangle is scaled to fit inside the cell, you see a padding.");
            doc.Add(table);

            doc.Add(new Paragraph("Option 2:"));
            table = new Table(UnitValue.CreatePercentArray(3)).UseAllAvailableWidth();
            table.AddCell("A rectangle:");
            Cell cell = new Cell().Add(new Image(formXObject));
            table.AddCell(cell);
            table.AddCell("The rectangle keeps its original size, but can overlap other cells in the same row.");
            doc.Add(table);

            doc.Add(new Paragraph("Option 3:"));
            table = new Table(UnitValue.CreatePercentArray(3)).UseAllAvailableWidth();
            table.AddCell("A rectangle:");
            cell = new Cell().Add(new Image(formXObject).SetAutoScale(true));
            table.AddCell(cell);
            table.AddCell("The rectangle is scaled to fit inside the cell, no padding.");
            doc.Add(table);

            doc.Close();
        }
    }
}