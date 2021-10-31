using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nekinu.Editor
{
    internal static class EditorList
    {
        public static List<IEditorPanel> allEditors { get; private set; }

        internal static void Init()
        {
            allEditors = new List<IEditorPanel>();

            getEditorAssembly();
        }

        private static void getEditorAssembly()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type editor in assembly.GetTypes().Where(my => my.IsClass && !my.IsAbstract && my.IsSubclassOf(typeof(IEditorPanel)) && my.IsPublic))
                {
                    allEditors.Add((IEditorPanel)Activator.CreateInstance(editor));
                }
            }
        }
    }
}
