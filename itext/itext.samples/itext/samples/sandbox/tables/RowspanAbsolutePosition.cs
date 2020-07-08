using System;
using System.IO;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class RowspanAbsolutePosition
    {
        public static readonly string DEST = "results/sandbox/tables/rowspan_absolute_position.pdf";

        public static readonly String IMG = "../../../resources/img/berlin2013.jpg";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new RowspanAbsolutePosition().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table1 = new Table(new float[] {150, 200, 200});

            Cell cell = new Cell(1, 2).Add(new Paragraph("{Month}"));
            cell.SetHorizontalAlignment(HorizontalAlignment.LEFT);

            Image img = new Image(ImageDataFactory.Create(IMG));
            img.SetWidth(UnitValue.CreatePercentValue(100));
            img.SetAutoScale(true);

            Cell cell2 = new Cell(2, 1).Add(img);
            Cell cell3 = new Cell(1, 2).Add(new Paragraph("Mr Fname Lname"));
            cell3.SetHorizontalAlignment(HorizontalAlignment.LEFT);

            table1.AddCell(cell);
            table1.AddCell(cell2);
            table1.AddCell(cell3);

            doc.Add(table1);

            doc.Close();
        }
    }
}