using System;
using System.IO;
using iText.Kernel;
using iText.Kernel.Exceptions;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Parse
{
   
    // ExtractStreams.cs
    //
    // Example showing how to extract all stream objects from a PDF document.
    // Demonstrates iterating through PDF objects and saving stream data to files.
 
    public class ExtractStreams
    {
        public static readonly String DEST = "results/sandbox/parse";

        public static readonly String SRC = "../../../resources/pdfs/image.pdf";

        public static void Main(String[] args)
        {
            Directory.CreateDirectory(DEST);

            new ExtractStreams().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC));

            int numberOfPdfObject = pdfDoc.GetNumberOfPdfObjects();
            for (int i = 1; i <= numberOfPdfObject; i++)
            {
                PdfObject obj = pdfDoc.GetPdfObject(i);
                if (obj != null && obj.IsStream())
                {
                    byte[] b;
                    try
                    {
                        
                        // Get decoded stream bytes.
                        b = ((PdfStream) obj).GetBytes();
                    }
                    catch (PdfException)
                    {
                        
                        // Get originally encoded stream bytes
                        b = ((PdfStream) obj).GetBytes(false);
                    }

                    using (FileStream fos = new FileStream(String.Format(dest + "/extract_streams{0}.dat", i), FileMode.Create))
                    {
                        fos.Write(b, 0, b.Length);
                    }
                }
            }

            pdfDoc.Close();
        }
    }
}