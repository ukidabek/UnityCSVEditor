using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace CSVEditor
{
    internal class CSVFileParser
    {
        public int ColumnsCount { get; private set; }
        public int RowsCount { get; private set; }

        private Action <int> ReziseRowAction = null;
        private Action AddColumnAction = null;

        private List<CSVRow> _rows = new List<CSVRow>();
        public List<CSVRow> Rows { get { return _rows; } }

        public void FromCSV(TextAsset csvFile)
        {
            string[] rows = csvFile.text.Split(CSVEditorWindowStrings.RowSeparators, StringSplitOptions.None);

            RowsCount = rows.Length;

            for (int i = 0; i < RowsCount; i++)
            {
                string[] columns = rows[i].Split(CSVEditorWindowStrings.ColumnSeparators, StringSplitOptions.None);
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