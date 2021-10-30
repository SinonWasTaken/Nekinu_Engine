namespace Nekinu
{
    public class AreaLight : Light
    {
        [SerializedProperty]
        private Vector3 attenuation;

        public Vector3 Attenuation
        {
            get => attenuation;
            set => attenuation = value;
        }
        
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