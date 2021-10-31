using System.ComponentModel;
using Nekinu.Render;
using Nekinu.SystemCache;
using Nekinu.WindowUpdater;
using OpenTK.Graphics.ES20;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Nekinu
{
    public class Window : GameWindow
    {
        public static int w_width { get; private set; }
        public static int w_height { get; private set; }

        public static float aspectRatio { get; private set; }

        private string title;

        private Time time;
        private Input input;

        private IWindowUpdater update;
        
        private static MasterRenderer renderer;

        public Window(IWindowUpdater updater, string title) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            update = updater;
            
            WindowState = WindowState.Maximized;

            w_width = this.Size.X;
            w_height = this.Size.Y;

            this.VSync = VSyncMode.Off;

            this.title = title;

            aspectRatio = (float)w_width / (float)w_height;

            time = new Time();

            Cache.InitCache();
            AudioSystem.InitAudio();

            Context.MakeCurrent();
            MakeCurrent();
        }

        public Window(IWindowUpdater updater, string title, int width, int height, bool is_fullscreen = false) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            update = updater;
            
            w_width = width;
            w_height = height;

            WindowState = is_fullscreen ? WindowState.Fullscreen : WindowState.Maximized;

            GL.Viewport(0, 0, width, height);
            Size = new OpenTK.Mathematics.Vector2i(width, height);

            this.VSync = VSyncMode.Off;

            this.title = title;

            aspectRatio = (float)w_width / (float)w_height;

            time = new Time();

            Cache.InitCache();
            AudioSystem.InitAudio();

            Context.MakeCurrent();
            MakeCurrent();
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            
            input = new Input(this);
            
            new ProjectDetails();
            
            update.OnInit(this);

            Title = title;
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            ProcessEvents();

            update.OnUpdate();
            
            time.updateTime();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            SwapBuffers();

            base.OnRenderFrame(e);
            
            update.OnRender();
        }

        protected void changeTitle(string title)
        {
            Title = title;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            w_width = e.Width;
            w_height = e.Height;

            GL.Viewport(0, 0, w_width, w_height);

            aspectRatio = (float)w_width / (float)w_height;
            
            update.OnResize(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            time.stop();

            Cache.DestroyCache();
            AudioSystem.CleanUpAudio();

            update.OnClose();
            
            MasterRenderer.End();
        }
    }
}