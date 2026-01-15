using System;

namespace iText.Samples.Util
{
    /// <summary>
    /// The class is used to simplify running samples locally by specifying paths to license files
    /// </summary>
    public class LicenseUtil
    {
        private readonly static String INVALID_LICENSE_FOLDER_PATH = "To run the samples locally, provide the absolute path to your %s license file - either directly or by updating %s method.";
        
        /// <summary>
        /// Method is used to get absolute path to iTextCore, PdfHtml, PdfCalligraph license file.
        /// </summary>
        /// <remarks>
        /// Used to run samples locally.
        /// If you want to run samples locally, you could either replace in sample code calling this method to absolute path of your license file or update the method.
        /// </remarks>
        /// <returns>
        /// Result license file name.
        /// </returns>
        public static String GetPathToLicenseFileWithITextCoreAndPdfHtmlAndPdfCalligraphProducts() {
            String licencePath = Environment.GetEnvironmentVariable("ITEXT_LICENSE_FILE_LOCAL_STORAGE");
            if (licencePath == null) {
                throw new ArgumentException(String.Format(INVALID_LICENSE_FOLDER_PATH, "iTextCore, PdfHtml, PdfCalligraph", "getPathToLicenseFileWithITextCoreAndPdfHtmlAndPdfCalligraphProducts"));
            }
            return licencePath + "/dev_all_products.json";
        }
        
        /// <summary>
        /// Method is used to get absolute path to iTextCore, PdfCalligraph license file.
        /// </summary>
        /// <remarks>
        /// Used to run samples locally.
        /// If you want to run samples locally, you could either replace in sample code calling this method to absolute path of your license file or update the method.
        /// </remarks>
        /// <returns>
        /// Result license file name.
        /// </returns>
        public static String GetPathToLicenseFileWithITextCoreAndPdfCalligraphProducts() {
            String licencePath = Environment.GetEnvironmentVariable("ITEXT_LICENSE_FILE_LOCAL_STORAGE");
            if (licencePath == null) {
                throw new ArgumentException(String.Format(INVALID_LICENSE_FOLDER_PATH, "iTextCore, PdfCalligraph", "getPathToLicenseFileWithITextCoreAndPdfCalligraphProducts"));
            }
            return licencePath + "/dev_all_products.json";
        }
    }
}