using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Pdfocr;
using iText.Pdfocr.Onnx;
using iText.Pdfocr.Onnx.Detection;
using iText.Pdfocr.Onnx.Orientation;
using iText.Pdfocr.Onnx.Recognition;
using iText.Pdfocr.Util;

namespace iText.Samples.Sandbox.Pdfocr.Onnx {
    /// <summary>PdfOcrOnnxDisableArbitraryRotationExample.cs</summary>
    /// <remarks>
    /// PdfOcrOnnxDisableArbitraryRotationExample.cs
    /// <para />
    /// This example demonstrates how to disable arbitrary rotation for OCR result for the given list of input images.
    /// As a result of that particular example, only 0, 90, 180 and 270 degrees text rotation will be used.
    /// <para />
    /// Required software: iText 9.6.0, pdfOCR-Onnx 5.0.0
    /// (itext.pdfocr.onnx.cpu dependency to execute ONNX models on CPU or
    /// itext.pdfocr.onnx.abstract and Microsoft.ML.OnnxRuntime.Gpu dependencies to execute ONNX models on GPU).
    /// </remarks>
    public class PdfOcrOnnxDisableArbitraryRotationExample {
        public const String DEST = "results/sandbox/pdfocr/onnx/PdfOcrOnnxDisableArbitraryRotationExample/result.pdf";

        private const String BASIC_IMAGE = "../../../resources/img/ocrExample.png";

        private const String MODELS = "../../../resources/models/";

        private const String FAST = MODELS + "rep_fast_tiny-28867779.onnx";

        private const String CRNNVGG16 = MODELS + "crnn_vgg16_bn-662979cc.onnx";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new PdfOcrOnnxDisableArbitraryRotationExample().Manipulate(DEST);
        }

        protected internal virtual void Manipulate(String destination) {
            IDetectionPredictor detectionPredictor = OnnxDetectionPredictor.Fast(FAST);
            IRecognitionPredictor recognitionPredictor = OnnxRecognitionPredictor.CrnnVgg16(CRNNVGG16);

            // OnnxOcrEngine shall be closed after usage to avoid native allocations leak.
            // It will also close all predictors used for its creation.
            using (OnnxOcrEngine ocrEngine = new PdfOcrOnnxDisableArbitraryRotationExample.RotationAgnosticOnnxOcrEngine(
                       detectionPredictor, null, recognitionPredictor, new OnnxEngineProperties()
                           .SetTextPositioning(iText.Pdfocr.Onnx.Text.TextPositioning.BY_WORDS))) {
                OcrPdfCreator pdfCreator = new OcrPdfCreator(ocrEngine, new OcrPdfCreatorProperties()
                    .SetTextColor(DeviceCmyk.CYAN).SetTextBBoxColor(DeviceCmyk.CYAN));
                pdfCreator.CreatePdf(new List<FileInfo> { new FileInfo(BASIC_IMAGE) }, new PdfWriter(destination))
                    .Close();
            }
        }

        /// <summary>
        /// Implementation of the
        /// <see cref="iText.Pdfocr.Onnx.OnnxOcrEngine"/>
        /// supporting only 0, 90, 180 and 270 degrees text rotation.
        /// </summary>
        public class RotationAgnosticOnnxOcrEngine : OnnxOcrEngine {
            /// <summary>Create a new OCR engine with the provided predictors.</summary>
            /// <param name="detectionPredictor">text detector. For an input image it outputs a list of text boxes</param>
            /// <param name="orientationPredictor">
            /// text orientation predictor. For an input image, which is a tight crop of text,
            /// it outputs its orientation in 90 degrees steps. Can be null, in that case all text
            /// is assumed to be upright
            /// </param>
            /// <param name="recognitionPredictor">
            /// text recognizer. For an input image, which is a tight crop of text, it outputs
            /// the displayed string
            /// </param>
            /// <param name="properties">set of properties</param>
            public RotationAgnosticOnnxOcrEngine(IDetectionPredictor detectionPredictor, 
                IOrientationPredictor orientationPredictor, IRecognitionPredictor recognitionPredictor, 
                OnnxEngineProperties properties) 
                : base(detectionPredictor, orientationPredictor, recognitionPredictor, properties) {
            }

            public override IDictionary<int, IList<TextInfo>> DoImageOcr(FileInfo input, 
                OcrProcessContext ocrProcessContext) {
                return PdfOcrTextBuilder.CorrectRotationAngle(base.DoImageOcr(input, ocrProcessContext));
            }

            public override IDictionary<int, IList<TextInfo>> DoImageOcr(IList<FileInfo> inputs, 
                OcrProcessContext ocrProcessContext) {
                return PdfOcrTextBuilder.CorrectRotationAngle(base.DoImageOcr(inputs, ocrProcessContext));
            }
        }
    }
}
