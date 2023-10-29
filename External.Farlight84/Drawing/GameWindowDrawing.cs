using System.Drawing;
using GameOverlay.Drawing;
using GameOverlay.Windows;
using Microsoft.Extensions.Logging;

namespace External.Farlight84.Drawing
{
    public interface IGameWindowDrawing
    {
        void Initialize();
        void DrawText(float x, float y, string text);
        void DrawLine(float x, float y, float endX, float endY, float stroke);
        void DrawBox(Rectangle rectangle, float stroke);
        void DrawProgessBar(Rectangle rectangle, float stroke, float percentage);
        void BeginScene();
        void EndScene();
    }

    public class GameWindowDrawing : IGameWindowDrawing
    {
        private readonly ILogger<GameWindowDrawing> _logger;
        private readonly MemoryService _memoryService;
        private readonly Graphics _graphics;
        private readonly GameWindowDrawingConfig _config;

        public GameWindowDrawing(ILogger<GameWindowDrawing> logger, MemoryService memoryService, Graphics graphics, GameWindowDrawingConfig config)
        {
            _logger = logger;
            _memoryService = memoryService;
            _graphics = graphics;
            _config = config;
        }

        public void Initialize()
        {
            _graphics.MeasureFPS = true;
            _graphics.PerPrimitiveAntiAliasing = true;
            _graphics.TextAntiAliasing = true;
            _graphics.VSync = true;
            _graphics.UseMultiThreadedFactories = true;

            var window = new StickyWindow(_memoryService.MainWindowHandle, _graphics)
            {
                IsTopmost = true,
                IsVisible = true,
                FPS = _config.FPS
            };

            window.Create();
            _graphics.WindowHandle = window.Handle;
            _graphics.Setup();

            _logger.LogInformation("Initialized GameWindowDrawing");
        }

        public void DrawText(float x, float y, string text)
        {
            _graphics.DrawText(_graphics.CreateFont(_config.FontName, _config.FontSize), _graphics.CreateSolidBrush(_config.TextColor), x, y, text);
        }

        public void DrawLine(float x, float y, float endX, float endY, float stroke)
        {
            _graphics.DrawLine(_graphics.CreateSolidBrush(_config.LineColor), x, y, endX, endY, stroke);
        }

        public void DrawBox(Rectangle rectangle, float stroke)
        {
            _graphics.DrawBox2D(_graphics.CreateSolidBrush(_config.BoxColor), _graphics.CreateSolidBrush(_config.BoxFillColor), rectangle, stroke);
        }

        public void DrawProgessBar(Rectangle rectangle, float stroke, float percentage)
        {
            _graphics.DrawHorizontalProgressBar(_graphics.CreateSolidBrush(_config.ProgressBarColor), _graphics.CreateSolidBrush(_config.ProgressBarFillColor), rectangle, stroke, percentage);
        }

        public void BeginScene()
        {
            _graphics.BeginScene();
            _graphics.ClearScene();
        }

        public void EndScene()
        {
            _graphics.EndScene();
        }
    }

    public class GameWindowDrawingConfig
    {
        public string FontName { get; set; } = "Futura";
        public int FontSize { get; set; } = 12;
        public Color TextColor { get; set; } = Color.White;
        public Color LineColor { get; set; } = Color.White;
        public Color BoxColor { get; set; } = Color.White;
        public Color BoxFillColor { get; set; } = Color.Transparent;
        public Color ProgressBarColor { get; set; } = Color.White;
        public Color ProgressBarFillColor { get; set; } = Color.Green;
        public int FPS { get; set; } = 120;
    }
}
