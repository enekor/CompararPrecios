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

        public async Task<Dictionary<string,List<Product>>> GetCatalog(string searchKey,int page)
        { 
            List<Product> products = await GetProducts(searchKey,page);
            Dictionary<string,List<Product>> ret = SplitByShopNames(products);

            return ret;
        }

        private async Task<List<Product>> GetProducts(string product, int page)
        {
            string url = configuration[$"ShopUrls:UrlSoySuper"];
            return await scrapper.GetProductFromShopAsync(url.Replace("$$nombre$$",product).Replace("$$page$$",page.ToString()));
        }

        private Dictionary<string,List<Product>> SplitByShopNames(List<Product> products)
        {
            HashSet<string> shops = products.Select(v=>v.Shop).ToHashSet();
            Dictionary<string,List<Product>> ret = new();

            foreach (string shop in shops) { 
                ret.Add(shop, new List<Product>());
                List<Product> productsByShop = products.Where(v=>v.Shop == shop).OrderBy(v=>v.Price).ToList();
                ret[shop].AddRange(productsByShop);
            }

            return ret;
        }
    }
}
