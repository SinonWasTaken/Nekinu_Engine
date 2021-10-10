using System;
using System.Threading.Tasks;

namespace Nekinu
{
    public class WaitUntil : IWait
    {
        private Func<bool> predicate;

        public WaitUntil(Func<bool> predicate)
        {
            this.predicate = predicate;
        }

        public async Task run()
        {
            while (!predicate())
            {
                await Task.Delay(100);
            }
        }
    }
}