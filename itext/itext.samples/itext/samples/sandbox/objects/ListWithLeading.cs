using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Objects
{
    public class ListWithLeading
    {
        public static readonly string DEST = "results/sandbox/objects/list_with_leading.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new ListWithLeading().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            List list1 = new List()
                .SetSymbolIndent(12)
                
                // Bullet •
                .SetListSymbol("\u2022")
                .SetFont(font);
            list1.Add(new ListItem("Value 1")).Add(new ListItem("Value 2")).Add(new ListItem("Value 3"));
            doc.Add(list1);

            List list2 = new List()
                .SetSymbolIndent(12)
                
                // Bullet •
                .SetListSymbol("\u2022");
            
            // The Leading is a spacing between lines of text
            list2.Add((ListItem) new ListItem().Add(new Paragraph("Value 1").SetFixedLeading(30).SetMargins(0, 0, 0, 0)))
                .Add((ListItem) new ListItem().Add(new Paragraph("Value 2").SetFixedLeading(30).SetMargins(0, 0, 0, 0)))
                .Add((ListItem) new ListItem().Add(new Paragraph("Value 3").SetFixedLeading(30).SetMargins(0, 0, 0, 0)));
            list2.SetMarginLeft(60);
            doc.Add(list2);

            doc.Close();
        }
    }
}