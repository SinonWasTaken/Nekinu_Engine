using System;
using System.Diagnostics;

namespace Nekinu
{
    class Time
    {
        private Stopwatch stopWatch;

        private long lastTime;
        private long lastFPSTime;

        private static int frames;

        public static float deltaTime { get; private set; }
        public static int fps { get; set; }

        public Time()
        {
            stopWatch = new Stopwatch();
            stopWatch.Start();

            lastTime = stopWatch.ElapsedMilliseconds;
            lastFPSTime = lastTime;
        }

        public void updateTime()
        {
            frames++;

            long now = stopWatch.ElapsedMilliseconds;

            if (now >= lastFPSTime + 1000)
            {
                fps = frames;
                frames = 0;
                lastFPSTime = now;
            }

            long last = now - lastTime;

            deltaTime = (float)last / 1000f;

            lastTime = now;
        }

        public void stop()
        {
            stopWatch.Stop();
        }
    }
}