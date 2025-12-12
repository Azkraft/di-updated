using System.Text.Json;

namespace TagCloudLibrary.Preprocessor;

public class WordAnalysis
{
	public List<AnalysisInfo> Analysis { get; set; }
	public string Text { get; set; }

	public WordWithPartOfSpeech ToWordWithPartOfSpeech()
	{
		var firstGramme = Analysis.First().Gramme;
		var endPartOfSpeechText = firstGramme.IndexOfAny([',', '=']);
		var partOfSpeech = JsonSerializer.Deserialize<PartOfSpeech>(firstGramme[..endPartOfSpeechText]);

		return new(Text, partOfSpeech);
	}
}
