using System;
using System.IO;
using System.Linq;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Images
{
   
    // FlateCompressJPEG2Passes.cs
    //
    // Example showing how to add Flate compression to existing JPEG images.
    // Demonstrates post-processing a PDF to apply dual-filter compression.
 
    public class FlateCompressJPEG2Passes
    {
        public static readonly String DEST = "results/sandbox/images/flate_compress_jpeg_2passes.pdf";

        public static readonly String SRC = "../../../resources/pdfs/image.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new FlateCompressJPEG2Passes().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfReader reader = new PdfReader(SRC);
            PdfDocument pdfDoc = new PdfDocument(reader, new PdfWriter(dest));

            // Assume that there is a single XObject in the source document
            // and this single object is an image.
            PdfDictionary pageDict = pdfDoc.GetFirstPage().GetPdfObject();
            PdfDictionary pageResources = pageDict.GetAsDictionary(PdfName.Resources);
            PdfDictionary pageXObjects = pageResources.GetAsDictionary(PdfName.XObject);
            PdfName imgName = pageXObjects.KeySet().First();
            PdfStream imgStream = pageXObjects.GetAsStream(imgName);
            imgStream.SetData(reader.ReadStreamBytesRaw(imgStream));

            PdfArray array = new PdfArray();
            array.Add(PdfName.FlateDecode);
            array.Add(PdfName.DCTDecode);
            imgStream.Put(PdfName.Filter, array);

            pdfDoc.Close();
        }
    }
}