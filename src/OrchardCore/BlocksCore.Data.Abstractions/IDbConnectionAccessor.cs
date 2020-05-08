using System.Data.Common;

namespace BlocksCore.Data
{
    public interface IDbConnectionAccessor
    {
        DbConnection CreateConnection();
    }
}
