using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Nekinu.EngineDebug;
using Nekinu.MeshLoader.VAO;
using OpenTK.Graphics.ES30;

namespace Nekinu
{
    public class Loader
    {
        public static Mesh loadModel(string location, float[] pos, float[] text, float[] normal, int[] indicies)
        {
            VAO vao = new VAO(location);
            vao.createVAO();
            GL.BindVertexArray(vao.vao);
            vao.bindIndiciesBuffer(indicies);
            vao.storeData(0, 3, pos);
            vao.storeData(1, 2, text);
            vao.storeData(2, 3, normal);
            GL.BindVertexArray(0);
            return new Mesh(location, indicies.Length);
        }

        public static Mesh loadModel(string location, float[] pos, float[] text, int[] indicies)
        {
            VAO vao = new VAO(location);
            vao.createVAO();
            GL.BindVertexArray(vao.vao);
            vao.bindIndiciesBuffer(indicies);
            vao.storeData(0, 3, pos);
            vao.storeData(1, 2, text);
            GL.BindVertexArray(0);
            return new Mesh(location, indicies.Length);
        }

        public static Mesh loadModel(string location, float[] pos, float[] text)
        {
            VAO vao = new VAO(location);
            vao.createVAO();
            GL.BindVertexArray(vao.vao);

            vao.storeData(0, 3, pos);
            vao.storeData(1, 2, text);

            GL.BindVertexArray(0);
            return new Mesh(location, pos.Length / 3);
        }

        public static Mesh loadModel(string location, float[] pos)
        {
            VAO vao = new VAO(location);
            vao.createVAO();
            GL.BindVertexArray(vao.vao);
            vao.storeData(0, 3, pos);
            GL.BindVertexArray(0);
            return new Mesh(location, vao.vao);
        }

        public static Mesh loadModel(string location, Vector3[] pos)
        {
            VAO vao = new VAO(location);
            vao.createVAO();
            GL.BindVertexArray(vao.vao);

            float[] position = new float[pos.Length * 3];

            int index = 0;

            for (int i = 0; i < pos.Length; i++)
            {
                position[index++] = pos[i].x;
                position[index++] = pos[i].y;
                position[index++] = pos[i].z;
            }

            vao.storeData(0, 3, position);
            GL.BindVertexArray(0);
            return new Mesh(location, position.Length / 3);
        }

        //https://stackoverflow.com/questions/11645368/opengl-c-sharp-opentk-load-and-draw-image-functions-not-working
        public static int loadTexture(string file)
        {
            Texture t = Cache.TextureExists(file);
            if (t == null)
            {
                try
                {
                    int texture = 0;
                    
                    StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + file);
                    Bitmap bitmap = (Bitmap) Bitmap.FromStream(reader.BaseStream);

                    GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
                    texture = GL.GenTexture();
                    GL.BindTexture(TextureTarget.Texture2D, texture);

                    BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                        ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    GL.TexImage2D((All) TextureTarget.Texture2D, 0, (All) PixelInternalFormat.Rgba, data.Width,
                        data.Height, 0, (All) OpenTK.Graphics.OpenGL.PixelFormat.Bgra, (All) PixelType.UnsignedByte,
                        data.Scan0);

                    bitmap.UnlockBits(data);

                    OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D,
                        OpenTK.Graphics.OpenGL.TextureParameterName.TextureMinFilter,
                        (int) OpenTK.Graphics.OpenGL.TextureMinFilter.Linear);
                    OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D,
                        OpenTK.Graphics.OpenGL.TextureParameterName.TextureMagFilter,
                        (int) OpenTK.Graphics.OpenGL.TextureMagFilter.Linear);
                    OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D,
                        OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapS,
                        (int) OpenTK.Graphics.OpenGL.TextureWrapMode.Repeat);
                    OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D,
                        OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapT,
                        (int) OpenTK.Graphics.OpenGL.TextureWrapMode.Repeat);

                    GL.BindTexture(TextureTarget.Texture2D, 0);

                    return texture;
                }
                catch (Exception e)
                {
                    Debug.WriteError($"Error loading texture! {e}");
                    return -1;
                }
            }

            return t.id;
        }
    }
}