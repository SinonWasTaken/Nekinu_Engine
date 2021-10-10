using System;
using OpenTK.Graphics.ES30;
using OpenTK.Mathematics;
using System.IO;
using System.Collections.Generic;
using Nekinu.Editor;

namespace Nekinu.Shaders
{
    public abstract class Shader
    {
        protected static int programID { get; private set; }

        private int vertexID, geometryID, fragmentID;

        public Shader(string vertexShader, string fragmentShader)
        {
            programID = GL.CreateProgram();

            if ((vertexID = CreateShader(vertexShader, ShaderType.VertexShader)) == 0)
                Environment.Exit(-125);

            if ((fragmentID = CreateShader(fragmentShader, ShaderType.FragmentShader)) == 0)
                Environment.Exit(-125);

            GL.AttachShader(programID, vertexID);
            GL.AttachShader(programID, fragmentID);

            BindAttributes();
            GL.LinkProgram(programID);

            string log = GL.GetProgramInfoLog(programID);

            if (GL.GetProgramInfoLog(programID) != String.Empty)
            {
                Crash_Report.generate_crash_report($"Error linking program! {log}");
                Console.WriteLine("Error linking program! " + log);
                Environment.Exit(-126);
            }

            GL.ValidateProgram(programID);

            GetAllUniformLocations();

            Cache.Loaded_Shaders.Add(this);
        }

        public Shader(string vertexShader, string geometryShader, string fragmentShader)
        {
            programID = GL.CreateProgram();

            if ((vertexID = CreateShader(vertexShader, ShaderType.VertexShader)) == 0)
                Environment.Exit(-125);

            if ((geometryID = CreateShader(geometryShader, ShaderType.GeometryShader)) == 0)
                Environment.Exit(-125);

            if ((fragmentID = CreateShader(fragmentShader, ShaderType.FragmentShader)) == 0)
                Environment.Exit(-125);

            GL.AttachShader(programID, vertexID);
            GL.AttachShader(programID, geometryID);
            GL.AttachShader(programID, fragmentID);

            BindAttributes();
            GL.LinkProgram(programID);

            string log = GL.GetProgramInfoLog(programID);

            if (GL.GetProgramInfoLog(programID) != String.Empty)
            {
                Crash_Report.generate_crash_report($"Error linking program! {log}");
                Console.WriteLine("Error linking program! " + log);
                Environment.Exit(-126);
            }

            GL.ValidateProgram(programID);

            GetAllUniformLocations();

            Cache.Loaded_Shaders.Add(this);
        }

        public abstract void doShaderLoad(Entity self, List<Entity> scene_entities);

        public abstract void loadCamera(Camera camera);

        public abstract void GetAllUniformLocations();
        protected int GetUniformLocation(string name)
        {
            return GL.GetUniformLocation(programID, name);
        }

        public abstract void BindAttributes();
        protected void BindAttribute(int attrib, string name)
        {
            GL.BindAttribLocation(programID, attrib, name);
        }

        private int CreateShader(string shader, ShaderType type)
        {
            string o = "";
            try
            {
                o = new StreamReader(ProjectDetails.rootDirectory + shader).ReadToEnd();
            }
            catch (Exception e)
            {
                Crash_Report.generate_crash_report($"Error loading shader! {e}");
                Environment.Exit(-130);
            }

            if (o != String.Empty)
            {
                int shaderID = GL.CreateShader(type);

                GL.ShaderSource(shaderID, o);

                GL.CompileShader(shaderID);

                string log = GL.GetShaderInfoLog(shaderID);

                if (log != String.Empty)
                {
                    Console.WriteLine("Error compiling shader! " + log + " ShaderType: " + type.ToString() + " " + shader);
                    return 0;
                }

                GL.AttachShader(programID, shaderID);

                return shaderID;
            }
            else
            {
                Crash_Report.generate_crash_report($"Error! Shader: {shader} is empty!");
                Environment.Exit(-126);
                return -1;
            }
        }

        public void Uniform1f(int location, float value)
        {
            GL.Uniform1(location, value);
        }
        public void Uniform2f(int location, Vector2 value)
        {
            GL.Uniform2(location, value.x, value.y);
        }
        public void Uniform2f(int location, float x, float y)
        {
            GL.Uniform2(location, x, y);
        }
        public void Uniform3f(int location, Vector3 value)
        {
            GL.Uniform3(location, value.x, value.y, value.z);
        }
        public void Uniform3f(int location, float x, float y, float z)
        {
            GL.Uniform3(location, x, y, z);
        }
        public void Uniform3f(int location, Vector3 value, int size)
        {
            GL.Uniform3(location, size, new float[] { value.x, value.y, value.z });
        }
        public void Uniform3f(int location, float x, float y, float z, int size)
        {
            GL.Uniform3(location, size, new float[] { x, y, z });
        }
        public void Uniform4f(int location, Vector4 value)
        {
            GL.Uniform4(location, value.x, value.y, value.z, value.w);
        }
        public void Uniform4f(int location, float X, float Y, float Z, float W)
        {
            GL.Uniform4(location, X, Y, Z, W);
        }
        public void Uniform4f(int location, Vector4 value, int size)
        {
            GL.Uniform4(location, size, new float[] { value.x, value.y, value.z, value.w });
        }
        public void Uniform4f(int location, float X, float Y, float Z, float W, int size)
        {
            GL.Uniform4(location, size, new float[] { X, Y, Z, W });
        }
        public void UniformMatrix4(int location, Matrix4 matrix)
        {
            GL.UniformMatrix4(location, true, ref matrix);
        }
        public void UniformBool(int location, bool value)
        {
            GL.Uniform1(location, value == true ? 1 : 0);
        }

        public void Bind()
        {
            GL.UseProgram(programID);
        }

        public void UnBind()
        {
            GL.UseProgram(0);
        }

        public void CleanUp()
        {
            GL.DetachShader(programID, vertexID);
            GL.DetachShader(programID, geometryID);
            GL.DetachShader(programID, fragmentID);
            GL.DeleteShader(vertexID);
            GL.DeleteShader(geometryID);
            GL.DeleteShader(fragmentID);
            GL.DeleteProgram(programID);
        }
    }
}