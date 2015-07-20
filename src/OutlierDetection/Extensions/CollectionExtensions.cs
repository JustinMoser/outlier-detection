using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OutlierDetection.Extensions
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> ZipN<T>(this IEnumerable<IEnumerable<T>> s)
        {
            int max = s.Any() ? s.Max(l => l.Count()) : 0;
            List<List<T>> result = new List<List<T>>(max);

            for (int i = 0; i < max; i++)
                result.Add(new List<T>(s.Count()));

            for (int j = 0; j < s.Count(); ++j)
                for (int k = 0; k < max; ++k)
                    result[k].Add(s.ElementAt(j).Count() > k ? s.ElementAt(j).ElementAt(k) : default(T));

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static IEnumerable<U> ZipWith<T, U>(this IEnumerable<T> s1, IEnumerable<T> s2, Func<T, T, U> f)
        {
            if (s1.Count() != s2.Count()) throw new ArgumentException("Cannot zip collections of different lengths. Both source collections must be the same length.");
            if (s1.GetType() != s2.GetType()) throw new ArgumentException("Cannot zip collections of different type. Both source collections must be of, or inherit from the same type.");


            using (IEnumerator<T> e1 = s1.GetEnumerator())
            using (IEnumerator<T> e2 = s2.GetEnumerator())
                while (e1.MoveNext() && e2.MoveNext())
                    yield return f(e1.Current, e2.Current);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="s"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static IEnumerable<U> Map<T, U>(this IEnumerable<T> s, Func<T, U> f)
        {
            using (IEnumerator<T> e = s.GetEnumerator())
                while (e.MoveNext())
                    yield return f(e.Current);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> s, Func<T, bool> f)
        {
            using (IEnumerator<T> e = s.GetEnumerator())
                while (e.MoveNext())
                    if (f(e.Current)) yield return e.Current;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <param name="s"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static A FoldL<A>(this IEnumerable<A> s, Func<A, A, A> f)
        {
            A tmpVal = default(A);
            bool firstValSet = false;

            if (!s.Any()) return tmpVal;

            using (IEnumerator<A> e = s.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    if (!firstValSet)
                    {
                        tmpVal = f(e.Current, tmpVal);
                        firstValSet = true;
                    }
                    else
                    {
                        tmpVal = f(e.Current, tmpVal);
                    }
                }
            }

            return tmpVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static T FoldR<T>(this IEnumerable<T> s, Func<T, T, T> f)
        {
            Stack<T> stack = new Stack<T>();
            T firstVal = default(T);
            bool containsVal = false;
            bool firstValSet = false;
            T tmpVal = default(T);

            for (int i = 0; i < s.Count(); ++i)
            {
                stack.Push(s.ElementAt(i));
            }

            while (stack.Count > 0)
            {
                firstVal = stack.Pop();
                containsVal = true;

                if (firstValSet) tmpVal = f(tmpVal, firstVal);
                else
                {
                    firstValSet = true;
                    tmpVal = firstVal;
                }
            }

            return containsVal ? tmpVal : default(T);
        }

    }
}
