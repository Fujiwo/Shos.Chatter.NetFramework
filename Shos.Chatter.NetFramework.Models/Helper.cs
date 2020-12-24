//using System.Collections.Generic;
//using System.Linq;

//namespace Shos.Chatter.NetFramework.Models
//{
//    public static class HelperExtensions
//    {
//        public static bool AreSame<TElement>(this IEnumerable<TElement> @this, IEnumerable<TElement> another)
//        {
//            if (@this == null)
//                return another == null;
//            if (another == null)
//                return false;
//            return @this.ToList().AreSameList(another.ToList());
//        }

//        static bool AreSameList<TElement>(this IList<TElement> @this, IList<TElement> another)
//        {
//            if (@this.Count != another.Count)
//                return false;

//            for (var index = 0; index < @this.Count; index++) {
//                if (!@this[index].Equals(another[index]))
//                    return false;
//            }
//            return true;
//        }
//    }
//}
