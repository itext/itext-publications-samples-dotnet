using System;
using System.IO;
using iText.Forms.Form.Element;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Acroforms
{
    public class CreateFormFieldThroughLayout
    {
        public static readonly String DEST = "results/sandbox/acroforms/createFormFieldThroughLayout.pdf";
        
        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new CreateFormFieldThroughLayout().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            using (Document document = new Document(new PdfDocument(new PdfWriter(dest)))) {
                InputField inputField = new InputField("input field");
                inputField.SetValue("John");
                inputField.SetInteractive(true);

                TextArea textArea = new TextArea("text area");
                textArea.SetValue("I'm a chess player.\n" 
                                  + "In future I want to compete in professional chess and be the world champion.\n" 
                                  + "My favorite opening is caro-kann.\n" 
                                  + "Also I play sicilian defense a lot.");
                textArea.SetInteractive(true);

                Table table = new Table(2, false);
                table.AddCell("Name:");
                table.AddCell(new Cell().Add(inputField));
                table.AddCell("Personal info:");
                table.AddCell(new Cell().Add(textArea));

                Radio male = new Radio("male", "radioGroup");
                male.SetChecked(false);
                male.SetInteractive(true);
                male.SetBorder(new SolidBorder(1));
            
                Paragraph maleText = new Paragraph("Male: ");
                maleText.Add(male);
            
                Radio female = new Radio("female", "radioGroup");
                female.SetChecked(true);
                female.SetInteractive(true);
                female.SetBorder(new SolidBorder(1));

                Paragraph femaleText = new Paragraph("Female: ");
                femaleText.Add(female);

                Button button = new Button("submit");
                button.SetValue("Submit");
                button.SetInteractive(true);
                button.SetBorder(new SolidBorder(2));
                button.SetWidth(50);
                button.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
            
                document.Add(table);
                document.Add(maleText);
                document.Add(femaleText);
                document.Add(button);
            }
        }
    }
}