using OpenTK.Graphics.ES30;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace Nekinu.Shaders
{
    public class AnimatedShader : Shader
    {
        private static int max_num_joints = 50;

        private static string vertex = "/Shader/AnimatedVertex.txt";
        private static string fragment = "/Shader/AnimatedFragment.txt";

        private ShaderUniform transformationMatrix = new ShaderUniform(programID, "transformation");
        private ShaderUniform projectionMatrix = new ShaderUniform(programID, "projectionview");
        private ShaderUniform lightDirection = new ShaderUniform(programID, "lightDirection");
        private ShaderArrayUniform jointTransforms = new ShaderArrayUniform(max_num_joints, programID, "joints");

        public AnimatedShader() : base(vertex, fragment)
        {
        }

        public override void BindAttributes()
        {
            GL.BindAttribLocation(programID, 0, "positions");
            GL.BindAttribLocation(programID, 1, "textures");
            GL.BindAttribLocation(programID, 2, "normals");
            GL.BindAttribLocation(programID, 3, "ids");
            GL.BindAttribLocation(programID, 4, "weights");
        }

        public override void doShaderLoad(Entity self, List<Entity> scene_entities)
        {
            transformationMatrix.LoadValue(self.transformationMatrix);
        }

        public override void GetAllUniformLocations()
        {}

        public override void loadCamera(Camera camera)
        {
            projectionMatrix.LoadValue(camera.View * camera.projection);
        }

        public void loadLight(Vector3 direction)
        {
            lightDirection.LoadValue(direction);
        }

        public void loadJoints(Matrix4[] joints)
        {
            jointTransforms.LoadValue(programID, true, joints);
        }
    }
}
