using System;
using System.Collections.Generic;
using System.IO;
using Nekinu.Editor;
using Nekinu.EngineDebug;

namespace Nekinu.SceneManage
{
    internal static class Scene_List
    {
        public static List<Scene> list;

        internal static void Init()
        {
            list = new List<Scene>();

            if (!Directory.Exists(@"./Project"))
            {
                try
                {
                    Directory.CreateDirectory(@"./Project");
                }
                catch (Exception e)
                {
                    Debug.WriteError($"Error creating scene list file! {e}");
                }
            }

            if (File.Exists(@"./Project/list.txt"))
            {
                File.Create(@"./Project/list.txt");
            }
        }

        internal static void AddScene(Scene scene)
        {
            list?.Add(scene);
            write_scene_list();
        }

        internal static void AddSceneAt(Scene scene, int index)
        {
            AddAt(scene, index);
            write_scene_list();
        }

        internal static void RemoveScene(Scene scene)
        {
            list?.Remove(scene);
            write_scene_list();
        }

        internal static void RemoveScene(int index)
        {
            list?.RemoveAt(index);
            write_scene_list();
        }

        internal static void MoveScene(int index, int amount)
        {
            Scene scene = list[index];
            RemoveScene(scene);
            int newIndex = index + amount;

            if (newIndex >= list.Count)
                AddScene(scene);
            else
                AddAt(scene, newIndex);

            write_scene_list();
        }

        private static void AddAt(Scene scene, int index)
        {
            List<Scene> newList = new List<Scene>();

            for (int i = 0; i < list.Count; i++)
            {
                if(i + 1 != index)
                {
                    newList.Add(list[i]);
                }
                else
                {
                    newList.Add(scene);
                }
            }

            list = newList;
        }

        private static void write_scene_list()
        {
            try
            {
                StreamWriter writer = new StreamWriter(@"./Project/list.txt");

                writer.Flush();

                for (int i = 0; i < list.Count; i++)
                {
                    writer.WriteLine(list[i].scene_name);
                }

                writer.Close();
            }
            catch (Exception e)
            {
                Debug.WriteError($"Error writing scene list! {e}");
            }
        }
    }
}
