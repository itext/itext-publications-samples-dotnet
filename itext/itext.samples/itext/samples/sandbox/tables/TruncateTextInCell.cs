/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Layout;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Tables
{
    public class TruncateTextInCell
    {
        public static readonly string DEST = "results/sandbox/tables/truncate_text_in_cell.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new TruncateTextInCell().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(5)).UseAllAvailableWidth();
 
            for (int r = 'A'; r <= 'Z'; r++)
            {
                for (int c = 1; c <= 5; c++)
                {
                    Cell cell = new Cell();
                    if (r == 'D' && c == 2)
                    {
                        cell.SetNextRenderer(new FitCellRenderer(cell,
                            "D2 is a cell with more content than we can fit into the cell."));
                    }
                    else
                    {
                        cell.Add(new Paragraph(((char) r).ToString() + c));
                    }

                    table.AddCell(cell);
                }
            }

            doc.Add(table);

            doc.Close();
        }

        private class FitCellRenderer : CellRenderer
        {
            private String content;

            public FitCellRenderer(Cell modelElement, String content)
                : base(modelElement)
            {
                this.content = content;
            }           
            
            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
            {
                return new FitCellRenderer((Cell) modelElement, content);
            }

            /**
             * Method adapts content, that can't be fit into the cell,
             * to prevent truncation by replacing truncated part of content with '...'
             */
            public override LayoutResult Layout(LayoutContext layoutContext)
            {
                PdfFont bf = GetPropertyAsFont(Property.FONT);
                int contentLength = content.Length;
                int leftCharIndex = 0;
                int rightCharIndex = contentLength - 1;
                float availableWidth = layoutContext.GetArea().GetBBox().GetWidth();

                availableWidth -= bf.GetWidth("...", 12);

                while (leftCharIndex < contentLength && rightCharIndex != leftCharIndex)
                {
                    availableWidth -= bf.GetWidth(content[leftCharIndex], 12);
                    if (availableWidth > 0)
                    {
                        leftCharIndex++;
                    }
                    else
                    {
                        break;
                    }

                    availableWidth -= bf.GetWidth(content[rightCharIndex], 12);

                    if (availableWidth > 0)
                    {
                        rightCharIndex--;
                    }
                    else
                    {
                        break;
                    }
                }

                String newContent = content.Substring(0, leftCharIndex) + "..." + content.Substring(rightCharIndex);
                Paragraph p = new Paragraph(newContent);
                
                // We're operating on a Renderer level here, that's why we need to process a renderer,
                // created with the updated paragraph
                IRenderer pr = p.CreateRendererSubTree().SetParent(this);
                childRenderers.Add(pr);
                
                return base.Layout(layoutContext);
            }
        }
    }
}