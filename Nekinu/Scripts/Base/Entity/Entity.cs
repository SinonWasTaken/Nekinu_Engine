using System;
using System.Collections.Generic;
using Nekinu.Editor;
using Nekinu.SceneManage;
using Newtonsoft.Json;
using OpenTK.Mathematics;

namespace Nekinu
{
    public class Entity
    {
        public Transform transform { get; set; }

        [JsonIgnore]
        public Matrix4 transformationMatrix { get; set; }

        [JsonProperty]
        public List<Component> components = new List<Component>();
        public List<Entity> children = new List<Entity>();

        public bool isActive { get; set; }

        public Entity parent { get; set; }

        private int posHash, rotHash, scaHash;

        [JsonConstructor]
        public Entity()
        {}

        public Entity(Transform transform)
        {
            this.transform = transform;

            isActive = true;

            UpdateTransformationMatrix();

            posHash = transform.position.GetHashCode();
            rotHash = transform.rotation.GetHashCode();
            scaHash = transform.scale.GetHashCode();
        }

        public void setActive(bool value)
        {
            isActive = value;
        }

        public void AddChild(Entity entity)
        {
            entity.parent = this;
            children.Add(entity);

            if (SceneManager.state == SceneState.Playing)
            {
                entity.Awake();
                entity.Start();
            }
        }

        public void RemoveChild(Entity entity)
        {
            children.Remove(entity);
        }

        public void Awake()
        {
            for (int c = 0; c < components.Count; c++)
            {
                Component component = components[c];

                if (component.isActive)
                    component.Awake();
            }

            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].isActive)
                    children[i].Awake();
            }
        }

        public void Start()
        {
            for (int c = 0; c < components.Count; c++)
            {
                Component component = components[c];

                if (component.isActive)
                    component.Start();
            }

            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].isActive)
                    children[i].Start();
            }
        }

        public void Update()
        {
            UpdateTransformationMatrix();

            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].isActive)
                    children[i].Update();
            }

            for (int c = 0; c < components.Count; c++)
            {
                Component component = components[c];

                if(component.isActive)
                    component.Update();
            }
        }

        public void AddComponent<T>() where T : Component
        {
            Type type = typeof(T);

            Component c = (Component)Activator.CreateInstance(type);

            RequireComponentAttribute require = (RequireComponentAttribute)Attribute.GetCustomAttribute(c.GetType(), typeof(RequireComponentAttribute));

            if (require != null)
            {
                Component comp = require.Component;
                comp.parent = this;
                components.Add(comp);
            }

            c.parent = this;
            components.Add(c);
        }

        public void AddComponent(Component component)
        {
            RequireComponentAttribute require = (RequireComponentAttribute)Attribute.GetCustomAttribute(component.GetType(), typeof(RequireComponentAttribute));

            if (require != null)
            {
                Component comp = require.Component;
                comp.parent = this;
                components.Add(comp);
            }

            component.parent = this;
            components.Add(component);
        }

        public T GetComponent<T>() where T : Component
        {
            foreach (Component component in components)
            {
                if (component as T != null)
                {
                    return (T)component;
                }
            }

            return null;
        }

        [UpdateInEditor]
        public void UpdateTransformationMatrix()
        {
            if (posHash != transform.position.GetHashCode() || rotHash != transform.rotation.GetHashCode() || scaHash != transform.scale.GetHashCode())
            {
                transformationMatrix = Matrix4x4.entityTransformationMatrix(parent, transform);
                posHash = transform.position.GetHashCode();
                rotHash = transform.rotation.GetHashCode();
                scaHash = transform.scale.GetHashCode();
            }
        }

        public void Destroy()
        {
            for (int i = 0; i < children.Count; i++)
            {
                children[i].Destroy();
            }

            for (int i = 0; i < components.Count; i++)
            {
                components[i].OnDestroy();
            }
        }

        public static List<Entity> GetAllEntitiesWithComponentsOfType<T>() where T : Component
        {
            List<Entity> allComponentsOfType = new List<Entity>();

            List<Entity> sceneEntities = SceneManager.loadedScene.sceneEntities;

            for (int i = 0; i < sceneEntities.Count; i++)
            {
                Component component = sceneEntities[i].GetComponent<T>();
                if (component != null)
                {
                    allComponentsOfType.Add(sceneEntities[i]);
                }
            }

            return allComponentsOfType;
        }
    }
}