using System.IO;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Objects
{
   
    // ColumnTextChunkImage.cs
    //
    // Example showing how to inline images within text paragraphs.
    // Demonstrates mixing text and images seamlessly in a single line.
 
    public class ColumnTextChunkImage
    {
        public static readonly string DOG = "../../../resources/img/dog.bmp";
        public static readonly string FOX = "../../../resources/img/fox.bmp";
        public static readonly string DEST = "results/sandbox/objects/column_text_chunk_image.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new ColumnTextChunkImage().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PdfImageXObject dog = new PdfImageXObject(ImageDataFactory.Create(DOG));
            PdfImageXObject fox = new PdfImageXObject(ImageDataFactory.Create(FOX));
            Paragraph p = new Paragraph("quick brown fox jumps over the lazy dog.")
                .Add("Or, to say it in a more colorful way: quick brown ")
                .Add(new Image(fox))
                .Add(" jumps over the lazy ")
                .Add(new Image(dog))
                .Add(".")
                
                // The Leading is a spacing between lines of text
                .SetMultipliedLeading(1);
            doc.Add(p);

            doc.Close();
        }
    }
}