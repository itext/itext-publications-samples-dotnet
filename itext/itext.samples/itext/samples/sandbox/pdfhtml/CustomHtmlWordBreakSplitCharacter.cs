using System;
using System.IO;
using iText.Html2pdf;
using iText.Html2pdf.Attach;
using iText.Html2pdf.Attach.Impl.Tags;
using iText.Html2pdf.Css.Apply;
using iText.Html2pdf.Css.Apply.Impl;
using iText.IO.Font.Otf;
using iText.Layout;
using iText.Layout.Properties;
using iText.Layout.Splitting;
using iText.StyledXmlParser.Node;

namespace iText.Samples.Sandbox.Pdfhtml
{
   
    // CustomHtmlWordBreakSplitCharacter.cs
    //
    // Example showing custom word breaking at slash characters in HTML to PDF.
    // Demonstrates custom CSS applier and split character implementation.
 
    public class CustomHtmlWordBreakSplitCharacter
    {
        public static readonly String SRC =
            "../../../resources/pdfhtml/CustomHtmlWordBreakSplitCharacter/CustomHtmlWordBreakSplitCharacter.html";

        public static readonly String DEST = "results/sandbox/pdfhtml/CustomHtmlWordBreakSplitCharacter.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new CustomHtmlWordBreakSplitCharacter().ManipulatePdf(SRC, DEST);
        }

        protected void ManipulatePdf(String src, String dest)
        {
            ConverterProperties converterProperties = new ConverterProperties();

            // Set custom css applier factory instance, that provides a functionality
            // to use custom word break split character setting in every default CSS applier.
            converterProperties.SetCssApplierFactory(new CustomCssApplierFactory());

            HtmlConverter.ConvertToPdf(new FileInfo(src), new FileInfo(dest), converterProperties);
        }

        private class CustomCssApplierFactory : ICssApplierFactory
        {
            private ICssApplierFactory defaultCssApplierFactory = new DefaultCssApplierFactory();

            public ICssApplier GetCssApplier(IElementNode tag)
            {
                ICssApplier defaultCssApplier = defaultCssApplierFactory.GetCssApplier(tag);

                return defaultCssApplier != null ? new CustomCssApplier(defaultCssApplier) : null;
            }
        }

        private class CustomCssApplier : ICssApplier
        {
            private readonly ICssApplier defaultCssApplier;

            public CustomCssApplier(ICssApplier defaultCssApplier)
            {
                this.defaultCssApplier = defaultCssApplier;
            }

            public void Apply(ProcessorContext context, IStylesContainer stylesContainer, ITagWorker tagWorker)
            {
                defaultCssApplier.Apply(context, stylesContainer, tagWorker);
                IPropertyContainer elementResult = tagWorker.GetElementResult();
                if (elementResult != null)
                {
                    SetCustomSplitCharacter(elementResult);
                }
                else if (tagWorker is SpanTagWorker)
                {
                    // If current element is span, then set the custom split character to nested elements
                    foreach (IPropertyContainer ownLeafElement in ((SpanTagWorker) tagWorker).GetOwnLeafElements())
                    {
                        SetCustomSplitCharacter(ownLeafElement);
                    }
                }
            }

            private void SetCustomSplitCharacter(IPropertyContainer elementResult)
            {
                // If Property.SPLIT_CHARACTERS was null then DefaultSplitCharacters class would be used during layout.
                Object property = elementResult.GetProperty<ISplitCharacters>(Property.SPLIT_CHARACTERS);
                
                //  BreakAllSplitCharacters and KeepAllSplitCharacters instances can be set
                //  if CSS word-break property was applied.
                if (!(property is BreakAllSplitCharacters))
                {
                    elementResult.SetProperty(Property.SPLIT_CHARACTERS, new CustomSlashSplitCharacters());
                }
            }
        }

        private class CustomSlashSplitCharacters : DefaultSplitCharacters
        {
            private static readonly String SPLIT_CHARACTER = "/";

            public override bool IsSplitCharacter(GlyphLine text, int glyphPos)
            {
                Glyph glyph = text.Get(glyphPos);
                return glyph.HasValidUnicode() && SPLIT_CHARACTER.Equals(glyph.GetUnicodeString())
                       || base.IsSplitCharacter(text, glyphPos);
            }
        }
    }
}