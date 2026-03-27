using DocumentProcessor.davetn657.Data;
using IronXL;

namespace DocumentProcessor.davetn657.Services;

public interface IExportDataService
{
    void ExportToXlsx();
    void ExportToCsv();
}

public class ExportDataService : IExportDataService
{
    private readonly PhonebookContext _dbContext;
    public ExportDataService(PhonebookContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void ExportToXlsx()
    {
        ExportWorkBook(wb => wb.SaveAs("DocFiles\\Contacts.xlsx"));
    }

    public void ExportToCsv()
    {
        ExportWorkBook(wb => wb.SaveAsCsv("DocFiles\\Contacts.csv"));
    }

    private void ExportWorkBook(Action<WorkBook> save)
    {
        var contacts = _dbContext.Contacts.ToList();

        var workbook = WorkBook.Create(ExcelFileFormat.XLSX);
        var worksheet = workbook.CreateWorkSheet("Contacts");

        worksheet["A1"].Value = "Names";
        worksheet["B1"].Value = "Phone Numbers";
        worksheet["C1"].Value = "Emails";
        worksheet["D1"].Value = "Categories";

        for (int i = 0; i < contacts.Count; i++)
        {
            var row = i + 2;
            worksheet["A" + row].Value = contacts[i].Name;
            worksheet["B" + row].Value = contacts[i].PhoneNumber;
            worksheet["C" + row].Value = contacts[i].Email;
            worksheet["D" + row].Value = contacts[i].Category;
        }

        save(workbook);
    }
}