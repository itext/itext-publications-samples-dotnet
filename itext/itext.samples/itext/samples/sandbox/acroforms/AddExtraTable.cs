/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.Collections.Generic;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Layout;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Acroforms
{
    public class AddExtraTable
    {
        public static readonly String DEST = "results/sandbox/acroforms/add_extra_table.pdf";

        public static readonly String SRC = "../../resources/pdfs/form.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new AddExtraTable().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
            IDictionary<String, PdfFormField> fields = form.GetFormFields();
            fields["Name"].SetValue("Jeniffer");
            fields["Company"].SetValue("iText's next customer");
            fields["Country"].SetValue("No Man's Land");
            form.FlattenFields();

            Table table = new Table(UnitValue.CreatePercentArray(new float[] {1, 15}));
            table.SetWidth(UnitValue.CreatePercentValue(80));
            table.AddHeaderCell("#");
            table.AddHeaderCell("description");
            for (int i = 1; i <= 150; i++)
            {
                table.AddCell(i.ToString());
                table.AddCell("test " + i);
            }

            // The custom renderer decreases the first page's area.
            // As a result, there is not overlapping between the table from acroform and the new one.
            doc.SetRenderer(new ExtraTableRenderer(doc));
            doc.Add(table);

            doc.Close();
        }

        protected class ExtraTableRenderer : DocumentRenderer
        {
            public ExtraTableRenderer(Document baseArg1)
                : base(baseArg1)
            {
            }            
            
            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
            {
                return new ExtraTableRenderer(document);
            }

            protected override LayoutArea UpdateCurrentArea(LayoutResult overflowResult)
            {
                LayoutArea area = base.UpdateCurrentArea(overflowResult);
                if (area.GetPageNumber() == 1)
                {
                    area.GetBBox().DecreaseHeight(266);
                }

                return area;
            }
        }
    }
}