using FileFormatter;

string? slnPath;
do
{
    Console.Write("Путь до папки Solution: ");
    slnPath = Console.ReadLine();
} while (string.IsNullOrWhiteSpace(slnPath));

var sln = new Solution(slnPath);
var projects = sln.GetProjects();
foreach (var proj in projects)
{
    var programCharacteristic = proj.GetCharacteristic();
    programCharacteristic.Save();
}
Console.WriteLine($"Файлы сохранены по пути: {Settings.ResultPath}");
Console.ReadKey();