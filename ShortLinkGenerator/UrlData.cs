using System.ComponentModel.DataAnnotations;

namespace ShortLinkGenerator;

public class UrlData
{

    [Key]
    public int Id { get; set; }

    public string Token { get; set; }
    
    public string OriginalUrl { get; set; }
    
    public int Count { get; set; }
    
    public DateTime CreateDate { get; set; }
}