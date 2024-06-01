using CoreAndFood.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CoreAndFood.Repositories
{
    public class GenericRepository<T> where T : class, new()
    {
        Context c = new Context();


        public List<T> TList()
        {
            return c.Set<T>().ToList();
        }
        public void TAdd(T parametre)
        {
            c.Set<T>().Add(parametre);
            c.SaveChanges();
        }
        public void TDelete(T parametre)
        {
            c.Set<T>().Remove(parametre);
            c.SaveChanges();
        }
        public void TUpdate(T parametre)
        {
            c.Set<T>().Update(parametre);
            c.SaveChanges();
        }
        public T TGet(int id)
        {
            return c.Set<T>().Find(id);
        }
        public void TCategory(int id)
        {
            c.Set<T>().Find(id);
        }
        public List<T> TList(string parametre)
        {
            return c.Set<T>().Include(parametre).ToList();
        }
        public List<T> List (Expression<Func<T, bool>> filter)
        {
            return c.Set<T>().Where(filter).ToList();
        }
        //public List<T> GenericGetList (Expression<Func<T, bool>> filter = null)
        //{
        //    var query = c.Set<T>().AsQueryable();
        //    if (filter != null)
        //    {
        //        query = query.Where(filter);
        //    }
        //    return query.ToList();
        //}
    }
}
