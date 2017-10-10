using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IoT_Newspaper.Models;
using System.Xml.Linq;
using System.Net.Http;
using IoT_Newspaper.Data;
using IoT_Newspaper.Extensions;
using IoT_Newspaper.Models.HomeViewModels;
using Microsoft.EntityFrameworkCore;

namespace IoT_Newspaper.Controllers
{
    public class HomeController : Controller
    {
        private readonly IViewRenderService _viewRenderService;
        private readonly ApplicationDbContext _context;

        /*public HomeController(IViewRenderService viewRenderService)
        {
            _viewRenderService = viewRenderService;
        }*/

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string _noImagePath = "http://iotnews.azurewebsites.net/img/noimage.jpg";
        public async Task<IActionResult> Index()
        {
            var model = new PortalViewModel();
            var feeds = new List<FeedModel>();

            feeds.AddRange(await GetFeed("http://internetofthingsagenda.techtarget.com/rss/IoT-Agenda.xml"));
            feeds.AddRange(await GetFeed("https://www.iotforall.com/feed/"));
            feeds.AddRange(await GetFeed("https://www.ibm.com/blogs/internet-of-things/feed/"));

            model.Feeds = feeds.OrderBy(o => o.PublishDate);
            var query = _context.Section.Where(s => s.Disabled == false);

            var sessions = query.ToList();

            for (var count = 0; count < sessions.Count(); count++)
            {
                var id = sessions.ElementAt(count).Id;

                var news = _context.News.Where(s => s.Disabled == false).OrderBy(o => Guid.NewGuid()).Take(8);

                sessions.ElementAt(count).News = news.ToList();

            }


            model.Sections = sessions;

            return View("Index", model);

        }

        [HttpGet]
        public async Task<IActionResult> GetNews(int order)
        {
            var news = _context.News.Include(n => n.Section.Order == order).Where(s => s.Disabled == false).OrderBy(o => Guid.NewGuid()).Take(8);

            return PartialView("_MainNews", news.ToList());
        }

        public async Task<IEnumerable<FeedModel>> GetFeed(string feedUrl)
        {
            IEnumerable<FeedModel> feedItems;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(feedUrl);
                var responseMessage = await client.GetAsync(feedUrl);
                var responseString = await responseMessage.Content.ReadAsStringAsync();

                //extract feed items
                XDocument doc = XDocument.Parse(responseString);
                    
                feedItems = from item in doc.Root.Descendants().First(i => i.Name.LocalName == "channel").Elements()
                        .Where(i => i.Name.LocalName == "item")
                    select new FeedModel
                    {
                        Content = Content(item.Elements().First(i => i.Name.LocalName == "description").Value),
                        Link = item.Elements().First(i => i.Name.LocalName == "link").Value,
                        Img = ImgUrl(item.Elements().First(i => i.Name.LocalName == "description").Value),
                        ValidImg = ImgUrl(item.Elements().First(i => i.Name.LocalName == "description").Value) != _noImagePath,
                        PublishDate = ParseDate(item.Elements().First(i => i.Name.LocalName == "pubDate").Value),
                        Title = item.Elements().First(i => i.Name.LocalName == "title").Value
                    };

            }

            return feedItems.ToList();
        }


        public IActionResult About()
        {
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

        private DateTime ParseDate(string date)
        {
            DateTime result;
            if (DateTime.TryParse(date, out result))
                return result;


            string[] formats = {"ddd, dd MMM yyyy HH:mm:ss K",
                "ddd, dd MMM yyyy HH:mm:ss EDT",
                "ddd, d MMM yyyy HH:mm:ss EDT",
                "ddd, dd MMM yyyy hh:mm:ss EDT",
                "ddd, d MMM yyyy hh:mm:ss EDT",
                "M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt",
                "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss",
                "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt",
                "M/d/yyyy h:mm", "M/d/yyyy h:mm",
                "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm"};

            if (DateTime.TryParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                return result;

            return DateTime.Now;
        }

        private string Content(string content)
        {
            if (content.IndexOf("<") >= 0)
            {
                content = content.Remove(0, content.LastIndexOf(">") + 2);
                return content;
            }
            return content;
        }

        private string ImgUrl(string url)
        {
            url += "";
            if (url.IndexOf("<") >= 0)
            {
                url = url.Remove(0, url.IndexOf("\"") + 1);
                url = url.Remove(url.LastIndexOf("\""));
                url += "";
                return url;
            }

            return _noImagePath;
        }
    }
}
