using Moq.Language;
using Moq.Language.Flow;
using System.Reflection;

namespace StroopTest.Tests.Utils
{
    public static class MoqExtension
    {
        public delegate void OutAction<in TIn, TOut>(TIn argIn, out TOut outVal);

        public static IReturnsThrows<TMock, TReturn> OutCallback<TMock, TReturn, TIn, TOut>(this ICallback<TMock, TReturn> mock, OutAction<TIn, TOut> action)
            where TMock : class
        {            
            mock.GetType()
                .Assembly.GetType("Moq.MethodCall")
                .InvokeMember("SetCallbackWithArguments",
                              BindingFlags.InvokeMethod
                                  | BindingFlags.NonPublic
                                  | BindingFlags.Instance,
                              null, mock, new object[] { action });
            return mock as IReturnsThrows<TMock, TReturn>;
        }
    }
}
