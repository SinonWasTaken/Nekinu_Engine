using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FileBrowser;
using Nekinu.EngineDebug;
using Nekinu.SystemCache;
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
        public static event OnSceneUpdate editor_update;
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

            //Create a system that stores all scenes that are to be used in the game, and then load the first one in the system
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

        public static void LoadScene(Scene scene)
        {
            if (loadedScene != null)
            {
                unLoaded();
            }

            Cache.On_NewScene_Loaded();
            
            loadedScene = scene;
            
            loadedScene.SubscribeEvent();

            if (state == SceneState.Editor)
            {
                loadedScene.OnEditorLoad();
            }
            else if(state == SceneState.Playing)
            {
                loadedScene.OnLoad();
            }
        }

        public static void Update()
        {
            if (state == SceneState.Playing)
            {
                if (loadedScene != null)
                {
                    update();
                }
            }
            else if (state == SceneState.Editor)
            {
                if (loadedScene != null)
                {
                    editor_update();
                }
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
            state = SceneState.Editor;

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
            string path = OpenFile.saveFile("Save scene", ".scene", ProjectDetails.projectDeveloper);

            if (path != string.Empty)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                List<string> lines = new List<string>();

                for (int i = 0; i < scene.sceneEntities.Count; i++)
                {
                    string s = JsonConvert.SerializeObject(scene.sceneEntities[i], Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            TypeNameHandling = TypeNameHandling.All, ConstructorHandling = ConstructorHandling.Default
                        });
                    lines.Add(s);
                }

                try
                {
                    StreamWriter writer = new StreamWriter(path, true, new UTF32Encoding(true, false));

                    foreach (string line in lines)
                    {
                        writer.WriteLine(line);
                    }

                    writer.Close();
                }
                catch (Exception e)
                {
                    Debug.WriteError($"Error saving scene! {e}");
                }
            }
        }

        public static void LoadSceneInfo(Dictionary<Stream, String> file)
        {
            foreach (Stream stream in file.Keys)
            {
                try
                {
                    StreamReader reader = new StreamReader(stream, new UTF32Encoding(true, false));
                    String name = "";
                    file.TryGetValue(stream, out name);
                    LoadSceneDetail(reader, name);
                }
                catch (Exception e)
                {
                    Crash_Report.generate_crash_report($"Error loading scene! {e}");
                    Debug.WriteError($"Error loading scene! {e}");
                    NewScene();
                }
            }
        }

        private static void LoadSceneDetail(StreamReader reader, string name)
        {
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