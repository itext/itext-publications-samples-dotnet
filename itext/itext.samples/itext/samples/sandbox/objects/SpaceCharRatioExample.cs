/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Objects
{
    public class SpaceCharRatioExample
    {
        public static readonly string DEST = "results/sandbox/objects/space_char_ratio.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new SpaceCharRatioExample().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Paragraph p = new Paragraph()
                .SetSpacingRatio(1f)
                .SetTextAlignment(TextAlignment.JUSTIFIED)
                .SetMarginLeft(20f)
                .SetMarginRight(20f)
                .Add(
                    "HelloWorld HelloWorld HelloWorld HelloWorld HelloWorldHelloWorld HelloWorldHelloWorldHelloWorld" +
                    "HelloWorldHelloWorld HelloWorld HelloWorld HelloWorldHelloWorldHelloWorld");
            doc.Add(p);

            doc.Close();
        }
    }
}