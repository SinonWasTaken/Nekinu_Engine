using OpenTK.Windowing.Common;

namespace Nekinu.WindowUpdater
{
    public interface IWindowUpdater
    {
        void OnInit(Window window = null);
        void OnUpdate();
        void OnRender();
        void OnResize(ResizeEventArgs e);
        void OnClose();
    }
}