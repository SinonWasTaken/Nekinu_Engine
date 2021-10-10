using System.Threading.Tasks;

namespace Nekinu
{
    public class WaitForSeconds : IWait
    {
        private float waitSeconds;

        public WaitForSeconds(float seconds)
        {
            waitSeconds = seconds;
        }

        public async Task run()
        {
            await Task.Delay((int)(waitSeconds * 1000));
        }
    }
}