using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

namespace CSVEditor
{
    internal class CSVEditorEditWindow : EditorWindow
    {
        private List<int> _columnSize = new List<int>();
        private List<int> _rowSize = new List<int>();

        private int _columnToResizeIndex = 0;
        private int _rowToResizeIndex = 0;

        private bool _reSizeColumn = false;
        private bool _reSizeRow = false;
        private bool _insertColumn = false;
        private bool _insertRow = false;

        private int _firstIndex = -1;
        private int _lastIndex = 0;

        private List<Rect> _columnResizeRectList = new List<Rect>();
        private List<Rect> _rowResizeRectList = new List<Rect>();

        private Vector2 scrollPosition = Vector2.zero;
        private Rect windowRect = Rect.zero;
        private Rect windowScrollRect = Rect.zero;

        private CSVFileParser _parser = null;

        private GenericMenu _contextMenu = new GenericMenu();
        private GenericMenu _insertRowContextMenu = new GenericMenu();
        private GenericMenu _insertColumnContextMenu = new GenericMenu();

        public CSVEditorEditWindow(CSVFileParser parser)
        {
            _parser = parser;
            this.wantsMouseMove = true;

            GUIContent guiContent = new GUIContent(_parser.FileName);
            this.titleContent = guiContent;

            guiContent = new GUIContent(CSVEditorWindowConsts.ADD_COLUMN_CONTEXT_MENU_ELEMENT);
            _contextMenu.AddItem(guiContent, false, AddColumn);
            guiContent = new GUIContent(CSVEditorWindowConsts.ADD_ROW_CONTEXT_MENU_ELEMENT);
            _contextMenu.AddItem(guiContent, false, AddRow);

            guiContent = new GUIContent(CSVEditorWindowConsts.INSERT_ROW_CONTEXT_MENU_ELEMENT);
            _insertRowContextMenu.AddItem(guiContent, false, InsertRow);
            guiContent = new GUIContent(CSVEditorWindowConsts.INSERT_COLUMN_CONTEXT_MENU_ELEMENT);
            _insertColumnContextMenu.AddItem(guiContent, false, InsertColumn);

            for (int i = 0; i < _parser.ColumnsCount; i++)
            {
                _columnSize.Add(CSVEditorWindowConsts.DEFAULT_COLUMN_WIDTH);
            }

            for (int i = 0; i < _parser.RowsCount; i++)
            {
                _rowSize.Add(CSVEditorWindowConsts.DEFAULT_ROW_HEIGHT);
            }

            CalculateReSizeRects();
        }

        private void CalculateReSizeRects()
        {
            int _totalRowsLenght = 0;

            for (int i = 0; i < _rowSize.Count; i++)
            {
                _totalRowsLenght += _rowSize[i];
            }

            _totalRowsLenght += CSVEditorWindowConsts.ROW_RESIZE_RECT_HEIGHT * _rowSize.Count + CSVEditorWindowConsts.FOR_MENU_GAP;

            _columnResizeRectList.Clear();
            int totalWidtch = 0;
            for (int i = 0; i < _parser.ColumnsCount; i++)
            {
                _columnResizeRectList.Add(new Rect(
                    totalWidtch + _columnSize[i] + (CSVEditorWindowConsts.COLUMN_RESIZE_RECT_WIDTCH * i), 
                    0, 
                    CSVEditorWindowConsts.COLUMN_RESIZE_RECT_WIDTCH,
                    _totalRowsLenght));

                totalWidtch += _columnSize[i];
            }

            totalWidtch += CSVEditorWindowConsts.COLUMN_RESIZE_RECT_WIDTCH * _parser.ColumnsCount;

            windowScrollRect = new Rect(Vector2.zero, new Vector2(totalWidtch, _totalRowsLenght));

            _totalRowsLenght = CSVEditorWindowConsts.FOR_MENU_GAP;
            _rowResizeRectList.Clear();
            for (int i = 0; i < _rowSize.Count; i++)
            {
                _rowResizeRectList.Add(new Rect(
                    0,
                    _totalRowsLenght + _rowSize[i] + (CSVEditorWindowConsts.ROW_RESIZE_RECT_HEIGHT * i),
                    totalWidtch,
                    CSVEditorWindowConsts.ROW_RESIZE_RECT_HEIGHT));
                _totalRowsLenght += _rowSize[i];
            }
        }

        public void AddColumn()
        {
            _parser.AddColumn();
            _columnSize.Add(CSVEditorWindowConsts.DEFAULT_COLUMN_WIDTH);
        }

