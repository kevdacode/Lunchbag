﻿using System.Linq.Expressions;

namespace Lunchbag.API.Services
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> ExistsAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
