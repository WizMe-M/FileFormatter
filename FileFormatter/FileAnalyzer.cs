using System.Text;

namespace FileFormatter;

public class FileAnalyzer
{
    private readonly FileInfo _file;

    public FileAnalyzer(FileInfo file)
    {
        _file = file;
    }

    private long WeightBytes => FileSizeReceiver.GetFileSizeOnDisk(_file.FullName);
    public long WeightKiloBytes => WeightBytes / 1024;

    public string Name => _file.Name;

    public string Imports
    {
        get
        {
            var lines = EnumerateLibraryImports();
            var builder = new StringBuilder();
            builder.AppendJoin('\n', lines);
            return builder.ToString();
        }
    }

    private IEnumerable<string> EnumerateLibraryImports()
    {
        using var streamReader = _file.OpenText();
        while (!streamReader.EndOfStream)
        {
            var line = streamReader.ReadLine();
            if (string.IsNullOrWhiteSpace(line)) continue;

            line = line.Trim();
            if (line.StartsWith("namespace")) yield break;
            if (line.StartsWith("using")) yield return line;
            if (line.StartsWith("xmlns:")) yield return line;
        }
    }

    public string GetCode()
    {
        using var text = _file.OpenText();
        return text.ReadToEnd();
    }
}