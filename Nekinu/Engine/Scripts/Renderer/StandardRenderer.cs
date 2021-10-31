using OpenTK.Graphics.ES30;
using System;
using System.Collections.Generic;
using Nekinu.EngineDebug;
using Nekinu.SystemCache;

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

                shader.Bind();

                if (mesh.VaoInitialized)
                {
                    bindModel(mesh);

                    draw_calls++;

                    foreach (Entity entity in batch.batch_render)
                    {
                        shader.LoadTransformationMatrix(entity.transformationMatrix);
                        shader.LoadCameraView(camera.View);
                        shader.LoadCameraProjection(camera.projection);

                        GL.DrawElements(PrimitiveType.Triangles, mesh.VertexCount, DrawElementsType.UnsignedInt,
                            IntPtr.Zero);

                        vertex_count += mesh.VertexCount;

                        //unBindTexture();
                    }

                    unBindModel(mesh);
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

        private void bindModel(Mesh mesh)
        {
            Debug.WriteLine(mesh.VAOID + " " + mesh.VBOS.Count);
            
            GL.BindVertexArray(mesh.VAOID);

            for (int i = 0; i < mesh.VBOS.Count; i++)
            {
                GL.EnableVertexAttribArray(i);
            }
        }

        private void unBindTexture()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        private void unBindModel(Mesh mesh)
        {
            for (int i = 0; i < mesh.VBOS.Count; i++)
            {
                GL.DisableVertexAttribArray(i);
            }

            GL.BindVertexArray(0);
        }
    }
}