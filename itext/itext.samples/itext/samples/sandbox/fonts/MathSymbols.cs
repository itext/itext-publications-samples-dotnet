/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */
/**
 * This example was written by Bruno Lowagie in answer to the following question:
 * http://stackoverflow.com/questions/31268867/
 */

using System;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;


namespace iText.Samples.Sandbox.Fonts
{
    public class MathSymbols
    {
        public static readonly String DEST = "results/sandbox/fonts/math_symbols.pdf";
        
        public static readonly String FONT = "../../resources/font/FreeSans.ttf";
        
        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new MathSymbols().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            
            PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H, true);
            
            // "Testing math symbols ∈, ∩, ∑, ∫, ∆"
            Paragraph p = new Paragraph("Testing math symbols \u2208, \u2229, \u2211, \u222b, \u2206")
                .SetFont(font);

            doc.Add(p);
            doc.Close();
        }
    }
}