using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Nekinu.SceneManage
{
    public class SceneManager
    {
        public delegate void OnSceneLoaded();
        public delegate void OnSceneUpdate();
        public delegate void OnSceneUnloaded();

        public static event OnSceneLoaded loaded;
        public static event OnSceneUpdate update;
        public static event OnSceneUnloaded unLoaded;

        public static Scene loadedScene { get; private set; }
        public static Scene dontDestroy { get; private set; }

        public static SceneState state;

        private static List<string> temp_scene_info = new List<string>();

        public SceneManager()
        {
            if (!Directory.Exists(@"./Data"))
            {
                Directory.CreateDirectory(@"./Data");
            }

            Scene_List.Init();

#if RELEASE
            LoadSceneInfo($@".\Data\New Scene.scene");
            state = SceneState.Playing;
            loaded();
#endif
        }

        public static string renameScene(Scene scene)
        {
            int count = 0;
            for (int i = 0; i < Scene_List.list.Count; i++)
            {
                Scene name = Scene_List.list[i];

                if (name.scene_name.Split("-")[0] == scene.scene_name)
                {
                    count++;
                }
            }

            return count == 0 ? scene.scene_name : $"{scene.scene_name}-{count}";
        }

        public static void LoadScene(int id)
        {
            if (id >= Scene_List.list.Count)
            {
                Console.WriteLine("Couldn't load desired scene! Id was out of range!");
                return;
            }
            LoadScene(Scene_List.list[id]);
        }

        public static void LoadScene(string name)
        {
            for (int i = 0; i < Scene_List.list.Count; i++)
            {
                if (Scene_List.list[i].scene_name == name)
                    LoadScene(Scene_List.list[i]);
                return;
            }
        }

        public static void LoadScene(Scene scene)
        {
            if (loadedScene != null)
            {
                unLoaded();
            }

            loadedScene = scene;

            loadedScene.SubscribeEvent();
        }

        public static void Update()
        {
            if (state == SceneState.Playing)
                if (loadedScene != null)
                {
                    update();
                }
        }

        public static void BeingPlay()
        {
            SaveTempSceneInfo();
            loaded();
            state = SceneState.Playing;
        }

        public static void EndPlay()
        {
            state = SceneState.Ready;

            LoadTempSceneInfo();
        }

        public static void Save()
        {
            SaveSceneInfo();
        }

        public static void SaveTempSceneInfo() 
        {
            if (loadedScene != null)
            {
                Scene scene = loadedScene;

                List<string> lines = new List<string>();

                for (int i = 0; i < scene.sceneEntities.Count; i++)
                {
                    string s = JsonConvert.SerializeObject(scene.sceneEntities[i], Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, TypeNameHandling = TypeNameHandling.All, ConstructorHandling = ConstructorHandling.Default });
                    lines.Add(s);
                }

                temp_scene_info = lines;
            }
        }

        public static void LoadTempSceneInfo()
        {
            if (temp_scene_info.Count != 0)
            {
                Cache.ClearMesh();
                Scene scene = new Scene(loadedScene.scene_name);

                foreach (string line in temp_scene_info)
                {
                    string s = line;
                    Entity entity = (Entity)JsonConvert.DeserializeObject(s, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, TypeNameHandling = TypeNameHandling.All, ConstructorHandling = ConstructorHandling.Default });

                    Entity newEntity = new Entity(entity.transform);

                    for (int i = 0; i < entity.components.Count; i++)
                    {
                        newEntity.AddComponent(entity.components[i]);
                    }

                    for (int i = 0; i < entity.children.Count; i++)
                    {
                        newEntity.AddChild(entity.children[i]);
                    }

                    scene.AddEntity(newEntity);
                }

                LoadScene(scene);
                temp_scene_info.Clear();
            }
        }

        private static void SaveSceneInfo()
        {
            SaveSceneInfo(loadedScene);
        }
        private static void SaveSceneInfo(Scene scene)
        {
            if (File.Exists($@"./Data/{scene.scene_name}.scene")) File.Delete($@"./Data/{scene.scene_name}.scene");

            List<string> lines = new List<string>();

            for (int i = 0; i < scene.sceneEntities.Count; i++)
            {
                string s = JsonConvert.SerializeObject(scene.sceneEntities[i], Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, TypeNameHandling = TypeNameHandling.All, ConstructorHandling = ConstructorHandling.Default });
                lines.Add(s);
            }

            StreamWriter writer = new StreamWriter($@"./Data/{scene.scene_name}.scene", true, new UTF32Encoding(true, false));

            foreach (string line in lines)
            {
                writer.WriteLine(line);
            }

            writer.Close();
        }

        public static void LoadSceneInfo(string name)
        {
            StreamReader reader = null;

            if (name.Contains(".scene"))
            {
                string[] lines = name.Split("\\");

                string n = lines[lines.Length - 1];

                try
                {
                    reader = new StreamReader($@".\Data\{n}", new UTF32Encoding(true, false));
                    LoadSceneDetail(reader, n);
                }
                catch (Exception e)
                {
                    Editor.Debug.WriteLine(e);
                }
            }
            else
            {
                try
                {
                    reader = new StreamReader($@".\Data\{name}.scene", new UTF32Encoding(true, false));
                    LoadSceneDetail(reader, name);
                }
                catch (Exception e)
                {
                    Editor.Debug.WriteLine(e);
                }
            }
        }

        private static void LoadSceneDetail(StreamReader reader, string name)
        {
            Cache.ClearMesh();

            string line = "";

            Scene scene = new Scene(name.Replace(".scene", ""));

            while ((line = reader.ReadLine()) != null)
            {
                string s = line;
                Entity entity = (Entity)JsonConvert.DeserializeObject(s, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, TypeNameHandling = TypeNameHandling.All, ConstructorHandling = ConstructorHandling.Default });

                Entity newEntity = new Entity(entity.transform);

                for (int i = 0; i < entity.components.Count; i++)
                {
                    newEntity.AddComponent(entity.components[i]);
                }

                for (int i = 0; i < entity.children.Count; i++)
                {
                    newEntity.AddChild(entity.children[i]);
                }

                scene.AddEntity(newEntity);
            }

            LoadScene(scene);
        }

        public static void NewScene()
        {
            Scene scene = new Scene("New Scene");

            Entity sun = new Entity(new Transform("Sun", new Vector3(0, 10, 0)));
            sun.AddComponent<Sun>();

            Entity camera = new Entity(new Transform("Camera", new Vector3(0, 0, -5)));
            camera.AddComponent(new Camera(90, 0.0001f, 1000f));

            scene.AddEntity(sun);
            scene.AddEntity(camera);

            LoadScene(scene);
        }
    }
}