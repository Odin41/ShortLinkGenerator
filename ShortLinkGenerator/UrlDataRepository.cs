using Base62;

namespace ShortLinkGenerator;

public class UrlDataRepository : IUrlDataRepository
{
    private readonly Db _context;

    public UrlDataRepository(Db context)
    {
        _context = context;
    }

    public string SaveUrlData(string originalUrl)
    {
        var shortUrl = new Base62Converter().Encode(originalUrl);

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