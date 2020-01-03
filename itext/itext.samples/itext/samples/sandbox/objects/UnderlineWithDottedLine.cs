/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Objects
{
    public class UnderlineWithDottedLine
    {
        public static readonly string DEST = "results/sandbox/objects/underline_with_dotted_line.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new UnderlineWithDottedLine().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            doc.Add(new Paragraph("This line will be underlined with a dotted line.").SetMarginBottom(0));
            doc.Add(new LineSeparator(new DottedLine(1, 2)).SetMarginTop(-4));

            doc.Close();
        }
    }
}