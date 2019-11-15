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
using iText.IO.Source;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Acroforms
{
    public class MergeForms2
    {
        public static readonly String DEST = "../../results/sandbox/acroforms/merge_forms2.pdf";

        public static readonly String SRC = "../../resources/pdfs/state.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new MergeForms2().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));

            // This method initializes an outline tree of the document and sets outline mode to true.
            pdfDoc.InitializeOutlines();

            // Copier contains the logic to copy only acroform fields to a new page.
            // PdfPageFormCopier uses some caching logic which can potentially improve performance
            // in case of the reusing of the same instance.
            PdfPageFormCopier formCopier = new PdfPageFormCopier();

            for (int i = 0; i < 3; i++)
            {
                // This method reads and renames form fields,
                // because the same source pdf with the same form fields will be copied.
                byte[] content = RenameFields(SRC, i + 1);
                IRandomAccessSource source = new RandomAccessSourceFactory().CreateSource(content);
                PdfDocument readerDoc = new PdfDocument(new PdfReader(source, new ReaderProperties()));
                readerDoc.CopyPagesTo(1, readerDoc.GetNumberOfPages(), pdfDoc, formCopier);
                readerDoc.Close();
            }

            pdfDoc.Close();
        }

        protected byte[] RenameFields(String src, int i)
        {
            ByteArrayOutputStream baos = new ByteArrayOutputStream();
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(src), new PdfWriter(baos));

            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
            foreach (PdfFormField field in form.GetFormFields().Values)
            {
                field.SetFieldName(String.Format("{0}_{1}", field.GetFieldName().ToString(), i));
            }

            pdfDoc.Close();

            return baos.ToArray();
        }
    }
}