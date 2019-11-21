/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

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
    public class CheckboxCell2
    {
        public static readonly String DEST = "results/sandbox/acroforms/checkbox_cell2.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new CheckboxCell2().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(6)).UseAllAvailableWidth();
            for (int i = 0; i < 6; i++)
            {
                Cell cell = new Cell();

                // Custom renderer creates checkbox in the current cell
                cell.SetNextRenderer(new CheckboxCellRenderer(cell, "cb" + i, i));
                cell.SetHeight(50);
                table.AddCell(cell);
            }

            doc.Add(table);
            doc.Close();
        }

        private class CheckboxCellRenderer : CellRenderer
        {
            // The name of the check box field
            protected String name;
            protected int checkboxTypeIndex;

            public CheckboxCellRenderer(Cell modelElement, String name, int checkboxTypeIndex)
                : base(modelElement)
            {
                this.name = name;
                this.checkboxTypeIndex = checkboxTypeIndex;
            }            
            
            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
            {
                return new CheckboxCellRenderer((Cell) modelElement, name, checkboxTypeIndex);
            }

            public override void Draw(DrawContext drawContext)
            {
                Rectangle position = GetOccupiedAreaBBox();
                PdfAcroForm form = PdfAcroForm.GetAcroForm(drawContext.GetDocument(), true);

                // Define the coordinates of the middle
                float x = (position.GetLeft() + position.GetRight()) / 2;
                float y = (position.GetTop() + position.GetBottom()) / 2;

                // Define the position of a check box that measures 20 by 20
                Rectangle rect = new Rectangle(x - 10, y - 10, 20, 20);

                // The 4th parameter is the initial value of checkbox: 'Yes' - checked, 'Off' - unchecked
                // By default, checkbox value type is cross.
                PdfButtonFormField checkBox =
                    PdfFormField.CreateCheckBox(drawContext.GetDocument(), rect, this.name, "Yes");
                switch (checkboxTypeIndex)
                {
                    case 0:
                    {
                        checkBox.SetCheckType(PdfFormField.TYPE_CHECK);

                        // Use this method if you changed any field parameters and didn't use setValue
                        checkBox.RegenerateField();
                        break;
                    }

                    case 1:
                    {
                        checkBox.SetCheckType(PdfFormField.TYPE_CIRCLE);
                        checkBox.RegenerateField();
                        break;
                    }

                    case 2:
                    {
                        checkBox.SetCheckType(PdfFormField.TYPE_CROSS);
                        checkBox.RegenerateField();
                        break;
                    }

                    case 3:
                    {
                        checkBox.SetCheckType(PdfFormField.TYPE_DIAMOND);
                        checkBox.RegenerateField();
                        break;
                    }

                    case 4:
                    {
                        checkBox.SetCheckType(PdfFormField.TYPE_SQUARE);
                        checkBox.RegenerateField();
                        break;
                    }

                    case 5:
                    {
                        checkBox.SetCheckType(PdfFormField.TYPE_STAR);
                        checkBox.RegenerateField();
                        break;
                    }
                }

                form.AddField(checkBox);
            }
        }
    }
}