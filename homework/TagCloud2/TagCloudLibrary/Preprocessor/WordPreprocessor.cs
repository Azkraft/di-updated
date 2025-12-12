using System.Diagnostics;
using System.Text.Json;

namespace TagCloudLibrary.Preprocessor;

public class WordPreprocessor(WordPreprocessorOptions options) : IWordPreprocessor
{
	public List<string> Process(List<string> words)
	{
		return GetAnalyzedWords(words)
			.Where(t => !options.ExcludedPartsOfSpeech.Contains(t.PartOfSpeech))
			.Select(t => t.Word)
			.ToList();
	}

	public static List<WordWithPartOfSpeech> GetAnalyzedWords(List<string> words)
	{
		var startInfo = new ProcessStartInfo
		{
			FileName = Path.Combine(Directory.GetCurrentDirectory(), "mystem.exe"),
			Arguments = "-i --format json",
			UseShellExecute = false,
			CreateNoWindow = true,
			RedirectStandardInput = true,
			RedirectStandardOutput = true
		};

		string output;
		using (var myStemProcess = new Process { StartInfo = startInfo })
		{
			myStemProcess.Start();
			myStemProcess.StandardInput.Write(string.Join('\n', words));
			output = myStemProcess.StandardOutput.ReadToEnd();
			myStemProcess.WaitForExit();
		}

		var jsonOutput = JsonSerializer.Deserialize<List<WordAnalysis>>(output);
		return jsonOutput.Select(t => t.ToWordWithPartOfSpeech()).ToList();
	}
}
