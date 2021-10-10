using Nekinu.MeshLoader.VAO;
using OpenTK.Graphics.ES30;
using System;
using System.Collections.Generic;
using Nekinu.Editor;
using Nekinu.Shaders;

namespace Nekinu.Render
{
    public class StandardRenderer : IRenderer
    {
        private static int draw_calls;

        public static int DRAW_CALLS { get; private set; }

        private static int vertex_count;

        public static int VERTEX_COUNT { get; private set; }

        private TestShader shader;

        public StandardRenderer()
        {
            shader = new TestShader("/Resources/ShaderFiles/VertexShader.txt", "/Resources/ShaderFiles/FragmentShader.txt");
        }

        public void doRender(Camera camera, List<Entity> entities)
        {
            render(camera, entities);
        }

        private void render(Camera camera, List<Entity> entities)
        {
            Entity sun = getSun(entities);

            List<Entity> lights = getLights(entities);

            foreach (Batch batch in Batch.batches)
            {
                Mesh mesh = batch.mesh;

                VAO vao = Cache.VAOExists(mesh.Location);

                if (vao != null)
                {
                    shader.Bind();
                    bindModel(vao);

                    draw_calls++;

                    foreach (Entity entity in batch.batch_render)
                    {
                        shader.LoadTransformationMatrix(entity.transformationMatrix);
                        shader.LoadCameraView(camera.View);
                        shader.LoadCameraProjection(camera.projection);

                        GL.DrawElements(PrimitiveType.Triangles, mesh.vertexCount, DrawElementsType.UnsignedInt, IntPtr.Zero);

                        vertex_count += mesh.vertexCount;

                        unBindTexture();
                    }

                    unBindModel(vao);
                    shader.UnBind();
                }
            }

            DRAW_CALLS = draw_calls;
            draw_calls = 0;

            VERTEX_COUNT = vertex_count;
            vertex_count = 0;
        }

        private Entity getSun(List<Entity> scene_Entities)
        {
            for (int i = 0; i < scene_Entities.Count; i++)
            {
                if (scene_Entities[i].GetComponent<Sun>() != null)
                {
                    return scene_Entities[i];
                }
            }

            return null;
        }

        private List<Entity> getLights(List<Entity> scene_Entities)
        {
            List<Entity> lights = new List<Entity>();
            for (int i = 0; i < scene_Entities.Count; i++)
            {
                AreaLight light = scene_Entities[i].GetComponent<AreaLight>();

                if (light != null)
                {
                    lights.Add(scene_Entities[i]);
                }
            }

            return lights;
        }

        private void bindModel(VAO mesh)
        {
            GL.BindVertexArray(mesh.vao);

            for (int i = 0; i < mesh.vbos.Count; i++)
            {
                GL.EnableVertexAttribArray(i);
            }
        }

        private void unBindTexture()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        private void unBindModel(VAO vao)
        {
            for (int i = 0; i < vao.vbos.Count; i++)
            {
                GL.DisableVertexAttribArray(i);
            }

            GL.BindVertexArray(0);
        }
    }
}