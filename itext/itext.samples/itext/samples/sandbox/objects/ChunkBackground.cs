/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System.IO;
using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Objects
{
    public class ChunkBackground
    {
        public static readonly string DEST = "results/sandbox/objects/chunk_background.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new ChunkBackground().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PdfFont f = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD, PdfEncodings.WINANSI);
            Text text = new Text("White text on red background")
                .SetFont(f)
                .SetFontSize(25.0f)
                .SetFontColor(ColorConstants.WHITE)
                .SetBackgroundColor(ColorConstants.RED);

            Paragraph p = new Paragraph(text);
            doc.Add(p);

            doc.Close();
        }
    }
}