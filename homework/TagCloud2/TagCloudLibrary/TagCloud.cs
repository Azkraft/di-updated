using SkiaSharp;
using TagCloudLibrary.Layouter;
using TagCloudLibrary.Preprocessor;
using TagCloudLibrary.Visualizer;

namespace TagCloudLibrary;

public class TagCloud(IWordPreprocessor preprocessor, ICloudLayouter layouter, ITagCloudVisualizer visualizer, TagCloudOptions options)
{
	private readonly List<TextInBox> cloud = [];

	public void BuildTagTree(List<string> words)
	{
		var rnd = new Random();

		var preparedWords = preprocessor.Process(words);
		foreach (var word in preparedWords)
		{
			var fontSize = options.MinFontSize + rnd.NextSingle() * (options.MaxFontSize - options.MinFontSize);
			var text = SKTextBlob.Create(word, new SKFont(SKTypeface.Default, fontSize));
			var rectangle = layouter.PutNextRectangle(new((int)text.Bounds.Width, (int)text.Bounds.Height));
			cloud.Add(new TextInBox(text, new(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom)));
		}
	}

	public SKImage CreateImage() => visualizer.DrawTagCloud(cloud);
}
