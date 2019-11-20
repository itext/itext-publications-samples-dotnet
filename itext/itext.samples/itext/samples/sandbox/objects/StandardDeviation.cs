/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Objects
{
    public class StandardDeviation
    {
        public static readonly string DEST = "results/sandbox/objects/standard_deviation.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new StandardDeviation().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            doc.Add(new Paragraph("The standard deviation symbol doesn't exist in Helvetica."));

            PdfFont symbolFont = PdfFontFactory.CreateFont(StandardFonts.SYMBOL);
            Paragraph p = new Paragraph("So we use the Symbol font: ");
            p.Add(new Text("s").SetFont(symbolFont));
            doc.Add(p);

            doc.Close();
        }
    }
}