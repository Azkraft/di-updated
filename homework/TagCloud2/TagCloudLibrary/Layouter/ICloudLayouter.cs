using SkiaSharp;
using System.Drawing;

namespace TagCloudLibrary.Layouter;

public interface ICloudLayouter
{
	Rectangle PutNextRectangle(Size rectangleSize);
}
