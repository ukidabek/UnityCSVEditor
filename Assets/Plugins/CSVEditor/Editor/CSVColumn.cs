using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

namespace CSVEditor
{
    internal class CSVColumn
    {
        private string _value = string.Empty;

        public CSVColumn(string value)
        {
            _value = value;
        }

        public void EditValue(Rect rowRect)
        {
            _value = GUI.TextArea(rowRect, _value);
        }
    }
}