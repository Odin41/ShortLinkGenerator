using System.Security.Cryptography;
using MhanoHarkness;
using ShortLinkGenerator.Models;

namespace ShortLinkGenerator.Services;

public class UrlDataRepository : IUrlDataRepository
{
    private readonly Db _context;

    public UrlDataRepository(Db context)
    {
        _context = context;
    }

    public string SaveUrlData(string originalUrl)
    {
        var rng = RandomNumberGenerator.Create();
        var uint32Buffer = new byte[8];
        rng.GetBytes(uint32Buffer);
        
        var shortUrl = Base32Url.ToBase32String(uint32Buffer);

        if (!_context.UrlsDataTable.Any(u => u.Token == shortUrl))
        {
            _context.UrlsDataTable.Add(new UrlData
            {
                OriginalUrl = originalUrl,
                Count = 1,
                CreateDate = DateTime.Now,
                Token = shortUrl
            });

            _context.SaveChanges();
        }
        
        return shortUrl;
    }

    public string GetOriginalUrl(string shortUrl)
    {
        var existUrl = _context.UrlsDataTable.FirstOrDefault(u => u.Token == shortUrl);
        
        if (existUrl == null)
        {
            throw new Exception($"{shortUrl} is not found");
        }
        
        existUrl.Count++;
        _context.SaveChanges();
        
        return existUrl.OriginalUrl;
    }
}