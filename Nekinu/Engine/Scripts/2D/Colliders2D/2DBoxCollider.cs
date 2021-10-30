namespace Nekinu.TwoD.Colliders
{
    public class _2DBoxCollider : _2DCollider 
    {
        public _2DBoxCollider() : base(Vector2.zero, Vector2.one)
        {
        }

        public bool checkForBoxCollision(_2DBoxCollider a, _2DBoxCollider b)
        {
            // Exit with no intersection if found separated along an axis
            if (a.max.x < b.min.x || a.min.x > b.max.x) return false;
            if (a.max.y < b.min.y || a.min.y > b.max.y) return false;

            // No separating axis found, therefor there is at least one overlapping axis
            return true;
        }
    }
}