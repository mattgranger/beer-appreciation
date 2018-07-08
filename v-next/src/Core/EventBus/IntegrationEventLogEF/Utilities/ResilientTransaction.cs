namespace BeerAppreciation.Core.IntegrationEventLogEF.Utilities
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    public class ResilientTransaction
    {
        private readonly DbContext context;

        private ResilientTransaction(DbContext context) =>
            this.context = context ?? throw new ArgumentNullException(nameof(context));

        public static ResilientTransaction New (DbContext context) =>
            new ResilientTransaction(context);        

        public async Task ExecuteAsync(Func<Task> action)
        {
            //Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
            //See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
            var strategy = this.context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = this.context.Database.BeginTransaction())
                {
                    await action();
                    transaction.Commit();
                }
            });
        }
    }
}
