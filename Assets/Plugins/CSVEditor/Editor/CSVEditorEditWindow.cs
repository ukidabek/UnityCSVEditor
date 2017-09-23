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
        private bool _reSizeColumn = false;

        private List<Rect> _columnResizeRectList = new List<Rect>();

        private Vector2 scrollPosition = Vector2.zero;
        private Rect windowRect = Rect.zero;
        private Rect windowScrollRect = Rect.zero;

        private CSVFileParser _parser = null;

        public CSVEditorEditWindow(CSVFileParser parser)
        {
            _parser = parser;
            GUIContent titleContent = new GUIContent(_parser.FileName);

            this.titleContent = titleContent;

            for (int i = 0; i < _parser.ColumnsCount; i++)
            {
                _columnSize.Add(100);
            }


            for (int i = 0; i < _parser.RowsCount; i++)
            {
                _rowSize.Add(20);
            }

            this.wantsMouseMove = true;
            CalculateReSizeRects();

        }

        private void CalculateReSizeRects()
        {
            int _totalRowsLenght = 0;

            for (int i = 0; i < _rowSize.Count; i++)
            {
                _totalRowsLenght += _rowSize[i];
            }

            _totalRowsLenght += CSVEditorWindowConsts.ROW_RESIZE_RECT_WIDTCH * _rowSize.Count;

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

            windowScrollRect = new Rect(Vector2.zero, new Vector2(totalWidtch, _totalRowsLenght));

        }

        private void OnGUI()
        {
            windowRect = new Rect(new Vector2(0, 0), new Vector2(Screen.width, Screen.height - 22));

            switch(Event.current.type)
            {
                case EventType.MouseDown:
                    if(Event.current.button == 0)
                    {
                        for (int i = 0; i < _columnResizeRectList.Count; i++)
                        {
                            Rect mouseRect = new Rect(Event.current.mousePosition, Vector2.one);
                            if(_columnResizeRectList[i].Overlaps(mouseRect))
                            {
                                _columnToResizeIndex = i;
                                _reSizeColumn = true;
                            }
                        }
                    }
                    
                    break;

                case EventType.MouseUp:
                    if (Event.current.button == 0)
                    {
                        _reSizeColumn = false;
                        CalculateReSizeRects();
                    }
                    break;

                case EventType.MouseDrag:
                    if(_reSizeColumn && Event.current.button == 0)
                    {
                        _columnSize[_columnToResizeIndex] += (int)Event.current.delta.x;
                        Event.current.Use();
                    }
                    break;
            }

            scrollPosition = GUI.BeginScrollView(windowRect, scrollPosition, windowScrollRect, false, false);
            {
                for (int i = 0; i < _parser.RowsCount; i++)
                {
                    Rect rect = new Rect(new Vector2(0, 20 * i), new Vector2(100, 20));
                    if (scrollPosition.y <= rect.y + 20 && (scrollPosition.y + Screen.height - 40) >= rect.y)
                    {
                        _parser.Rows[i].EditRow(rect, i, ref _columnSize);
                    }
                }
            }
            GUI.EndScrollView();
        }
    }
}