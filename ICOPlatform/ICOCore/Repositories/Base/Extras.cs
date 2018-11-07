using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

/// <summary>
/// Interface describing columns found on all database tables.
/// </summary>
public interface IDbTable
{
    //DateTime ModifiedDate { get; set; }
    //int ID { get; set; }

}

public enum GenericStatus
{
    Active = 1,
    Inactive = 2
}

public static class Extras
{
    public static PropertyInfo GetPrimaryKey(this Type entityType)
    {
        foreach (PropertyInfo property in entityType.GetProperties())
        {
            if (property.IsPrimaryKey())
            {
                if (property.PropertyType != typeof(long))
                {
                    throw new ApplicationException(string.Format("Primary key, '{0}', of type '{1}' is not int",
                                                                 property.Name, entityType));
                }
                return property;
            }
        }

        throw new ApplicationException(string.Format("No primary key defined for type {0}", entityType.Name));
    }

    public static bool IsPrimaryKey(this PropertyInfo propertyInfo)
    {
        var columnAttribute = propertyInfo.GetAttributeOf<ColumnAttribute>();
        if (columnAttribute == null) return false;
        return columnAttribute.IsPrimaryKey;
    }

    public static TAttribute GetAttributeOf<TAttribute>(this PropertyInfo propertyInfo)
    {
        object[] attributes = propertyInfo.GetCustomAttributes(typeof(TAttribute), true);
        if (attributes.Length == 0)
        {
            return default(TAttribute);
        }
        return (TAttribute)attributes[0];
    }

}

[Serializable]
public class PrimaryKeyNotFoundException : Exception
{
    //
    // For guidelines regarding the creation of new exception types, see
    //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
    // and
    //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
    //

    public PrimaryKeyNotFoundException()
    {
    }

    public PrimaryKeyNotFoundException(string message)
        : base(message)
    {
    }

    public PrimaryKeyNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }

    protected PrimaryKeyNotFoundException(
        SerializationInfo info,
        StreamingContext context)
        : base(info, context)
    {
    }
}

