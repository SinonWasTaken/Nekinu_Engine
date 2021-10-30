using Newtonsoft.Json;

namespace Nekinu
{
    public class Light : Component
    {
        [SerializedProperty] private float intensity;
        [SerializedProperty] private Vector3 lightColor;
        [SerializedProperty] private Vector3 pos_offset;

        [JsonIgnore]
        public float Intensity
        {
            get => intensity;
            set => intensity = value;
        }

        [JsonIgnore]
        public Vector3 LightColor
        {
            get => lightColor;
            set => lightColor = value;
        }

        [JsonIgnore]
        public Vector3 Position_Offset
        {
            get => pos_offset;
            set => pos_offset = value;
        }
        
        public Light()
        {
            intensity = 0;
            lightColor = Vector3.zero;
            pos_offset = Vector3.zero;
        }

        public Light(float intensity, Vector3 color)
        {
            this.intensity = intensity;
            lightColor = color;
            pos_offset = Vector3.zero;
        }
    }
}