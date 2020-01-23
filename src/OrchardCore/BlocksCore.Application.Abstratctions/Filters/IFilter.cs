namespace BlocksCore.Application.Abstratctions.Filters
{
    public interface IFilter
    {
        /// <summary>Gets or sets a value indicating whether more than one instance of the indicated attribute can be specified for a single program element.</summary>
        /// <returns>true if more than one instance is allowed to be specified; otherwise, false. The default is false.</returns>
        bool AllowMultiple { get; }
    }
}