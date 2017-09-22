using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

namespace CSVEditor
{
    internal class CSVEditorEditWindow : EditorWindow
    {
        private Vector2 scrollPos = Vector2.zero;
        private Rect windowRect = Rect.zero;
        private Rect windowScrollRect = Rect.zero;

        private CSVFileParser _parser = null;

        public CSVEditorEditWindow(CSVFileParser parser)
        {
            _parser = parser;
            GUIContent titleContent = new GUIContent(_parser.FileName);

            this.titleContent = titleContent;
        }

        private void OnGUI()
        {
            windowRect = new Rect(new Vector2(0, 0), new Vector2(Screen.width, Screen.height));
            windowScrollRect = new Rect(Vector2.zero, new Vector2(Screen.width, _parser.RowsCount * 20));

            scrollPos = GUI.BeginScrollView(windowRect, scrollPos, windowScrollRect, true, false);
            {
                for (int i = 0; i < _parser.RowsCount; i++)
                {
                    Rect rect = new Rect(new Vector2(0, 20 * i), new Vector2(100, 20));
                    if (scrollPos.y <= rect.y + 20 && (scrollPos.y + Screen.height - 40) >= rect.y)
                    {
                        _parser.Rows[i].EditRow(rect, i);
                    }
                }
            }
            GUI.EndScrollView();
        }
    }
}