using Kiwipedia2._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kiwipedia.Controllers
{
    public class ArticleController : Controller
    {
        // GET: lista tuturor articolelor
        public ActionResult Index()
        {
            Article[] articles = GetArticles();

            ViewBag.articles = articles;

            return View();
        }

        // GET: vizualizarea unui articol
        public ActionResult Show(int id)
        {
            Article[] articles = GetArticles();

            try
            {
                ViewBag.article = articles[id];
                return View();
            }
            catch (Exception e)
            {
                ViewBag.errorMessage = e.Message;
                return View("Error");
            }
        }

        // GET: afisam formularul de crearea a unui articol
        public ActionResult New()
        {
            return View();
        }

        // POST: trimitem datele articolului catre server pentru creare 
        [HttpPost]
        public ActionResult New(Article article)
        {
            // ... cod creare articol ...
            return View("NewPostMethod");
        }

        // GET: vrem sa editam un articol
        public ActionResult Edit(int id)
        {
            Article[] articles = GetArticles();

            try
            {
                ViewBag.article = articles[id];
                return View();
            }
            catch (Exception e)
            {
                ViewBag.errorMessage = e.Message;
                return View("Error");
            }
        }

        // PUT: vrem sa trimitem modificaile la server si sa le salvam
        [HttpPut]
        public ActionResult Edit(Article id)
        {
            // ... cod udpate articol ...
            return View("EditPutMethod");
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            // ... cod stergere articol ...
            return View("DeleteMethod");
        }


        [NonAction]
        private Article[] GetArticles()             //il folosesc in loc de baza de date
        {
            Article[] articles = new Article[3];

            for (int i = 0; i < 3; i++)
            {
                Article article = new Article();

                article.id = i.ToString();
                article.creationDate = DateTime.Now.AddMinutes(i);
                article.creatorId = i.ToString();

                ArticleVersion version = new ArticleVersion();
                version.articleId = i.ToString();
                version.versionId = "1";
                version.thumbnail = "/Content/App_Resources/Images/Kiwipeda.jpg";
                version.creationDate = DateTime.Now.AddMinutes(i);
                version.editorId = i.ToString();

                article.currentVersionId = version;
                articles[i] = article;
            }

            articles[0].currentVersionId.title = "Pasarea kiwi";
            articles[0].currentVersionId.description = "Despre pasarea Kiwi si minunatiile pe care le poate face (poate sa transforme apa in suc de fruct kiwi!!).";
            articles[0].currentVersionId.content = "Kiwi (Apteryx australis) este o pasăre acarinată din Noua Zeelandă, înaltă de 30 cm, cu aripile atrofiate, lipsită de claviculă, care se hrănește cu larve de insecte și duce o viață nocturnă.[3] Unul din punctele de atracție a orașului neozeelandez Rotorua este o casă kiwi, unde vizitatorii au ocazia să admire pasărea națională și simbolul Noii Zeelande. Fiind păsări de noapte, păsările kiwi nu pot fi întâlnite la fiecare pas. O pasăre kiwi nu este capabilă să zboare, purtând doar o pereche de aripi rudimentare și nu are coadă. Este singura pasăre care se ghidează în vânătorile sale după miros.La capătul ciocului prelung este prevăzută cu nări, pe care și le lipește de pământul umed pentru a adulmeca râmele, larvele, insectele sau rădăcinile. De mărimea puilor de găină, păsările kiwi sunt aproape oarbe.Ele trăiesc în vizuini săpate în pământul umed își depun ouăle de dimensiuni uriașe câte unul sau două pe an, cântărind până la 450 grame, ceea ce reprezintă aproximativ 25 % din greutatea femelei. Raportate la proporțiile păsării, ouăle sunt cele mai mari ouă ale vreuneia dintre specii. Schelet de pasăre moa Dinornis giganteus(robustus). Bărbătușul incubează ouăle, în timp ce adoarme în vizuină, pentru ca după aproape 3 luni puișorii să spargă coaja și să iasă acoperiți cu pene.Localnicii maori considerau toate speciile de păsări sacre, ca odraslele zeului pădurilor, păsările kiwi fiind sacre. Numai căpeteniilor le era permis să mănânce carnea de kiwi, iar pieile cu pene erau purtate de mai marii tribului ca pelerine. Kiwi este înrudită cu mult mai marea pasăre moa, de asemenea incapabilă să zboare, și dispărută în secolul al XVIII-lea.O anumită specie de moa măsura 3,7 metri și era cea mai mare pasăre văzută vreodată. În studiul „Ancient DNA reveals elephant birds and kiwi are sister taxa and clarifies ratite bird evolution”, publicat în revista Science din 23 mai 2014,[4] cercetătorii Universității Adelaide din Australia susțin că pasărea Kiwi și pasărea elefant din Madagascar au evoluat dintr-un strămoș comun în urmă cu aproximativ 50 de milioane de ani. ";

            articles[1].currentVersionId.title = "Fructul kiwi";
            articles[1].currentVersionId.description = "Despre fructul kiwi si minunatiile pe care le poate face (puteti citi cum se poate transforma in pasarea Kiwi!)";
            articles[1].currentVersionId.content = "Kiwi este fructul comestibil al plantelor Actinidia chinensis și Actinidia deliciosa din Asia. Denumirea chinezească este Yang Tao (mura sau strugurele gâștei). Cunoscut original sub denumirea de „agrișe chinezești”. Istoria \"europeană\" a fructului a început în Valea Ghang Kiang, din Sud - Estul Chinei, de unde în anul 1847 un membru al Societății Regale de Horticultură din Marea Britanie a trimis primele semințe în afara granițelor țării de origine. În 1906 a fost plantată și în Noua Zeelandă unde a rodit prima dată în 1910.Aici fructul a fost \"rebotezat\" cu numele de astăzi, după numele păsării naționale, probabil datorită asemănării ca formă și culoare.Astăzi kiwi se cultivă în Noua Zeelandă, SUA, Italia, Japonia, Franța, Grecia, Spania, Australia și Chile. În pădurile din care este originară, planta care face kiwi(Actinidia chinesis) poate fi descrisă ca un arbust cu vițe lungi, viguroase și lemnoase, asemănătoare lianelor, cu aspect de arbust cățărător. Adesea un astfel de arbust acoperă o zonă de 3 - 4,5 m lățime, 5–7 m lungime și 2,7 - 3,6 m înălțime. De aceea, atunci când este cultivată, planta are nevoie absolută de un spalier.În câmp, planta se cultivă pe araci înalți de 1,7–2 m, legați între ei cu sârmă. Fructul are o formă ovaloidă, de mărimea unui măr mijlociu(7–10 cm lungime și o greutate de circa 80 - 100 g), cu miezul verde, cărnos, acrișor.Coaja verde-maronie pufoasă(acoperită cu perișori aspri), este comestibilă. Pulpa fructului este cărnoasă, de culoare verde, cu vreo sută de semințe mici, negre, comestibile și viabile, dispuse circular către interiorul fructului.Kiwi este sursă importantă de vitamina C, dar și de vitamina A și vitamina E, de calciu, fier și de acid folic. Planta de kiwi este foarte sensibilă la ger și la vânt, preferând solurile umede, umbroase și puțin calcaroase. După recoltare, depozitarea acestor fructe se face în încăperi curate, uscate și fără infestie, în lăzi stivuite și așezate pe grătare, la o temperatură de 12 °C și o umiditate relativă a aerului de 85 - 90 %.În aceste condiții ele pot fi păstrate o perioadă de circa 2 - 5 săptămâni. Printr - un Grant de Cercetare, finanțat de Academia Română, Facultatea de Horticultură din Universitatea de Știinte Agronomice și Medicină Veterinară București, având ca director de grant pe Stănică Florin, a întreprins o cercetare cu titlul Selecția unor elite hibride de kiwi(Actinidia arguta) și înmulțirea rapidă a acestora în vederea omologării și introducerii în cultură în România. ";

            articles[2].currentVersionId.title = "Bananierul";
            articles[2].currentVersionId.description = "Despre copacul care da nastere unui fruct folosit universal ca unitate de masura!";
            articles[2].currentVersionId.content = "Bananierul este o plantă ierboasă din genul Musa, care, din cauza mărimii și structurii, este deseori confundat cu un arbore. Este cultivat pentru fructele sale. Bananierul face parte din Familia Musaceae. În lume, bananele ocupă locul patru după orez, grâu și porumb în consumul uman; sunt crescute în 130 de țări din lume, mai multe decât pentru orice alt fruct de cultură. Bananele sunt originare din Asia de sud-est. Marea parte a producătorilor este formată din fermieri mici, care folosesc aceste culturi pentru consumul casnic sau pentru comercializarea locală.Deoarece bananele sunt produse în continuu de - a lungul unui an, acestea reprezintă o sursă de hrană importantă în timpul sezonului foametei(acea perioadă a anului când alimentele provenite din recoltarea anteioară s - a terminat, iar cea din anul curent nu este gata de cules).Din aceste motive bananele sunt extrem de importante pentru siguranța alimentației. În anul 2014, recolta de banane a lumii era estimată la valoarea de 31,4 miliarde de euro, fiind indispensabilă pentru hrana a mai bine de 400 de milioane de persoane.[1] ";
                
            return articles;
        }
    }
}