using Newtonsoft.Json;

namespace Nekinu
{
    public class Mesh : Component
    {
        [JsonProperty]
        public string Location { get; set; }

        [JsonIgnore]
        public int vertexCount { get; set; }

        public Mesh() { }

        public Mesh(string location)
        {
            Mesh m = ObjectLoader.loadOBJ(location);
            Location = location;
            vertexCount = m.vertexCount;

            Cache.AddMesh(m);
        }

        public Mesh(string location, int count)
        {
            Location = location;
            vertexCount = count;

            Cache.AddMesh(this);
        }

        public override void Awake()
        {
            base.Awake();

            if (!string.IsNullOrWhiteSpace(Location))
            {
                Mesh m = ObjectLoader.loadOBJ(Location);
                Location = m.Location;
                vertexCount = m.vertexCount;

                Cache.AddMesh(m);
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            Cache.RemoveMesh(this);
        }
    }
}