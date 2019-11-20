/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Objects
{
    public class DottedLineEnder
    {
        public static readonly string DEST = "results/sandbox/objects/dotted_line_ender.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new DottedLineEnder().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Paragraph p = new Paragraph("Ends with dots ");
            p.AddTabStops(new TabStop(523, TabAlignment.RIGHT, new DottedLine()));
            p.Add(new Tab());
            doc.Add(p);

            p = new Paragraph("This is a much longer paragraph that spans "
                              + "several lines. The String used to create this paragraph "
                              + "will be split automatically at the end of the line. The "
                              + "final line of this paragraph will end in a dotted line. ");
            p.AddTabStops(new TabStop(523, TabAlignment.LEFT, new DottedLine()));
            p.Add(new Tab());
            doc.Add(p);

            doc.Close();
        }
    }
}