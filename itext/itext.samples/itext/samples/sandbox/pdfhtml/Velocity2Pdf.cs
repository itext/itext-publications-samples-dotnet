using System;
using System.IO;
using iText.Html2pdf;
using NVelocity;
using NVelocity.App;

namespace iText.Samples.Sandbox.Pdfhtml
{
    public class Velocity2Pdf
    {
        public static readonly String DEST = "results/sandbox/pdfhtml/velocity-test.pdf";

        public static readonly String SRC = "../../../resources/pdfhtml/templates/velocity-test.vm";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new Velocity2Pdf().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            // Initialize the velocity engine
            VelocityEngine engine = new VelocityEngine();
            engine.Init();

            // Create a velocity context and populate it
            VelocityContext context = new VelocityContext();
            context.Put("message", "Hello World!");

            // Load the template
            StringWriter writer = new StringWriter();
            Template template = engine.GetTemplate(SRC);
            template.Merge(context, writer);

            using (FileStream stream = new FileStream(dest, FileMode.Create))
            {
                HtmlConverter.ConvertToPdf(writer.ToString(), stream);
            }
        }
    }
}