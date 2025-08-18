using System;
using System.IO;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;

namespace iText.Samples.Sandbox.Acroforms
{
   
    // ReuseFont.cs
    //
    // This example demonstrates how to reuse an existing font from a PDF document
    // and use it to add new text to the document.
 
    public class ReuseFont
    {
        public static readonly String DEST = "results/sandbox/acroforms/reuse_font.pdf";

        public static readonly String SRC = "../../../resources/pdfs/form.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ReuseFont().ManipulatePdf(DEST);
        }

        // Method searches and returns font object by the passed font name.
        public PdfFont FindFontInForm(PdfDocument pdfDoc, PdfName fontName)
        {
            PdfDictionary acroForm = pdfDoc.GetCatalog().GetPdfObject().GetAsDictionary(PdfName.AcroForm);
            if (acroForm == null)
            {
                return null;
            }

            PdfDictionary dr = acroForm.GetAsDictionary(PdfName.DR);
            if (dr == null)
            {
                return null;
            }

            PdfDictionary font = dr.GetAsDictionary(PdfName.Font);
            if (font == null)
            {
                return null;
            }

            foreach (PdfName key in font.KeySet())
            {
                if (key.Equals(fontName))
                {
                    return PdfFontFactory.CreateFont(font.GetAsDictionary(key));
                }
            }

            return null;
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));

            PdfFont font = FindFontInForm(pdfDoc, new PdfName("Calibri"));

            PdfCanvas canvas = new PdfCanvas(pdfDoc.GetFirstPage());
            canvas.BeginText();
            canvas.SetFontAndSize(font, 13);
            canvas.MoveText(36, 806);
            canvas.ShowText("Some text in Calibri");
            canvas.EndText();
            canvas.Stroke();

            pdfDoc.Close();
        }
    }
}