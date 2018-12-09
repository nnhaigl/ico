using AutoMapper;
using ICOCore.Dtos.Components;
using ICOCore.Repositories;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;

namespace ICOCore.Utils
{
    public class MapperUtils
    {
        static MapperUtils()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ProvideHelp, ProvideHelpDto>();
                cfg.CreateMap<CfgProvideHelpPolicy, CfgProvideHelpPolicyDto>();
                cfg.CreateMap<CfgTokenDemand, CfgTokenDemandDto>();
            });
        }

        private static void CreateMap<Entity, Dto>()
        {
            //Mapper.Initialize(cfg =>
            //  {
            //      cfg.CreateMap<Entity, Dto>();
            //  });
            //Mapper.Initialize(cfg =>
            //  {
            //      cfg.CreateMap<User, UserDto>();
            //      cfg.CreateMap<Order, OrderVM>();
            //      cfg.CreateMap<Transaction, TransactionVM>();
            //  });
        }

        /// <summary>
        /// Covert Entity sang Dto và ngược lại (test lại cho kỹ)
        /// TODO : Chưa thêm hết các kiểu nullable và 1 số primitive type v..v
        /// </summary>
        /// <typeparam name="Source">Entity Type nguồn copy</typeparam>
        /// <typeparam name="Des">Entity Type đích cần copy</typeparam>
        /// <param name="entities"></param>
        /// <param name="excludeProperties">THuộc tính không áp dụng shallow copy</param>
        /// <returns></returns>
        public static List<Des> ConvertToList<Source, Des>(List<Source> entities, string[] excludeProperties = null)
        {
            if (entities == null || entities.Count == 0)
                return new List<Des>(0);

            CreateMap<Source, Des>();
            List<Des> dess = new List<Des>(entities.Count);

            foreach (var value in entities)
            {
                var result = (Des)Mapper.Map(value, value.GetType(), typeof(Des));

                if (excludeProperties != null && excludeProperties.Length > 0)
                {
                    foreach (string prop in excludeProperties)
                    {
                        PropertyInfo propertyInfo = result.GetType().GetProperty(prop);

                        if (propertyInfo.PropertyType == typeof(string))
                        {
                            //string value = propertyInfo.GetValue(data, null);
                            propertyInfo.SetValue(result, Convert.ChangeType(null, propertyInfo.PropertyType), null);
                        }
                        else if (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(long)
                            || propertyInfo.PropertyType == typeof(decimal) || propertyInfo.PropertyType == typeof(float))
                        {
                            propertyInfo.SetValue(result, Convert.ChangeType(0, propertyInfo.PropertyType), null);
                        }
                        else if (propertyInfo.PropertyType == typeof(DateTime)) // typeof(DateTime?)
                        {
                            propertyInfo.SetValue(result, Convert.ChangeType(DateTime.Now, propertyInfo.PropertyType), null);
                        }
                    }
                }

                dess.Add(result);
            }

            return dess;

        }

        public static Des ConvertTo<Source, Des>(Source value, string[] excludeProperties = null)
        {
            //System.Type type = typeof(Dto);

            CreateMap<Source, Des>();

            //var result = (Dto)Mapper.Map(value, typeof(Entity), typeof(Dto));
            //var result1 = Mapper.Map(value, value.GetType(), typeof(Dto));

            var result = (Des)Mapper.Map(value, value.GetType(), typeof(Des));

            if (excludeProperties != null && excludeProperties.Length > 0)
            {
                foreach (string prop in excludeProperties)
                {
                    PropertyInfo propertyInfo = result.GetType().GetProperty(prop);

                    //// Gets what the data type is of our property (Foreign Key Property)
                    //System.Type propertyType = propertyInfo.PropertyType;

                    //// Get the type code so we can switch
                    //System.TypeCode typeCode = System.Type.GetTypeCode(propertyType);

                    //switch (typeCode)
                    //{
                    //    case TypeCode.Int32:
                    //        propertyInfo.SetValue(result, Convert.ToInt32(0), null);
                    //        break;
                    //    case TypeCode.Int64:
                    //        propertyInfo.SetValue(result, Convert.ToInt64(0), null);
                    //        break;
                    //    case TypeCode.String:
                    //        propertyInfo.SetValue(result, 0, null);
                    //        break;
                    //    case TypeCode.Object:
                    //        if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                    //        {
                    //            //propertyInfo.SetValue(result, Guid.Parse(value), null);
                    //        }
                    //        break;
                    //    default:
                    //        propertyInfo.SetValue(type, value, null);
                    //        break;
                    //}

                    if (propertyInfo.PropertyType == typeof(string))
                    {
                        //string value = propertyInfo.GetValue(data, null);
                        propertyInfo.SetValue(result, Convert.ChangeType(null, propertyInfo.PropertyType), null);
                    }
                    else if (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(long)
                        || propertyInfo.PropertyType == typeof(decimal) || propertyInfo.PropertyType == typeof(float))
                    {
                        propertyInfo.SetValue(result, Convert.ChangeType(0, propertyInfo.PropertyType), null);
                    }
                    else if (propertyInfo.PropertyType == typeof(DateTime)) // typeof(DateTime?)
                    {
                        propertyInfo.SetValue(result, Convert.ChangeType(DateTime.Now, propertyInfo.PropertyType), null);
                    }
                }
            }

            return result;
        }

        public static void SetKey<T>(T obj, TempDataDictionary _tempData)
        {
            System.Type type = typeof(T);

            // Get our Foreign Key that we want to maintain
            String foreignKey = _tempData["ForeignKey"].ToString();

            // If we do not have a Foreign Key, we do not need to set a property
            if (String.IsNullOrEmpty(foreignKey))
                return;

            // Get our the value that we need to set our Foreign Key to
            String value = _tempData[foreignKey].ToString();

            // Get our property via reflection so that we can invoke methods against property
            System.Reflection.PropertyInfo prop = type.GetProperty(foreignKey.ToString());

            // Gets what the data type is of our property (Foreign Key Property)
            System.Type propertyType = prop.PropertyType;

            // Get the type code so we can switch
            System.TypeCode typeCode = System.Type.GetTypeCode(propertyType);
            try
            {

                switch (typeCode)
                {
                    case TypeCode.Int32:
                        prop.SetValue(type, Convert.ToInt32(value), null);
                        break;
                    case TypeCode.Int64:
                        prop.SetValue(type, Convert.ToInt64(value), null);
                        break;
                    case TypeCode.String:
                        prop.SetValue(type, value, null);
                        break;
                    case TypeCode.Object:
                        if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                        {
                            prop.SetValue(obj, Guid.Parse(value), null);
                            return;
                        }
                        break;
                    default:
                        prop.SetValue(type, value, null);
                        break;
                }

                return;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to set property value for our Foreign Key");
            }
        }

    }
}
