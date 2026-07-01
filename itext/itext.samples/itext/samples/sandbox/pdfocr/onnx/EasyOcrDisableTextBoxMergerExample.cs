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
    /// <summary>EasyOcrDisableTextBoxMergerExample.cs</summary>
    /// <remarks>
    /// EasyOcrDisableTextBoxMergerExample.cs
    /// <para />
    /// This example demonstrates how to perform OCR using
    /// <see cref="iText.Pdfocr.Onnx.OnnxOcrEngine"/>
    /// and EasyOCR ML-models
    /// for the given list of input images disabling text box merging algorithm
    /// (see
    /// <see cref="iText.Pdfocr.Onnx.Merging.EasyOcrTextBoxMerger"/>
    /// ).
    /// <para />
    /// Required software: iText 9.6.0, pdfOCR-Onnx 5.0.0
    /// (itext.pdfocr.onnx.cpu dependency to execute ONNX models on CPU or
    /// itext.pdfocr.onnx.abstract and Microsoft.ML.OnnxRuntime.Gpu dependencies to execute ONNX models on GPU).
    /// </remarks>
    public class EasyOcrDisableTextBoxMergerExample {
        public const String DEST = "results/sandbox/pdfocr/onnx/EasyOcrDisableTextBoxMergerExample/result.pdf";

        private const String BASIC_IMAGE = "../../../resources/img/ocrExample.png";

        private const String MODELS = "../../../resources/models/easyocr/";

        private const String DETECTION = MODELS + "craft_mlt_25k.onnx";

        private const String RECOGNITION = MODELS + "latin_g2.onnx";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new EasyOcrDisableTextBoxMergerExample().Manipulate(DEST);
        }

        protected internal virtual void Manipulate(String destination) {
            // Temporary default EasyOCR detection predictor properties are used to easily create custom ones:
            OnnxDetectionPredictorProperties tempProperties = OnnxDetectionPredictorProperties.EasyOcr(DETECTION);
            // Custom EasyOCR detection predictor properties:
            OnnxDetectionPredictorProperties properties = new OnnxDetectionPredictorProperties(
                tempProperties.GetModelPath(), tempProperties.GetInputProperties(), 
                new EasyOcrDisableTextBoxMergerExample.TextBoxMergeAgnosticDetectionPostProcessor(),
                tempProperties.GetOrtSessionOptionsCreator());
            IDetectionPredictor detectionPredictor = new OnnxDetectionPredictor(properties);
            IRecognitionPredictor recognitionPredictor = 
                OnnxRecognitionPredictor.EasyOcr(RECOGNITION, EasyOcrMapper.LATIN_G2);

            // OnnxOcrEngine shall be closed after usage to avoid native allocations leak.
            // It will also close all predictors used for its creation.
            using (OnnxOcrEngine ocrEngine = new OnnxOcrEngine(detectionPredictor, recognitionPredictor)) {
                OcrPdfCreator pdfCreator = new OcrPdfCreator(ocrEngine, new OcrPdfCreatorProperties()
                    .SetTextBBoxColor(DeviceCmyk.CYAN));
                pdfCreator.CreatePdf(new List<FileInfo> { new FileInfo(BASIC_IMAGE) }, new PdfWriter(destination))
                    .Close();
            }
        }

        private class TextBoxMergeAgnosticDetectionPostProcessor : EasyOcrDetectionPostProcessor {
            protected override IList<iText.Kernel.Geom.Point[]> ApplyTextBoxMerger(
                IList<iText.Kernel.Geom.Point[]> detectedTextBoxes) {
                // Don't merge detected text boxes and return them as is.
                return detectedTextBoxes;
            }
        }
    }
}
