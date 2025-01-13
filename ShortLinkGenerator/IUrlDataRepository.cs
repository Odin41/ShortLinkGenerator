namespace ShortLinkGenerator;

public interface IUrlDataRepository
{
    string SaveUrlData(string originalUrl);
    string GetOriginalUrl(string shortUrl);
}