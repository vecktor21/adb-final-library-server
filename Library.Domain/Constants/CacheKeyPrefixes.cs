using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Constants
{
    public static class CacheKeyPrefixes
    {
        private static string bser = "user_{0}";
        private static string book = "book_{0}";
        private static string cart = "cart_{0}";
        private static string history = "history_{0}";

        public static string BookKey(string id)
        {
            return String.Format(book, id);
        }
        public static string UserKey(string id)
        {
            return String.Format(bser, id);
        }
        public static string CartKey(string id)
        {
            return String.Format(cart, id);
        }
        public static string HistoryKey(string id)
        {
            return String.Format(history, id);
        }
    }
}
