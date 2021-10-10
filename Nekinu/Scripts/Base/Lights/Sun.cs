namespace Nekinu
{
    public class Sun : Light
    {
        public Sun() : base(1, Vector3.one)
        {

        }

        public Sun(float intensity, Vector3 lightColor) : base(intensity, lightColor)
        { }
    }
}