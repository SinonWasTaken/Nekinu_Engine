using System;
using Nekinu;
//https://gamedevelopment.tutsplus.com/tutorials/how-to-create-a-custom-2d-physics-engine-the-basics-and-impulse-resolution--gamedev-6331
namespace Nekinu.TwoD.Colliders
{
    public class _2DCollider : Collider
    {
        public Vector2 min;
        public Vector2 max;

        public _2DCollider() { }

        public _2DCollider(Vector2 min, Vector2 max)
        {
            this.min = min;
            this.max = max;
        }

        public _2DCollider(Vector2 min, Vector2 max, bool trigger = false) : base(trigger)
        {
            this.min = min;
            this.max = max;
        }

        public void resolveCollision(_2DCollider a, _2DCollider b)
        {
            Vector3 rv = b.velocity - a.velocity;

            Vector3 rvNormal = Vector3.Normalize(rv);

            float velAlongNormal = Vector3.Dot(rv, rvNormal);

            if (velAlongNormal > 0)
                return;

            float e = MathF.Min(a.restitution, b.restitution);

            float j = -(1 + e) * velAlongNormal;
            j /= a.invMass + b.invMass;
            Vector3 impulse = j * rvNormal;

            float mass_sum = a.mass + b.mass;
            float ratio = a.mass / mass_sum;
            a.velocity -= ratio * impulse;
            
            ratio = b.mass / mass_sum;
            b.velocity += ratio * impulse;
        }

        public override bool collisionCheck(Manifold m)
        {
            throw new NotImplementedException();
        }
    }
}