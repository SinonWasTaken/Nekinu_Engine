using Newtonsoft.Json;
using OpenTK.Graphics.ES30;
using System.Collections.Generic;
using Nekinu.Shaders;

namespace Nekinu
{
    public class Material : Component
    {
        [JsonIgnore]
        private Texture texture;

        [JsonProperty] private string location;

        public Material() { }

        public Material(string location)
        {
            this.location = location;

            texture = new Texture(location);
        }

        public Material(Texture texture)
        {
            this.texture = texture;
            location = texture.location;
        }
    }
}