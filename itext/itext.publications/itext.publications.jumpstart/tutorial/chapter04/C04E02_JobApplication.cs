using System;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace Tutorial.Chapter04 {
    /// <summary>Simple widget annotation example.</summary>
    public class C04E02_JobApplication {
        public const String DEST = "../../../results/chapter04/job_application.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C04E02_JobApplication().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            PageSize ps = PageSize.A4;
            pdf.SetDefaultPageSize(ps);
            // Initialize document
            Document document = new Document(pdf);
            C04E02_JobApplication.AddAcroForm(document);
            //Close document
            document.Close();
        }

        public static PdfAcroForm AddAcroForm(Document doc) {
            Paragraph title = new Paragraph("Application for employment").SetTextAlignment(TextAlignment.CENTER).SetFontSize
                (16);
            doc.Add(title);
            doc.Add(new Paragraph("Full name:").SetFontSize(12));
            doc.Add(new Paragraph("Native language:      English         French       German        Russian        Spanish"
                ).SetFontSize(12));
            doc.Add(new Paragraph("Experience in:       cooking        driving           software development").SetFontSize
                (12));
            doc.Add(new Paragraph("Preferred working shift:").SetFontSize(12));
            doc.Add(new Paragraph("Additional information:").SetFontSize(12));
            //Add acroform
            PdfAcroForm form = PdfAcroForm.GetAcroForm(doc.GetPdfDocument(), true);
            //Create text field
            PdfTextFormField nameField = new TextFormFieldBuilder(doc.GetPdfDocument(), "name")
                .SetWidgetRectangle(new Rectangle(99, 753, 425, 15)).CreateText();
            nameField.SetValue("");
            form.AddField(nameField);
            //Create radio buttons
            PdfButtonFormField group = new RadioFormFieldBuilder(doc.GetPdfDocument(), "language").CreateRadioGroup();
            group.SetValue("");
            new RadioFormFieldBuilder(doc.GetPdfDocument()).SetWidgetRectangle(new Rectangle(130, 728, 15, 15))
                .CreateRadioButton(group, "English");
            new RadioFormFieldBuilder(doc.GetPdfDocument()).SetWidgetRectangle(new Rectangle(200, 728, 15, 15))
                .CreateRadioButton(group, "French");
            new RadioFormFieldBuilder(doc.GetPdfDocument()).SetWidgetRectangle(new Rectangle(260, 728, 15, 15))
                .CreateRadioButton(group, "German");
            new RadioFormFieldBuilder(doc.GetPdfDocument()).SetWidgetRectangle(new Rectangle(330, 728, 15, 15))
                .CreateRadioButton(group, "Russian");
            new RadioFormFieldBuilder(doc.GetPdfDocument()).SetWidgetRectangle(new Rectangle(400, 728, 15, 15))
                .CreateRadioButton(group, "Spanish");
            form.AddField(group);
            //Create checkboxes
            for (int i = 0; i < 3; i++) {
                PdfButtonFormField checkField = new CheckBoxFormFieldBuilder(doc.GetPdfDocument(),
                    String.Concat("experience", (i + 1).ToString()))
                    .SetWidgetRectangle(new Rectangle(119 + i * 69, 701, 15, 15))
                    .SetCheckType(PdfFormField.TYPE_CHECK).CreateCheckBox();
                checkField.SetValue("Off");
                form.AddField(checkField);
            }
            //Create combobox
            String[] options = new String[] { "Any", "6.30 am - 2.30 pm", "1.30 pm - 9.30 pm" };
            PdfChoiceFormField choiceField = new ChoiceFormFieldBuilder(doc.GetPdfDocument(), "shift")
                .SetWidgetRectangle(new Rectangle(163, 676, 115, 15)).SetOptions(options).CreateComboBox();
            choiceField.SetValue("Any");
            form.AddField(choiceField);
            //Create multiline text field
            PdfTextFormField infoField = new TextFormFieldBuilder(doc.GetPdfDocument(), "info")
                .SetWidgetRectangle(new Rectangle(158, 625, 366, 40)).CreateMultilineText();
            infoField.SetValue("");
            form.AddField(infoField);
            //Create push button field
            PdfButtonFormField button = new PushButtonFormFieldBuilder(doc.GetPdfDocument(), "reset")
                .SetWidgetRectangle(new Rectangle(479, 594, 45, 15)).SetCaption("RESET").CreatePushButton();
            button.SetAction(PdfAction.CreateResetForm(new String[] { "name", "language", "experience1", "experience2"
                , "experience3", "shift", "info" }, 0));
            form.AddField(button);
            return form;
        }
    }
}
