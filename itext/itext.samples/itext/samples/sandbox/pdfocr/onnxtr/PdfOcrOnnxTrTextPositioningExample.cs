using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Pdfocr;
using iText.Pdfocr.Onnxtr;
using iText.Pdfocr.Onnxtr.Detection;
using iText.Pdfocr.Onnxtr.Recognition;

namespace iText.Samples.Sandbox.Pdfocr.Onnxtr {
    /// <summary>PdfOcrOnnxTrTextPositioningExample.java</summary>
    /// <remarks>
    /// PdfOcrOnnxTrTextPositioningExample.java
    /// <para />
    /// This example demonstrates how to define the way text is retrieved from ocr engine output
    /// specifying
    /// <see cref="iText.Pdfocr.Onnxtr.TextPositioning"/>
    /// in
    /// <see cref="OnnxTrEngineProperties"/>
    /// in order to perform OCR
    /// using provided
    /// <see cref="OnnxTrOcrEngine"/>
    /// for the given images and save output to a PDF file.
    /// <para />
    ///  Also, this example demonstrates how to show the recognition result using
    /// <see cref="iText.Pdfocr.OcrPdfCreatorProperties"/>
    /// to set color for recognized text.
    /// <para />
    /// Required software: iText 9.3.0, pdfOCR-OnnxTR 4.1.0.
    /// </remarks>
    public class PdfOcrOnnxTrTextPositioningExample {
        public const String DEST = "results/sandbox/pdfocr/onnxtr/PdfOcrOnnxTrTextPositioningExample/result.pdf";

        private const String IMAGE = "../../../resources/img/scanned.png";

        private const String MODELS = "../../../resources/models/";

        private const String FAST = MODELS + "rep_fast_tiny-28867779.onnx";

        private const String CRNNVGG16 = MODELS + "crnn_vgg16_bn-662979cc.onnx";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new PdfOcrOnnxTrTextPositioningExample().Manipulate();
        }

        protected internal virtual void Manipulate() {
            IList<FileInfo> images = new List<FileInfo> { new FileInfo(IMAGE) };
            IDetectionPredictor detectionPredictor = OnnxDetectionPredictor.Fast(FAST);
            IRecognitionPredictor recognitionPredictor = OnnxRecognitionPredictor.CrnnVgg16(CRNNVGG16);
            // It is possible to specify text positioning mode through OnnxTrEngineProperties. Default value is BY_LINES.
            using (OnnxTrOcrEngine ocrEngine = new OnnxTrOcrEngine(detectionPredictor, null, recognitionPredictor, new 
                OnnxTrEngineProperties().SetTextPositioning(TextPositioning.BY_WORDS))) {
                // Set green text color to show the recognition result. Skip that step for real usages.
                OcrPdfCreatorProperties ocrPdfCreatorProperties = new OcrPdfCreatorProperties().SetTextLayerName("OnnxTR by lines example"
                    ).SetTextColor(ColorConstants.GREEN);
                OcrPdfCreator pdfCreator = new OcrPdfCreator(ocrEngine, ocrPdfCreatorProperties);
                pdfCreator.CreatePdf(images, new PdfWriter(DEST)).Close();
            }
        }
    }
}
