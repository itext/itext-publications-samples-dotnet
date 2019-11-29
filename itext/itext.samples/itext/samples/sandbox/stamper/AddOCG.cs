/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.Collections.Generic;
using System.IO;
using iText.IO.Util;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Layer;
using iText.Layout;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Stamper 
{
    public class AddOCG 
    {
        public static readonly String DEST = "results/sandbox/stamper/add_ocg.pdf";
        public static readonly String SRC = "../../resources/pdfs/hello.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new AddOCG().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfCanvas canvas = new PdfCanvas(pdfDoc.GetFirstPage());
            
            PdfLayer nested = new PdfLayer("Nested layers", pdfDoc);
            PdfLayer nested1 = new PdfLayer("Nested layer 1", pdfDoc);
            PdfLayer nested2 = new PdfLayer("Nested layer 2", pdfDoc);
            nested2.SetLocked(true);
            nested.AddChild(nested1);
            nested.AddChild(nested2);
            
            canvas.BeginLayer(nested);
            Canvas canvasModel = new Canvas(canvas, pdfDoc, pdfDoc.GetDefaultPageSize());
            canvasModel.ShowTextAligned("nested layers", 50, 765, TextAlignment.LEFT, 0);
            canvas.EndLayer();
            
            canvas.BeginLayer(nested1);
            canvasModel.ShowTextAligned("nested layers 1", 100, 800, TextAlignment.LEFT, 0);
            canvas.EndLayer();
            
            canvas.BeginLayer(nested2);
            canvasModel.ShowTextAligned("nested layers 2", 100, 750, TextAlignment.LEFT, 0);
            canvas.EndLayer();
            
            PdfLayer group = PdfLayer.CreateTitle("Grouped layers", pdfDoc);
            PdfLayer layer1 = new PdfLayer("Group: layer 1", pdfDoc);
            PdfLayer layer2 = new PdfLayer("Group: layer 2", pdfDoc);
            group.AddChild(layer1);
            group.AddChild(layer2);
            
            canvas.BeginLayer(layer1);
            canvasModel.ShowTextAligned("layer 1 in the group", 50, 700, TextAlignment.LEFT, 0);
            canvas.EndLayer();
            
            canvas.BeginLayer(layer2);
            canvasModel.ShowTextAligned("layer 2 in the group", 50, 675, TextAlignment.LEFT, 0);
            canvas.EndLayer();
            
            PdfLayer radiogroup = PdfLayer.CreateTitle("Radio group", pdfDoc);
            PdfLayer radio1 = new PdfLayer("Radiogroup: layer 1", pdfDoc);
            radio1.SetOn(true);
            PdfLayer radio2 = new PdfLayer("Radiogroup: layer 2", pdfDoc);
            radio2.SetOn(false);
            PdfLayer radio3 = new PdfLayer("Radiogroup: layer 3", pdfDoc);
            radio3.SetOn(false);
            radiogroup.AddChild(radio1);
            radiogroup.AddChild(radio2);
            radiogroup.AddChild(radio3);
            IList<PdfLayer> options = new List<PdfLayer>();
            options.Add(radio1);
            options.Add(radio2);
            options.Add(radio3);
            PdfLayer.AddOCGRadioGroup(pdfDoc, options);
            
            canvas.BeginLayer(radio1);
            canvasModel.ShowTextAligned("option 1", 50, 600, TextAlignment.LEFT, 0);
            canvas.EndLayer();
            
            canvas.BeginLayer(radio2);
            canvasModel.ShowTextAligned("option 2", 50, 575, TextAlignment.LEFT, 0);
            canvas.EndLayer();
            
            canvas.BeginLayer(radio3);
            canvasModel.ShowTextAligned("option 3", 50, 550, TextAlignment.LEFT, 0);
            canvas.EndLayer();
            
            PdfLayer not_printed = new PdfLayer("not printed", pdfDoc);
            not_printed.SetOnPanel(false);
            not_printed.SetPrint("Print", false);
            
            canvas.BeginLayer(not_printed);
            canvasModel.ShowTextAligned("PRINT THIS PAGE", 300, 700, TextAlignment.CENTER, 
                    (float)MathUtil.ToRadians(90));
            canvas.EndLayer();
            
            PdfLayer zoom = new PdfLayer("Zoom 0.75-1.25", pdfDoc);
            zoom.SetOnPanel(false);
            zoom.SetZoom(0.75f, 1.25f);
            
            canvas.BeginLayer(zoom);
            canvasModel.ShowTextAligned("Only visible if the zoomfactor is between 75 and 125%", 
                    30, 530, TextAlignment.LEFT, (float)MathUtil.ToRadians(90));
            canvas.EndLayer();
            
            pdfDoc.Close();
        }
    }
}
