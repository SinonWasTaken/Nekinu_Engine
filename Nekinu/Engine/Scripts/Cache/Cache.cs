using System.Collections.Generic;
using Nekinu.SceneManage;
using Nekinu.Shaders;
using OpenTK.Graphics.ES30;
using Nekinu.EngineDebug;

namespace Nekinu.SystemCache
{
    public class Cache
    {
        private static List<CacheDictionary> mesh_dictionary;
        private static List<CacheDictionary> texture_dictionary;
        private static List<CacheDictionary> shader_dictionary;

        private static List<Mesh> loaded_meshes;
        private static List<Texture> loaded_textures;
        private static List<Shader> loaded_shaders;

        public static void InitCache()
        {
            loaded_meshes = new List<Mesh>();
            loaded_textures = new List<Texture>();
            loaded_shaders = new List<Shader>();

            mesh_dictionary = new List<CacheDictionary>();
            texture_dictionary = new List<CacheDictionary>();
            shader_dictionary = new List<CacheDictionary>();
        }

        public static void AddMesh(Mesh mesh)
        {
            if (MeshExists(mesh.Location) == null)
            {
                Debug.WriteLine($"Adding mesh {mesh.Location}");
                loaded_meshes.Add(mesh);

                mesh_dictionary.Add(new CacheDictionary(mesh.VAOID, 1));
            }
            else
            {
                for (int i = 0; i < mesh_dictionary.Count; i++)
                {
                    Debug.WriteLine($"{mesh_dictionary[i].Key_ID} {mesh.VAOID}");
                    
                    if (mesh_dictionary[i].isKey(mesh.VAOID))
                    {
                        Debug.WriteLine("The same!");
                        mesh_dictionary[i].Increment();
                        return;
                    }
                }
                
                Debug.WriteError("Error adding mesh: The mesh exists, but doesn't? WTF");
            }
        }

        public static void RemoveMesh(Mesh mesh)
        {
            foreach (Mesh loadedMesh in loaded_meshes)
            {
                if (MeshExists(mesh.Location) != null)
                {
                    Debug.WriteLine("Beginning to remove mesh!");
                    foreach (CacheDictionary dictionary in mesh_dictionary)
                    {
                        if (dictionary.isKey(mesh.VAOID))
                        {
                            Debug.WriteLine("Mesh exists in dictionary!");
                            dictionary.Remove();

                            if (dictionary.Count == 0)
                            {
                                Debug.WriteLine($"Cleaning up mesh! {mesh.Location}");
                                mesh.CleanUp();
                                mesh_dictionary.Remove(dictionary);
                                loaded_meshes.Remove(loadedMesh);
                            }
                        }
                    }
                }
            }
        }
        
        public static void AddTexture(Texture texture)
        {
            if (TextureExists(texture) == null)
            {
                loaded_textures.Add(texture);
                
                texture_dictionary.Add(new CacheDictionary(texture.id, 1));
            }
            else
            {
                for (int i = 0; i < texture_dictionary.Count; i++)
                {
                    if (texture_dictionary[i].isKey(texture.id))
                    {
                        texture_dictionary[i].Increment();

                        return;
                    }
                }
                
                Debug.WriteError("Error adding texture: Texture exists, but doesn't? What is going on?");
            }
        }

        public static void RemoveTexture(Texture texture)
        {
            foreach (Texture loadedTexture in loaded_textures)
            {
                if (TextureExists(texture) != null)
                {
                    foreach (CacheDictionary dictionary in texture_dictionary)
                    {
                        if (dictionary.isKey(texture.id))
                        {
                            dictionary.Remove();

                            if (dictionary.Count == 0)
                            {
                                GL.DeleteTexture(texture.id);
                                texture_dictionary.Remove(dictionary);
                                loaded_textures.Remove(loadedTexture);
                            }
                        }
                    }
                }
            }
        }
        
        public static void RemoveTexture(int texture)
        {
            foreach (Texture loadedTexture in loaded_textures)
            {
                if (TextureExists(texture) != null)
                {
                    foreach (CacheDictionary dictionary in texture_dictionary)
                    {
                        if (dictionary.isKey(texture))
                        {
                            dictionary.Remove();

                            if (dictionary.Count == 0)
                            {
                                GL.DeleteTexture(texture);
                                texture_dictionary.Remove(dictionary);
                                loaded_textures.Remove(loadedTexture);
                            }
                        }
                    }
                }
            }
        }

        public static void AddShaderProgram(Shader shader)
        {
            if (ShaderExists(shader) == null)
            {
                loaded_shaders.Add(shader);
                
                shader_dictionary.Add(new CacheDictionary(shader.programID, 1));
            }
            else
            {
                for (int i = 0; i < shader_dictionary.Count; i++)
                {
                    if (shader_dictionary[i].isKey(shader.programID))
                    {
                        shader_dictionary[i].Increment();

                        return;
                    }
                }
                
                Debug.WriteError("Error adding shader: Shader exists, but doesn't? Hell has emerged!");
            }
        }

        public static void RemoveShader(Shader shader)
        {
            for (int i = 0; i < loaded_shaders.Count; i++)
            {
                if (ShaderExists(shader) != null)
                {
                    foreach (CacheDictionary dictionary in shader_dictionary)
                    {
                        if (dictionary.isKey(shader.programID))
                        {
                            dictionary.Remove();

                            if (dictionary.Count == 0)
                            {
                                shader.CleanUp();
                                shader_dictionary.Remove(dictionary);
                                loaded_shaders.Remove(shader);
                            }
                        }
                    }
                }
            }
        }
        
        public static Texture TextureExists(Texture texture)
        {
            for (int i = 0; i < loaded_textures.Count; i++)
            {
                if(loaded_textures[i].location == texture.location)
                {
                    return loaded_textures[i];
                }
            }

            return null;
        }
        
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
        
        public static Texture TextureExists(int texture)
        {
            for (int i = 0; i < loaded_textures.Count; i++)
            {
                if(loaded_textures[i].id == texture)
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

        public static Shader ShaderExists(Shader shader)
        {
            for (int i = 0; i < loaded_shaders.Count; i++)
            {
                if (loaded_shaders[i].programID == shader.programID)
                {
                    return loaded_shaders[i];
                }
            }

            return null;
        }
        
        public static void On_NewScene_Loaded()
        {
            DestroyCache();
            InitCache();
            Debug.WriteLine("Destroyed the cache, and remade it");
        }

        public static void DestroyCache()
        {
            foreach (Mesh mesh in loaded_meshes)
            {
                mesh.CleanUp();
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

    public class CacheDictionary
    {
        public int Key_ID { get; private set; }
        public int Count { get; private set; }

        public CacheDictionary(int key, int count)
        {
            Key_ID = key;
            Count = count;
        }

        public bool isKey(int key)
        {
            return Key_ID == key;
        }

        public void Increment()
        {
            Count++;
        }

        public void Remove()
        {
            Count--;
        }
    }
}
