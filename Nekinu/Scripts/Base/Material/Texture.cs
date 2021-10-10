using Newtonsoft.Json;

namespace Nekinu
{
    public class Texture
    {
        [JsonIgnore]
        public int id { get; private set; }

        [JsonProperty]
        public string location { get; private set; }

        [JsonConstructor]
        public Texture()
        {
            id = Loader.loadTexture(location);
            Cache.AddTexture(this);
        }

        public Texture(string location)
        {
            id = Loader.loadTexture(location);
            this.location = location;

            Cache.AddTexture(this);
        }
    }
}