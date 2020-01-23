namespace BlocksCore.SyntacticAbstractions.NullObject
{
    public static class NullObjectExtensions
    {
        public static bool IsNullObject(this object obj)
        {
            return obj == null || obj is INullObject;
        }
    }
}