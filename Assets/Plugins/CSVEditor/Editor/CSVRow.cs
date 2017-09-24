using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

namespace CSVEditor
{
    internal class CSVRow
    {
        public Rect test = new Rect();
        private List<CSVColumn> _columns = new List<CSVColumn>();

        public CSVRow(int length)
        {
            for (int i = 0; i < length; i++)
            {
                AddColumn();
            }
        }

        public CSVRow(string[]columns)
        {
            for (int i = 0; i < columns.Length; i++)
            {
                _columns.Add(new CSVColumn(columns[i]));
            }
        }

        public void AddColumn()
        {
            CSVColumn newColumn = new CSVColumn(string.Empty);
            _columns.Add(newColumn);
        }

        public void ResizeRow(int columnsCount)
        {
            int count = columnsCount - _columns.Count;
            for (int j = 0; j < count; j++)
            {
                AddColumn();
            }
        }

        public void EditRow(Rect rowRect, int index, ref List<int> columnSizes)
        {
            Rect newRect = new Rect();
            newRect.y = rowRect.y;
            for (int i = 0; i < _columns.Count; i++)
            {
                newRect.width = columnSizes[i];
                newRect.height = rowRect.height;

                _columns[i].EditValue(newRect);
                newRect.x += newRect.width;
                newRect.x += 5;
            }
        }

        public override string ToString()
        {
            string line = string.Empty;

            for (int i = 0; i < _columns.Count; i++)
            {
                string value = _columns[i].GetValue();
                line += string.Format("{0}{1}", value, CSVEditorWindowConsts.COLUMN_SEPARATORS[0]);

                if(i == _columns.Count -1)
                {
                    line = string.Format("{0}{1}", line, CSVEditorWindowConsts.ROW_SEPARATORS[0]);
                }
            }

            return line;
        }

    }
}