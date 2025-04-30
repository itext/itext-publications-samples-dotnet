using System;
using System.IO;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Annotations
{
   
    // AddPointerAnnotation.cs
    // 
    // This class demonstrates how to create a PDF document with a map image and add
    // a pointer annotation to highlight a specific location. The code creates a new PDF
    // with an image of a map, then adds a line annotation configured as an arrow pointer
    // with custom styling (red color, specific border style). The annotation includes a
    // title and description that identify a specific location on the map. This example
    // shows how to create visual indicators on PDF content.
 
    public class AddPointerAnnotation
    {
        public static readonly String DEST = "results/sandbox/annotations/add_pointer_annotation.pdf";

        public static readonly String IMG = "../../../resources/img/map_cic.png";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new AddPointerAnnotation().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Image img = new Image(ImageDataFactory.Create(IMG));
            Document doc = new Document(pdfDoc, new PageSize(img.GetImageWidth(), img.GetImageHeight()));
            img.SetFixedPosition(0, 0);
            doc.Add(img);

            Rectangle rect = new Rectangle(220, 350, 255, 245);
            PdfLineAnnotation lineAnnotation = new PdfLineAnnotation(rect,
                new float[] {220 + 5, 350 + 5, 220 + 255 - 5, 350 + 245 - 5});
            lineAnnotation.SetTitle(new PdfString("You are here:"));

            // This method sets the text that will be displayed for the annotation or the alternate description,
            // if this type of annotation does not display text.
            lineAnnotation.SetContents("Cambridge Innovation Center");
            lineAnnotation.SetColor(ColorConstants.RED);

            // Set to print the annotation when the page is printed
            lineAnnotation.SetFlag(PdfAnnotation.PRINT);

            // Set arrow's border style
            PdfDictionary borderStyle = new PdfDictionary();
            borderStyle.Put(PdfName.S, PdfName.S);
            borderStyle.Put(PdfName.W, new PdfNumber(5));
            lineAnnotation.SetBorderStyle(borderStyle);

            PdfArray le = new PdfArray();
            le.Add(PdfName.OpenArrow);
            le.Add(PdfName.None);
            lineAnnotation.Put(PdfName.LE, le);
            lineAnnotation.Put(PdfName.IT, PdfName.LineArrow);

            pdfDoc.GetFirstPage().AddAnnotation(lineAnnotation);

            doc.Close();
        }
    }
}