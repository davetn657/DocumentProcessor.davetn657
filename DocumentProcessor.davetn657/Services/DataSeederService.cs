using DocumentProcessor.davetn657.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Text;

namespace DocumentProcessor.davetn657.Services;

public interface IDataSeederService
{
    public void SeedIfEmpty();
}

public class DataSeederService : IDataSeederService
{
    private readonly PhonebookContext _dbContext;
    private readonly IFileReaderService _fileReader;
    public DataSeederService(PhonebookContext dbContext, IFileReaderService fileReader)
    {
        _dbContext = dbContext;
        _fileReader = fileReader;
    }

    public void SeedIfEmpty()
    {

        if (!_dbContext.Contacts.Any())
        {
            var contacts = _fileReader.FormatFile(Directory.GetCurrentDirectory(), "SeedData.xlsx");
            _dbContext.Contacts.AddRange(contacts);
            _dbContext.SaveChanges();
        }
    }
}