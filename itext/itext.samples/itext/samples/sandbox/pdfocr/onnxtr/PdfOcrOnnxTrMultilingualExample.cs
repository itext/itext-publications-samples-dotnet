using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Pdfocr;
using iText.Pdfocr.Onnxtr;
using iText.Pdfocr.Onnxtr.Detection;
using iText.Pdfocr.Onnxtr.Orientation;
using iText.Pdfocr.Onnxtr.Recognition;

namespace iText.Samples.Sandbox.Pdfocr.Onnxtr {
    /// <summary>PdfOcrOnnxTrMultilingualExample.java</summary>
    /// <remarks>
    /// PdfOcrOnnxTrMultilingualExample.java
    /// <para />
    /// This example demonstrates how to perform OCR using
    /// <c>onnxtr-parseq-multilingual-v1.onnx</c>
    /// recognition model for the given list of input images with different latin languages.
    /// <para />
    ///  Also, this example demonstrates how to show the recognition result using
    /// <see cref="iText.Pdfocr.OcrPdfCreatorProperties"/>
    /// to set color for recognized text.
    /// <para />
    /// Required software: iText 9.3.0, pdfOCR-OnnxTR 4.1.0.
    /// </remarks>
    public class PdfOcrOnnxTrMultilingualExample {
        public const String DEST = "results/sandbox/pdfocr/onnxtr/PdfOcrOnnxTrMultilingualExample/result.pdf";

        private const String FRENCH = "../../../resources/img/french.png";

        private const String GERMAN = "../../../resources/img/german.jpg";

        private const String SPANISH = "../../../resources/img/spanish.jpg";

        private const String MODELS = "../../../resources/models/";

        private const String FAST = MODELS + "rep_fast_tiny-28867779.onnx";

        private const String MOBILENETV3 = MODELS + "mobilenet_v3_small_crop_orientation-5620cf7e.onnx";

        private const String MULTILANG = MODELS + "onnxtr-parseq-multilingual-v1.onnx";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new PdfOcrOnnxTrMultilingualExample().Manipulate();
        }

        protected internal virtual void Manipulate() {
            IList<FileInfo> images = new List<FileInfo> { 
                new FileInfo(FRENCH), new FileInfo(GERMAN), new FileInfo(SPANISH)
            };
            IDetectionPredictor detectionPredictor = OnnxDetectionPredictor.Fast(FAST);
            IOrientationPredictor orientationPredictor = OnnxOrientationPredictor.MobileNetV3(MOBILENETV3);
            // This PARSeq model supports latin languages/symbols collected into Vocabulary.LATIN_EXTENDED.
            IRecognitionPredictor recognitionPredictor = OnnxRecognitionPredictor.ParSeq(MULTILANG, Vocabulary.LATIN_EXTENDED
                , 0);
            using (OnnxTrOcrEngine ocrEngine = new OnnxTrOcrEngine(detectionPredictor, orientationPredictor, recognitionPredictor
                )) {
                // Set green text color to show the recognition result. Skip that step for real usages.
                OcrPdfCreatorProperties ocrPdfCreatorProperties = new OcrPdfCreatorProperties().SetTextLayerName("OnnxTR multilingual example"
                    ).SetTextColor(ColorConstants.GREEN);
                OcrPdfCreator pdfCreator = new OcrPdfCreator(ocrEngine, ocrPdfCreatorProperties);
                using (PdfWriter writer = new PdfWriter(DEST)) {
                    pdfCreator.CreatePdf(images, writer).Close();
                }
            }
        }
    }
}
