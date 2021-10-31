using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Nekinu.EngineDebug;
using Nekinu.SystemCache;
using OpenTK.Graphics.ES30;

namespace Nekinu
{
    public class Loader
    {
        public static Mesh loadModel(string location, float[] pos, float[] text, float[] normal, int[] indicies)
        {
            Mesh mesh = new Mesh();
            
            mesh.VertexCount = indicies.Length;
            mesh.create_new_vao(location, indicies, pos, text, normal);
            
            return mesh;
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