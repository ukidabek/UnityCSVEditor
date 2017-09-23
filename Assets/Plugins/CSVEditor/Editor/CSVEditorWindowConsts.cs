namespace CSVEditor
{
    internal class CSVEditorWindowConsts
    {
        #region Strings
        public const string CSV_EDITOR_WINDOW_TITLE = "CSV Editor";

        public static readonly string [] ROW_SEPARATORS = { "\r\n", "\n" };
        public static readonly string [] COLUMN_SEPARATORS = { ";" };
        public static readonly string [] PATH_SEPARATORS = { "/" };

        public static readonly string [] META_DATA_FILE_PARAMETER_SEPARATORS = { "=" };

        public const string META_DATA_FILE_NAME = "CSVEditorData.metadata";
        public const string PATH_MARKER = "Path";
        #endregion

        public const int COLUMN_RESIZE_RECT_WIDTCH = 5;
        public const int ROW_RESIZE_RECT_WIDTCH = 5;

    }
}