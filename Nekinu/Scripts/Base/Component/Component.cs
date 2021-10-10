using Nekinu.SceneManage;
using Newtonsoft.Json;

namespace Nekinu
{
    public class Component
    {
        [JsonProperty]
        public bool isActive = true;

        [JsonIgnore]
        public Entity parent { get; set; }

        public virtual void Awake() { isActive = true; }
        public virtual void Start() { }

        public virtual void Update() { }
        public virtual void LateUpdate() { }

        public virtual void OnEnabled() { isActive = true; }
        public virtual void OnDisabled() { isActive = false; }

        public virtual void OnDestroy() { }

        public void Destroy(Entity entity)
        {
            SceneManager.loadedScene.RemoveEntity(entity);
        }
    }
}