using System;

namespace Nekinu.TwoD.Colliders
{
    public class _2DCircleCollider : _2DCollider
    {
        public float radius;
        private Vector3 position_offset;
        
        public _2DCircleCollider(){}
        
        public _2DCircleCollider(Vector2 min, Vector2 max) : base(min, max)
        {
        }

        public _2DCircleCollider(Vector2 min, Vector2 max, bool trigger = false) : base(min, max, trigger)
        {
        }

        public Vector3 Position
        {
            get => parent.transform.position + position_offset;
        }

        public bool checkForCircleCollision(_2DCircleCollider a, _2DCircleCollider b)
        {
            float r = a.radius + b.radius;
            r *= r;
            return r < (MathF.Pow(a.position_offset.x + b.Position.x, 2)) + MathF.Pow(a.position_offset.y + b.Position.y, 2);
        }
    }
}