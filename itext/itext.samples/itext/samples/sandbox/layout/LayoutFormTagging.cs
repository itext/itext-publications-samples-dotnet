using System;
using System.IO;
using iText.Forms.Form;
using iText.Forms.Form.Element;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Tagging;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Layout
{
   
    // LayoutFormTagging.cs
    //
    // Example showing how to assign custom accessibility roles to form fields.
    // Demonstrates overriding default roles for tagged PDF compliance.
 
    public class LayoutFormTagging
    {
        public static readonly string DEST = "results/sandbox/layout/changeFormRole.pdf";
        
        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new LayoutFormTagging().ChangeFormRole(DEST);
        }

        private void ChangeFormRole(string dest)
        {
        using (Document document = new Document(new PdfDocument(new PdfWriter(dest)))) {
            document.GetPdfDocument().SetTagged();

            // Creating Button and setting the Div role
            Button formButton = new Button("form button");
            formButton.Add(new Paragraph("paragraph with yellow border inside button")
                    .SetBorder(new SolidBorder(ColorConstants.YELLOW, 1)));
            formButton.GetAccessibilityProperties().SetRole(StandardRoles.DIV);
            document.Add(formButton);

            // Creating a CheckBox and setting the SECT role
            CheckBox checkBoxUnset = new CheckBox("CheckBox");
            checkBoxUnset.SetBorder(new SolidBorder(ColorConstants.GREEN, 10));
            checkBoxUnset.GetAccessibilityProperties().SetRole(StandardRoles.SECT);
            document.Add(checkBoxUnset);

            // Creating a ComboBoxField and setting the SPAN role
            ComboBoxField comboBoxField = new ComboBoxField("empty combo box field");
            comboBoxField.SetBackgroundColor(ColorConstants.RED);
            comboBoxField.GetAccessibilityProperties().SetRole(StandardRoles.SPAN);
            document.Add(comboBoxField);

            // Creating an InputField and setting the ANNOT role
            InputField formInputField = new InputField("form input field");
            formInputField.SetProperty(FormProperty.FORM_FIELD_VALUE, "form input field");
            formInputField.GetAccessibilityProperties().SetRole(StandardRoles.ANNOT);
            document.Add(formInputField);

            // Creating a ListBoxField and setting the LBL role
            ListBoxField formListBoxField = new ListBoxField("form list box field", 2, false);
            formListBoxField.AddOption("option 1", false);
            formListBoxField.AddOption("option 2", true);
            formListBoxField.GetAccessibilityProperties().SetRole(StandardRoles.LBL);
            document.Add(formListBoxField);

            // Creating a Radio and setting the CAPTION role
            Radio formRadioField = new Radio("form radio field");
            formRadioField.SetChecked(true);
            formRadioField.GetAccessibilityProperties().SetRole(StandardRoles.CAPTION);
            document.Add(formRadioField);

            // Creating a SignatureFieldAppearance and setting the FORMULA role
            SignatureFieldAppearance formSigField = new SignatureFieldAppearance("form SigField");
            formSigField.SetContent("form SigField");
            formSigField.SetBorder(new SolidBorder(ColorConstants.YELLOW, 1));
            formSigField.GetAccessibilityProperties().SetRole(StandardRoles.FORMULA);
            document.Add(formSigField);

            // Creating a TextArea and setting the H role
            TextArea flattenedTextArea = new TextArea("flattened text area");
            flattenedTextArea.SetValue("Text area with custom border");
            flattenedTextArea.SetInteractive(false);
            flattenedTextArea.SetBorder(new DashedBorder(ColorConstants.ORANGE, 10));
            flattenedTextArea.GetAccessibilityProperties().SetRole(StandardRoles.H);
            document.Add(flattenedTextArea); 
        } 
        }
    }
}
