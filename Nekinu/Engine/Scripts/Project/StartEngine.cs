using System;
using Nekinu.WindowUpdater;

namespace Nekinu.StartUp
{
    public class StartEngine
    {
        /// <summary>
        /// Constructor to start the Nekinu engine
        /// </summary>
        /// <param name="updater">There are 2 types, the GameUpdater, and the EditorUpdater</param>
        /// <param name="title">The window title</param>
        /// <param name="width">The screen width</param>
        /// <param name="height">The screen height</param>
        public static void Start_Engine<T>(string title) where T : IWindowUpdater
        {
            Type type = typeof(T);
            IWindowUpdater updater = (IWindowUpdater) Activator.CreateInstance(type);
            
            Window window = new Window(updater, title);
            window.Run();
        }
        
        /// <summary>
        /// Constructor to start the Nekinu engine
        /// </summary>
        /// <param name="updater">There are 2 types, the GameUpdater, and the EditorUpdater</param>
        /// <param name="title">The window title</param>
        /// <param name="width">The screen width</param>
        /// <param name="height">The screen height</param>
        /// <param name="full_screen">Does the window start in fullscreen?</param>
        public static void Start_Engine<T>(string title, int width, int height, bool full_screen = false) where T : IWindowUpdater
        {
            Type type = typeof(T);
            IWindowUpdater updater = (IWindowUpdater) Activator.CreateInstance(type);
            
            Window window = new Window(updater, title, width, height, full_screen);
            window.Run();
        }
    }
}