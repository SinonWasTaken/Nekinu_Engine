using Nekinu.SceneManage;
using OpenTK.Graphics.ES30;
using OpenTK.Mathematics;
using System.Collections.Generic;
using Nekinu.EngineDebug;

namespace Nekinu.Render
{
    public class MasterRenderer
    {
        private static List<IRenderer> Renderers = new List<IRenderer>();

        public static FrameBuffer buffer;

        private static FrameBuffer.FrameBufferSpecification spec;

        public MasterRenderer(params IRenderer[] renderers)
        {
            Batch.InitBatch();

            foreach (IRenderer renderer in renderers)
            {
                Renderers.Add(renderer);
            }

#if DEBUG
            spec = new FrameBuffer.FrameBufferSpecification();
            spec.width = Window.w_width;
            spec.height = Window.w_height;
            buffer = new FrameBuffer(spec);
#endif
        }

        public static void Render()
        {
            if (SceneManager.loadedScene != null)
            {
                List<Entity> entities = getActiveEntities();

                Entity cameraEntity = getMainCamera(entities);

                if (cameraEntity != null)
                {
#if DEBUG
                    buffer.Bind();
#endif
                    Camera camera = cameraEntity.GetComponent<Camera>();

                    sortEntities(entities, camera.View * camera.projection);

                    GL.Enable(EnableCap.CullFace);
                    GL.Enable(EnableCap.DepthTest);

                    GL.CullFace(CullFaceMode.Back);

                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                    GL.ClearColor(camera.color.x, camera.color.y, camera.color.z, camera.color.w);

                    foreach (IRenderer renderer in Renderers)
                    {
                        renderer.doRender(camera, entities);
                    }

#if DEBUG
                    buffer.Unbind();
#endif
                }
                else
                {
                    Debug.WriteError("There is no main camera in the scene!");
                }

                Batch.Clear();
            }
            else
            {
                SceneManager.NewScene();
            }
        }

        public static void addRenderer(IRenderer renderer)
        {
            Renderers.Add(renderer);
        }

        private static List<Entity> getActiveEntities()
        {
            List<Entity> entities = new List<Entity>();

            if (SceneManager.loadedScene != null)
            {
                for (int i = 0; i < SceneManager.loadedScene.sceneEntities.Count; i++)
                {
                    Entity entity = SceneManager.loadedScene.sceneEntities[i];

                    if (entity.isActive)
                    {
                        entities.Add(entity);

                        entities.AddRange(sortActiveEntityChildren(entity));
                    }
                }
            }

            return entities;
        }

        private static List<Entity> sortActiveEntityChildren(Entity entity)
        {
            List<Entity> entities = new List<Entity>();

            for (int i = 0; i < entity.children.Count; i++)
            {
                Entity child = entity.children[i];
                if (child.isActive)
                {
                    entities.Add(child);

                    entities.AddRange(sortActiveEntityChildren(child));
                }
            }

            return entities;
        }

        public static Entity getMainCamera(List<Entity> entities)
        {
            foreach (Entity entity in entities)
            {
                if (entity.isActive)
                {
                    Camera camera = entity.GetComponent<Camera>();
                    if (camera != null)
                    {
                        if (camera.isActive)
                        {
                            return entity;
                        }
                    }
                }
            }

            return null;
        }

        public static void OnWindowResize()
        {
            if (buffer != null) 
            {
                buffer.Delete();
                spec.width = Window.w_width;
                spec.height = Window.w_height;
                buffer = new FrameBuffer(spec);
            }
        }

        public static void End()
        {
#if DEBUG
            buffer.Delete();
#endif
        }

        public static void sortEntities(List<Entity> entities, Matrix4 vp)
        {
            foreach (Entity entity in entities)
            {
                Mesh mesh = entity.GetComponent<Mesh>();

                if (mesh != null && mesh.isActive)
                {
                    Batch batch = Batch.doesBatchExist(entity, mesh);
                    if (batch != null)
                    {
                        batch.addEntity(entity);
                    }
                    else
                    {
                        Batch.addNewBatch(entity, mesh);
                    }
                }
            }
        }

        public static bool isPointVisible(Vector3 point, Matrix4 VP)
        {
            OpenTK.Mathematics.Vector4 clip = OpenTK.Mathematics.Vector4.TransformRow(new OpenTK.Mathematics.Vector4(point.x, point.y, point.z, 1), VP);
            if (clip.Z <= 0)
                return false;
            Vector2 UV = new Vector2(clip.X / clip.W, clip.Y / clip.W);

            if (UV.x <= -1 || UV.x >= 1 || UV.y <= -1 || UV.y >= 1)
                return false;
            return true;
        }
    }
}