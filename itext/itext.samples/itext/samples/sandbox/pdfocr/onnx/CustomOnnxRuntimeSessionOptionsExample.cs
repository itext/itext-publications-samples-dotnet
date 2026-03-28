using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML.OnnxRuntime;
using iText.Kernel.Pdf;
using iText.Pdfocr;
using iText.Pdfocr.Onnx;
using iText.Pdfocr.Onnx.Detection;
using iText.Pdfocr.Onnx.Recognition;

namespace iText.Samples.Sandbox.Pdfocr.Onnx {
    /// <summary>CustomOnnxRuntimeSessionOptionsExample.cs</summary>
    /// <remarks>
    /// CustomOnnxRuntimeSessionOptionsExample.cs
    /// <para />
    /// This example demonstrates how to provide custom
    /// <see cref="Microsoft.ML.OnnxRuntime.SessionOptions"/>
    /// used to construct
    /// <see cref="Microsoft.ML.OnnxRuntime.OrtSession"/>
    /// which wraps an ONNX model and allows inference calls.
    /// This will allow to specify whether to run OCR on GPU or CPU,
    /// execution mode, optimization level and other options.
    /// <para />
    /// In order to run models on GPU, add itext.pdfocr.onnx.abstract and Microsoft.ML.OnnxRuntime.Gpu dependencies.
    /// <see cref="iText.Pdfocr.Onnx.DefaultOrtSessionOptionsCreator"/>
    /// supports GPU mode by default,
    /// so no additional changes required unless you want to set up some custom options.
    /// <para />
    /// Required software: iText 9.6.0, pdfOCR-Onnx 5.0.0
    /// (itext.pdfocr.onnx.cpu dependency to execute ONNX models on CPU or
    /// itext.pdfocr.onnx.abstract and Microsoft.ML.OnnxRuntime.Gpu dependencies to execute ONNX models on GPU).
    /// </remarks>
    public class CustomOnnxRuntimeSessionOptionsExample {
        public const String DEST = "results/sandbox/pdfocr/onnx/CustomOnnxRuntimeSessionOptionsExample/result.pdf";

        private const String BASIC_IMAGE = "../../../resources/img/ocrExample.png";

        private const String MODELS = "../../../resources/models/paddleocr/";

        private const String DET = MODELS + "PP-OCRv5_mobile_det_infer";

        private const String REC = MODELS + "PP-OCRv5_mobile_rec_infer";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new CustomOnnxRuntimeSessionOptionsExample().Manipulate(DEST);
        }

        protected internal virtual void Manipulate(String destination) {
            // Create custom IOrtSessionOptionsCreator and use it to create predictors.
            IOrtSessionOptionsCreator sessionOptionsCreator = 
                new CustomOnnxRuntimeSessionOptionsExample.CustomOrtSessionOptionsCreator();

            IDetectionPredictor detectionPredictor = OnnxDetectionPredictor.PaddleOcr(DET, sessionOptionsCreator);
            IRecognitionPredictor recognitionPredictor = OnnxRecognitionPredictor.PaddleOcr(REC, sessionOptionsCreator);

            // OnnxOcrEngine shall be closed after usage to avoid native allocations leak.
            // It will also close all predictors used for its creation.
            using (OnnxOcrEngine ocrEngine = new OnnxOcrEngine(detectionPredictor, recognitionPredictor)) {
                OcrPdfCreator pdfCreator = new OcrPdfCreator(ocrEngine);
                pdfCreator.CreatePdf(new List<FileInfo> { new FileInfo(BASIC_IMAGE) }, new PdfWriter(destination))
                    .Close();
            }
        }

        /// <summary>
        /// Implementation of
        /// <see cref="iText.Pdfocr.Onnx.IOrtSessionOptionsCreator"/>.
        /// </summary>
        /// <remarks>
        /// Implementation of
        /// <see cref="iText.Pdfocr.Onnx.IOrtSessionOptionsCreator"/>.
        /// <para />
        /// <c>CUDA</c>
        /// execution provider is added if available, otherwise default
        /// <c>CPU</c>
        /// execution provider is used.
        /// </remarks>
        public class CustomOrtSessionOptionsCreator : IOrtSessionOptionsCreator {
            public virtual SessionOptions Create() {
                SessionOptions ortOptions = new SessionOptions();
                try {
                    if (OrtEnv.Instance().GetAvailableProviders().Contains("CUDAExecutionProvider")) {
                        // Use CUDA provider to run OCR on GPU.
                        ortOptions.AppendExecutionProvider_CUDA(0);    
                    }
                    else {
                        ortOptions.AppendExecutionProvider_CPU();    
                        ortOptions.IntraOpNumThreads = -1;
                        ortOptions.InterOpNumThreads = -1;
                    }
                    ortOptions.ExecutionMode = ExecutionMode.ORT_SEQUENTIAL;
                    ortOptions.GraphOptimizationLevel = GraphOptimizationLevel.ORT_ENABLE_ALL;
                    return ortOptions;
                } catch (Exception e) {
                    ortOptions.Close();
                    throw;
                }
            }
        }
    }
}
