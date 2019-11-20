/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Objects
{
    public class ParagraphSpacingBefore
    {
        public static readonly string DEST = "results/sandbox/objects/paragraph_spacing_before.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new ParagraphSpacingBefore().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Paragraph paragraph1 = new Paragraph("First paragraph");
            doc.Add(paragraph1);

            Paragraph paragraph2 = new Paragraph("Second paragraph");
            paragraph2.SetMarginTop(380f);
            doc.Add(paragraph2);

            doc.Close();
        }
    }
}