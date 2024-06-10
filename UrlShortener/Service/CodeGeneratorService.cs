using Microsoft.EntityFrameworkCore;
using UrlShortener.Database;

namespace UrlShortener.Service
{
    public class CodeGeneratorService
    {
        private readonly Random _random = new();
        private readonly DbStorageContext _context;
        private readonly int BatchSize = 10;
        private const int CodeLength = 7;
        private const string Alphabet =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public CodeGeneratorService(DbStorageContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateUniqueCode()
        {
            string? code;

            do
            {
                var codesBatch = GenerateCodesBatch(BatchSize);

                code = await FindUniqeCodeInBatch(codesBatch);
            } 
            while (code is null);

            return code;
        }

        private List<string> GenerateCodesBatch(int batchSize)
        {
            var codesBatch = new List<string>();
            int maxValue = Alphabet.Length;

            for (int i = 0; i< batchSize; i++)
            {
                var codeChars = new char[CodeLength];

                for (int j = 0; j < CodeLength; j++)
                {
                    int randomIndex = _random.Next(maxValue);
                    codeChars[j] = Alphabet[randomIndex];
                }

                codesBatch.Add(new string(codeChars));
            }

            return codesBatch;
        }

        private async Task<string?> FindUniqeCodeInBatch(List<string> codesBatch)
        {
            foreach (var code in codesBatch)
            {
                bool exists = await _context.ShortenedUrls.AnyAsync(r => r.Code == code);

                if (!exists)
                {
                    return code;
                }
            }

            return null;
        }
    }
}
