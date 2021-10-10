using Nekinu.SceneManage;
using System.Collections.Generic;

namespace Nekinu
{
    public class Ray
    {
        private Vector3 origin;
        private Vector3 direction;

        public Ray(Vector3 origin, Vector3 direction)
        {
            this.origin = origin;
            this.direction = direction.Normalize();
        }

        public Vector3 Origin => origin;

        public Vector3 Direction => direction;

        public static RayHit Out(Ray ray, float distance)
        {
            //Instead of RayOut being static, have user create a RayOut object.
            //That way I can see if doing all this in a different thread will help ease the load.

            Vector3 newRayPos = new Vector3(ray.Origin);

            List<Entity> sceneEntities = SceneManager.loadedScene.sceneEntities;

            float dist = Vector3.Distance(ray.Origin, newRayPos);

            /*while (dist <= distance)
            {
                newRayPos = ray.Direction;

                for (int i = 0; i < sceneEntities.Count; i++)
                {
                    if (sceneEntities[i].getComponent<Collider>() != null)
                    {
                        if(!entityIsIgnored(ignore, sceneEntities.get(i)))
                        {
                            if (closeEnough(newRayPos, sceneEntities.get(i).getTransform().getPosition(), distance))
                            {
                                Collider collider = (Collider)(sceneEntities.get(i).getComponent(Collider.class));
                                //bool inside = collider.checkForCollision(newRayPos);

                                if (inside)
                                {
                                    return new RayHit(sceneEntities.get(i));
                                }
                            }
                        }
                    }
                }

                dist = Vector3.distance(ray.getOrigin(), newRayPos);
            }*/
            return null;
        }

        /*private static bool entityIsIgnored(RayIgnore ignore, Entity entity)
        {
            if (ignore == null || ignore.getIgnoredEntities().size() == 0)
            {
                return false;
            }

            foreach (Entity e in ignore.getIgnoredEntities())
            {
                if (e == entity)
                {
                    return true;
                }
                else if (e.Name == entity.Name)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool closeEnough(Vector3 ray, Vector3 entityPos, float distance)
        {
            return (Vector3.distance(ray, entityPos) <= distance);
        }*/
    }
}