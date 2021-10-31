using System.Collections.Generic;

namespace Nekinu.SceneManage
{
    public class Scene
    {
        public string scene_name { get; set; }

        public List<Entity> sceneEntities { get; private set; }

        public Scene(string name)
        {
            init(name);
        }

        public Scene()
        {
            init("New Scene");
        }

        private void init(string name)
        {
            scene_name = name;
            sceneEntities = new List<Entity>();

            scene_name = SceneManager.renameScene(this);
        }

        public void SubscribeEvent()
        {
            SceneManager.loaded += OnLoad;
            SceneManager.update += Update;
            SceneManager.editor_update += Editor_Update;
            SceneManager.unLoaded += OnUnload;
        }

        private void UnsubscribeEvent()
        {
            SceneManager.loaded -= OnLoad;
            SceneManager.update -= Update;
            SceneManager.editor_update -= Editor_Update;
            SceneManager.unLoaded -= OnUnload;
        }

        public void OnLoad()
        {
            for (int i = 0; i < sceneEntities.Count; i++)
            {
                Entity entity = sceneEntities[i];
                
                entity.Awake();
                entity.Start();
            }
        }

        public void OnEditorLoad()
        {
            for (int i = 0; i < sceneEntities.Count; i++)
            {
                Entity entity = sceneEntities[i];
                
                entity.Editor_Awake();
                entity.Editor_Start();
            }
        }

        public void OnUnload()
        {
            for (int i = 0; i < sceneEntities.Count; i++)
            {
                Entity entity = sceneEntities[i];

                entity.Destroy();
            }

            UnsubscribeEvent();
        }

        private void changeName(Entity entity)
        {
            int amount = 0;
            for (int i = 0; i < sceneEntities.Count; i++)
            {
                if(sceneEntities[i].transform.name.Split(" :(")[0].Contains(entity.transform.name))
                {
                    amount++;
                }
            }

            if(amount != 0)
            {
                entity.transform.name = $"{entity.transform.name} :({amount})";
            }
        }

        public void AddEntity(Entity entity)
        {
            changeName(entity);
            sceneEntities.Add(entity);

            if (SceneManager.state == SceneState.Editor)
            {
                entity.Editor_Awake();
                entity.Editor_Start();
            }
            else if(SceneManager.state == SceneState.Playing)
            {
                entity.Awake();
                entity.Start();
            }
        }

        public void RemoveEntity(Entity entity)
        {
            sceneEntities.Remove(entity);
            entity.Destroy();
        }

        public Entity GetEntity(string name)
        {
            for (int i = 0; i < sceneEntities.Count; i++)
            {
                Entity entity = sceneEntities[i];

                if (entity.transform.name == name)
                {
                    return entity;
                }
            }

            return null;
        }

        public void Update()
        {
            for (int i = 0; i < sceneEntities.Count; i++)
            {
                Entity entity = sceneEntities[i];
                if(entity.isActive)
                    entity.Update();
            }
        }

        public void Editor_Update()
        {
            for (int i = 0; i < sceneEntities.Count; i++)
            {
                if(sceneEntities[i].isActive)
                    sceneEntities[i].Editor_Update();
            }
        } 
    }
}