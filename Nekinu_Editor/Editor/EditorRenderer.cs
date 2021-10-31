using System.Collections.Generic;

namespace Nekinu.Editor
{
    internal class EditorRenderer
    {
        private static List<IEditorPanel> editor_panels;

        private static ImGuiController controller;

        private static Window window;

        public static void Init(Window wind)
        {
            window = wind;

            EditorList.Init();

            editor_panels = new List<IEditorPanel>();

            controller = new ImGuiController(Window.w_width, Window.w_height, wind);

            editor_panels.Add(new DockPanel());
            editor_panels.Add(new SceneHeirarchyPanel());
            editor_panels.Add(new PropertiesPanel());
            editor_panels.Add(new Debug());
            editor_panels.Add(new ScenePanel());
            editor_panels.Add(new ContentBrowser());

            for (int i = 0; i < editor_panels.Count; i++)
            {
                editor_panels[i].Init();
            }
        }

        public static void Render()
        {
            controller.Update(window, Time.deltaTime);

            for (int i = 0; i < editor_panels.Count; i++)
            {
                editor_panels[i].Render();
            }

            controller.Render();
        }

        public static void OnResize(int width, int height)
        {
            controller?.WindowResized(width, height);
        }

        public static void Dispose()
        {
            controller.Dispose();
        }

        public static void addEditor(IEditorPanel editor)
        {
            editor.Init();
            editor_panels.Add(editor);
        }

        public static bool hasEditor(IEditorPanel editor)
        {
            for (int i = 0; i < editor_panels.Count; i++)
            {
                if((editor_panels[i].GetType() == editor.GetType()))
                {
                    return true;
                }
            }

            return false;
        }

        public static void removeEditor(IEditorPanel editor)
        {
            editor_panels.Remove(editor);
        }
    }
}
