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
        private Action<int> InsertColumnAction = null;

        private List<CSVRow> _rows = new List<CSVRow>();
        public List<CSVRow> Rows { get { return _rows; } }

        private string _path = string.Empty;

        public void FromCSV(string path)
        {
            _path = path;

            string[] pathPart = path.Split(CSVEditorWindowConsts.PATH_SEPARATORS, StringSplitOptions.None);
            FileName = pathPart[pathPart.Length - 1];

            if (File.Exists(path))
            {
                StreamReader streamReader = new StreamReader(path);

                while (true)
                {
                    if (streamReader.EndOfStream)
                        break;

                    string line = streamReader.ReadLine();

                    string[] columns = line.Split(CSVEditorWindowConsts.COLUMN_SEPARATORS, StringSplitOptions.None);


                    if (ColumnsCount < columns.Length)
                    {
                        ColumnsCount = columns.Length;
                    }

                    CSVRow newRow = new CSVRow(columns);

                    AddColumnAction -= newRow.AddColumn;
                    AddColumnAction += newRow.AddColumn;

                    InsertColumnAction -= newRow.InsertColumn;
                    InsertColumnAction += newRow.InsertColumn;

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

        public void AddColumn()
        {
            if(AddColumnAction != null)
            {
                AddColumnAction();
                ColumnsCount++;
            }
        }

        public void InsertColumn(int index)
        {
            if (InsertColumnAction != null)
            {
                InsertColumnAction(index);
                ColumnsCount++;
            }
        }

        public CSVRow CreateRow()
        {
            CSVRow newRow = new CSVRow(ColumnsCount);

            AddColumnAction -= newRow.AddColumn;
            AddColumnAction += newRow.AddColumn;

            InsertColumnAction -= newRow.InsertColumn;
            InsertColumnAction += newRow.InsertColumn;

            ReziseRowAction -= newRow.ResizeRow;
            ReziseRowAction += newRow.ResizeRow;

            return newRow;
        }

        public void AddRow()
        {
            CSVRow newRow = CreateRow();

            Rows.Add(newRow);
        }

        public void InsertRow(int index)
        { 
            CSVRow newRow = CreateRow();

            Rows.Insert(index, newRow);
        }

        public void ToCSV()
        {
            ToCSV(_path);
        }

        public void ToCSV(string path)
        {
            StreamWriter streamWriter = new StreamWriter(path);

            for (int i = 0; i < Rows.Count; i++)
            {
                streamWriter.Write(Rows[i].ToString());
            }

            streamWriter.Close();
        }
    }
}