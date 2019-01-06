using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiwipedia2._0.Models
{
    public class Article
    {
        public string id { get; set; }
        public ArticleVersion currentVersionId { get; set; }  //deocamdata fac legatura directa iar terminam baza de date o sa fac cu id
        public DateTime creationDate { get; set; }
        public string creatorId { get; set; }
    }

}