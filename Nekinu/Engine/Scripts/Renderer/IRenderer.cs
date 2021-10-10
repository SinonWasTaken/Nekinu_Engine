using System.Collections.Generic;

namespace Nekinu.Render
{
    public interface IRenderer
    {
        void doRender(Camera camera, List<Entity> entities);
    }
}