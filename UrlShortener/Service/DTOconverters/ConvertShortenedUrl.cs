using UrlShortener.Entities;

namespace UrlShortener.Service.DTOconverters
{
    public class ConvertShortenedUrl
    {
        public static ShortenedUrlData ShortendedUrlToShortendedUrlData(ShortenedUrl shortenedUrl)
        {
            return new ShortenedUrlData()
            {
                Id = shortenedUrl.Id,
                Url = shortenedUrl.Url,
                Code = shortenedUrl.Code,
                ShortUrl = shortenedUrl.ShortUrl,
                CreationTime = shortenedUrl.CreationTime,
            };
        }

        public static ShortenedUrl ShortenedUrlDataToShortenedUrl(ShortenedUrlData shortenedUrlData)
        {
            return new ShortenedUrl() 
            {  
                Id = shortenedUrlData.Id,
                Code = shortenedUrlData.Code,
                Url = shortenedUrlData.ShortUrl,
                ShortUrl = shortenedUrlData.ShortUrl,
                CreationTime = shortenedUrlData.CreationTime 
            };
        }
    }
}
