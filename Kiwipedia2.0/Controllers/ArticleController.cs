using Kiwipedia2._0.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kiwipedia.Controllers
{
    public class ArticleController : Controller
    {
        //conexiunea cu baza de date
        private ApplicationDbContext kdbc = new ApplicationDbContext();


        // GET: lista tuturor articolelor + filtru dupa categorii
        //[Authorize(Roles ="User,Visitor,Editor,Administrator")]
        public ActionResult Index(string cat) // category
        {
            //

            List<ArticleData> articlesData = GetArticles();

            var categories = from category in kdbc.Categories //kdbc vine de la KiwipediaBataBaseContext 
                             orderby category.categoryName //(variabila veche; am lasat asa sa nu mai fie nevoie de update peste tot)(variabila veche; am lasat asa sa nu mai fie nevoie de update peste tot)
                             select category;

            ViewBag.categories = categories;

            if (cat == "")
            {
                ViewBag.Title = "Toate articolele";
            }
            else
            {
                IQueryable<Category> categoryQuery = from c in kdbc.Categories
                                                     where c.categoryName == cat
                                                     select c;

                Category category = categoryQuery.First();

                foreach (ArticleData ad in articlesData.ToList())
                {
                    if (ad.article.categoryId != category.id)
                        articlesData.Remove(ad);
                }

                if (articlesData.Count() == 0)
                {
                    ViewBag.articles = new List<ArticleData>();
                    ViewBag.title = "Nu exista articole in categoria \"" + cat + "\"";
                    return View();
                }
                else
                    ViewBag.title = "Articole din categoria \"" + cat + "\"";
            }

            ViewBag.articles = articlesData;

            return View();
        }

        // GET: lista articolelor sortate dupa vechime sau ordine alfabetica
        //[Authorize(Roles = "User,Visitor,Editor,Administrator")]
        public ActionResult Sort(string type)
        {
            List<ArticleData> articlesData = GetArticles();

            var categories = from category in kdbc.Categories
                             orderby category.categoryName
                             select category;

            ViewBag.categories = categories;
            if (type == "Old")
            {
                ViewBag.title = "Toate articolele sortate dupa vechime";
                articlesData.Sort((a, b) => a.article.creationDate.CompareTo(b.article.creationDate));
            }
            else
            {
                ViewBag.title = "Toate articolele sortate dupa ordinea alfabetica";
                articlesData.Sort((a, b) => a.articleVersion.title.CompareTo(b.articleVersion.title));
            }

            ViewBag.articles = articlesData;

            return View("Index");
        }

        // GET: lista articolelor care au in denumire searchString-ul dat
        //[Authorize(Roles = "User,Visitor,Editor,Administrator")]
        public ActionResult Search(string search)
        {
            List<ArticleData> articlesData = GetArticles();
            var categories = from category in kdbc.Categories
                             orderby category.categoryName
                             select category;
            
            if (!String.IsNullOrEmpty(search))
            {
                foreach (ArticleData ad in articlesData.ToList())
                {
                    if (!ad.articleVersion.title.ToLower().Contains(search.ToLower()))
                        articlesData.Remove(ad);
                }
            }

            if (articlesData.Count == 0)
                ViewBag.title = "Nu a fost gasit niciun articol al carui titlu sa contina \"" + search + "\"";
            else
                ViewBag.title = "Articole ale caror titluri contin \"" + search + "\"";

            ViewBag.articles = articlesData;
            ViewBag.categories = categories;
            return View("Index");
        }

        // GET: vizualizarea unui articol
        //[Authorize(Roles = "User,Visitor,Editor,Administrator")]
        public ActionResult Show(Guid id)
        {
            ArticleVersion articleVersion = kdbc.ArticleVersions.Find(id);

            if (articleVersion != null)
                ViewBag.articleVersion = articleVersion;
            else
                return View("Error");

            Article article = kdbc.Articles.Find(articleVersion.articleId);
            ViewBag.article = article;

            return View();
        }

        // GET: afisam formularul de crearea a unui articol
        //[Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult New()
        {
            if (User.IsInRole("Visitor"))
            {
                //return View("NoAccess");
                ViewBag.Title = "Nu aveti dreptul sa creati articole noi!";
                return View("Error");
            }

            return View();
        }

        // POST: trimitem datele articolului catre server pentru creare 
        [HttpPost]
        //[Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult New(NewArticleData newArticleData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Article article = new Article();
                    article.id = Guid.NewGuid();
                    article.creatorId = new Guid(User.Identity.GetUserId());
                    article.creationDate = DateTime.Now;

                    IQueryable<Category> categories = from c in kdbc.Categories
                                                      select c;

                    Category cat = new Category();
                    bool found = false;
                    foreach (Category c in categories)
                    {
                        if (c.categoryName == newArticleData.category)
                        {
                            cat = c;
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        cat.id = Guid.NewGuid();
                        cat.categoryName = newArticleData.category;
                    }
                    article.category = cat;

                    ArticleVersion articleVersion = new ArticleVersion();
                    articleVersion.articleId = article.id;
                    articleVersion.versionId = Guid.NewGuid();
                    articleVersion.editorId = article.creatorId;
                    articleVersion.creationDate = article.creationDate;
                    articleVersion.title = newArticleData.title;

                    if (newArticleData.thumbnail == null)
                        articleVersion.thumbnail = "/Content/App_Resources/Images/Kiwipeda.jpg";
                    else
                        articleVersion.thumbnail = newArticleData.thumbnail;

                    articleVersion.description = newArticleData.description;
                    articleVersion.content = newArticleData.content;
                    article.crrtArticleVersion = articleVersion;

                    kdbc.Articles.Add(article);

                    kdbc.SaveChanges();

                    return View("NewPostMethod");
                }
                else
                {
                    ViewBag.Title = "Datele de intrare sunt invalide!";
                    return View("Error");
                }
            }

            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }

        // GET: vrem sa editam un articol
        //[Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult Edit(Guid id)
        {
            ArticleVersion articleVersion = kdbc.ArticleVersions.Find(id);
            if (articleVersion != null)
                return View(articleVersion);
            else
                return View("Error");
        }

        // PUT: vrem sa trimitem modificaile la server si sa le salvam
        [HttpPut]
        [Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult Edit(Guid id, string title, string thumbnail, string description, string content)
        {
            Article article = kdbc.Articles.Find(articleVersion.articleId);
            ArticleVersion newArticleVersion = new ArticleVersion();

            var ceva = articleVersion.versionId;
            newArticleVersion.versionId = Guid.NewGuid();
            newArticleVersion.articleId = article.id;
            newArticleVersion.editorId = new Guid(User.Identity.GetUserId());
            newArticleVersion.title = articleVersion.title;
            if (articleVersion.thumbnail == "")
                newArticleVersion.thumbnail = "/Content/App_Resources/Images/Kiwipeda.jpg";
            else
                newArticleVersion.thumbnail = articleVersion.thumbnail;
            newArticleVersion.description = articleVersion.description;
            newArticleVersion.content = articleVersion.content;
            newArticleVersion.creationDate = DateTime.Now;

            article.crrtArticleVersion = newArticleVersion;
            kdbc.ArticleVersions.Add(newArticleVersion);
            kdbc.SaveChanges();

            return View("EditPutMethod");
        }

        [HttpDelete]
        [Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult Delete(Guid id)
        {
            Article article = kdbc.Articles.Find(id);

            if (User.IsInRole("User") && new Guid(User.Identity.GetUserId()) != article.creatorId)
            {
                //return View("NoAccess");
                ViewBag.Title = "Nu aveti dreptul sa stergeti articole care nu va apartin!";
                return View("Error");
            }

            IQueryable<ArticleVersion> articleVersions = from av in kdbc.ArticleVersions
                                                         where av.articleId == id
                                                         select av;

            kdbc.Articles.Remove(article);
            foreach (ArticleVersion av in articleVersions)
                kdbc.ArticleVersions.Remove(av);
            kdbc.SaveChanges();

            return View("DeleteMethod");
        }

        [Authorize(Roles = "User,Editor,Administrator")]
        [HttpPut]
        public ActionResult Rollback(Guid articleId)
        {
            Article article = kdbc.Articles.Find(articleId);

            if (User.IsInRole("User") && new Guid(User.Identity.GetUserId()) != article.creatorId)
            {
                //return View("NoAccess");
                TempData["message"] = "Nu aveti dreptul sa derulati schimbarile la articole care nu va apartin!";
                return RedirectToAction("Index");
            }

            IQueryable<ArticleVersion> articleVersions = from av in kdbc.ArticleVersions
                                                         where av.articleId == articleId
                                                         orderby av.creationDate descending
                                                         select av;
            if(articleVersions.Count() < 2)
            {
                ViewBag.title = "Nu s-au putut derula schimbarile. Exista o singura versiune a acestui articol.";
                return View("RollbackMethod");
            }
            
            ArticleVersion oldArticleVersion = articleVersions.First();


            articleVersions = from av in kdbc.ArticleVersions
                              where av.articleId == articleId && av.versionId != oldArticleVersion.versionId
                              orderby av.creationDate descending
                              select av;

            ArticleVersion newArticleVersion = articleVersions.First();
            article.crrtArticleVersion = newArticleVersion;

            kdbc.ArticleVersions.Remove(oldArticleVersion);
            kdbc.SaveChanges();

            ViewBag.title = "Schimbarile au fost derulate cu succes.";
            return View("RollbackMethod");
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
                ad.articleVersion = kdbc.ArticleVersions.Find(article.currentVersionId);
                ad.category = kdbc.Categories.Find(article.categoryId);

                articleData.Add(ad);
            }
            return articleData;
        }
    }
}