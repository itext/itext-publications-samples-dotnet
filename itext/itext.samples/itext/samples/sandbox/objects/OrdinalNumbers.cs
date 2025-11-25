using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Objects
{
   
    // OrdinalNumbers.cs
    //
    // Example showing how to create superscript ordinal number suffixes.
    // Demonstrates using text rise for proper ordinal indicator positioning.
 
    public class OrdinalNumbers
    {
        public static readonly string DEST = "results/sandbox/objects/ordinal_numbers.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new OrdinalNumbers().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            Text st = new Text("st").SetFont(font).SetFontSize(6);
            st.SetTextRise(7);
            Text nd = new Text("nd").SetFont(font);
            nd.SetTextRise(7);
            Text rd = new Text("rd").SetFont(font);
            rd.SetTextRise(7);
            Text th = new Text("th").SetFont(font);
            th.SetTextRise(7);

            Paragraph first = new Paragraph();
            first.Add("The 1");
            first.Add(st);
            first.Add(" of May");
            doc.Add(first);

            Paragraph second = new Paragraph();
            second.Add("The 2");
            second.Add(nd);
            second.Add(" and the 3");
            second.Add(rd);
            second.Add(" of June");
            doc.Add(second);

            Paragraph third = new Paragraph();
            third.Add("The 4");
            third.Add(th);
            third.Add(" of July");
            doc.Add(third);

            doc.Close();
        }
    }
}