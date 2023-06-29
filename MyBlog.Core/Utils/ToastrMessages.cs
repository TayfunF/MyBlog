using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Core.Utils
{
    public static class ToastrMessages
    {
        public static class ArticleMessage
        {
            public static string AddMessage(string articleTitle)
            {
                return $"{articleTitle} başlıklı makale eklendi";
            }

            public static string UpdateMessage(string articleTitle)
            {
                return $"{articleTitle} başlıklı makale güncellendi";
            }

            public static string DeleteMessage(string articleTitle)
            {
                return $"{articleTitle} başlıklı makale silinmiştir.";
            }
        }

        public static class CategoryMessage
        {
            public static string AddMessage(string categoryName)
            {
                return $"{categoryName} başlıklı kategori eklendi";
            }

            public static string UpdateMessage(string categoryName)
            {
                return $"{categoryName} başlıklı kategori güncellendi";
            }

            public static string DeleteMessage(string categoryName)
            {
                return $"{categoryName} başlıklı kategori silinmiştir.";
            }
        }
    }
}
