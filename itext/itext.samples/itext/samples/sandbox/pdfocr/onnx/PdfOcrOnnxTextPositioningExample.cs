using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Pdfocr;
using iText.Pdfocr.Onnx;
using iText.Pdfocr.Onnx.Detection;
using iText.Pdfocr.Onnx.Recognition;

namespace iText.Samples.Sandbox.Pdfocr.Onnx {
    /// <summary>PdfOcrOnnxTextPositioningExample.java</summary>
    /// <remarks>
    /// PdfOcrOnnxTextPositioningExample.java
    /// <para />
    /// This example demonstrates how to define the way text is retrieved from ocr engine output
    /// specifying
    /// <see cref="iText.Pdfocr.Onnx.Text.TextPositioning"/>
    /// in
    /// <see cref="OnnxEngineProperties"/>
    /// in order to perform OCR
    /// using provided
    /// <see cref="OnnxOcrEngine"/>
    /// for the given images and save output to a PDF file.
    /// <para />
    ///  Also, this example demonstrates how to show the recognition result using
    /// <see cref="iText.Pdfocr.OcrPdfCreatorProperties"/>
    /// to set color for recognized text.
    /// <para />
    /// Required software: iText 9.3.0, pdfOCR-Onnx 5.0.0.
    /// </remarks>
    public class PdfOcrOnnxTextPositioningExample {
        public const String DEST = "results/sandbox/pdfocr/onnx/PdfOcrOnnxTextPositioningExample/result.pdf";

        private const String IMAGE = "../../../resources/img/scanned.png";

        private const String MODELS = "../../../resources/models/";

        private const String FAST = MODELS + "rep_fast_tiny-28867779.onnx";

        private const String CRNNVGG16 = MODELS + "crnn_vgg16_bn-662979cc.onnx";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new PdfOcrOnnxTextPositioningExample().Manipulate();
        }

        protected internal virtual void Manipulate() {
            IList<FileInfo> images = new List<FileInfo> { new FileInfo(IMAGE) };
            IDetectionPredictor detectionPredictor = OnnxDetectionPredictor.Fast(FAST);
            IRecognitionPredictor recognitionPredictor = OnnxRecognitionPredictor.CrnnVgg16(CRNNVGG16);
            // It is possible to specify text positioning mode through OnnxEngineProperties.
            // Default value is BY_WORDS_AND_LINES.
            using (OnnxOcrEngine ocrEngine = new OnnxOcrEngine(detectionPredictor, null, recognitionPredictor, new 
                OnnxEngineProperties().SetTextPositioning(iText.Pdfocr.Onnx.Text.TextPositioning.BY_WORDS))) {
                // Set green text color to show the recognition result. Skip that step for real usages.
                OcrPdfCreatorProperties ocrPdfCreatorProperties = new OcrPdfCreatorProperties().SetTextLayerName("OnnxTR by lines example"
                    ).SetTextColor(ColorConstants.GREEN);
                OcrPdfCreator pdfCreator = new OcrPdfCreator(ocrEngine, ocrPdfCreatorProperties);
                pdfCreator.CreatePdf(images, new PdfWriter(DEST)).Close();
            }
        }
    }
}
