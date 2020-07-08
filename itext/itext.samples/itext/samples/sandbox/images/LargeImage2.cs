using System;
using System.IO;
using System.Linq;
using iText.IO.Source;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Images
{
    public class LargeImage2
    {
        public static readonly String DEST = "results/sandbox/images/large_image2.pdf";

        public static readonly String SRC = "../../../resources/pdfs/large_image.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new LargeImage2().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument resultDoc = new PdfDocument(new PdfWriter(dest));
            MemoryStream tempFile = new ByteArrayOutputStream();
            
            // The source pdf document's page size is expected to be huge: more than 14400 in width in height
            PdfDocument tempDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(tempFile));

            // Assume that there is a single XObject in the source document
            // and this single object is an image.
            PdfDictionary pageDict = tempDoc.GetFirstPage().GetPdfObject();
            PdfDictionary pageResources = pageDict.GetAsDictionary(PdfName.Resources);
            PdfDictionary pageXObjects = pageResources.GetAsDictionary(PdfName.XObject);
            PdfName imgRef = pageXObjects.KeySet().First();
            PdfStream imgStream = pageXObjects.GetAsStream(imgRef);
            PdfImageXObject imgObject = new PdfImageXObject(imgStream);
            Image img = new Image(imgObject);
            img.ScaleToFit(14400, 14400);
            img.SetFixedPosition(0, 0);

            tempDoc.AddNewPage(1, new PageSize(img.GetImageScaledWidth(), img.GetImageScaledHeight()));
            PdfPage page = tempDoc.GetFirstPage();
            new Canvas(page, page.GetPageSize())
                .Add(img)
                .Close();
            tempDoc.Close();
            
            PdfDocument docToCopy = new PdfDocument(new PdfReader(new MemoryStream(tempFile.ToArray())));
            docToCopy.CopyPagesTo(1, 1, resultDoc);

            docToCopy.Close();
            resultDoc.Close();
        }
    }
}