using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class SimpleTable8
    {
        public static readonly string DEST = "results/sandbox/tables/simple_table8.pdf";

        public static String SRC = "../../../resources/pdfs/";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new SimpleTable8().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PdfDocument srcDoc = new PdfDocument(new PdfReader(SRC + "header_test_file.pdf"));
            PdfFormXObject header = srcDoc.GetFirstPage().CopyAsFormXObject(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(3)).UseAllAvailableWidth();
            Cell cell = new Cell(1, 3).Add(new Image(header).SetWidth(UnitValue.CreatePercentValue(100))
                .SetAutoScale(true));
            table.AddCell(cell);

            for (int row = 1; row <= 50; row++)
            {
                for (int column = 1; column <= 3; column++)
                {
                    table.AddCell(String.Format("row {0}, column {1}", row, column));
                }
            }

            srcDoc = new PdfDocument(new PdfReader(SRC + "footer_test_file.pdf"));
           
            PdfFormXObject footer = srcDoc.GetFirstPage().CopyAsFormXObject(pdfDoc);

            cell = new Cell(1, 3).Add(new Image(footer).SetWidth(UnitValue.CreatePercentValue(100))
                .SetAutoScale(true));
            table.AddCell(cell);

            doc.Add(table);

            doc.Close();
        }
    }
}