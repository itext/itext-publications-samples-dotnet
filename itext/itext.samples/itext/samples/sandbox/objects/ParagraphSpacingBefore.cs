using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Objects
{
   
    // ParagraphSpacingBefore.cs
    //
    // Example showing how to add vertical spacing before a paragraph.
    // Demonstrates using margin-top to control spacing between paragraphs.
 
    public class ParagraphSpacingBefore
    {
        public static readonly string DEST = "results/sandbox/objects/paragraph_spacing_before.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new ParagraphSpacingBefore().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Paragraph paragraph1 = new Paragraph("First paragraph");
            doc.Add(paragraph1);

            Paragraph paragraph2 = new Paragraph("Second paragraph");
            paragraph2.SetMarginTop(380f);
            doc.Add(paragraph2);

            doc.Close();
        }
    }
}