using CompararPrecios.Models;

namespace CompararPrecios.Utils
{
    public class PriceComparator
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        Scrapper scrapper = new Scrapper();

        public async Task<Dictionary<string,List<Product>>> GetCatalog(string searchKey)
        { 
            List<Product> products = await GetProducts(searchKey);
            Dictionary<string,List<Product>> ret = SplitByShopNames(products);

            return ret;
        }

        private async Task<List<Product>> GetProducts(string product)
        {
            string url = configuration[$"ShopUrls:UrlSoySuper"];
            return await scrapper.GetProductFromShopAsync(url.Replace("$$nombre$$",product));
        }

        private Dictionary<string,List<Product>> SplitByShopNames(List<Product> products)
        {
            return new();
        }
    }
}
