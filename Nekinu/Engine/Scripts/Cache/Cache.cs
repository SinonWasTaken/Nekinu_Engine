using System.Collections.Generic;
using Nekinu.MeshLoader.VAO;
using Nekinu.SceneManage;
using Nekinu.Shaders;
using OpenTK.Graphics.ES30;
using Nekinu.EngineDebug;

namespace Nekinu
{
    public class Cache
    {
        private static List<Mesh> loaded_meshes;
        private static List<VAO> loaded_vaos;
        private static List<Texture> loaded_textures;
        private static List<Shader> loaded_shaders;

        public static void InitCache()
        {
            loaded_meshes = new List<Mesh>();
            loaded_vaos = new List<VAO>();
            loaded_textures = new List<Texture>();
            loaded_shaders = new List<Shader>();
        }

        public static List<Shader> Loaded_Shaders
        {
            get => loaded_shaders;
        }

        public static void AddMesh(Mesh mesh)
        {
            if(MeshExists(mesh.Location) == null)
                loaded_meshes.Add(mesh);
        }

        public static void AddTexture(Texture texture)
        {
            loaded_textures.Add(texture);
        }

        public static void AddVAO(VAO mesh)
        {
            if(VAOExists(mesh.mesh_name) == null)
                loaded_vaos.Add(mesh);
        }

        public static int MeshCount => loaded_meshes.Count;

        public static Texture TextureExists(string texture)
        {
            for (int i = 0; i < loaded_textures.Count; i++)
            {
                if(loaded_textures[i].location == texture)
                {
                    return loaded_textures[i];
                }
            }

            return null;
        }

        public static Mesh MeshExists(string mesh)
        {
            for (int i = 0; i < loaded_meshes.Count; i++)
            {
                if (loaded_meshes[i].Location == mesh)
                {
                    return loaded_meshes[i];
                }
            }

            return null;
        }

        public static VAO VAOExists(string mesh)
        {
            for (int i = 0; i < loaded_vaos.Count; i++)
            {
                if(loaded_vaos[i].mesh_name == mesh)
                {
                    return loaded_vaos[i];
                }
            }

            return null;
        }

        public static void RemoveTexture(Texture texture)
        {
            GL.DeleteTexture(texture.id);
        }

        public static void RemoveTexture(int id)
        {
            GL.DeleteTexture(id);
        }

        public static void RemoveMesh(Mesh mesh)
        {
            if (SceneManager.loadedScene != null)
            {
                for (int i = 0; i < SceneManager.loadedScene.sceneEntities.Count; i++)
                {
                    Mesh findMesh = SceneManager.loadedScene.sceneEntities[i].GetComponent<Mesh>();

                    if (findMesh != null)
                    {
                        if (findMesh.Location == mesh.Location)
                        {
                            Debug.WriteLine("Other exists");
                            return;
                        }
                    }
                }

                VAO vao = VAOExists(mesh.Location);

                if(vao != null)
                {
                    RemoveVAO(vao);
                }
            }
        }

        public static void RemoveVAO(VAO vao)
        {
            Debug.WriteLine("Deleting VAO");
            
            foreach (VAO mesh in loaded_vaos)
            {
                GL.DeleteVertexArray(mesh.vao);

                foreach (int vbo in mesh.vbos)
                {
                    GL.DeleteBuffer(vbo);
                }
            }

            loaded_vaos.Remove(vao);
        }

        internal static void ClearMesh()
        {
            if(loaded_vaos != null)
            foreach (VAO mesh in loaded_vaos)
            {
                GL.DeleteVertexArray(mesh.vao);

                foreach (int vbo in mesh.vbos)
                {
                    GL.DeleteBuffer(vbo);
                }
            }

            loaded_meshes = new List<Mesh>();
            loaded_vaos = new List<VAO>();
        }

        public static void DestroyCache()
        {
            foreach (VAO mesh in loaded_vaos)
            {
                GL.DeleteVertexArray(mesh.vao);

                foreach (int vbo in mesh.vbos)
                {
                    GL.DeleteBuffer(vbo);
                }
            }

            foreach (Texture texture in loaded_textures)
            {
                GL.DeleteTexture(texture.id);
            }

            foreach (Shader shader in loaded_shaders)
            {
                shader.CleanUp();
            }
        }
    }
}
