using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FirstAppNetCore.Models;
using System.Xml.Linq;
using System.Net.Http;
using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using Microsoft.Azure.Documents.Linq;

namespace FirstAppNetCore.Controllers
{
    public class HomeController : Controller
    {

        public async Task<IActionResult> Index()
        {
            Program Connection = new Program();
            string EndpointUrl = Connection.EndpointUrl;
            string PrimaryKey = Connection.PrimaryKey;
            DocumentClient client;

            using (client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey))
            {
                IQueryable<NewsModel> queryable =
                client.CreateDocumentQuery<NewsModel>(UriFactory.CreateDocumentCollectionUri("Teste", "CTeste"));
                List<NewsModel> posts = queryable.ToList();
                posts.RemoveRange(0, 180);

                posts.RemoveAll(post => post.Status == false);

                return View("Index", posts.OrderByDescending(o => o.PublishDate));
            }
        }

        /*
        public async Task<IActionResult> Index()
        {
            Program Connection = new Program();
            string EndpointUrl = Connection.EndpointUrl;
            string PrimaryKey = Connection.PrimaryKey;
            DocumentClient client;

            var articles = new List<FeedModel>();

            articles.AddRange(await GetFeed("https://blogs.microsoft.com/iot/feed/"));
            articles.AddRange(await GetFeed("https://staceyoniot.com/feed/"));

            //Bloco para gravar no banco
            /*
            using (client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey))
            {/*
                try
                {
                    foreach (var item in articles)
                    {
                        await client.CreateDocumentAsync(
                        UriFactory.CreateDocumentCollectionUri("Teste", "CTeste"),
                        new FeedModel
                        {
                            Title = item.Title,
                            Content = item.Content,
                            Link = item.Link,
                            Img = item.Img,
                            PublishDate = item.PublishDate,
                        }
                        );
                    }

                    //Bloco para ler do banco (Aproveitando espaço e using)
                    IQueryable<FeedModel> queryable =
                        client.CreateDocumentQuery<FeedModel>(UriFactory.CreateDocumentCollectionUri("Teste", "CTeste"));
                    List<FeedModel> posts = queryable.ToList();
                    posts.RemoveRange(20, posts.Count - 20);

                    return View("Index", posts.OrderBy(o => o.PublishDate));
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: {0}", e.Message);
                }
            }


            return View("Index", articles.OrderByDescending(o => o.PublishDate));



            //Bloco para ler do banco (bloco seprado)
            /*
            using (client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey))
            {
                IQueryable<FeedModel> queryable =
                client.CreateDocumentQuery<FeedModel>(UriFactory.CreateDocumentCollectionUri("Teste", "CTeste"));
                List<FeedModel> posts = queryable.ToList();
                posts.RemoveRange(20, posts.Count - 20);

                return View("Index", posts.OrderBy(o => o.PublishDate));
            }


        }
    */

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

    }

}
