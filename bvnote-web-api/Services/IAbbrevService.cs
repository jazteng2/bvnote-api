using bvnote_web_api.Data.DTO;
namespace bvnote_web_api.Services;
public interface IAbbrevService
{
    public Task<List<AbbrevDTO>> GetAbbrevsAsync();
}
