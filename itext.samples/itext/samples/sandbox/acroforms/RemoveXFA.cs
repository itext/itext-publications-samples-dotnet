using System;
using System.Collections.Generic;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Acroforms {
    public class RemoveXFA : GenericTest {
        public static readonly String DEST = NUnit.Framework.TestContext.CurrentContext.TestDirectory + "/test/resources/sandbox/acroforms/remove_xfa.pdf";
        public static readonly String SRC = NUnit.Framework.TestContext.CurrentContext.TestDirectory + "/../../resources/pdfs/reportcardinitial.pdf";

        protected override void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(DEST));
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
            form.RemoveXfaForm();
            IDictionary<String, PdfFormField> fields = form.GetFormFields();
            foreach (KeyValuePair<String, PdfFormField> name in fields)
            {
                if (name.Key.IndexOf("Total") > 0)
                {
                    name.Value.GetWidgets()[0].SetColor(iText.Kernel.Color.Color.RED);
                }
                name.Value.SetValue("X");
            }
            pdfDoc.Close();
        }

        [NUnit.Framework.Test]
        public override void Test()
        {
            base.Test();
        }
    }
}
