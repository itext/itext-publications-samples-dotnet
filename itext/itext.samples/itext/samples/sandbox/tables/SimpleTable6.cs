using System;
using System.IO;
using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class SimpleTable6
    {
        public static readonly string DEST = "results/sandbox/tables/simple_table6.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new SimpleTable6().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            table.AddCell("0123456789");

            PdfFont font = PdfFontFactory.CreateFont(FontProgramFactory.CreateFont(StandardFonts.HELVETICA));
            table.AddCell(new Cell().Add(new Paragraph("0123456789").SetFont(font).SetLineThrough()));

            Text text1 = new Text("0123456789");
            text1.SetUnderline(1.5f, -1);
            table.AddCell(new Paragraph(text1));

            Text text2 = new Text("0123456789");
            text2.SetUnderline(1.5f, 3.5f);
            table.AddCell(new Paragraph(text2));

            doc.Add(table);

            doc.Close();
        }
    }
}