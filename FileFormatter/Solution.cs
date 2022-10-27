namespace FileFormatter;

public class Solution
{
    private readonly IEnumerable<string> _ignoreDirs = new[] { ".idea", ".git", ".vs" };
    private readonly string _path;

    public Solution(string path)
    {
        _path = path;
    }

    public IEnumerable<Project> GetProjects()
    {
        var solutionDirectory = new DirectoryInfo(_path);
        foreach (var directory in solutionDirectory.EnumerateDirectories())
        {
            if (ShouldIgnore(directory.Name)) continue;

            var projAnalyzer = new Project(directory.FullName);
            yield return projAnalyzer;
        }
    }

    private bool ShouldIgnore(string directoryName) => _ignoreDirs.Contains(directoryName);
}