using System;
using System.Collections.Generic;
using Nekinu.SystemCache;
using Newtonsoft.Json;
using OpenTK.Graphics.ES30;

namespace Nekinu
{
    public class Mesh : Component
    {
        [JsonProperty] [SerializedProperty] private string location;
        [JsonProperty] [SerializedProperty] private int vertexCount;
        [JsonProperty] [SerializedProperty] private VAO vao;
        
        [JsonIgnore]
        public string Location
        {
            get => location;
            set
            {
                location = value;
                
                Cache.RemoveMesh(this);
                Mesh me = ObjectLoader.loadOBJ(location);
                if (me != null)
                {
                    vertexCount = me.vertexCount;
                    Cache.AddMesh(me);
                }
            }
        }
        [JsonIgnore]
        public int VertexCount
        {
            get => vertexCount;
            set => vertexCount = value;
        }

        [JsonIgnore] public bool VaoInitialized => vao != null;
        [JsonIgnore] public int VAOID => vao.VAOID;
        [JsonIgnore] public List<int> VBOS => vao.VBOS;
        
        public Mesh() { }

        public void create_new_vao(string location, int[] indicies, float[] pos, float[] text, float[] normal)
        {
            if(vao != null)
                Cache.RemoveMesh(this);

            this.location = location;
            
            vao = new VAO();
            vao.createVAO();
            vao.Bind();
            vao.bindIndiciesBuffer(indicies);
            vao.storeData(0, 3, pos);
            vao.storeData(1, 2, text);
            vao.storeData(2, 3, normal);
            vao.Unbind();
            
            Cache.AddMesh(this);
        }
        
        public void create_new_vao(int[] indicies, float[] pos, float[] text)
        {
            if(vao != null)
                Cache.RemoveMesh(this);
            
            vao = new VAO();
            vao.createVAO();
            vao.Bind();
            vao.bindIndiciesBuffer(indicies);
            vao.storeData(0, 3, pos);
            vao.storeData(1, 2, text);
            vao.Unbind();
            
            Cache.AddMesh(this);
        }
        
        public override void Awake()
        {
            base.Awake();
            
            if (location != string.Empty)
            {
                Mesh mesh = ObjectLoader.loadOBJ(location);

                if (mesh != null)
                {
                    location = mesh.location;
                    vao = mesh.vao;
                    vertexCount = mesh.vertexCount;
                }
            }
        }

        public override void Editor_Awake()
        {
            base.Editor_Awake();
            
            Awake();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            Cache.RemoveMesh(this);
        }

        public void CleanUp()
        {
            vao.CleanUp();
        }
    }
    
    public class VAO
    {
        [JsonIgnore] private int vao; 

        [JsonIgnore] private List<int> vbos = new List<int>();

        [JsonIgnore] public int VAOID => vao;

        [JsonIgnore] public List<int> VBOS => vbos;
        
        public VAO()
        { }

        public void createVAO()
        {
            vao = GL.GenVertexArray();
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

        public void CleanUp()
        {
            GL.DeleteVertexArray(vao);

            for (int i = 0; i < vbos.Count; i++)
            {
                GL.DeleteBuffer(vbos[i]);
            }
        }
    }
}