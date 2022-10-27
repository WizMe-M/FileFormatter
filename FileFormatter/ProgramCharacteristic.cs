using OfficeOpenXml;

namespace FileFormatter;

public class ProgramCharacteristic
{
    private readonly string _filePath;
    private readonly FileAnalyzer[] _analyzers;

    private ProgramCharacteristic(string name, IEnumerable<FileAnalyzer> analyzers)
    {
        _analyzers = analyzers.ToArray();
        _filePath = Path.Combine(Settings.ResultPath, $"{name}.xlsx");
    }

    public static ProgramCharacteristic FromFiles(string name, IEnumerable<FileInfo> files)
    {
        var fileAnalyzers = files.Select(info => new FileAnalyzer(info)).ToArray();
        return new ProgramCharacteristic(name, fileAnalyzers);
    }

    public void Save()
    {
        var bytes = GenerateExcel();
        Directory.CreateDirectory(Settings.ResultPath);
        File.WriteAllBytes(_filePath, bytes);
    }

    private byte[] GenerateExcel()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        var package = new ExcelPackage();

        var sheet = package.Workbook.Worksheets.Add("Характеристика программы")!;

        sheet.Cells[1, 1].Value = "№";
        sheet.Cells[1, 2].Value = "Название класса";
        sheet.Cells[1, 3].Value = "Размер модуля";
        sheet.Cells[1, 4].Value = "Используемые библиотеки";
        sheet.Cells[1, 5].Value = "Дополнительные файлы";
        sheet.Cells[2, 1].Value = 1;
        sheet.Cells[2, 2].Value = 2;
        sheet.Cells[2, 3].Value = 3;
        sheet.Cells[2, 4].Value = 4;
        sheet.Cells[2, 5].Value = 5;

        var startRow = sheet.Dimension.Rows + 1;
        WriteToTable(sheet, startRow);

        return package.GetAsByteArray();
    }

    private void WriteToTable(ExcelWorksheet sheet, int startRow)
    {
        for (var i = 0; i < _analyzers.Length; i++)
        {
            var row = startRow + i;
            sheet.Cells[row, 1].Value = i + 1;

            var file = _analyzers[i];
            sheet.Cells[row, 2].Value = file.Name;
            sheet.Cells[row, 3].Value = $"{file.WeightKiloBytes} Кб";
            sheet.Cells[row, 4].Value = file.Imports;
            sheet.Cells[row, 5].Value = "Нет";
        }
    }
}