
using System.Collections.Generic;
using System.Reflection;
using System.Collections.Specialized;
using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.ComponentModel;
using System.Web.Script.Serialization;

namespace ControlPanel.Core.Entities
{
		
	public static class EntityHelper
    {
        public static bool IsTransient(object entity)
        {

            PropertyInfo[] propertiesArray = entity.GetType().GetProperties();
            PropertyInfo prop = null;
            foreach (PropertyInfo info in propertiesArray)
            {
                object obj = info.GetCustomAttributes(typeof(PrimaryKeyAttribute), true).FirstOrDefault();
                if (obj != null)
                {
                    prop = info;
                    break;
                }
            }

            if (prop == null)
            {
                return true;
            }
            else
            {
                object value = prop.GetValue(entity, null);
                if (Convert.ToInt32(value) == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            
        }

        public static List<T> LoadDataTable<T>(this List<T> list, DataTable dt)
        {
            string columnName = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                T entity = (T)Activator.CreateInstance(typeof(T));
                foreach (DataColumn col in dt.Columns)
                {
                    Type type = entity.GetType().BaseType == null ? entity.GetType() : entity.GetType().BaseType;
                    PropertyInfo propInfo = type.GetProperties().Where(x => x.Name.ToLower() == col.ColumnName.ToLower()).FirstOrDefault();
                    if (propInfo == null)
                        propInfo = type.GetProperties().Where(x => ((FieldNameAttribute)x.GetCustomAttributes(typeof(FieldNameAttribute), true).FirstOrDefault()) != null && ((FieldNameAttribute)x.GetCustomAttributes(typeof(FieldNameAttribute), true).FirstOrDefault()).FieldName.ToLower() == col.ColumnName.ToLower()).FirstOrDefault();
                    object value = dr[col.ColumnName];
                    if (value != null && !string.IsNullOrEmpty(value.ToString()) && propInfo != null)
                    {
                        propInfo.SetValue(entity, value, null);
                    }
                }
                list.Add(entity);
            }
            return list;
        }
        public static List<T> LoadDataTable<T>(this DataTable dt)
        {
            List<T> list = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                T entity = (T)Activator.CreateInstance(typeof(T));
                foreach (DataColumn col in dt.Columns)
                {
                    Type type = entity.GetType();
                    PropertyInfo propInfo = type.GetProperties().Where(x => x.Name.ToLower() == col.ColumnName.ToLower()).First();
                    object value = dr[col.ColumnName];
                    if (value != null && !string.IsNullOrEmpty(value.ToString()))
                    {
                        propInfo.SetValue(entity, value, null);

                    }
                }
                list.Add(entity);
            }
            return list;
        }
        public static bool IsNullableType(Type theType)
        {
            return (theType.IsGenericType && theType.
              GetGenericTypeDefinition().Equals
              (typeof(Nullable<>)));
        }
        public static Type GetObjectType(object obj)
        {
            Type type = obj.GetType().BaseType == null ? obj.GetType() : obj.GetType().BaseType;
            if (type.Name.ToLower() == "object")
                return obj.GetType();
            else
                return type;
        }
        public static DataTable GetDataTableStructure<T>(this T obj)
        {
            DataTable dt = new DataTable(obj.GetType().Name);
            Type type = GetObjectType(obj);
            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                if (propertyInfo.DeclaringType != typeof(EntityBase))
                {
                    DataColumn column = new DataColumn(propertyInfo.Name);
                    Type propertyType = propertyInfo.PropertyType;
                    if (IsNullableType(propertyType))
                    {
                        NullableConverter nc = new NullableConverter(propertyType);
                        propertyType = nc.UnderlyingType;
                        column.AllowDBNull = true;
                    }
                    column.DataType = propertyType;
                    dt.Columns.Add(column);
                }
            }
            return dt;
        }

        public static DataRow ToDataRow<T>(this T obj, DataRow dr)
        {
            foreach (DataColumn col in dr.Table.Columns)
            {
                Type type = GetObjectType(obj);

                PropertyInfo pinfo = type.GetProperties().Where(x=>x.Name.ToLower()==col.ColumnName.ToLower()).FirstOrDefault();
                if (pinfo.DeclaringType != typeof(EntityBase))
                {
                    if (pinfo == null)
                    {
                        throw new Exception("Invalid DataRow");
                    }
                    object value = pinfo.GetValue(obj, null);
                    if (value != null)
                    {
                        dr[col.ColumnName] = pinfo.GetValue(obj, null);
                    }
                }
            }
            return dr;
        }

