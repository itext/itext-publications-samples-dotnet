using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Hyphenation;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class HyphenationExample
    {
        public static readonly string DEST = "results/sandbox/tables/hyphenation_example.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new HyphenationExample().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            // See hyphenation example of specified word in console
            // For the correct run of sample, please, add an itext.hyph dependency,
            // which could be found on the following web-page: https://mvnrepository.com/artifact/com.itextpdf/hyph
            Hyphenation s = Hyphenator.Hyphenate("de", "DE", "Leistungsscheinziffer", 2, 2);
            Console.Out.WriteLine(s);

            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            doc.SetMargins(0, 0, 0, 0);

            Table table = new Table(UnitValue.CreatePercentArray(1)).SetFixedLayout();
            table.SetWidth(UnitValue.CreatePercentValue(10));

            Text text = new Text("Leistungsscheinziffer");
            text.SetHyphenation(new HyphenationConfig("de", "DE", 2, 2));
            table.AddCell(new Cell().Add(new Paragraph(text)));

            Paragraph paragraph = new Paragraph();
            paragraph.SetHyphenation(new HyphenationConfig("de", "DE", 2, 2));
            paragraph.Add("Leistungsscheinziffer");
            table.AddCell(new Cell().Add(paragraph));

            // soft hyphens
            table.AddCell(new Cell().Add(new Paragraph("Le\u00adistun\u00ADgssch\u00ADeinziffe\u00ADr").SetHyphenation
                (new HyphenationConfig(3, 2))));

            doc.Add(table);

            doc.Close();
        }
    }
}