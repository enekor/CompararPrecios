using CompararPrecios.Models;
using HtmlAgilityPack;
using System.Net.Http;
using System;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace CompararPrecios.Utils
{
    public class Scrapper
    {
        public async Task<List<Product>> GetProductFromShopAsync(string shopUrl)
        {
            using var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(shopUrl);
            response.EnsureSuccessStatusCode();

            var htmlContent = await response.Content.ReadAsStringAsync();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);

            string xpath = "//ul[contains(concat(' ', @class, ' '), ' basiclist')]";
            var ulNode = htmlDoc.DocumentNode.SelectSingleNode(xpath);

            if (ulNode != null)
            {
                var liNodes = ulNode.SelectNodes(".//li");

                if (liNodes != null)
                {
                    var liList = new List<string>();

                    foreach (var liNode in liNodes)
                    {
                        Product product = new Product();

                        //nombre y descripcion
                        var productNode = liNode.SelectSingleNode(".//a[contains(concat(' ', @class, ' '), 'name')]");
                        product.Name = CleanString(productNode.SelectSingleNode(".//span[contains(concat(' ', @class, ' '), 'brand')]").InnerText);
                        product.Description = CleanString(productNode.SelectSingleNode(".//span[contains(concat(' ', @class, ' '), 'productname')]").InnerText);

                        //precio
                        productNode = liNode.SelectSingleNode(".//span[contains(concat(' ', @class, ' '), 'details')]");


                        //foto
                        productNode = liNode.SelectSingleNode(".//span[contains(concat(' ', @class, ' '), 'img')]");
                        var priceNode = productNode.SelectSingleNode(".//span[contains(concat(' ', @class, ' '), 'price')]");
                        product.Price = ExtractValue(priceNode.InnerText);

                        //url
                        string innerLiUrlNode = liNode.SelectNodes(".//*")[0].SelectNodes(".//*")[1].InnerHtml;
                        innerLiUrlNode = "https://soysuper.com" + innerLiUrlNode.Split("href=")[1].Split("\"")[1];
                        product.Url = innerLiUrlNode;

                        //supermercado


                    }

                    Console.WriteLine("Lista de elementos <li>:");
                    foreach (var li in liList)
                    {
                        Console.WriteLine(li);
                    }
                }
            }
            else
            {
                Console.WriteLine("No se encontró un <ul> con las clases especificadas.");
            }



            return [];
        }

        private string CleanString(string input)
        {
            // Eliminar saltos de línea y espacios adicionales
            string result = input
                .Trim()                      // Eliminar espacios al principio y al final
                .Replace("\n", " ")          // Reemplazar saltos de línea por espacio
                .Replace("\r", " ")          // Reemplazar retornos de carro por espacio
                .Replace("  ", " ")          // Reemplazar múltiples espacios por un solo espacio
                .Replace("  ", " ");         // Repetir el reemplazo de espacios hasta eliminar los dobles espacios

            // Eliminar espacios adicionales que aún puedan quedar
            result = System.Text.RegularExpressions.Regex.Replace(result, @"\s+", " ");

            return result;
        }

        private double ExtractValue(string input)
        {
            // Expresión regular para encontrar un número decimal seguido de un símbolo de moneda
            string pattern = @"(\d+(\.\d+)?)\s*€";

            // Buscar el patrón en el texto
            var match = Regex.Match(input, pattern);

            if (match.Success)
            {
                // Convertir el valor extraído a decimal
                if (decimal.TryParse(match.Groups[1].Value, out decimal result))
                {
                    return Convert.ToDouble(result);
                }
            }

            // Si no se encuentra un valor válido, devolver 0
            return -1;
        }
    }
}
