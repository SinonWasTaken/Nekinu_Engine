namespace Nekinu
{
    public class SceneEvent
    {
        public delegate void OnSceneLoaded();
        public delegate void OnSceneUnloaded();

        public delegate void OnAwake();
        public delegate void OnStart();

        public delegate void OnSceneUpdate();

        public static event OnSceneLoaded onSceneLoaded;
        public static event OnSceneUnloaded onSceneUnloaded;
        public static event OnSceneUpdate onSceneUpdated;

        public static event OnAwake onAwake;
        public static event OnStart onStart;

        public static void Awake()
        {
            onAwake?.Invoke();
        }

        public static void Update()
        {
            onSceneUpdated?.Invoke();
        }
    }
}