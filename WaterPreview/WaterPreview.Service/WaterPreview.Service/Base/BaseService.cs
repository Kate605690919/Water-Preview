using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WaterPreview.Service.Base
{
    public class BaseService<T> where T:class
    {
        public dpnetwork_data_20160419_NewEntities WaterEntities = new dpnetwork_data_20160419_NewEntities();

        protected DbContext db;

        public BaseService(DbContext context)
        {
            this.db = context;
        }

        public BaseService()
        {

        }

        public DbContext Context
        {
            get { return db; }
        }

        public bool Add(T input) 
        {
            try
            {

                Type type = typeof(dpnetwork_data_20160419_NewEntities);
                PropertyInfo property = type.GetProperty(typeof(T).Name);
                DbSet<T> data = (DbSet<T>)property.GetValue(WaterEntities, null);
                data.Add(input);
                return true;
            }
            catch (Exception e)
            {
                return false;
                throw e;
            }
        }


        public bool Delete(string keyName, long id)
        {
            //using (dpnetwork_data_20160419_NewEntities WaterEntities = new dpnetwork_data_20160419_NewEntities())
            //{
            try
            {
                Type type = typeof(dpnetwork_data_20160419_NewEntities);
                PropertyInfo property = type.GetProperty(typeof(T).Name);
                DbSet<T> value = (DbSet<T>)property.GetValue(WaterEntities, null);

                PropertyInfo propertyT = typeof(T).GetProperty(keyName);
                var record = value.ToList().Where(p => (long)propertyT.GetValue(p, null) == id).SingleOrDefault();
                value.Remove(record);
                //            db.SaveChanges();
                return true;

            }
            catch (Exception e)
            {
                return false;
                throw e;
            }
            //}
        }

        public bool Save()
        {
            try
            {
                WaterEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
                throw e;
            }
        }

        public bool UpdateById(string keyName, T input)
        {
            using (dpnetwork_data_20160419_NewEntities WaterEntities = new dpnetwork_data_20160419_NewEntities())
            {
                try
                {
                    PropertyInfo[] propertyList = typeof(T).GetProperties();
                    //获取T类里面的主键名称keyName
                    Type type = typeof(dpnetwork_data_20160419_NewEntities);
                    PropertyInfo property = type.GetProperty(typeof(T).Name);
                    DbSet<T> value = (DbSet<T>)property.GetValue(WaterEntities, null);
                    //获取数据库实体集
                    PropertyInfo propertyKey = typeof(T).GetProperty(keyName);
                    //获取T类关键字段属性
                    var recordAll = value.ToArray();
                    object recordOld = new object();
                    long keyValueNew = (long)(propertyKey.GetValue(input, null));//要修改保存的关键字段属性值
                    for (int i = 0; i < recordAll.Length; i++)
                    {
                        if ((long)(propertyKey.GetValue(recordAll[i], null)) == keyValueNew)
                        {
                            recordOld = recordAll;
                            break;
                        }
                    }
                    //找出关键字段匹配的T实例
                    foreach (var item in propertyList)
                    {
                        if (!item.Name.Equals(keyName))
                        {
                            item.SetValue(recordOld, item.GetValue(input, null), null);
                        }
                    }
                    //给T实例的非关键字段属性进行赋值
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                    throw e;
                }
            }

        }


        public List<T> FindAll()
        {
            using (dpnetwork_data_20160419_NewEntities WaterEntities = new dpnetwork_data_20160419_NewEntities())
            {
                try
                {
                    Type type = typeof(dpnetwork_data_20160419_NewEntities);
                    PropertyInfo[] properties = type.GetProperties();
                    PropertyInfo property = type.GetProperty(typeof(T).Name);
                    DbSet<T> value = (DbSet<T>)property.GetValue(WaterEntities, null);
                    //List<T> list = new List<T>();
                    List<T> valuelist = value.ToList();
                    //IEnumerable<T> list = valuelist.AsEnumerable();
                    return valuelist;
                }
                catch (Exception e)
                {
                    return null;
                    throw e;
                }
            }
        }

        public bool Modify(string keyName, T input)
        {
            //using (dpnetwork_data_20160419_NewEntities WaterEntities = new dpnetwork_data_20160419_NewEntities())
            //{
            try
            {
                Type type = typeof(dpnetwork_data_20160419_NewEntities);
                PropertyInfo property = type.GetProperty(typeof(T).Name);
                DbSet<T> value = (DbSet<T>)property.GetValue(WaterEntities, null);
                PropertyInfo[] propertyinput = input.GetType().GetProperties();
                long keyValue = 0;
                foreach (var item in propertyinput)
                {
                    if (item.Name.Equals(keyName))
                    {
                        keyValue = (long)item.GetValue(input, null);//获取input的关键字段属性值，用于下文匹配出要修改的实体
                        break;
                    }
                }
                PropertyInfo propertyT = typeof(T).GetProperty(keyName);
                var record = value.ToList().Where(p => (long)propertyT.GetValue(p, null) == keyValue).SingleOrDefault();
                PropertyInfo[] list_property = record.GetType().GetProperties();//各字段
                foreach (var item in list_property)
                {
                    if (!(item.Name).Equals(keyName))
                    {
                        item.SetValue(record, item.GetValue(input, null), null);//除了_Id的字段名对应的值，其他字段名对应的值都赋值。
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
                throw e;
            }
            //}

        }


    }
}
