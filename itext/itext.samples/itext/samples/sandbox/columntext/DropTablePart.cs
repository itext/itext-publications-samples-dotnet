using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Columntext
{
    public class DropTablePart
    {
        public static readonly String DEST = "results/sandbox/columntext/drop_table_part.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new DropTablePart().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            for (int i = 0; i < 4;)
            {
                Table table = new Table(UnitValue.CreatePercentArray(new float[] {25, 25, 25, 25}))
                    .UseAllAvailableWidth();

                Cell cell = new Cell(1, 4).Add(new Paragraph("inner table " + (++i)));
                table.AddCell(cell);
                for (int j = 0; j < 18; j++)
                {
                    table.AddCell("test Data " + (j + 1) + ".1");
                    table.AddCell("test Data " + (j + 1) + ".1");
                    table.AddCell("test Data " + (j + 1) + ".1");
                    table.AddCell("test Data " + (j + 1) + ".1");
                }

                doc.Add(table);
            }

            doc.Close();
        }
    }
}