namespace Nekinu
{
    public class Light : Component
    {
        public float intensity { get; set; }
        public Vector3 lightColor { get; set; }

        public Light()
        {
            intensity = 0;
            lightColor = Vector3.zero;
        }

        public Light(float intensity, Vector3 color)
        {
            this.intensity = intensity;
            lightColor = color;
        }
    }
}