using System;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.Forms.Form.Element;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout.Borders;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Acroforms
{
    public class ModifyFormFieldAppearance
    {
        public static readonly String DEST = "results/sandbox/acroforms/modifyFormFieldAppearance.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ModifyFormFieldAppearance().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            using (PdfDocument document = new PdfDocument(new PdfWriter(dest))) {
                PdfTextFormField textFormField = new TextFormFieldBuilder(document, "text")
                    .SetWidgetRectangle(new Rectangle(50, 400, 200, 200)).CreateMultilineText();
                textFormField.SetValue("Some text\nto show that\nleading and font changes\n work as expected");
                TextArea textArea = new TextArea("text");
                // It is not recommended to change font preferences since those won't be preserved after appearance changes,
                // but it is still possible.
                textArea.SetProperty(Property.LEADING, new Leading(Leading.MULTIPLIED, 3f));
                textArea.SetItalic();
                textFormField.GetFirstFormAnnotation().SetFormFieldElement(textArea);

                PdfButtonFormField buttonFormField = new PushButtonFormFieldBuilder(document, "button")
                    .SetWidgetRectangle(new Rectangle(300, 400, 200, 200)).SetCaption("Send").CreatePushButton();
                Button button = new Button("button");
                button.SetOpacity(0.5f);
                button.SetBorderLeft(new SolidBorder(ColorConstants.RED, 10));
                button.SetProperty(Property.PADDING_LEFT, UnitValue.CreatePointValue(50));
                button.SetProperty(Property.PADDING_TOP, UnitValue.CreatePointValue(50));
                buttonFormField.GetFirstFormAnnotation().SetFormFieldElement(button);

                PdfAcroForm acroForm = PdfFormCreator.GetAcroForm(document, true);
                acroForm.AddField(textFormField);
                acroForm.AddField(buttonFormField);
            }
        }
    }
}