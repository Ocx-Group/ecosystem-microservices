using System.Data;
using System.Data.Common;
using System.Dynamic;

namespace Ecosystem.WalletService.Domain.Extensions;

public static class SqlHelper
{
    private static dynamic ToExpando(IDataRecord record)
    {
        var expandoObject = new ExpandoObject() as IDictionary<string, object?>;

        for (var i = 0; i < record.FieldCount; i++)
        {
            var value = record[i] == DBNull.Value ? null: record[i];
            expandoObject.Add(record.GetName(i), value);
        }

        return expandoObject;
    }

    private static IEnumerable<IDictionary<string, object>> Read(DbDataReader reader)
    {
        while (reader.Read())
        {
            yield return ToExpando(reader) as IDictionary<string, object>;
        }
    }

    public static List<IDictionary<string, object>> ToDynamicList(this DbDataReader reader)
    {
        var dd = Read(reader);
        return dd.ToList();
    }
}