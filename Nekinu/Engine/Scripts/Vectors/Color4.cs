namespace Nekinu
{
    public class Color4 : Vector4
    {
        public Color4()
        { }

        public Color4(float value) : base(value)
        { }

        public Color4(float x, float y, float z, float w) : base(x, y, z, w)
        { }

        public Color4(Vector4 other) : base(other)
        { }

        public Vector4 toColor255()
        {
            return this * 255f;
        }

        public Vector4 toColor1()
        {
            return this / 255f;
        }
    }
}
