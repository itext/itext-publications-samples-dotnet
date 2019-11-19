/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;

namespace iText.Samples.Sandbox.Stamper 
{
    public class AddExtraMargin 
    {
        public static readonly String DEST = "results/sandbox/stamper/add_extra_margin.pdf";
        public static readonly String SRC = "../../resources/pdfs/primes.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new AddExtraMargin().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            
            // Loop over every page
            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++) {
                PdfDictionary pageDict = pdfDoc.GetPage(i).GetPdfObject();
                PdfArray mediaBox = pageDict.GetAsArray(PdfName.MediaBox);
                float llx = mediaBox.GetAsNumber(0).FloatValue();
                float lly = mediaBox.GetAsNumber(1).FloatValue();
                float ury = mediaBox.GetAsNumber(3).FloatValue();
                mediaBox.Set(0, new PdfNumber(llx - 36));
                PdfCanvas over = new PdfCanvas(pdfDoc.GetPage(i));
                over.SaveState();
                over.SetFillColor(new DeviceGray(0.5f));
                over.Rectangle(llx - 36, lly, 36, ury - llx);
                over.Fill();
                over.RestoreState();
            }
            
            pdfDoc.Close();
        }
    }
}
