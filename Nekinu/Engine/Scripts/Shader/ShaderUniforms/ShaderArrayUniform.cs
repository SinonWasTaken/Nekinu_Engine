using OpenTK.Graphics.ES30;
using OpenTK.Mathematics;

namespace Nekinu.Shaders
{
    public class ShaderArrayUniform
    {
        private int length;

        private int[] locations;

        public ShaderArrayUniform(int length, int programID, string name)
        {
            this.length = length;

            locations = new int[length];

            for (int i = 0; i < length; i++)
            {
                locations[i] = GL.GetUniformLocation(programID, $"{name}[{i}]");
            }
        }

        public void LoadValue(int programID, Vector4[] value)
        {
            for (int i = 0; i < length; i++)
            {
                if (i < value?.Length)
                {
                    GL.Uniform4(locations[i], value[i].x, value[i].y, value[i].z, value[i].w);
                }
                else
                {
                    break;
                }
            }
        }

        public void LoadValue(int programID, bool transpose, Matrix4[] value)
        {
            for (int i = 0; i < length; i++)
            {
                if (i < value?.Length)
                {
                    GL.UniformMatrix4(locations[i], transpose, ref value[i]);
                }
                else
                {
                    break;
                }
            }
        }
    }
}
