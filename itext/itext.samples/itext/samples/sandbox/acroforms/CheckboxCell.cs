using System;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Acroforms
{
    public class CheckboxCell
    {
        public static readonly String DEST = "results/sandbox/acroforms/checkbox_cell.pdf";
        
        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new CheckboxCell().ManipulatePdf(DEST);
        }
        
        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            
            Table table = new Table(UnitValue.CreatePercentArray(5)).UseAllAvailableWidth();
            for (int i = 0; i < 5; i++)
            {
                Cell cell = new Cell();
                
                // Custom renderer creates checkbox in the current cell
                cell.SetNextRenderer(new CheckboxCellRenderer(cell, "cb" + i));
                cell.SetHeight(50);
                table.AddCell(cell);
            }

            doc.Add(table);
            
            doc.Close();
        }

        private class CheckboxCellRenderer : CellRenderer
        {
            // The name of the check box field
            protected internal String name;

            public CheckboxCellRenderer(Cell modelElement, String name)
                : base(modelElement)
            {
                this.name = name;
            }            
            
            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
            {
                return new CheckboxCellRenderer((Cell) modelElement, name);
            }

            public override void Draw(DrawContext drawContext)
            {
                PdfAcroForm form = PdfAcroForm.GetAcroForm(drawContext.GetDocument(), true);
                
                // Define the coordinates of the middle
                float x = (GetOccupiedAreaBBox().GetLeft() + GetOccupiedAreaBBox().GetRight()) / 2;
                float y = (GetOccupiedAreaBBox().GetTop() + GetOccupiedAreaBBox().GetBottom()) / 2;
                
                // Define the position of a check box that measures 20 by 20
                Rectangle rect = new Rectangle(x - 10, y - 10, 20, 20);
                
                // The 4th parameter is the initial value of checkbox: 'Yes' - checked, 'Off' - unchecked
                // By default, checkbox value type is cross.
                PdfButtonFormField checkBox = new CheckBoxFormFieldBuilder(drawContext.GetDocument(), name)
                    .SetWidgetRectangle(rect).CreateCheckBox();
                checkBox.SetValue("Yes");
                form.AddField(checkBox);
            }
        }
    }
}