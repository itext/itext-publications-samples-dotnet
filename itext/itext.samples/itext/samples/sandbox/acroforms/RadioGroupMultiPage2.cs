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
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Acroforms
{
    public class RadioGroupMultiPage2
    {
        public static readonly String DEST = "../../results/sandbox/acroforms/radio_group_multi_page2.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new RadioGroupMultiPage2().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);

            // Radio buttons will be added to this radio group
            PdfButtonFormField radioGroup = PdfFormField.CreateRadioGroup(pdfDoc, "answer", "answer 1");

            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            for (int i = 0; i < 25; i++)
            {
                Cell cell = new Cell().Add(new Paragraph("Question " + i));
                table.AddCell(cell);

                cell = new Cell().Add(new Paragraph("Answer " + i));
                table.AddCell(cell);
            }

            for (int i = 0; i < 25; i++)
            {
                Cell cell = new Cell().Add(new Paragraph("Radio: " + i));
                table.AddCell(cell);

                cell = new Cell();

                // The renderer creates radio button for the current radio group in the current cell
                cell.SetNextRenderer(new AddRadioButtonRenderer(cell, radioGroup, "answer " + i));
                table.AddCell(cell);
            }

            doc.Add(table);

            form.AddField(radioGroup);

            pdfDoc.Close();
        }

        private class AddRadioButtonRenderer : CellRenderer
        {
            protected PdfButtonFormField radioGroup;
            protected String value;

            public AddRadioButtonRenderer(Cell modelElement, PdfButtonFormField radioGroup, String value)
                : base(modelElement)
            {
                this.radioGroup = radioGroup;
                this.value = value;
            }

            public override void Draw(DrawContext drawContext)
            {
                PdfDocument document = drawContext.GetDocument();
                PdfAcroForm form = PdfAcroForm.GetAcroForm(document, true);

                // Create a radio button that is added to a radio group.
                PdfFormField field = PdfFormField.CreateRadioButton(document, GetOccupiedAreaBBox(),
                    radioGroup, value);

                // This method merges field with its annotation and place it on the given page.
                // This method won't work if the field has no or more than one widget annotations.
                form.AddFieldAppearanceToPage(field, document.GetPage(GetOccupiedArea().GetPageNumber()));
            }
        }
    }
}