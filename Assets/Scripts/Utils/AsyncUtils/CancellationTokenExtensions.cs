using System.Threading;

namespace Utils.AsyncUtils
{
    public static class CancellationTokenExtensions
    {
        public static void TryCancel(this CancellationTokenSource cancellationTokenSource)
        {
            if (cancellationTokenSource == null)
            {
                return;
            }

            if (cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }
    }
}
