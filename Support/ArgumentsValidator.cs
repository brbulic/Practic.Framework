using System;

namespace Practic.Framework.Support
{
    public class ArgumentsValidator
    {
        public static void NotNull(string argumentName, object argument)
        {
            if(argument == null)
                throw new ArgumentNullException(argumentName);
        }

        public static void Satisfies<T>(string argumentName, T argument, Predicate<T> condition)
        {
            if(!condition(argument))
                throw new ArgumentOutOfRangeException(argumentName,"Argument doesn't satisfy the specified condition");
        }
    }
}
