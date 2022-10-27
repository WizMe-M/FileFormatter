namespace FileFormatter;

public class Project
{
    private readonly IEnumerable<string> _ignoreDirs = new[] { "bin", "obj", "Migrations" };
    private readonly IEnumerable<string> _includeExtensions = new[] { ".cs", ".axaml", ".axaml.cs" };
    private readonly DirectoryInfo _projectDir;

    public Project(string path)
    {
        _projectDir = new DirectoryInfo(path);
    }

    public ProgramCharacteristic GetCharacteristic()
    {
        var files = ScanDirectories();
        return ProgramCharacteristic.FromFiles(_projectDir.Name, files);
    }

    public IEnumerable<FileInfo> ScanDirectories() => GetChildFiles(_projectDir);

    /// <summary>
    /// Recursively enumerates files with ignore and include settings
    /// </summary>
    /// <param name="directory">Root directory</param>
    /// <returns>All child files' infos</returns>
    private IEnumerable<FileInfo> GetChildFiles(DirectoryInfo directory)
    {
        if (ShouldIgnoreDir(directory)) yield break;

        foreach (var file in EnumerateIncludingFiles(directory))
            yield return file;

        foreach (var subDirectory in directory.EnumerateDirectories())
        {
            foreach (var file in GetChildFiles(subDirectory)) 
                yield return file;
        }
    }

    private IEnumerable<FileInfo> EnumerateIncludingFiles(DirectoryInfo dir)
    {
        return dir.EnumerateFiles().Where(ShouldIncludeFile);
    }

    private bool ShouldIgnoreDir(FileSystemInfo directory) => _ignoreDirs.Contains(directory.Name);

    private bool ShouldIncludeFile(FileSystemInfo file) => _includeExtensions.Contains(file.Extension);
}