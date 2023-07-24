using System;
using System.IO;
using iText.Commons.Utils;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Properties;
using iText.Licensing.Base;

namespace iText.Samples.Sandbox.Typography.Gujarati
{
    public class GujaratiDropDownBox
    {
        public const String DEST = "results/sandbox/typography/GujaratiDropDownBox.pdf";
        public const String FONTS_FOLDER = "../../../resources/font/";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            using (Stream license = FileUtil.GetInputStreamForFile(
                Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") + "/itextkey-typography.json"))
            {
                LicenseKey.LoadLicenseFile(license);
            }

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new GujaratiDropDownBox().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            PdfAcroForm form = PdfFormCreator.GetAcroForm(document.GetPdfDocument(), true);

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansGujarati-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // Embed entire font without any subsetting. Please note that without subset it's impossible to edit a form field
            // with the predefined font
            font.SetSubset(false);

            // વાઈસરૉયને
            String line1 = "\u0AB5\u0ABE\u0A88\u0AB8\u0AB0\u0AC9\u0AAF\u0AA8\u0AC7";

            // રાજ્યમાં
            String line2 = "\u0AB0\u0ABE\u0A9C\u0ACD\u0AAF\u0AAE\u0ABE\u0A82";

            // વસતા
            String line3 = "\u0AB5\u0AB8\u0AA4\u0ABE";

            // Initialize the array with 3 lines of text. These lines will be used as combo box options
            String[] comboText = new String[] {line1, line2, line3};

            // Create a form field and apply the properties on it
            PdfFormField formField = new ChoiceFormFieldBuilder(document.GetPdfDocument(), "test")
                .SetWidgetRectangle(new Rectangle(50, 750,50, 15)).SetOptions(comboText).CreateComboBox();
            formField.SetValue(line1);
            formField
                    .SetJustification(TextAlignment.CENTER)
                    .SetFont(font)
                    .SetFontSizeAutoScale();
            formField.GetFirstFormAnnotation().SetBorderWidth(1);

            form.AddField(formField);

            document.Close();
        }
    }
}