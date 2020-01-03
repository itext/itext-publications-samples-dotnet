/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System.Collections.Generic;
using iText.Html2pdf.Attach;
using iText.Html2pdf.Css;
using iText.Html2pdf.Css.Apply.Impl;
using iText.Kernel.Colors;
using iText.StyledXmlParser.Node;

namespace iText.Samples.Sandbox.Pdfhtml.Colorblindness
{
    public class ColorBlindSpanTagCssApplier : SpanTagCssApplier
    {
        private static readonly double RGB_MAX_VAL = 255.0;
        
        private string colorBlindness = ColorBlindnessTransforms.PROTANOPIA;
        
        /// <summary>
        /// Set the from of color blindness to simulate.
        /// </summary>
        /// <remarks>
        /// Accepted values are Protanopia, Protanomaly, Deuteranopia, Deuteranomaly, Tritanopia, Tritanomaly, Achromatopsia, Achromatomaly.
        /// Default value is Protanopia
        /// </remarks>
        /// <param name="colorBlindness"></param>
        public void SetColorBlindness(string colorBlindness)
        {
            this.colorBlindness = colorBlindness;
        }
        
        public override void Apply(ProcessorContext context, IStylesContainer stylesContainer, ITagWorker tagWorker)
        {
            IDictionary<string, string> cssStyles = stylesContainer.GetStyles();
            if (cssStyles.ContainsKey(CssConstants.COLOR))
            {
                string newColor = TransformColor(cssStyles[CssConstants.COLOR]);
                cssStyles[CssConstants.COLOR] = newColor;
                stylesContainer.SetStyles(cssStyles);
            }

            if (cssStyles.ContainsKey(CssConstants.BACKGROUND_COLOR))
            {
                string newColor = TransformColor(cssStyles[CssConstants.BACKGROUND_COLOR]);
                cssStyles[CssConstants.BACKGROUND_COLOR] = newColor;
                stylesContainer.SetStyles(cssStyles);
            }

            base.Apply(context, stylesContainer, tagWorker);
        }

        private string TransformColor(string originalColor)
        {
            float[] rgbaColor = WebColors.GetRGBAColor(originalColor);
            float[] rgbColor = {rgbaColor[0], rgbaColor[1], rgbaColor[2]};
            float[] newColorRgb = ColorBlindnessTransforms.SimulateColorBlindness(colorBlindness, rgbColor);
            float[] newColorRgba = {newColorRgb[0], newColorRgb[1], newColorRgb[2], rgbaColor[3]};
            double[] newColorArray = ScaleColorFloatArray(newColorRgba);
            string newColorString = "rgba(" + (int) newColorArray[0] + "," + (int) newColorArray[1] + ","
                                    + (int) newColorArray[2] + "," + newColorArray[3] + ")";
            return newColorString;
        }

        private double[] ScaleColorFloatArray(float[] colors)
        {
            double red = (colors[0] * RGB_MAX_VAL);
            double green = (colors[1] * RGB_MAX_VAL);
            double blue = (colors[2] * RGB_MAX_VAL);
            double[] res = {red, green, blue, (double) colors[3]};
            return res;
        }
    }
}