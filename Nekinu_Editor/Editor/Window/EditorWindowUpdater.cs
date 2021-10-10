using Nekinu.Render;
using Nekinu.SceneManage;
using Nekinu.WindowUpdater;
using OpenTK.Windowing.Common;

namespace Nekinu.Editor
{
    class EditorWindowUpdater : IWindowUpdater
    {
        public void OnInit(Window window = null)
        {
            new SceneManager();
            new MasterRenderer(new StandardRenderer());
            EditorRenderer.Init(window);
        }

        public void OnUpdate() { }

        public void OnRender()
        {
            MasterRenderer.Render();
            EditorRenderer.Render();
        }

        public void OnResize(ResizeEventArgs e)
        {
            MasterRenderer.OnWindowResize();
            EditorRenderer.OnResize(e.Width, e.Height);
        }

        public void OnClose()
        {
            MasterRenderer.End();
            EditorRenderer.Dispose();
        }
    }
}
