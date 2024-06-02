﻿using Microsoft.EntityFrameworkCore;
using UrlShortener.Database;

namespace UrlShortener.Service
{
    public class CodeGenerator
    {
        private readonly Random _random = new();
        private readonly DbStorageContext _dbContext;
        private const int CodeLength = 7;
        private const string Alphabet =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public CodeGenerator(DbStorageContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> GenerateUniqueCode()
        {
            string code;
            int batchSize = 10;

            do
            {
                var codesBatch = GenerateCodesBatch(batchSize);

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
            }

            return codesBatch;
        }

        private async Task<string> FindUniqeCodeInBatch(List<string> codesBatch)
        {
            foreach (var code in codesBatch)
            {
                bool exists = await _dbContext.ShortenedUrls.AnyAsync(r => r.Code == code);

                if (!exists)
                {
                    return code;
                }
            }

            return null;
        }
    }
}
