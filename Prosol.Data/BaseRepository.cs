using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;
using builder = MongoDB.Driver.Builders;
using Prosol.Core.Interface;
using System.IO;
using MongoDB.Driver.GridFS;


namespace Prosol.Data
{
    public class BaseRepository<T> : IRepository<T> where T : class, new()
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["ProsolConnect"].ConnectionString;
        private static MongoServer _server = new MongoClient(_connectionString).GetServer();
        // private string _collectionName;
        private MongoDatabase _db;

        //Constructor
        public BaseRepository()
        {

            _db = _server.GetDatabase(MongoUrl.Create(_connectionString).DatabaseName);
        }


        // Properties
        protected MongoCollection<T> _collection
        {
            get
            {
                return _db.GetCollection<T>(typeof(T).Name);
            }
            set
            {
                _collection = value;
            }
        }

        public IQueryable<T> Query
        {
            get
            {
                return _collection.AsQueryable<T>();
            }
            set
            {
                Query = value;
            }
        }

        // Autocomplete

        public string[] AutoSearch(IMongoQuery query, string key)
        {

            var arrStr = _collection.Distinct<string>(key, query).ToArray();
            return arrStr;
        }
        public string[] AutoSearch1(string key)
        {

            var arrStr = _collection.Distinct<string>(key).ToArray();
            return arrStr;
        }

        // Insert/update
        public bool Add(T item)
        {
            try
            {
                var result = _collection.Save(item);
                return result.Response.ToBoolean();
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());

            }

        }
        public string GetGridFsFileId(IMongoQuery query)
        {
            string fileId = "";

            MongoGridFSFileInfo file = _db.GridFS.FindOne(query, 1);
            if (file != null)
            {
                fileId = file.Id.ToString();
            }
            return fileId;
        }
        public int Add(IEnumerable<T> items)
        {
            int count = 0;

            foreach (T item in items)
            {
                if (Add(item))
                {
                    count++;
                }
            }

            return count;
        }

        public bool Update(IMongoQuery query, IMongoUpdate items, UpdateFlags flg)
        {
            try
            {
                var result = _collection.Update(query, items, flg);
                return result.Response.ToBoolean();
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());

            }
        }

        // Image/Auto/Video 
        public void GridFsDel(IMongoQuery query)
        {
            _db.GridFS.Delete(query);
        }
        public string GridFsUpload(Stream stream, String RemoteFileName)
        {
            // _db.GridFS.Chunks.EnsureIndex();
            var gfsi = _db.GridFS.Upload(stream, RemoteFileName);
            return gfsi.Id.ToString();
        }
        public byte[] GridFsFindOne(IMongoQuery query)
        {
            byte[] bytes = null;
            MongoGridFSFileInfo file = _db.GridFS.FindOne(query);
            if (file != null)
            {
                using (var stream = file.OpenRead())
                {
                    bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, (int)stream.Length);

                }
            }
            return bytes;
        }


        // Select
        public T FindOne()
        {
            var QryItem = _collection.FindOne();
            return QryItem;
        }
        public T FindOne(IMongoQuery query)
        {
            var QryItem = _collection.FindOne(query);
            return QryItem;
        }

        public IEnumerable<T> FindAll()
        {
            var QryItem = _collection.FindAll();
            return QryItem;
        }

        public IEnumerable<T> FindAll(IMongoSortBy sort)
        {
            var QryItem = _collection.FindAll().SetSortOrder(sort);
            return QryItem;

        }
        public IEnumerable<T> Find()
        {

            var QryItem = _collection.FindAll().SetLimit(10);
            return QryItem;
        }

        public IEnumerable<T> FindAll(IMongoFields Flds, IMongoQuery query)
        {
            var QryItem = _collection.Find(query).SetFields(Flds);
            return QryItem;
        }
        public IEnumerable<T> FindAll(IMongoFields Flds)
        {
            var QryItem = _collection.FindAll().SetFields(Flds);
            return QryItem;
        }
        public IEnumerable<T> FindAll(IMongoFields Flds, IMongoSortBy sort)
        {
            var QryItem = _collection.FindAll().SetFields(Flds).SetSortOrder(sort);
            return QryItem;
        }
        public IEnumerable<T> FindAll(IMongoQuery query)
        {
            var QryItem = _collection.Find(query);
            return QryItem;
        }
        public IEnumerable<T> FindAll(IMongoQuery query, IMongoSortBy sort)
        {

            var QryItem = _collection.Find(query).SetSortOrder(sort);
            return QryItem;
        }
        public IEnumerable<T> FindAll(IMongoFields Flds, IMongoQuery query, IMongoSortBy sort)
        {
            var QryItem = _collection.Find(query).SetFields(Flds).SetSortOrder(sort);
            return QryItem;
        }


        //Delete
        public bool Delete(IMongoQuery query)
        {
            // ObjectId id = new ObjectId(typeof(T).GetProperty("Id").GetValue(item, null).ToString());
            // var query = builder.Query.EQ("_id", id);

            // Remove the object.s
            var result = _collection.Remove(query);
            return result.DocumentsAffected > 0;
        }
        public bool DeleteAll(IMongoQuery query)
        {
            var QryItem = _collection.Remove(query);
            return QryItem.DocumentsAffected > 0;
        }

        public void DeleteAll()
        {
            _db.DropCollection(typeof(T).Name);
        }


        // Unused Items
        public T Single(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return Query.Where(expression).SingleOrDefault();
        }
        public IQueryable<T> All(int page, int pageSize)
        {
            return PagingExtensions.Page(Query, page, pageSize);
        }
        public int Delete(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            int count = 0;

            //var items = Query.Where(expression);
            //foreach (T item in items)
            //{
            //    if (Delete(item))
            //    {
            //        count++;
            //    }
            //}

            return count;
        }

        //public void Delete(string uniqueId)
        //{
        //    throw new NotImplementedException();
        //}
    }
}


