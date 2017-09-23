using UnityEngine;
using UnityEditor;

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace CSVEditor
{
    public class CSVEditorMainWindow : EditorWindow
    {
        private TextAsset _csvFile = null;
        private CSVFileParser _parser = new CSVFileParser();
        private List<string> _recentlyOpenedFiles = new List<string>();

        [MenuItem("Window/CSV Editor")]
        private static void Init()
        {
            CSVEditorMainWindow window = (CSVEditorMainWindow)GetWindow(typeof(CSVEditorMainWindow));
            GUIContent titleContent = new GUIContent(CSVEditorWindowConsts.CSV_EDITOR_WINDOW_TITLE);

            window.titleContent = titleContent;
            window.Show();
        }

        public CSVEditorMainWindow()
        {
            ParseMataData();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                _csvFile = (TextAsset)EditorGUILayout.ObjectField(_csvFile, typeof(TextAsset), false);

                if (GUILayout.Button("Open", GUILayout.Width(100)))
                {
                    string path = AssetDatabase.GetAssetPath(_csvFile);
                    OpenEditWindow(path);
                }
            }
            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < _recentlyOpenedFiles.Count; i++)
            {
                if (GUILayout.Button(_recentlyOpenedFiles[i]))
                {
                    OpenEditWindow(_recentlyOpenedFiles[i]);
                }
            }
        }

        private void OpenEditWindow(string path)
        {
            if (_recentlyOpenedFiles.Contains(path))
            {
                int index = _recentlyOpenedFiles.IndexOf(path);
                _recentlyOpenedFiles.RemoveAt(index);
                _recentlyOpenedFiles.Insert(0, path);
            }
            else
            {
                _recentlyOpenedFiles.Insert(0, path);
                if(_recentlyOpenedFiles.Count > 10)
                {
                    _recentlyOpenedFiles.RemoveAt(_recentlyOpenedFiles.Count - 1);
                }
            }

            _parser.FromCSV(path);

            CSVEditorEditWindow window = new CSVEditorEditWindow(_parser);
            window.Show();
        }

        private void ParseMataData()
        {
            StreamReader streamWriter = new StreamReader(CSVEditorWindowConsts.META_DATA_FILE_NAME);
            while(true)
            {
                if(streamWriter.EndOfStream)
                {
                    break;
                }

                string line = streamWriter.ReadLine();
                string[] parts = line.Split(
                    CSVEditorWindowConsts.META_DATA_FILE_PARAMETER_SEPARATORS,
                    StringSplitOptions.None);

                switch(parts[0])
                {
                    case CSVEditorWindowConsts.PATH_MARKER:
                        _recentlyOpenedFiles.Add(parts[1]);
                        break;
                }
            }
            streamWriter.Close();
        }

        private void WriteMetaData()
        {
            StreamWriter streamWriter = new StreamWriter(CSVEditorWindowConsts.META_DATA_FILE_NAME);
            for (int i = 0; i < _recentlyOpenedFiles.Count; i++)
            {
                string line = string.Format("{0}={1}{2}",
                    CSVEditorWindowConsts.PATH_MARKER, 
                    _recentlyOpenedFiles[i],
                    CSVEditorWindowConsts.ROW_SEPARATORS[0]);
                streamWriter.Write(line);
            }
            streamWriter.Close();
        }

        private void OnDestroy()
        {
            WriteMetaData();
        }
    }
}
