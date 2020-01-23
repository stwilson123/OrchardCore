using System;
using System.Collections.Generic;
using System.Diagnostics;
using BlocksCore.SyntacticAbstractions.Types.Collections;

namespace BlocksCore.SyntacticAbstractions.Types
{
     [DebuggerStepThrough]
    public static class Check
    {
        public static T NotNull<T>(T value,   string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        public static string NotNullOrEmpty(string value,  string parameterName)
        {
            if (value.IsNullOrEmpty())
            {
                throw new ArgumentException($"{parameterName} can not be null or empty!", parameterName);
            }

            return value;
        }

        public static string NotNullOrWhiteSpace(string value, string parameterName)
        {
            if (value.IsNullOrWhiteSpace())
            {
                throw new ArgumentException($"{parameterName} can not be null, empty or white space!", parameterName);
            }

            return value;
        }

        public static ICollection<T> NotNullOrEmpty<T>(ICollection<T> value,  string parameterName)
        {
            if (value.IsNullOrEmpty())
            {
                throw new ArgumentException(parameterName + " can not be null or empty!", parameterName);
            }

            return value;
        }

        public static T Condition<T>(T value, Predicate<T> condition, string parameterName)
        {
            NotNull(condition, nameof(condition));
            NotNull(value, nameof(value));

            if (!condition(value))
            {
                NotNullOrEmpty(parameterName, nameof(parameterName));

                throw new ArgumentOutOfRangeException(parameterName);
            }

            return value;
        }
    }
}