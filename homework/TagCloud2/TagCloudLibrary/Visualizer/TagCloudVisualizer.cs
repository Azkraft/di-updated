using SkiaSharp;
using System.Drawing;
using TagCloudLibrary.Layouter;

namespace TagCloudLibrary.Visualizer;

public class TagCloudVisualizer : ITagCloudVisualizer
{
	private const int pictureBorderSize = 20;
	private const int lineWidth = 5;

	public SKImage DrawTagCloud(List<TextInBox> tagCloud)
	{
		var boundingRectangle = GetBoundingRectangle(tagCloud.Select(t
			=> new Rectangle((int)t.Box.Location.X, (int)t.Box.Location.Y, (int)t.Box.Width, (int)t.Box.Height)));
		var pictureOrigin = boundingRectangle.Location - new Size(pictureBorderSize, pictureBorderSize);

		var imageInfo = new SKImageInfo(
			width: boundingRectangle.Width + 2 * pictureBorderSize,
			height: boundingRectangle.Height + 2 * pictureBorderSize,
			colorType: SKColorType.Rgb888x,
			alphaType: SKAlphaType.Opaque);
		using var surface = SKSurface.Create(imageInfo);
		var canvas = surface.Canvas;

		canvas.Clear(SKColor.Parse("#000000"));
		DrawWords(tagCloud, pictureOrigin, canvas);

		return surface.Snapshot();
	}

	private static Rectangle GetBoundingRectangle(IEnumerable<Rectangle> rects)
	{
		var right = int.MinValue;
		var top = int.MaxValue;
		var left = int.MaxValue;
		var bottom = int.MinValue;

		foreach (var rectangle in rects)
		{
			right = int.Max(right, rectangle.Right);
			top = int.Min(top, rectangle.Top);
			left = int.Min(left, rectangle.Left);
			bottom = int.Max(bottom, rectangle.Bottom);
		}

		var width = right - left;
		var height = bottom - top;
		return new(left, top, width, height);
	}

	private static void DrawWords(List<TextInBox> tagCloud, Point pictureOrigin, SKCanvas canvas)
	{
		var rand = new Random();
		foreach (var word in tagCloud)
		{
			var color = new byte[3];
			rand.NextBytes(color);
			var lineColor = new SKColor(
				red: color[0],
				green: color[1],
				blue: color[2]);

			var paint = new SKPaint
			{
				Color = lineColor,
				StrokeWidth = lineWidth,
				IsAntialias = true,
				Style = SKPaintStyle.Stroke
			};

			canvas.DrawText(
				word.Text,
				word.Box.Location.X,
				word.Box.Location.Y,
				paint);
		}
	}
}
