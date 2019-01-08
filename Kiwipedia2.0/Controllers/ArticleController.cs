using Kiwipedia2._0.Models;
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
            List<ArticleData> articlesData = GetArticles();

            var categories = from category in kdbc.Categories
                             orderby category.categoryName
                             select category;

            ViewBag.categories = categories;

            if (cat == "")
            {
                ViewBag.Title = "Toate articolele";
            }
            else
            {
                var categoryQuery = from c in kdbc.Categories
                                    where c.categoryName == cat
                                    select c;

                Category category = new Category();

                foreach(Category c in categoryQuery)
                    category = c;

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
            return View();
        }

        // GET: afisam formularul de crearea a unui articol
        //[Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult New()
        {
            return View();
        }

        // POST: trimitem datele articolului catre server pentru creare 
        [HttpPost]
        //[Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult New(string title, string category, string thumbnail, string description, string content)
        {
            try
            {
                Article article = new Article();
                article.id = Guid.NewGuid();
                article.creatorId = Guid.NewGuid();
                article.creationDate = DateTime.Now;

                IQueryable<Category> categories = from c in kdbc.Categories
                                                  select c;

                Category cat = new Category();
                bool found = false;
                foreach (Category c in categories)
                {
                    if (c.categoryName == category)
                    {
                        cat = c;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    cat.id = Guid.NewGuid();
                    cat.categoryName = category;
                }
                article.category = cat;

                ArticleVersion articleVersion = new ArticleVersion();
                articleVersion.articleId = article.id;
                articleVersion.versionId = Guid.NewGuid();
                articleVersion.editorId = article.creatorId;
                articleVersion.creationDate = article.creationDate;
                articleVersion.title = title;

                if (thumbnail == "")
                    articleVersion.thumbnail = "/Content/App_Resources/Images/Kiwipeda.jpg";
                else
                    articleVersion.thumbnail = thumbnail;

                articleVersion.description = description;
                articleVersion.content = content;
                article.crrtArticleVersion = articleVersion;

                kdbc.Articles.Add(article);
                //kdbc.ArticleVersions.Add(articleVersion);
                //kdbc.Categories.Add(cat);

                kdbc.SaveChanges();
            }
            //dbArticleVersion.SaveChanges();
            //dbCategories.SaveChanges();
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

            return View("NewPostMethod");
        }

        // GET: vrem sa editam un articol
        //[Authorize(Roles = "Editor,Administrator")]
        public ActionResult Edit(Guid id)
        {
            ArticleVersion articleVersion = kdbc.ArticleVersions.Find(id);
            if (articleVersion != null)
                ViewBag.articleVersion = articleVersion;
            else
                return View("Error");

            return View();
        }

        // PUT: vrem sa trimitem modificaile la server si sa le salvam
        [HttpPut]
        public ActionResult Edit(Guid id, string title, string thumbnail, string description, string content)
        {
            Article article = kdbc.Articles.Find(id);
            ArticleVersion newArticleVersion = new ArticleVersion();

            newArticleVersion.versionId = Guid.NewGuid();
            newArticleVersion.articleId = id;
            newArticleVersion.editorId = article.creatorId;
            newArticleVersion.title = title;
            if (thumbnail == "")
                newArticleVersion.thumbnail = "/Content/App_Resources/Images/Kiwipeda.jpg";
            else
                newArticleVersion.thumbnail = thumbnail;
            newArticleVersion.description = description;
            newArticleVersion.content = content;
            newArticleVersion.creationDate = DateTime.Now;

            article.crrtArticleVersion = newArticleVersion;
            kdbc.ArticleVersions.Add(newArticleVersion);
            kdbc.SaveChanges();

            return View("EditPutMethod");
        }

        [HttpDelete]
        public ActionResult Delete(Guid id)
        {
            Article article = kdbc.Articles.Find(id);

            IQueryable<ArticleVersion> articleVersions = from av in kdbc.ArticleVersions
                                                         where av.articleId == id
                                                         select av;

            kdbc.Articles.Remove(article);
            foreach (ArticleVersion av in articleVersions)
                kdbc.ArticleVersions.Remove(av);
            kdbc.SaveChanges();

            return View("DeleteMethod");
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

        /*
        [NonAction]
        private Article[] GetArticles()             //il folosesc in loc de baza de date
        {
            Article[] articles = new Article[4];

            for (int i = 0; i < 4; i++)
            {
                Article article = new Article();

                article.id = new Guid(i.ToString());
                article.creationDate = DateTime.Now.AddMinutes(i);
                article.creatorId = new Guid(i.ToString());

                ArticleVersion version = new ArticleVersion();
                version.articleId = new Guid(i.ToString());
                version.versionId = new Guid("1");
                version.thumbnail = "/Content/App_Resources/Images/Kiwipeda.jpg";
                version.creationDate = DateTime.Now.AddMinutes(i);
                version.editorId = new Guid(i.ToString());

                article.currentVersionId = version.versionId;
                articles[i] = article;
            }

            List<Category> categories = new List<Category>();
            categories.Add(new Category());
            categories.Add(new Category());
            categories.Add(new Category());

            categories[0].id = new Guid("1");
            categories[1].id = new Guid("2");
            categories[2].id = new Guid("3");

            categories[0].categoryName = "Pasari";
            categories[0].categoryName = "Fructe";
            categories[0].categoryName = "Plante";


            articles[0].categoryId = categories[0].id;
            articles[0].currentVersionId.title = "Pasarea Kiwi";
            articles[0].currentVersionId.description = "Despre pasarea Kiwi si minunatiile pe care le poate face (poate sa transforme apa in suc de fruct kiwi!!).";
            articles[0].currentVersionId.content = "Kiwi (Apteryx australis) este o pasăre acarinată din Noua Zeelandă, înaltă de 30 cm, cu aripile atrofiate, lipsită de claviculă, care se hrănește cu larve de insecte și duce o viață nocturnă.[3] Unul din punctele de atracție a orașului neozeelandez Rotorua este o casă kiwi, unde vizitatorii au ocazia să admire pasărea națională și simbolul Noii Zeelande. Fiind păsări de noapte, păsările kiwi nu pot fi întâlnite la fiecare pas. O pasăre kiwi nu este capabilă să zboare, purtând doar o pereche de aripi rudimentare și nu are coadă. Este singura pasăre care se ghidează în vânătorile sale după miros.La capătul ciocului prelung este prevăzută cu nări, pe care și le lipește de pământul umed pentru a adulmeca râmele, larvele, insectele sau rădăcinile. De mărimea puilor de găină, păsările kiwi sunt aproape oarbe.Ele trăiesc în vizuini săpate în pământul umed își depun ouăle de dimensiuni uriașe câte unul sau două pe an, cântărind până la 450 grame, ceea ce reprezintă aproximativ 25 % din greutatea femelei. Raportate la proporțiile păsării, ouăle sunt cele mai mari ouă ale vreuneia dintre specii. Schelet de pasăre moa Dinornis giganteus(robustus). Bărbătușul incubează ouăle, în timp ce adoarme în vizuină, pentru ca după aproape 3 luni puișorii să spargă coaja și să iasă acoperiți cu pene.Localnicii maori considerau toate speciile de păsări sacre, ca odraslele zeului pădurilor, păsările kiwi fiind sacre. Numai căpeteniilor le era permis să mănânce carnea de kiwi, iar pieile cu pene erau purtate de mai marii tribului ca pelerine. Kiwi este înrudită cu mult mai marea pasăre moa, de asemenea incapabilă să zboare, și dispărută în secolul al XVIII-lea.O anumită specie de moa măsura 3,7 metri și era cea mai mare pasăre văzută vreodată. În studiul „Ancient DNA reveals elephant birds and kiwi are sister taxa and clarifies ratite bird evolution”, publicat în revista Science din 23 mai 2014,[4] cercetătorii Universității Adelaide din Australia susțin că pasărea Kiwi și pasărea elefant din Madagascar au evoluat dintr-un strămoș comun în urmă cu aproximativ 50 de milioane de ani. ";

            articles[1].categoryId = categories[1].id;
            articles[1].currentVersionId.title = "Fructul Kiwi";
            articles[1].currentVersionId.description = "Despre fructul kiwi si minunatiile pe care le poate face (puteti citi cum se poate transforma in pasarea Kiwi!)";
            articles[1].currentVersionId.content = "Kiwi este fructul comestibil al plantelor Actinidia chinensis și Actinidia deliciosa din Asia. Denumirea chinezească este Yang Tao (mura sau strugurele gâștei). Cunoscut original sub denumirea de „agrișe chinezești”. Istoria \"europeană\" a fructului a început în Valea Ghang Kiang, din Sud - Estul Chinei, de unde în anul 1847 un membru al Societății Regale de Horticultură din Marea Britanie a trimis primele semințe în afara granițelor țării de origine. În 1906 a fost plantată și în Noua Zeelandă unde a rodit prima dată în 1910.Aici fructul a fost \"rebotezat\" cu numele de astăzi, după numele păsării naționale, probabil datorită asemănării ca formă și culoare.Astăzi kiwi se cultivă în Noua Zeelandă, SUA, Italia, Japonia, Franța, Grecia, Spania, Australia și Chile. În pădurile din care este originară, planta care face kiwi(Actinidia chinesis) poate fi descrisă ca un arbust cu vițe lungi, viguroase și lemnoase, asemănătoare lianelor, cu aspect de arbust cățărător. Adesea un astfel de arbust acoperă o zonă de 3 - 4,5 m lățime, 5–7 m lungime și 2,7 - 3,6 m înălțime. De aceea, atunci când este cultivată, planta are nevoie absolută de un spalier.În câmp, planta se cultivă pe araci înalți de 1,7–2 m, legați între ei cu sârmă. Fructul are o formă ovaloidă, de mărimea unui măr mijlociu(7–10 cm lungime și o greutate de circa 80 - 100 g), cu miezul verde, cărnos, acrișor.Coaja verde-maronie pufoasă(acoperită cu perișori aspri), este comestibilă. Pulpa fructului este cărnoasă, de culoare verde, cu vreo sută de semințe mici, negre, comestibile și viabile, dispuse circular către interiorul fructului.Kiwi este sursă importantă de vitamina C, dar și de vitamina A și vitamina E, de calciu, fier și de acid folic. Planta de kiwi este foarte sensibilă la ger și la vânt, preferând solurile umede, umbroase și puțin calcaroase. După recoltare, depozitarea acestor fructe se face în încăperi curate, uscate și fără infestie, în lăzi stivuite și așezate pe grătare, la o temperatură de 12 °C și o umiditate relativă a aerului de 85 - 90 %.În aceste condiții ele pot fi păstrate o perioadă de circa 2 - 5 săptămâni. Printr - un Grant de Cercetare, finanțat de Academia Română, Facultatea de Horticultură din Universitatea de Știinte Agronomice și Medicină Veterinară București, având ca director de grant pe Stănică Florin, a întreprins o cercetare cu titlul Selecția unor elite hibride de kiwi(Actinidia arguta) și înmulțirea rapidă a acestora în vederea omologării și introducerii în cultură în România. ";

            articles[2].categoryId = categories[2].id;
            articles[2].currentVersionId.title = "Bananierul";
            articles[2].currentVersionId.description = "Despre copacul care da nastere unui fruct folosit universal ca unitate de masura!";
            articles[2].currentVersionId.content = "Bananierul este o plantă ierboasă din genul Musa, care, din cauza mărimii și structurii, este deseori confundat cu un arbore. Este cultivat pentru fructele sale. Bananierul face parte din Familia Musaceae. În lume, bananele ocupă locul patru după orez, grâu și porumb în consumul uman; sunt crescute în 130 de țări din lume, mai multe decât pentru orice alt fruct de cultură. Bananele sunt originare din Asia de sud-est. Marea parte a producătorilor este formată din fermieri mici, care folosesc aceste culturi pentru consumul casnic sau pentru comercializarea locală.Deoarece bananele sunt produse în continuu de - a lungul unui an, acestea reprezintă o sursă de hrană importantă în timpul sezonului foametei(acea perioadă a anului când alimentele provenite din recoltarea anteioară s - a terminat, iar cea din anul curent nu este gata de cules).Din aceste motive bananele sunt extrem de importante pentru siguranța alimentației. În anul 2014, recolta de banane a lumii era estimată la valoarea de 31,4 miliarde de euro, fiind indispensabilă pentru hrana a mai bine de 400 de milioane de persoane.[1] ";

            articles[3].categoryId = categories[0].id;
            articles[3].currentVersionId.title = "Bufnita";
            articles[3].currentVersionId.description = "Despre pasarea feroce si inspaimantatoare dar in acelasi timp dragalasa si iubitoare. Descoperiti de ce se fac atat de multe ornamente cu imaginea lor!";
            articles[3].currentVersionId.content = "Bufnița este o denumire comună pentru păsările răpitoare de noapte din ordinul strigiforme (Strigiformes), care cuprinde buhele, cucuvelele, huhurezii, strigile și alte păsări răpitoare de noapte. Au un zbor silențios și un colorit protector, de obicei brun, cea ce le ajută la prinderea insectelor, a păsărilor și a mamiferelor mici. În repaus corpul lor are o poziție verticală. Bufnițele au ochi globuloși așezați frontal, cioc puternic, încovoiat, văz și auz foarte bine dezvoltate. Degetele sunt scurte, prevăzute cu gheare lungi, ascuțite și încovoiate. Au o lungime de 13-70 cm, în funcție de specie. Penele formează discuri faciale sau smocuri în jurul urechilor, fapt care le ajută să localizeze prada cu mare precizie prin reflectarea sunetului spre urechi. Ochii nu sunt mobili, ci fixați în orbite, în schimb își pot roti capul cu 180° (unele specii chiar cu 270°). Se hrănesc cu rozătoare sau insecte, pe care le vânează noaptea sau în crepuscul și pe care le înghit de obicei întregi. Cele mai mari atacă păsări și iepuri. Resturile de la hrană sunt eliminate sub formă de cocoloașe, ingluviile. Cuibăresc în scorburi, cuiburi vechi ale altor păsări, vizuini, găuri în maluri, fisuri în stânci, iar unele în mediul antropic, în preajma hambarelor, silozurilor, în cimitire etc. Ponta constă din 2-10 ouă albe și rotunde. Puii sunt nidicoli, acoperiți de regulă la ieșirea din ou cu un puf des. Sunt păsări folositoare, deoarece consumă rozătoare și insecte dăunătoare. Bufnițele sunt răspândite pe întreaga suprafață a pământului, mai puțin în Antarctica. În România au fost semnalate 12 specii de bufnițe, iar în Republica Moldova 8 specii. În dicționarele generale ale limbii române termenul de \"bufniță\" se referă numai la o singură specie - buhă (Bubo bubo), însă în literatura ornitologică românească termenul de \"bufniță\" este sinonim cu strigidă, adică este o denumire comună pentru toate speciile din ordinul strigiforme (Strigiformes).";
            return articles;
        }*/
    }
}