using System.IO;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Objects
{
   
    // SubSuperScript.cs
    //
    // Example showing how to display subscript and superscript characters.
    // Demonstrates using Unicode subscript/superscript characters in text.
 
    public class SubSuperScript
    {
        public static readonly string DEST = "results/sandbox/objects/sub_super_script.pdf";
        public static readonly string FONT = "../../../resources/font/Cardo-Regular.ttf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new SubSuperScript().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H);
            
            // Subscript two and superscript four respectively
            Paragraph p = new Paragraph("H\u2082SO\u2074").SetFont(font).SetFontSize(10);
            doc.Add(p);

            doc.Close();
        }
    }
}