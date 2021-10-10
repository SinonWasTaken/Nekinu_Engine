using System;
using System.Collections.Generic;
using System.Text;

namespace Nekinu
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EditorTypeAttribute : Attribute
    {
        public string editorType { get; private set; }

        public EditorTypeAttribute(string type)
        {
            editorType = type;
        }
    }
}
