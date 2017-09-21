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

        public void EditRow(Rect rowRect, int index)
        {
            GUI.Label(rowRect, index.ToString());
            rowRect.x += 30;
            for (int i = 0; i < _columns.Count; i++)
            {
                Rect newRect = new Rect();
                newRect.x = 30 + rowRect.width * i;
                newRect.y = rowRect.y;
                newRect.width = rowRect.width;
                newRect.height = rowRect.height;

                _columns[i].EditValue(newRect);
            }
        }

    }
}