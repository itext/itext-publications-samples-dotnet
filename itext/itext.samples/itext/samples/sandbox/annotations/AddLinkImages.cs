using System;
using System.IO;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Canvas.Wmf;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Annotations
{
    public class AddLinkImages
    {
        public static readonly String DEST = "results/sandbox/annotations/add_link_images.pdf";

        public static readonly String sourceFolder = "../../../resources/img/";
        public static readonly String BUTTERFLY = sourceFolder + "butterfly.wmf";
        public static readonly String DOG = sourceFolder + "dog.bmp";
        public static readonly String FOX = sourceFolder + "fox.bmp";
        public static readonly String INFO = sourceFolder + "info.png";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new AddLinkImages().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Paragraph p = new Paragraph("Objects with links");
            p.Add(CreateImage(INFO, "https://itextpdf.com/"));
            p.Add(CreateImage(DOG, "https://pages.itextpdf.com/ebook-stackoverflow-questions.html"));
            p.Add(CreateImage(FOX, "https://stackoverflow.com/q/29388313/1622493"));

            // Create PdfFormXObject object to add .wmf format image to the document,
            // because the creation of an ImageData instance from .wmf format image isn't supported.
            PdfFormXObject wmfImage = new PdfFormXObject(new WmfImageData(BUTTERFLY), pdfDoc);
            p.Add(new Image(wmfImage)
                .SetAction(PdfAction.CreateURI("https://stackoverflow.com/questions/tagged/itext*")));
            doc.Add(p);

            doc.Close();
        }

        public Image CreateImage(String src, String url)
        {
            Image img = new Image(ImageDataFactory.Create(src));

            // Create the url in the image by setting action property directly
            img.SetProperty(Property.ACTION, PdfAction.CreateURI(url));
            return img;
        }
    }
}