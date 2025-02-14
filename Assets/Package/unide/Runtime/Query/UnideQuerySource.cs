using System.IO;
using Cysharp.Threading.Tasks;

namespace unide
{
    public sealed class UnideQuerySource
    {
        private const int DefaultTimeout = 1000;
        private const int DefaultDelay = 500;

        public IUnideDriver TestDriver { get; }

        public int Timeout { get; set; } = DefaultTimeout;
        public int Delay { get; set; } = DefaultDelay;

        public UnideQuerySource(IUnideDriver testDriver)
        {
            TestDriver = testDriver;
        }

        public UniTask<UnideQuery> CreateQueryContext()
        {
            return UniTask.FromResult(new UnideQuery(this));
        }

        public bool EnableCaptureScreenshotBeforeClick { get; set; }
        public string BaseScreenshotPath { get; set; }
        public string ScreenshotPrefix { get; set; } = string.Empty;

        private int _screenshotCaptureCounter;

        public string TakeScreenshotFilePath(string baseFileName = null)
        {
            if (string.IsNullOrEmpty(baseFileName))
                baseFileName = $"{ScreenshotPrefix}{_screenshotCaptureCounter:D8}.png";

            var filePath = Path.Combine(BaseScreenshotPath, baseFileName);
            _screenshotCaptureCounter++;

            return filePath;
        }
    }
}
