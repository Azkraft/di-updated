namespace TagCloudLibrary.Preprocessor;

public interface IWordPreprocessor
{
	List<string> Process(List<string> words);
}
