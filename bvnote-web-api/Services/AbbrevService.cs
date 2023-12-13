using bvnote_web_api.Data;
using bvnote_web_api.Data.DTO;
using Microsoft.EntityFrameworkCore;

namespace bvnote_web_api.Services
{
    public class AbbrevService : IAbbrevService
    {
        private readonly DbBvnContext _db;
        public AbbrevService(DbBvnContext db) 
        {
            _db = db;
        }

        public async Task<List<AbbrevDTO>> GetAbbrevsAsync()
        {
            var abbrevs = await _db.Abbrevs.ToListAsync();
            if (abbrevs is null) return new List<AbbrevDTO>();
            return AbbrevDTO.GetAbbrevDTOs(abbrevs);
        }
    }
}
