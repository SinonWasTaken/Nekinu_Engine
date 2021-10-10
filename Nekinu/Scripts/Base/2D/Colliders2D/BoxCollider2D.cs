using Nekinu;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nekinu2D.Physics2D
{
    public class BoxCollider2D : Collider_2D
    {
        public Vector2 min { get; set; }
        public Vector2 max { get; set; }

        public BoxCollider2D() : base(false)
        {
            min = Vector2.zero;
            max = Vector2.one;
        }

        public BoxCollider2D(Vector2 min, Vector2 max, bool trigger = false) : base(trigger)
        {
            this.min = min; this.max = max;
        }

        public override void Update()
        {
            if (canCheckForCollision)
            {
                doCollisionCheck();
            }
        }

        private async Task<bool> doCollisionCheck()
        {
            canCheckForCollision = false;
            List<Entity> allCollidersEntity = new List<Entity>();

            allCollidersEntity.AddRange(Entity.GetAllEntitiesWithComponentsOfType<Collider>());

            foreach (Entity entity in allCollidersEntity)
            {
                Manifold manifold = checkForCollision(entity);

                if (manifold != null)
                {
                    Collider collider = entity.GetComponent<Collider>();

                    doImpulseCollision(manifold);

                    if (isTrigger)
                    {
                        //collider.OnTriggerEnter(this);
                    }
                    else
                    {
                        //collider.OnColliderEnter(this);
                    }
                }
                await Task.Delay(200);
            }

            canCheckForCollision = true;
            return true;
        }

        protected override Manifold checkForCollision(Entity entity)
        {
            if (entity != this.parent)
            {
                Collider collider = entity.GetComponent<Collider>();

                Manifold manifold = new Manifold(this, collider);

                if (collider is BoxCollider2D)
                {
                    BoxCollider2D bCollider = (BoxCollider2D)collider;

                    Vector2 n = bCollider.Position - Position;

                    float a_extent = (max.x - bCollider.min.x) / 2;
                    float b_extent = (bCollider.max.x - min.x) / 2;

                    float x_overlap = a_extent + b_extent - MathF.Abs(n.x);

                    if (x_overlap > 0)
                    {
                        a_extent = (max.y - bCollider.min.y) / 2;
                        b_extent = (bCollider.max.y - min.y) / 2;

                        float y_overlap = a_extent + b_extent - MathF.Abs(n.y);

                        if (y_overlap > 0)
                        {
                            if (x_overlap > y_overlap)
                            {
                                if (n.x < 0)
                                {
                                    manifold.normal = new Vector2(-1, 0);
                                }
                                else
                                {
                                    manifold.normal = Vector2.zero;
                                }

                                manifold.penetration = x_overlap;
                                return manifold;
                            }
                            else
                            {
                                if (n.y < 0)
                                {
                                    manifold.normal = new Vector2(-1, 0);
                                }
                                else
                                {
                                    manifold.normal = Vector2.zero;
                                }

                                manifold.penetration = y_overlap;
                                return manifold;
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public override void doImpulseCollision(Manifold manifold)
        {
            if(manifold.B is BoxCollider2D)
            {
                BoxCollider2D col2D = (BoxCollider2D)manifold.B;

                Vector2 rv = col2D.Velocity - Velocity;

                Vector2 normal = rv.Normalize();

                float velAlongNormal = Vector2.Dot(rv, normal);

                if(velAlongNormal > 0)
                {
                    return;
                }

                float e = MathF.Min(restitution, col2D.restitution);

                float j = -(1 + e) * velAlongNormal;
                j /= invMass + col2D.invMass;

                Vector2 impulse = normal * j;
                Velocity -= invMass * impulse;
                col2D.Velocity += col2D.invMass * impulse;
            }
        }
    }
}
