using OpenTK.Graphics.ES30;
using System.Collections.Generic;

namespace Nekinu.MeshLoader.VAO
{
    internal class VAO
    {
        public int vao { get; private set; }

        public List<int> vbos = new List<int>();

        public string mesh_name { get; private set; }

        public VAO(string name)
        {
            mesh_name = name;

            Cache.AddVAO(this);
        }

        public int createVAO()
        {
            vao = GL.GenVertexArray();

            return vao;
        }

        public void Bind()
        {
            GL.BindVertexArray(vao);
        }

        public void BindVBOS()
        {
            for (int i = 0; i < vbos.Count; i++)
            {
                GL.EnableVertexAttribArray(i);
            }
        }

        public void UnbindVBOS()
        {
            for (int i = 0; i < vbos.Count; i++)
            {
                GL.DisableVertexAttribArray(i);
            }
        }

        public void Unbind()
        {
            GL.BindVertexArray(0);
        }

        public void bindIndiciesBuffer(int[] indicies)
        {
            int vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indicies.Length * sizeof(int), indicies, BufferUsageHint.StaticDraw);
            vbos.Add(vbo);
        }

        public void storeData(int number, int size, float[] data)
        {
            int vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(number, size, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            vbos.Add(vbo);
        }

        public void storeIntData(int number, int size, int[] data)
        {
            int vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(int), data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(number, size, VertexAttribPointerType.Int, false, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            vbos.Add(vbo);
        }
    }
}
