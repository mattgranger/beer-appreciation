namespace System
{
    public static class StringExtensions
    {
        public static string[] SplitCsvRows(this string csvData)
        {
            var csvRows = csvData.Split(Environment.NewLine.ToCharArray());
            return csvRows;
        }

        public static string GetHeaderRow(this string csvData)
        {
            return csvData.Substring(0, csvData.IndexOf(Environment.NewLine, StringComparison.Ordinal));
        }
    }
}
