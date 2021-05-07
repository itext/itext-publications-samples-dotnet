using System;
using System.Collections.Generic;
using System.IO;
using iText.Commons.Utils;

namespace iText.Highlevel.Util {
    /// <author>iText</author>
    public class CsvTo2DList {
        public static IList<IList<String>> Convert(String src, String separator) {
            IList<IList<String>> resultSet = new List<IList<String>>();
            StreamReader sr = File.OpenText(src);
            String line;
            IList<string> record;
            while ((line = sr.ReadLine()) != null) {
                StringTokenizer tokenizer = new StringTokenizer(line, separator);
                record = new List<string>();
                while (tokenizer.HasMoreTokens()) {
                    record.Add(tokenizer.NextToken());
                }
                resultSet.Add(record);
            }
            return resultSet;
        }
    }
}
