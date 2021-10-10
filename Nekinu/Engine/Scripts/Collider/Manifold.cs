namespace Nekinu
{
    public class Manifold
    {
        public Collider A { get; private set; }
        public Collider B { get; private set; }
        public float penetration { get; set; }
        public Vector2 normal { get; set; }

        public Manifold(Collider a, Collider b)
        {
            A = a;
            B = b;
        }
    }
}