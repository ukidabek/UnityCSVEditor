﻿using UnityEngine;

namespace CSVEditor
{
    internal class CSVEditorWindowConsts
    {
        #region Strings
        
        public static readonly string [] ROW_SEPARATORS = { "\r\n", "\n" };
        public static readonly string [] COLUMN_SEPARATORS = { ";" };
        public static readonly string [] PATH_SEPARATORS = { "/" };

        public static readonly string [] META_DATA_FILE_PARAMETER_SEPARATORS = { "=" };

        public const string CSV_EDITOR_WINDOW_TITLE = "CSV Editor";

        public const string META_DATA_FILE_NAME = "CSVEditorData.metadata";
        public const string PATH_MARKER = "Path";

        public const string CSV_EDITOR_MAIN_WINDOW_MENU_ITEM = "CSV Editor/Open";
        public const string CSV_EDITOR_NEW_FILE_MENU_ITEM = "CSV Editor/New";

        public const string MATA_DATA_PARAMETR_FORMAT = "{0}={1}{2}";

        public const string OPEN_BUTTON_TEXT = "Open";
        public const string SAVE_BUTTON_TEXT = "Save";

        #endregion

        public const string ADD_COLUMN_CONTEXT_MENU_ELEMENT = "Add/Column";
        public const string ADD_ROW_CONTEXT_MENU_ELEMENT = "Add/Row";

        public const string INSERT_COLUMN_CONTEXT_MENU_ELEMENT = "Insert/Column";
        public const string INSERT_ROW_CONTEXT_MENU_ELEMENT = "Insert/Row";

        public const int DEFAULT_COLUMN_WIDTH = 100;
        public const int DEFAULT_ROW_HEIGHT = 20;

        public const int COLUMN_RESIZE_RECT_WIDTCH = 5;
        public const int ROW_RESIZE_RECT_HEIGHT = 5;

        public const int FOR_MENU_GAP = 20;
        public const int FOR_VERTICAL_SCROLL_BAR_GAP = 42;

        public const int DEFAULT_BUTTON_WIDTCH = 100;

        public static readonly Rect SAVE_BUTTON_RECT = new Rect(Vector2.zero, new Vector2(DEFAULT_BUTTON_WIDTCH, FOR_MENU_GAP));
    }
}