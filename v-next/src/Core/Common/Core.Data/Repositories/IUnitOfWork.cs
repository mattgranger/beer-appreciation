﻿namespace Core.Shared.Data
{
    using System.Threading.Tasks;

    public interface IUnitOfWork
    {
        Task SaveChanges(bool ensureAutoHistory = false);
    }
}
