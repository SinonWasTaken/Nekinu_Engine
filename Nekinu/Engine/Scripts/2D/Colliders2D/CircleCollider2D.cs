using Nekinu;
using System.Collections.Generic;

namespace Nekinu2D.Physics2D
{
    public class CircleCollider2D : Collider
    {
        public float radius { get; set; }
        public Vector2 position { get; set; }

        public CircleCollider2D() : base(false)
        {
            radius = 1;
            position = Vector2.zero;
        }

        public CircleCollider2D(float radius, Vector2 position, bool trigger = false) : base(trigger)
        {
            this.radius = radius;
            this.position = position;
        }

        public override void Update()
        {
            base.Update();

            List<Entity> allCollidersEntity = new List<Entity>();

            allCollidersEntity.AddRange(Entity.GetAllEntitiesWithComponentsOfType<Collider>());

            foreach (Entity entity in allCollidersEntity)
            {
                checkForCollision(entity);
            }
        }

        protected override Manifold checkForCollision(Entity entity)
        {
            if (entity != this.parent)
            {
                Collider collider = entity.GetComponent<Collider>();

                if (collider is CircleCollider2D)
                {
                    CircleCollider2D cCollider = (CircleCollider2D)collider;
                    float r = radius + cCollider.radius;
                    r *= r;
                    //return r < MathF.Pow(manifold. position.x + cCollider.position.x, 2) + MathF.Pow(position.y + cCollider.position.y, 2) ? cCollider : null;
                }
            }

            return null;
        }

        /*public override void doImpulseCollision(Collider collider)
        {
            base.doImpulseCollision(collider);
        }*/
    }
}
