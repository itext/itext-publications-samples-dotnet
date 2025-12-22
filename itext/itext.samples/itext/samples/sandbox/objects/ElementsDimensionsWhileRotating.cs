using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Objects
{
   
    // ElementsDimensionsWhileRotating.cs
    //
    // Example showing how element dimensions behave with rotation angles.
    // Demonstrates auto and fixed width handling for rotated paragraphs.
 
    public class ElementsDimensionsWhileRotating
    {
        public static readonly string DEST = "results/sandbox/objects/elementsDimensionsWhileRotating.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new ElementsDimensionsWhileRotating().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDocument);
            
            String bigText = "Hello. I am a fairly long paragraph. I really want you to process me correctly."
                              + " You heard that? Correctly!!! Even if you will have to wrap me.";
            
            Style rotatedStyle = new Style().SetPadding(0).SetMargin(0).SetBorder(new SolidBorder(ColorConstants.BLUE, 
                2)).SetBackgroundColor(ColorConstants.LIGHT_GRAY);
            Style contentStyle = new Style().SetPadding(0).SetMargin(0).SetBorder(new SolidBorder(ColorConstants.GREEN
                , 2)).SetWidth(400);
            
            LineSeparator line = new LineSeparator(new SolidLine(1));
            
            //width of rotated element isn't set, so it's width will be set automatically
            Div contentDiv = new Div().AddStyle(contentStyle);
            contentDiv.Add(new Paragraph("Short paragraph").AddStyle(rotatedStyle).SetRotationAngle(Math.PI * 3 / 8));
            contentDiv.Add(line);
            
            contentDiv.Add(new Paragraph(bigText).AddStyle(rotatedStyle).SetRotationAngle(Math.PI * 3 / 8));
            contentDiv.Add(line);
            
            contentDiv.Add(new Paragraph(bigText).AddStyle(rotatedStyle).SetRotationAngle(Math.PI / 30));
            doc.Add(contentDiv);
            
            doc.Add(new AreaBreak());
            
            //fixed width of rotated elements, so the content inside will be located according set width
            contentDiv = new Div().AddStyle(contentStyle).SetWidth(200);
            
            contentDiv.Add(new Paragraph(bigText).AddStyle(rotatedStyle).SetWidth(400).SetRotationAngle(Math.PI / 2));
            doc.Add(contentDiv);
            
            doc.Add(new Paragraph(bigText).AddStyle(rotatedStyle).SetWidth(800).SetRotationAngle(Math.PI / 30));
            doc.Close();
        }
    }
}