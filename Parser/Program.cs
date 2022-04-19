using Newtonsoft.Json;
using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;

namespace Parser
{
    class Program
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
        static async Task Main(string[] args)
        {
            Encoding pure = Encoding.Default;
            Encoding utf8 = Encoding.UTF8;

            var result = Parse("https://yastatic.net/market-export/_/partner/help/YML.xml");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                await con.OpenAsync();
                if (con.State != ConnectionState.Open)
                {
                    Console.WriteLine("Ошибка подключения");
                }
                else
                {
                    string url = result.Values.First()["price"].ToString();
                    Console.WriteLine("Подключение установлено");

                    foreach(Offer key in result.Keys)
                    {
                        await AddOfferAsync("offer_id", key.Id, con);
                        await AddOfferAsync("type", key.Type, con, key.Id);
                        await AddOfferAsync("bid", key.Bid, con, key.Id);
                        await AddOfferAsync("cbid", key.Cbid, con, key.Id);
                        await AddOfferAsync("available", key.Available, con, key.Id);

                        foreach (KeyValuePair<string, object> val in result[key])
                        {
                            byte[] utfBytes = utf8.GetBytes(val.Value.ToString());
                            byte[] pureBytes = Encoding.Convert(utf8, pure, utfBytes);
                            string value = pure.GetString(pureBytes);
                            try
                            {
                                await AddOfferAsync(val.Key, value, con, key.Id);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }
            }
        }

        // парсинг данных с сайта
        private static Dictionary<Offer, Dictionary<string, object>> Parse(string url)
        {
            try
            {
                Dictionary<Offer, Dictionary<string, object>> res = new Dictionary<Offer, Dictionary<string, object>> ();

                using (HttpClientHandler handler = new HttpClientHandler { AllowAutoRedirect = false, AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.None })
                {
                    using (var client = new HttpClient(handler))
                    {
                        using (HttpResponseMessage response = client.GetAsync(url).Result)
                        {
                            if(response.IsSuccessStatusCode)
                            {
                                var html = response.Content.ReadAsStringAsync().Result;
                                if(!string.IsNullOrEmpty(html))
                                {
                                    HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                                    document.LoadHtml(html);

                                    var offers = document.DocumentNode.SelectNodes("/yml_catalog/shop/offers/offer");
                                    if(offers != null && offers.Count > 0)
                                    {
                                        foreach(var offer in offers)
                                        {
                                            Offer offerObj = new Offer();
                                            offerObj.Id = int.Parse(offer.Id);
                                            offerObj.Type = offer.Attributes["type"].Value;
                                            offerObj.Bid = offer.Attributes["bid"].Value != null ? int.Parse(offer.Attributes["bid"].Value) : null;
                                            offerObj.Cbid = offer.Attributes["cbid"] != null ? int.Parse(offer.Attributes["cbid"].Value) : null;
                                            offerObj.Available = offer.Attributes["available"].Value;
                                            string str = offer.OuterHtml;
                                            var xDoc = RemoveAttributes(str);
                                            string json = JsonConvert.SerializeXNode(xDoc, Newtonsoft.Json.Formatting.None, true);
                                            JObject offerJson = JObject.Parse(json);

                                            Dictionary<string, object> dictJson =
                                                offerJson.ToObject<Dictionary<string, object>>();
                                            res.Add(offerObj, dictJson);
                                        }

                                        return res;
                                    }
                                    else
                                    {
                                        Console.WriteLine("No offers");
                                    }
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        // добавление оффера в базу данных
        private static async Task AddOfferAsync(string field, object value, SqlConnection con, int id = 0)
        {
            string sqlExpression = "INSERT INTO Offers ("+field+") VALUES ('"+value+"')";
            if (id != 0) sqlExpression = "UPDATE Offers SET " + field + " = '" + value +"' WHERE offer_id = " + id;

            SqlCommand command = new SqlCommand(sqlExpression, con);
            await command.ExecuteNonQueryAsync();
        }

        // убрает атрибуты вложенных полей офферов
        public static XDocument RemoveAttributes(string xml)
        {
            var xDoc = XDocument.Parse(xml);
            IEnumerable<XElement>? xDescendants = xDoc.Document?.Descendants();
            foreach (var x in xDescendants)
            {
                foreach (var attr in x.Attributes().ToList())
                {
                    if (attr.IsNamespaceDeclaration)
                        continue;

                    attr.Remove();
                }
            }
            return xDoc;
        }
    }
}