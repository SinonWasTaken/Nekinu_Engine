using System;

namespace Nekinu
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RequireComponentAttribute : Attribute
    {
        private Component component;

        public RequireComponentAttribute(Type type)
        {
            Component comp = (Component)Activator.CreateInstance(type);
            this.component = comp;
        }

        public Component Component => component;
    }
}