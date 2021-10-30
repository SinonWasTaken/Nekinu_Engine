using System;
using Nekinu;

namespace Engine.Engine.Scripts.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    sealed class EditorAttribute : Attribute
    {
        
        public EditorAttribute(Component component)
        {
            
        }
    }
}