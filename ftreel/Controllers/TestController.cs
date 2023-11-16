using ftreel.DATA;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ftreel.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly AppDBContext _context;

        public TestController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            // On créée une table et on lui ajoute une row
            await _context.Database.EnsureCreatedAsync();
            await _context.Database.ExecuteSqlRawAsync("CREATE TABLE Test (Id int, Name varchar(255))");
            await _context.Database.ExecuteSqlRawAsync("INSERT INTO Test VALUES (1, 'Test')");
            // On récupère la row avec pour nom "Test"
            var test = await _context.Database.ExecuteSqlRawAsync("SELECT * FROM Test WHERE Name = 'Test'");

            // Si la row existe
            if (test != 0)
            {
                // On supprime la table
                await _context.Database.ExecuteSqlRawAsync("DROP TABLE Test");
                return Ok("Connexion établie");
            }
            else
            {
                return BadRequest("Connexion établie mais la table n'existe pas");
            }
        }
    }
}
