/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using System.Text;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace iText.Samples.Sandbox.Parse
{
    public class ParseCzech
    {
        public static readonly String DEST = "results/txt/czech.txt";

        public static readonly String SRC = "../../resources/pdfs/czech.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ParseCzech().ManipulatePdf(DEST);
        }

        protected virtual void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC));

            // Create a text extraction renderer
            LocationTextExtractionStrategy strategy = new LocationTextExtractionStrategy();

            // Note: if you want to re-use the PdfCanvasProcessor, you must call PdfCanvasProcessor.Reset()
            PdfCanvasProcessor parser = new PdfCanvasProcessor(strategy);
            parser.ProcessPageContent(pdfDoc.GetFirstPage());

            byte[] array = Encoding.UTF8.GetBytes(strategy.GetResultantText());
            using (FileStream stream = new FileStream(dest, FileMode.Create))
            {
                stream.Write(array, 0, array.Length);
            }

            pdfDoc.Close();
        }
    }
}