/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2019 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Events
{
    public class Every25Words
    {
        public static readonly String DEST = "results/sandbox/events/every25words.pdf";

        public static readonly String SRC = "../../resources/text/liber1_1_la.txt";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new Every25Words().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            String file = ReadFile(SRC);
            String[] words = Regex.Split(file, "\\s+");
            Paragraph paragraph = new Paragraph();
            Text text = null;
            int i = 0;
            foreach (String word in words)
            {
                if (text != null)
                {
                    paragraph.Add(" ");
                }

                text = new Text(word);
                text.SetNextRenderer(new Word25TextRenderer(text, ++i));
                paragraph.Add(text);
            }

            doc.Add(paragraph);

            doc.Close();
        }

        private static String ReadFile(String filePath)
        {
            StringBuilder sb = new StringBuilder();

            using (StreamReader reader = new StreamReader(new FileStream(filePath, FileMode.Open), Encoding.UTF8))
            {
                String str;
                while ((str = reader.ReadLine()) != null)
                {
                    sb.Append(str);
                }
            }

            return sb.ToString();
        }

        private class Word25TextRenderer : TextRenderer
        {
            private int count = 0;

            public Word25TextRenderer(Text textElement, int count) : base(textElement)
            {
                this.count = count;
            }

            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
            {
                return new Word25TextRenderer((Text) modelElement, count);
            }

            public override void Draw(DrawContext drawContext)
            {
                base.Draw(drawContext);

                // Draws a line to delimit the text every 25 words
                if (0 == count % 25)
                {
                    Rectangle textRect = GetOccupiedAreaBBox();
                    int pageNumber = GetOccupiedArea().GetPageNumber();
                    PdfCanvas canvas = drawContext.GetCanvas();
                    Rectangle pageRect = drawContext.GetDocument().GetPage(pageNumber).GetPageSize();
                    canvas
                        .SaveState()
                        .SetLineDash(5, 5)
                        .MoveTo(pageRect.GetLeft(), textRect.GetBottom())
                        .LineTo(textRect.GetRight(), textRect.GetBottom())
                        .LineTo(textRect.GetRight(), textRect.GetTop())
                        .LineTo(pageRect.GetRight(), textRect.GetTop())
                        .Stroke()
                        .RestoreState();
                }
            }
        }
    }
}