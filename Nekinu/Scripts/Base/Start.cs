using Nekinu.Render;

namespace Nekinu
{
    public class Start
    {
        public static void Main(string[] args)
        {
            Window window = new Window("Title");

            window.Run();
        }

        public static Window StartEngine(string title)
        {
            Window window = new Window("Title");

            return window;
        }

        public static Window StartEngine(string title, int width, int height) 
        {
            Window window = new Window("Title", width, height);

            return window;
        }

        public static Window StartEngine(params IRenderer[] renderers)
        {
            Window window = new Window("Title", 800, 600);

            foreach (IRenderer renderer in renderers)
            {
                MasterRenderer.addRenderer(renderer);
            }

            return window;
        }
    }
}