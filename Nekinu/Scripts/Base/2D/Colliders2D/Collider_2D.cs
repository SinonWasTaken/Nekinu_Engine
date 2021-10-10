using Nekinu;

namespace Nekinu2D.Physics2D
{
    public class Collider_2D : Collider
    {
        public Vector2 Velocity { get; set; }
        protected Vector2 Position { get; set; }

        public Collider_2D() : base(false)
        {
            Velocity = Vector2.zero;
            Position = Vector2.zero;
        }

        public Collider_2D(bool trigger = false) : base(trigger)
        {
            Velocity = Vector2.zero;
            Position = Vector2.zero;
        }
    }
}
