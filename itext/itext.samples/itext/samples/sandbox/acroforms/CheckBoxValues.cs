using System;
using System.Collections.Generic;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Acroforms
{
    public class CheckBoxValues
    {
        public static readonly String DEST = "results/sandbox/acroforms/check_box_values.pdf";

        public static readonly String SRC = "../../../resources/pdfs/checkboxExample.pdf";

        public static readonly String CHECKED_FIELD_NAME = "checked";
        public static readonly String UNCHECKED_FIELD_NAME = "unchecked";

        public static readonly String CHECKED_STATE_VALUE = "Yes";
        public static readonly String UNCHECKED_STATE_VALUE = "Off";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new CheckBoxValues().ManipulatePdf(DEST);
        }

        public void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfAcroForm form = PdfFormCreator.GetAcroForm(pdfDoc, true);
            IDictionary<String, PdfFormField> fields = form.GetAllFormFields();
            PdfFormField checkedField = fields[CHECKED_FIELD_NAME];
            PdfFormField uncheckedField = fields[UNCHECKED_FIELD_NAME];

            // Get array of possible values of the checkbox
            String[] states = checkedField.GetAppearanceStates();

            // See all possible values in the console
            foreach (String state in states)
            {
                Console.Write(state + "; ");
            }

            // Search and set checked state to the previously unchecked checkbox and vice versa
            foreach (String state in states)
            {
                if (state.Equals(CHECKED_STATE_VALUE))
                {
                    uncheckedField.SetValue(state);
                }
                else if (state.Equals(UNCHECKED_STATE_VALUE))
                {
                    checkedField.SetValue(state);
                }
            }

            pdfDoc.Close();
        }
    }
}