using Newtonsoft.Json;

namespace Nekinu
{
    public class Mesh : Component
    {
        [JsonProperty]
        public string Location { get; private set; }
        
        [JsonIgnore]
        public int vertexCount { get; private set; }

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