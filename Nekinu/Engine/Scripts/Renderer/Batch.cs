using System.Collections.Generic;

namespace Nekinu.Render
{
    public class Batch
    {
        public static List<Batch> batches = new List<Batch>();
        
        public Mesh mesh { get; private set; }

        public Entity entity { get; private set; }

        public List<Entity> batch_render { get; private set; }

        public Batch(Mesh mesh, Entity entity)
        {
            this.mesh = mesh;
            this.entity = entity;
            
            batch_render = new List<Entity>();
            batch_render.Add(entity);
        }

        public bool doBatchCheck(Entity mat, Mesh mesh)
        {
            if (isMeshEqual(mesh))
            {
                if (isEntityEqual(mat))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void addEntity(Entity entity)
        {
            batch_render.Add(entity);
        }

        private bool isEntityEqual(Entity entity)
        {
            return this.entity == entity ? true : false;
        }

        private bool isMeshEqual(Mesh mesh)
        {
            return this.mesh == mesh ? true : false;
        }

        #region Static

        public static void InitBatch()
        {
            batches = new List<Batch>();
        }

        public static Batch doesBatchExist(Entity entity, Mesh mesh)
        {
            if (batches == null)
            {
                batches = new List<Batch>();
            }

            for (int i = 0; i < batches.Count; i++)
            {
                if(batches[i].doBatchCheck(entity, mesh))
                {
                    return batches[i];
                }
            }

            return null;
        }

        public static void addNewBatch(Entity entity, Mesh mesh)
        {
            if (batches == null)
            {
                batches = new List<Batch>();
            }
            
            batches.Add(new Batch(mesh, entity));
        }
        
        public static void Clear()
        {
            batches.Clear();
        }
        #endregion
    }
}
