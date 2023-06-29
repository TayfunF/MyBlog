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
                return $"{articleTitle} başlıklı makale çöp kutusuna taşındı.";
            }

            public static string UndoDeleteMessage(string articleTitle)
            {
                return $"{articleTitle} başlıklı makale geri alındı.";
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
                return $"{categoryName} başlıklı kategori silindi.";
            }

            public static string UndoDeleteMessage(string categoryName)
            {
                return $"{categoryName} başlıklı kategori geri alındı.";
            }
        }

        public static class UserMessage
        {
            public static string AddMessage(string userName)
            {
                return $"{userName} e-posta adresli kullanıcı eklendi";
            }

            public static string UpdateMessage(string userName)
            {
                return $"{userName} e-posta adresli kullanıcı güncellendi";
            }

            public static string DeleteMessage(string userName)
            {
                return $"{userName} e-posta adresli kullanıcı silindi.";
            }
        }
    }
}
