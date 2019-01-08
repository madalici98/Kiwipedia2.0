using Kiwipedia2._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kiwipedia2._0.Controllers
{
    public class HomeController : Controller
    {
        private KiwipediaDbContext kdbc = new KiwipediaDbContext();

        public ActionResult Index()
        {

            List<ArticleData> articlesData = GetArticles();
            articlesData.Sort((a1, a2) => a1.articleVersion.creationDate.CompareTo(a2.articleVersion.creationDate));
            ViewBag.articles = articlesData.Take(6);

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [NonAction]
        private List<ArticleData> GetArticles()
        {
            var categories = from category in kdbc.Categories
                             select category;

            var articles = from article in kdbc.Articles
                           select article;

            // query-uri mai complexe cu lambda expresii; long live stack overflow <3
            var articleVersions = kdbc.ArticleVersions.GroupBy(av => av.articleId).Select(avs => avs.OrderByDescending(av => av.creationDate).FirstOrDefault());

            List<ArticleData> articleData = new List<ArticleData>();

            foreach (Article article in articles)
            {
                ArticleData ad = new ArticleData();
                ad.article = article;

                foreach (ArticleVersion av in articleVersions)
                {
                    if (av.articleId == article.id)
                    {
                        ad.articleVersion = av;
                        break;
                    }
                }

                foreach (Category c in categories)
                {
                    if (c.id == article.categoryId)
                    {
                        ad.category = c;
                        break;
                    }
                }

                articleData.Add(ad);
            }
            return articleData;
        }
    }
}