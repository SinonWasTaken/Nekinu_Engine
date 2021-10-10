namespace Nekinu
{
    public class RayHit
    {
        private Entity entity;

        public RayHit(Entity entity)
        {
            this.entity = entity;
        }

        public Entity Entity => entity;
    }
}