/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using iText.Html2pdf.Css.Apply;
using iText.Html2pdf.Css.Apply.Impl;
using iText.Html2pdf.Html;
using iText.StyledXmlParser.Node;

namespace iText.Samples.Pdfhtml.Colorblindness
{
    public class ColorBlindnessCssApplierFactory : DefaultCssApplierFactory
    {
        // Color blindness type
        private String colorType;

        public ColorBlindnessCssApplierFactory(String colorType)
        {
            this.colorType = colorType;
        }

        public override ICssApplier GetCustomCssApplier(IElementNode tag)
        {
            if (tag.Name().Equals(TagConstants.DIV))
            {
                ColorBlindBlockCssApplier applier = new ColorBlindBlockCssApplier();
                applier.SetColorBlindness(colorType);
                return applier;
            }

            if (tag.Name().Equals(TagConstants.SPAN))
            {
                ColorBlindSpanTagCssApplier applier = new ColorBlindSpanTagCssApplier();
                applier.SetColorBlindness(colorType);
                return applier;
            }

            return null;
        }
    }
}