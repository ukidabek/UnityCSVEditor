using UnityEngine;

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

        public const int DEFAULT_COLUMN_WIDTH = 100;
        public const int DEFAULT_ROW_HEIGHT = 20;

        public const int COLUMN_RESIZE_RECT_WIDTCH = 5;
        public const int ROW_RESIZE_RECT_HEIGHT = 5;

        public const int FOR_MENU_GAP = 20;
        public const int FOR_VERTICAL_SCROLL_BAR_GAP = 42;

        public static readonly Rect SAVE_BUTTON_RECT = new Rect(Vector2.zero, new Vector2(100, FOR_MENU_GAP));
    }
}