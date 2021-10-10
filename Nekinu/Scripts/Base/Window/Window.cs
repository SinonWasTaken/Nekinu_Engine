using System.ComponentModel;
using Nekinu.Editor;
using Nekinu.Render;
using Nekinu.SceneManage;
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

        private static MasterRenderer renderer;

#if DEBUG
        private Editor.EditorRenderer editor_renderer;
#endif

        public Window(string title) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            input = new Input(this);
            WindowState = WindowState.Maximized;

#if DEBUG
            editor_renderer = new EditorRenderer();
#endif

            w_width = this.Size.X;
            w_height = this.Size.Y;

            this.VSync = VSyncMode.Off;

            this.title = title;

            aspectRatio = (float)w_width / (float)w_height;

            time = new Time();

            Cache.InitCache();
            AudioSystem.InitAudio();

            renderer = new MasterRenderer(new StandardRenderer());

            new SceneManager();

            Context.MakeCurrent();
            MakeCurrent();
        }

        public Window(string title, int width, int height) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            new ProjectDetails();
            input = new Input(this);
            w_width = width;
            w_height = height;

#if DEBUG
            editor_renderer = new EditorRenderer();
#endif

            GL.Viewport(0, 0, width, height);
            Size = new OpenTK.Mathematics.Vector2i(width, height);

            this.VSync = VSyncMode.Off;

            this.title = title;

            aspectRatio = (float)w_width / (float)w_height;

            time = new Time();

            Cache.InitCache();
            AudioSystem.InitAudio();

            renderer = new MasterRenderer(new StandardRenderer());

            new SceneManager();

            Context.MakeCurrent();
            MakeCurrent();
        }

        protected override void OnLoad()
        {
            base.OnLoad();

#if DEBUG
            EditorRenderer.Init(this);
#endif

            Title = title;
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            ProcessEvents();

            time.updateTime();

            SceneManager.Update();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            SwapBuffers();

            base.OnRenderFrame(e);

            MasterRenderer.Render();

#if DEBUG
            EditorRenderer.Render();
#endif
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
#if DEBUG
            MasterRenderer.OnWindowResize();
            EditorRenderer.OnResize(w_width, w_height);
#endif
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            time.stop();

            Cache.DestroyCache();
            AudioSystem.CleanUpAudio();

            MasterRenderer.End();

#if DEBUG
            EditorRenderer.Dispose();
#endif
        }
    }
}