using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iText.IO.Util;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Layout;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Tables
{
    public class SplitRowAtSpecificRow
    {
        public static readonly string DEST = "results/sandbox/tables/split_row_at_specific_row.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new SplitRowAtSpecificRow().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            Table table = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();
            table.SetWidth(550);
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, new PageSize(612, 242));

            for (int i = 0; i < 40; i++)
            {
                Cell cell;
                if (i == 20)
                {
                    cell = new Cell().Add(new Paragraph("Multiple\n\n\n\nLines"));
                }
                else
                {
                    cell = new Cell().Add(new Paragraph(i.ToString()));
                }

                table.AddCell(cell);
            }

            table.SetNextRenderer(new SplitTableAtSpecificRowRenderer(table, new List<int>()
                {
                    // first break occurs on row 4
                    4,
                    // break at row 7 gets ignored because if we break at 9 it still fits the page
                    7, 9,
                    // last break occurs at 25 the other rows get rendered as normal
                    25,
                    // break point 36 does not break because the remaining rows still fit the page
                    36,
                    // break point 50 gets ignored because there are not enough rows
                    50
                }
            ));


            doc.Add(table);

            doc.Close();
        }

        class SplitTableAtSpecificRowRenderer : TableRenderer
        {
            private readonly List<int> breakPoints;


            private int amountOfRowsThatAreGoingToBeRendered = 0;

            public SplitTableAtSpecificRowRenderer(Table modelElement, List<int> breakPoints) : base(modelElement)
            {
                this.breakPoints = breakPoints;
            }

            public override IRenderer GetNextRenderer()
            {
                return new SplitTableAtSpecificRowRenderer((Table)modelElement, this.breakPoints);
            }

            public override LayoutResult Layout(LayoutContext layoutContext)
            {
                LayoutResult result = null;
                while (result == null)
                {
                    result = AttemptLayout(layoutContext, this.breakPoints);
                }

                for (var index = 0; index < this.breakPoints.Count; index++)
                {
                    this.breakPoints[index] -= amountOfRowsThatAreGoingToBeRendered;
                }

                return result;
            }

            private LayoutResult AttemptLayout(LayoutContext layoutContext, List<int> breakPoints)
            {
                LayoutResult layoutResult = base.Layout(layoutContext);
                if (layoutResult.GetStatus() == LayoutResult.FULL || breakPoints.Count == 0)
                {
                    this.amountOfRowsThatAreGoingToBeRendered = GetAmountOfRows(layoutResult);
                    return layoutResult;
                }

                int breakPointToFix = CalculateBreakPoint(layoutContext);
                if (breakPointToFix >= 0)
                {
                    ForceAreaBreak(breakPointToFix);
                    this.amountOfRowsThatAreGoingToBeRendered = breakPointToFix - 1;
                    return null;
                }

                return layoutResult;
            }


            private int CalculateBreakPoint(LayoutContext layoutContext)
            {
                LayoutResult layoutResultWithoutSplits = AttemptLayout(layoutContext, new List<int>());
                if (layoutResultWithoutSplits == null)
                {
                    return int.MinValue;
                }

                int amountOfRowsThatFitWithoutSplit = GetAmountOfRows(layoutResultWithoutSplits);
                int breakPointToFix = int.MinValue;
                foreach (int breakPoint in new List<int>(breakPoints))
                {
                    if (breakPoint <= amountOfRowsThatFitWithoutSplit)
                    {
                        breakPoints.Remove(breakPoint);
                        if (breakPoint < amountOfRowsThatFitWithoutSplit && breakPoint > breakPointToFix)
                        {
                            breakPointToFix = breakPoint;
                        }
                    }
                }

                return breakPointToFix;
            }

            private void ForceAreaBreak(int rowIndex)
            {
                rowIndex++;
                if (rowIndex > rows.Count)
                {
                    return;
                }

                foreach (CellRenderer cellRenderer in rows[rowIndex])
                {
                    if (cellRenderer != null)
                    {
                        cellRenderer.GetChildRenderers()
                            .Insert(0, new AreaBreakRenderer(new AreaBreak(AreaBreakType.NEXT_PAGE)));
                        break;
                    }
                }
            }

            private static int GetAmountOfRows(LayoutResult layoutResult)
            {
                if (layoutResult.GetSplitRenderer() == null)
                {
                    return 0;
                }

                return ((SplitTableAtSpecificRowRenderer)layoutResult.GetSplitRenderer()).rows.Count;
            }
        }
    }
}