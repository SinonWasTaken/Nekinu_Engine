using Newtonsoft.Json;

namespace Nekinu
{
    public class Mesh : Component
    {
        [JsonProperty] [SerializedProperty] private string location;
        
        [JsonIgnore]
        public int vertexCount { get; private set; }

        [JsonIgnore]
        public string Location
        {
            get => location;
            set
            {
                location = value;
                Mesh me = ObjectLoader.loadOBJ(location);
                if (me != null)
                {
                    vertexCount = me.vertexCount;
                }
            }
    }
        
        public Mesh() { }

        [JsonConstructor]
        public Mesh(string location)
        {
            Mesh m = ObjectLoader.loadOBJ(location);
            Location = location;
            vertexCount = m.vertexCount;

            Cache.AddMesh(m);
        }

        public Mesh(string location, int indicies)
        {
            Location = location;

            vertexCount = indicies;

            Cache.AddMesh(this);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            Cache.RemoveMesh(this);
        }
    }
}