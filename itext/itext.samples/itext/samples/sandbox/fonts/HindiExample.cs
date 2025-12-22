using System;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Fonts
{
   
    // HindiExample.cs
    //
    // Example showing Hindi (Devanagari) text rendering with Unicode.
    // Demonstrates character breakdown and colored text in tables.
 
    public class HindiExample
    {
        public static readonly String DEST = "results/sandbox/fonts/hindi_example.pdf";

        public static readonly String FONT = "../../../resources/font/FreeSans.ttf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new HindiExample().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            PdfFont f = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H);

            // "कार पार्किंग"
            Paragraph p1 = new Paragraph("\u0915\u093e\u0930 \u092a\u093e\u0930\u094d\u0915\u093f\u0902\u0917")
                .SetFont(f);
            doc.Add(p1);

            // \u0915 क \u093e ा \0930 र
            // \u092a प \u093e ा \u0930 र \u094d ्\u0915 क \u093f \u093f ि \u0902 ं\u0917 ग
            Paragraph p2 = new Paragraph("\\u0915 \u0915 \\u093e \u093e \\0930 \u0930\n"
                                         + "\\u092a \u092a \\u093e \u093e \\u0930 \u0930 \\u094d \u094d"
                                         + "\\u0915 \u0915 \\u093f \\u093f \u093f \\u0902 \u0902"
                                         + "\\u0917 \u0917");
            p2.SetFont(f);
            doc.Add(p2);

            Table table = new Table(UnitValue.CreatePercentArray(new float[] {10, 60, 30}))
                .UseAllAvailableWidth();
            Cell customerLblCell = new Cell().Add(new Paragraph("CUSTOMERS"));
            table.AddCell(customerLblCell);
            
            // "कारपार्किंग"
            p2 = new Paragraph("\u0915\u093e\u0930\u092a\u093e\u0930\u094d\u0915\u093f\u0902\u0917")
                .SetFont(f)
                .SetFontColor(new DeviceRgb(50, 205, 50));
            Cell balanceLblCell = new Cell().Add(p2);
            table.AddCell(balanceLblCell);
            
            table.SetMarginTop(10);
            doc.Add(table);

            doc.Close();
        }
    }
}