using Microsoft.EntityFrameworkCore;

namespace UrlShortener.Service
{
    public class CodeGenerator
    {
        private readonly DbContext _dbContext;

        public CodeGenerator(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
