using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nekinu.Editor
{
    internal static class EditorList
    {
        public static List<Editor> allEditors { get; private set; }

        internal static void Init()
        {
            allEditors = new List<Editor>();

            getEditorAssembly();
        }

        private static void getEditorAssembly()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type editor in assembly.GetTypes().Where(my => my.IsClass && !my.IsAbstract && my.IsSubclassOf(typeof(Editor)) && my.IsPublic))
                {
                    allEditors.Add((Editor)Activator.CreateInstance(editor));
                }
            }
        }
    }
}
