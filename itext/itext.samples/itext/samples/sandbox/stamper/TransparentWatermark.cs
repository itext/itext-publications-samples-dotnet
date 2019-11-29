/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Extgstate;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Stamper 
{
    public class TransparentWatermark 
    {
        public static readonly String DEST = "results/sandbox/stamper/transparent_watermark.pdf";
        public static readonly String SRC = "../../resources/pdfs/hero.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new TransparentWatermark().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfCanvas under = new PdfCanvas(pdfDoc.GetFirstPage().NewContentStreamBefore(), new PdfResources(), pdfDoc);
            PdfFont font = PdfFontFactory.CreateFont(FontProgramFactory.CreateFont(StandardFonts.HELVETICA));
            Paragraph paragraph = new Paragraph("This watermark is added UNDER the existing content")
                    .SetFont(font)
                    .SetFontSize(15);
            
            Canvas canvasWatermark1 = new Canvas(under, pdfDoc, pdfDoc.GetDefaultPageSize())
                    .ShowTextAligned(paragraph, 297, 550, 1, TextAlignment.CENTER, VerticalAlignment.TOP, 0);
            canvasWatermark1.Close();
            PdfCanvas over = new PdfCanvas(pdfDoc.GetFirstPage());
            over.SetFillColor(ColorConstants.BLACK);
            paragraph = new Paragraph("This watermark is added ON TOP OF the existing content")
                    .SetFont(font)
                    .SetFontSize(15);
            
            Canvas canvasWatermark2 = new Canvas(over, pdfDoc, pdfDoc.GetDefaultPageSize())
                    .ShowTextAligned(paragraph, 297, 500, 1, TextAlignment.CENTER, VerticalAlignment.TOP, 0);
            canvasWatermark2.Close();
            paragraph = new Paragraph("This TRANSPARENT watermark is added ON TOP OF the existing content")
                    .SetFont(font)
                    .SetFontSize(15);
            over.SaveState();
            
            // Creating a dictionary that maps resource names to graphics state parameter dictionaries
            PdfExtGState gs1 = new PdfExtGState();
            gs1.SetFillOpacity(0.5f);
            over.SetExtGState(gs1);
            Canvas canvasWatermark3 = new Canvas(over, pdfDoc, pdfDoc.GetDefaultPageSize())
                    .ShowTextAligned(paragraph, 297, 450, 1, TextAlignment.CENTER, VerticalAlignment.TOP, 0);
            canvasWatermark3.Close();
            over.RestoreState();
            
            pdfDoc.Close();
        }
    }
}