        public void AddRow()
        {
            _parser.AddRow();
            _rowSize.Add(CSVEditorWindowConsts.DEFAULT_ROW_HEIGHT);
        }

        public void InsertRow()
        {
            _insertRow = false;
            _parser.InsertRow(_rowToResizeIndex + 1);
            _rowSize.Insert(_rowToResizeIndex + 1, CSVEditorWindowConsts.DEFAULT_ROW_HEIGHT);
        }

        public void InsertColumn()
        {
            _insertRow = false;
            _parser.InsertColumn(_columnToResizeIndex + 1);
            _columnSize.Insert(_columnToResizeIndex + 1, CSVEditorWindowConsts.DEFAULT_COLUMN_WIDTH);
        }

        private void CheckReSizeRectHit(Vector2 mousePosition, ref bool row, ref bool column)
        {
            Rect mouseRect = new Rect(mousePosition, Vector2.one);

            for (int i = 0; i < _columnResizeRectList.Count; i++)
            {
                if (_columnResizeRectList[i].Overlaps(mouseRect))
                {
                    _columnToResizeIndex = i;
                    column = true;
                }
            }

            mouseRect.x += scrollPosition.x;
            mouseRect.y += scrollPosition.y;

            for (int i = _firstIndex; i < _lastIndex; i++)
            {
                Rect rr = _rowResizeRectList[i];
                if (_rowResizeRectList[i].Overlaps(mouseRect))
                {
                    _rowToResizeIndex = i;
                    row = true;
                }
            }
        }

        private void OnGUI()
        {
            if(GUI.Button(CSVEditorWindowConsts.SAVE_BUTTON_RECT, CSVEditorWindowConsts.SAVE_BUTTON_TEXT))
            {
                _parser.ToCSV();
            }

            windowRect = new Rect(
                new Vector2(0, CSVEditorWindowConsts.FOR_MENU_GAP), 
                new Vector2(Screen.width, Screen.height - CSVEditorWindowConsts.FOR_VERTICAL_SCROLL_BAR_GAP));

            switch(Event.current.type)
            {
                case EventType.MouseDown:
                    if(Event.current.button == 0)
                    {
                        CheckReSizeRectHit(Event.current.mousePosition, ref _reSizeRow, ref _reSizeColumn);
                    }

                    if (Event.current.button == 1)
                    {
                        CheckReSizeRectHit(Event.current.mousePosition, ref _insertRow, ref _insertColumn);

                        if(_insertRow)
                        {
                            _insertRowContextMenu.ShowAsContext();
                        }
                        else if(_insertColumn)
                        {
                            _insertColumnContextMenu.ShowAsContext();
                        }
                        else
                        {
                            _contextMenu.ShowAsContext();
                        }

                        _insertRow = false;
                        _insertColumn = false;
                    }
                    break;

                case EventType.MouseUp:
                    if (Event.current.button == 0)
                    {
                        _reSizeColumn = false;
                        _reSizeRow = false;

                        CalculateReSizeRects();
                    }
                    break;

                case EventType.MouseDrag:
                    if(_reSizeColumn && Event.current.button == 0)
                    {
                        _columnSize[_columnToResizeIndex] += (int)Event.current.delta.x;
                        Event.current.Use();
                    }

                    if (_reSizeRow && Event.current.button == 0)
                    {
                        _rowSize[_rowToResizeIndex] += (int)Event.current.delta.y;
                        Event.current.Use();
                    }
                    break;
            }

            scrollPosition = GUI.BeginScrollView(windowRect, scrollPosition, windowScrollRect, false, false);
            {
                _firstIndex = -1;
                _lastIndex = 0;
                int rowPosition = 0;

                for (int i = 0; i < _parser.RowsCount; i++)
                {
                    Rect rect = new Rect(new Vector2(0, rowPosition), new Vector2(0, _rowSize[i]));
                    if (scrollPosition.y <= rect.y + _rowSize[i] && (scrollPosition.y + Screen.height) >= rect.y)
                    {
                        if(_firstIndex < 0)
                        {
                            _firstIndex = i;
                        }

                        _lastIndex = i;

                        _parser.Rows[i].EditRow(rect, i, ref _columnSize);
                    }

                    rowPosition += _rowSize[i] + CSVEditorWindowConsts.ROW_RESIZE_RECT_HEIGHT;
                }
            }
            GUI.EndScrollView();
        }
    }
}