/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Objects
{
    public class ColoredText
    {
        public static readonly string DEST = "results/sandbox/objects/colored_text.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new ColoredText().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Text redText = new Text("This text is red. ")
                .SetFontColor(ColorConstants.RED)
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));
            Text blueText = new Text("This text is blue and bold. ")
                .SetFontColor(ColorConstants.BLUE)
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD));
            Text greenText = new Text("This text is green and italic. ")
                .SetFontColor(ColorConstants.GREEN)
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_OBLIQUE));

            Paragraph p1 = new Paragraph(redText).SetMargin(0);
            doc.Add(p1);
            Paragraph p2 = new Paragraph().SetMargin(0);
            p2.Add(blueText);
            p2.Add(greenText);
            doc.Add(p2);

            new Canvas(new PdfCanvas(pdfDoc.GetLastPage()), pdfDoc, new Rectangle(36, 600, 108, 160))
                .Add(p1)
                .Add(p2);

            doc.Close();
        }
    }
}