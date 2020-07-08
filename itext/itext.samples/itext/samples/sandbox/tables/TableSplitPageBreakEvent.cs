using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Layout;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Tables
{
    
    // In this sample we will show how one can change the text of the cell on split.
    // The table contains two columns: the first one will be processed as usually and
    // the second one is the on which the required split logic will be performed.
    // Mind that for simplification reasons the way in which we handle overflow rows is not designed for a generic case.
    // For example, if your table has cells with big rowspan and/or colspans, you may want to update the code a bit.
    public class TableSplitPageBreakEvent
    {
        public static readonly String DEST = "results/sandbox/tables/tables_split_pageBreak_event.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new TableSplitPageBreakEvent().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(2);
            String text =
                "In this sample we will show how one can change the text of the cell on split. The table contains two columns: the first one will be processed as usually and the second one is the on which the required split logic will be performed\n"
                + "In this sample we will show how one can change the text of the cell on split. The table contains two columns: the first one will be processed as usually and the second one is the on which the required split logic will be performed\n"
                + "In this sample we will show how one can change the text of the cell on split. The table contains two columns: the first one will be processed as usually and the second one is the on which the required split logic will be performed\n"
                + "In this sample we will show how one can change the text of the cell on split. The table contains two columns: the first one will be processed as usually and the second one is the on which the required split logic will be performed\n"
                + "In this sample we will show how one can change the text of the cell on split. The table contains two columns: the first one will be processed as usually and the second one is the on which the required split logic will be performed\n"
                + "In this sample we will show how one can change the text of the cell on split. The table contains two columns: the first one will be processed as usually and the second one is the on which the required split logic will be performed\n"
                + "In this sample we will show how one can change the text of the cell on split. The table contains two columns: the first one will be processed as usually and the second one is the on which the required split logic will be performed\n"
                + "In this sample we will show how one can change the text of the cell on split. The table contains two columns: the first one will be processed as usually and the second one is the on which the required split logic will be performed\n"
                + "\n";

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    table.AddCell(new Cell().Add(new Paragraph(text)));
                }
            }

            table.SetNextRenderer(new CustomTableRenderer(table));

            doc.Add(table);
            doc.Close();
        }

        private class CustomTableRenderer : TableRenderer
        {
            private static readonly int CUSTOM_CONTENT_COLUMN_NUMBER = 1;

            public CustomTableRenderer(Table modelElement) : base(modelElement)
            {
            }

            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
            {
                return new CustomTableRenderer((Table) modelElement);
            }

            public override LayoutResult Layout(LayoutContext layoutContext)
            {
                LayoutResult result = base.Layout(layoutContext);
                CustomTableRenderer split = (CustomTableRenderer) result.GetSplitRenderer();
                CustomTableRenderer overflow = (CustomTableRenderer) result.GetOverflowRenderer();

                // Page split happened
                if (result.GetStatus() == LayoutResult.PARTIAL)
                {
                    Table.RowRange splitRange = split.rowRange;
                    Table.RowRange overflowRange = overflow.rowRange;

                    // The table split happened
                    if (splitRange.GetFinishRow() == overflowRange.GetStartRow())
                    {
                        if (null != overflow.rows[0])
                        {
                            // Change cell contents on the new page
                            CellRenderer customContentCellRenderer = (CellRenderer) new Cell()
                                .Add(new Paragraph("Custom content"))
                                .CreateRendererSubTree()
                                .SetParent(this);
                            overflow.rows[0][CUSTOM_CONTENT_COLUMN_NUMBER] = customContentCellRenderer;
                        }
                    }
                }

                return result;
            }
        }
    }
}