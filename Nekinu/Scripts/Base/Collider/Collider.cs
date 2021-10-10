namespace Nekinu
{
    public class Collider : Component
    {
        protected bool canCheckForCollision { get; set; }

        public float invMass { get; private set; }

        public bool isTrigger { get; set; }

        public float restitution { get; set; }

        public float mass { get; set; }
        public float drag { get; set; }

        public Collider()
        {
            restitution = 1f;
            mass = 5;
            drag = 1;
            invMass = 1 / mass;

            isTrigger = false;

            canCheckForCollision = true;
        }

        public Collider(bool trigger = false)
        {
            restitution = 1f;
            mass = 5;
            drag = 1;
            invMass = 1 / mass;

            isTrigger = trigger;

            canCheckForCollision = true;
        }

        public virtual void doImpulseCollision(Manifold manifold)
        {}

        protected virtual Manifold checkForCollision(Entity entity) => null;
    }
}
