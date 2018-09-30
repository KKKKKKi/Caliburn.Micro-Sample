namespace CaliburnMicroSample.Helpers
{
    using System.Threading.Tasks;

    public static class TaskHelper
    {
#if SILVERLIGHT
        public static Task<T> FromResult<T>(T result)
        {
            var tcs = new TaskCompletionSource<T>();

            tcs.SetResult(result);

            return tcs.Task;
        }

        public static Task Delay(int milliseconds)
        {
            return Task.Factory.StartNew(() => Thread.Sleep(milliseconds));
        }
#else
        public static Task<T> FromResult<T>(T result) => Task.FromResult(result);

        public static Task Delay(int milliseconds) => Task.Delay(milliseconds);
#endif
    }
}
