namespace Nekinu
{
    public class AreaLight : Light
    {
        public Vector3 attenuation { get; set; }

        public AreaLight() : base(1, Vector3.one)
        {
            attenuation = Vector3.zero;
        }

        public AreaLight(float intensity, Vector3 color, Vector3 attenuation) : base(intensity, color)
        {
            this.attenuation = attenuation;
        }
    }
}