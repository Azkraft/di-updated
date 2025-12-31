using SkiaSharp;
using TagCloudLibrary.Layouter;
using TagCloudLibrary.Preprocessor;
using TagCloudLibrary.Visualizer;

namespace TagCloudLibrary;

public class TagCloud(IWordPreprocessor preprocessor, ICloudLayouter layouter, ITagCloudVisualizer visualizer, TagCloudOptions options)
{
	private readonly List<TextInBox> cloud = [];

	public void BuildTagTree(IEnumerable<string> words)
	{
		var preparedWords = preprocessor
			.Process(words)
			.GroupBy(t => t, (key, elements) => new { Word = key, Count = elements.Count() })
			.ToList();

		var fontCoeff = (float)(options.MaxFontSize - options.MinFontSize) / (preparedWords.Max(t => t.Count) - 1f);

		foreach (var group in preparedWords)
		{
			var fontSize = options.MinFontSize + fontCoeff * (group.Count - 1);
			var text = SKTextBlob.Create(group.Word, new SKFont(SKTypeface.Default, fontSize));
			var rectangle = layouter.PutNextRectangle(new((int)text.Bounds.Width, (int)text.Bounds.Height));
			cloud.Add(new TextInBox(text, new(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom)));
		}
	}

	public SKImage CreateImage() => visualizer.DrawTagCloud(cloud);
}
