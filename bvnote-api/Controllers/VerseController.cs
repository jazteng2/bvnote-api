using bvnote_api.Data;
using bvnote_api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace bvnote_api.Controllers
{
    [Route("api/verses")]
    public class VerseController : Controller
    {
        public readonly VerseContext _verseContext;
        public VerseController(DbContext dbContext)
        {
            _verseContext = new VerseContext(dbContext);
        }

        [HttpGet("{id}")]
        public ActionResult<Verse> GetById(Guid id)
        {
            return _verseContext.GetById(id);
        }

        [HttpPost]
        public void PostVerses()
        {
            _verseContext.Populate();
        }
    }
}
