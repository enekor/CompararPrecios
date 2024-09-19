using CompararPrecios.Models;
using CompararPrecios.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CompararPrecios.Controllers
{
    public class ComparadorController : ControllerBase
    {
        [HttpGet("/[controller]/test")]
        public IActionResult Index()
        {
            return Ok("hola");
        }

        [HttpGet("/[controller]/GetCatalogs")]
        public async Task<IActionResult> GetCatalogs(string nombre, int page)
        {
            PriceComparator comparator = new PriceComparator();
            var catalog = await comparator.GetCatalog(nombre,page);

            return Ok(catalog);
        }


    }
}
