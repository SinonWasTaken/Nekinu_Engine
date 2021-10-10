using OpenTK.Graphics.ES30;
using OpenTK.Mathematics;

namespace Nekinu.Shaders
{
    public class ShaderUniform
    {
        public string location { get; private set; }

        public ShaderUniform(int programID, string location)
        {
            this.location = location;

            uniform_location_id = GL.GetUniformLocation(programID, location);
        }

        public int uniform_location_id { get; private set; }

        public void LoadValue(float value)
        {
            GL.Uniform1(uniform_location_id, value);
        }
        public void LoadValue(Vector2 value)
        {
            GL.Uniform2(uniform_location_id, value.x, value.y);
        }
        public void LoadValue(float x, float y)
        {
            GL.Uniform2(uniform_location_id, x, y);
        }
        public void LoadValue(Vector3 value)
        {
            GL.Uniform3(uniform_location_id, value.x, value.y, value.z);
        }
        public void LoadValue(float x, float y, float z)
        {
            GL.Uniform3(uniform_location_id, x, y, z);
        }
        public void LoadValue(Vector3 value, int size)
        {
            GL.Uniform3(uniform_location_id, size, new float[] { value.x, value.y, value.z });
        }
        public void LoadValue(float x, float y, float z, int size)
        {
            GL.Uniform3(uniform_location_id, size, new float[] { x, y, z });
        }
        public void LoadValue(Vector4 value)
        {
            GL.Uniform4(uniform_location_id, value.x, value.y, value.z, value.w);
        }
        public void LoadValue(float X, float Y, float Z, float W)
        {
            GL.Uniform4(uniform_location_id, X, Y, Z, W);
        }
        public void LoadValue(Vector4 value, int size)
        {
            GL.Uniform4(uniform_location_id, size, new float[] { value.x, value.y, value.z, value.w });
        }
        public void LoadValue(float X, float Y, float Z, float W, int size)
        {
            GL.Uniform4(uniform_location_id, size, new float[] { X, Y, Z, W });
        }
        public void LoadValue(Matrix4 matrix)
        {
            GL.UniformMatrix4(uniform_location_id, true, ref matrix);
        }

        public void LoadValue(bool value)
        {
            GL.Uniform1(uniform_location_id, value == true ? 1 : 0);
        }
    }
}
