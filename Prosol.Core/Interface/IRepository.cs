using MongoDB.Driver;
using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Interface
{
    public interface IRepository<T> where T : class, new()
    {

        bool Add(T item);
        int Add(IEnumerable<T> items);
        bool Update(IMongoQuery query, IMongoUpdate items, UpdateFlags flg);
        byte[] GridFsFindOne(IMongoQuery query);
        T FindOne();
        T FindOne(IMongoQuery query);
        IEnumerable<T> FindAll();
        IEnumerable<T> FindAll(IMongoSortBy sort);
        IEnumerable<T> FindAll(IMongoQuery query);
        IEnumerable<T> FindAll(IMongoFields Flds);
        IEnumerable<T> FindAll(IMongoFields Flds, IMongoSortBy sort);
        IEnumerable<T> FindAll(IMongoQuery quer, IMongoSortBy sort);

        IEnumerable<T> FindAll(IMongoFields Flds, IMongoQuery query);
        IEnumerable<T> FindAll(IMongoFields Flds, IMongoQuery query, IMongoSortBy sort);

        T Single(Expression<Func<T, bool>> expression);

        void GridFsDel(IMongoQuery query);
        string GridFsUpload(Stream stream, String RemoteFileName);

        string[] AutoSearch(IMongoQuery query, string key);
        string[] AutoSearch1(string key);


        System.Linq.IQueryable<T> Query { get; set; }
        System.Linq.IQueryable<T> All(int page, int pageSize);

        int Delete(Expression<Func<T, bool>> expression);
        bool Delete(IMongoQuery query);

        void DeleteAll();
        bool DeleteAll(IMongoQuery query);
        //void Delete(string uniqueId);
        string GetGridFsFileId(IMongoQuery query);
    }
}
