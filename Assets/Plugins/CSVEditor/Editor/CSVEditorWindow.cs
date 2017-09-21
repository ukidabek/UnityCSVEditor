using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;
namespace CSVEditor
{
    public class CSVEditorWindow : EditorWindow
    {
        Vector2 scrollPos;

        private TextAsset csvFile = null;

        private CSVFileParser _parser = new CSVFileParser();

        [MenuItem("Window/My Window")]
        static void Init()
        {
            CSVEditorWindow window = (CSVEditorWindow)GetWindow(typeof(CSVEditorWindow));
            GUIContent titleContent = new GUIContent(CSVEditorWindowStrings.CSV_EDITOR_WINDOW_TITLE);

            window.titleContent = titleContent;
            window.Show();
        }

        bool open = false;

        private void OnGUI()
        {
            csvFile = (TextAsset)EditorGUILayout.ObjectField(csvFile, typeof(TextAsset), false);
            if(GUILayout.Button("Open"))
            {
                _parser.FromCSV(csvFile);
                open = true;
            }

            Rect superRect = new Rect(new Vector2(0,40), new Vector2(Screen.width, Screen.height - 40));
            Rect superDuperRecr = new Rect(Vector2.zero, new Vector2(Screen.width, _parser.RowsCount * 20));
            scrollPos = GUI.BeginScrollView(superRect, scrollPos, superDuperRecr, true, false);
            if (open)
            {
                int aaaa = 0;
                for (int i = 0; i < _parser.RowsCount; i++)
                {
                    Rect rect = new Rect(new Vector2(0, 20 * i), new Vector2(100, 20));
                    if(scrollPos.y <= rect.y + 20 && (scrollPos.y + Screen.height - 40) >= rect.y)
                    {
                        _parser.Rows[i].EditRow(rect,i);
                        aaaa++;
                    }
                }
                Debug.Log(aaaa);
            }
            GUI.EndScrollView();
        }
    }
}
