using UnityEngine;

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace CSVEditor
{
    internal class CSVFileParser
    {
        public string FileName { get; private set; }
        public int ColumnsCount { get; private set; }
        public int RowsCount { get { return _rows.Count; } }

        private Action <int> ReziseRowAction = null;
        private Action AddColumnAction = null;

        private List<CSVRow> _rows = new List<CSVRow>();
        public List<CSVRow> Rows { get { return _rows; } }

        public void FromCSV(string path)
        {
            string[] pathPart = path.Split(CSVEditorWindowStrings.PATH_SEPARATORS, StringSplitOptions.None);
            FileName = pathPart[pathPart.Length - 1];
            if (File.Exists(path))
            {
                StreamReader streamReader = new StreamReader(path);

                while (true)
                {
                    if (streamReader.EndOfStream)
                        break;

                    string line = streamReader.ReadLine();

                    string[] columns = line.Split(CSVEditorWindowStrings.COLUMN_SEPARATORS, StringSplitOptions.None);

                    CSVRow newRow = new CSVRow(columns);

                    if (ColumnsCount < columns.Length)
                    {
                        ColumnsCount = columns.Length;
                    }

                    AddColumnAction -= newRow.AddColumn;
                    AddColumnAction += newRow.AddColumn;

                    ReziseRowAction -= newRow.ResizeRow;
                    ReziseRowAction += newRow.ResizeRow;

                    _rows.Add(newRow);
                }
            }

            if(ReziseRowAction != null)
            {
                ReziseRowAction(ColumnsCount);
            }
        }

        public void ToCSV()
        {
            
        }
    }
}