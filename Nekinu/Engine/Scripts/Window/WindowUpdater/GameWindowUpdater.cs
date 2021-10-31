using Nekinu.Render;
using Nekinu.SceneManage;
using OpenTK.Windowing.Common;

namespace Nekinu.WindowUpdater
{
    public class GameWindowUpdater : IWindowUpdater
    {
        public void OnInit(Window window = null)
        {
            new SceneManager();
            new MasterRenderer(new StandardRenderer());
        }

        public void OnUpdate()
        { }

        public void OnRender()
        {
            MasterRenderer.Render();
        }

        public void OnResize(ResizeEventArgs e)
        {
            MasterRenderer.OnWindowResize();
        }

        public void OnClose()
        {
            MasterRenderer.End();
        }
    }
}