        public static DataTable ToDataTable<T>(this List<T> items, DataTable dt)
        {
            if (dt == null) throw new Exception("DataTabe is null");
            if (items == null || items.Count == 0) throw new Exception("List is empty or null");

            foreach (T item in items)
            {
                DataRow dr = dt.NewRow();
                dr = item.ToDataRow(dr);
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static string GetSerializedSearchCriteria(this EntityBase obj, string operand)
        {
            List<SearchColumn> searchColumns = new List<SearchColumn>();
            PropertyInfo[] propertyInfos = obj.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (propertyInfo.GetSetMethod() != null && propertyInfo.GetSetMethod().IsPublic && propertyInfo.DeclaringType != typeof(EntityBase))
                {
                    object val = propertyInfo.GetValue(obj, null);
                    if (val != null && !string.IsNullOrEmpty(val.ToString()))
                    {
                        if (propertyInfo.DeclaringType != typeof(System.String) && val.ToString() == "0")
                        {

                        }
                        else
                        {
                            SearchColumn searchColumn = new SearchColumn();
                            searchColumn.Name = propertyInfo.Name;
                            searchColumn.Value = val.ToString();
                            searchColumn.Operand = "=";
                            searchColumn.Criteria = operand;
                            searchColumn.DataType = val.GetType().ToString();
                            searchColumns.Add(searchColumn);
                        }

                    }
                }
            }
            if (searchColumns.Count > 0)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(searchColumns);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetSerializedSearchCriteria(this EntityBase obj, string[] properties, string[] criteria, string operand)
        {
            List<SearchColumn> searchColumns = new List<SearchColumn>();
            if (properties.Length != criteria.Length)
                return null;
            int i = 0;
            foreach (string prop in properties)
            {
                PropertyInfo propInfo = obj.GetType().GetProperty(prop);
                if (propInfo == null) throw new Exception(string.Format("Property '{0}' doesn't exist in the entity", prop));
                object val = propInfo.GetValue(obj, null);
                SearchColumn searchColumn = new SearchColumn();
                searchColumn.Name = propInfo.Name;
                searchColumn.Value = val.ToString();
                searchColumn.Operand = criteria[i];
                searchColumn.Criteria = operand;
                searchColumn.DataType = val.GetType().ToString();
                searchColumns.Add(searchColumn);
                i++;
            }

            if (searchColumns.Count > 0)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(searchColumns);
            }
            else
            {
                return string.Empty;
            }
        }

        public static List<SearchColumn> GetSearchCriteriaList(this EntityBase obj, string operand)
        {
            List<SearchColumn> searchColumns = new List<SearchColumn>();
            PropertyInfo[] propertyInfos = obj.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (propertyInfo.GetSetMethod() != null && propertyInfo.GetSetMethod().IsPublic && propertyInfo.DeclaringType != typeof(EntityBase))
                {
                    object val = propertyInfo.GetValue(obj, null);
                    if (val != null && !string.IsNullOrEmpty(val.ToString()))
                    {
                        if (propertyInfo.DeclaringType != typeof(System.String) && val.ToString() == "0")
                        {

                        }
                        else
                        {
                            SearchColumn searchColumn = new SearchColumn();
                            searchColumn.Name = propertyInfo.Name;
                            searchColumn.Value = val.ToString();
                            searchColumn.Operand = "=";
                            searchColumn.Criteria = operand;
                            searchColumn.DataType = val.GetType().ToString();
                            searchColumns.Add(searchColumn);
                        }

                    }
                }
            }
            return searchColumns;
        }

        public static string GetSerializedSearchCriteria(this EntityBase obj, string[] properties, string[] criteria, string operand, bool IsStartWithSearching)
        {
            List<SearchColumn> searchColumns = new List<SearchColumn>();
            string WhereClause = GetSerializedSearchCriteria(obj, properties, criteria, operand);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            if (!string.IsNullOrEmpty(WhereClause))
            {
                searchColumns = serializer.Deserialize<List<SearchColumn>>(WhereClause);
                if (searchColumns != null)
                {
                    foreach (SearchColumn sCol in searchColumns)
                    {
                        sCol.IsStartsWithSearching = IsStartWithSearching;
                    }
                }
            }
            return serializer.Serialize(searchColumns);
        }
    }
	
	public class PrimaryKeyAttribute : Attribute
    {

    }
	
	
}
