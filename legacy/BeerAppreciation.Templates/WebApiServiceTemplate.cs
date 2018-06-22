namespace EventSauce.Core.Services
{
	using System.Web.Http.OData;
	using System.Web.Http.OData.Query;
	using EventSauce.Domain;

	public interface IAccountUserService
	{
		/// <summary>
		/// Gets a list of all AccountUsers.
		/// </summary>
		/// <param name="queryOptions">The OData query options.</param>
		/// <param name="includes">The includes.</param>
		/// <returns>
		/// A list of all AccountUsers
		/// </returns>
		PageResult<AccountUser> GetAccountUsers(ODataQueryOptions queryOptions, string includes);

		/// <summary>
		/// Adds the AccountUser to the database.
		/// </summary>
		/// <param name="accountUser">The AccountUser.</param>
		/// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
		/// <returns>The identifier of the accountUser added to database</returns>
		string InsertAccountUser(AccountUser accountUser, bool updateGraph);

		/// <summary>
		/// Gets the AccountUser matching the specified Id
		/// </summary>
		/// <param name="accountUserId">The AccountUser unique identifier.</param>
		/// <param name="includes">The includes.</param>
		/// <returns>
		/// The AccountUser matching the id
		/// </returns>
		AccountUser GetAccountUser(string accountUserId, string includes);

		/// <summary>
		/// Updates the specified AccountUser.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="accountUser">The AccountUser.</param>
		/// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
		void UpdateAccountUser(string id, AccountUser accountUser, bool updateGraph);

		/// <summary>
		/// Deletes the specified AccountUser.
		/// </summary>
		/// <param name="id">The AccountUser identifier.</param>
		void DeleteAccountUser(string id);   
	}
}

namespace EventSauce.Core.Services
{
	using System;
	using System.Web.Http.OData;
	using System.Web.Http.OData.Query;
	using BeerAppreciation.Data;
    using BeerAppreciation.Data.EF.Extensions;
    using BeerAppreciation.Data.EF.UnitOfWork;
	using EventSauce.Core.Repositories;
	using EventSauce.Domain;

	public class AccountUserService : IAccountUserService
	{

		/// <summary>
        /// The unit of work factory
        /// </summary>
        private readonly IUnitOfWorkInterceptorFactory<EventSauceContext> unitOfWorkFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountUserService" /> class.
        /// </summary>
        /// <param name="unitOfWorkFactory"></param>
        public AccountUserService(IUnitOfWorkInterceptorFactory<EventSauceContext> unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

		/// <summary>
		/// Gets a list of all AccountUsers.
		/// </summary>
		/// <param name="queryOptions">The OData query options.</param>
		/// <param name="includes">The includes.</param>
		/// <returns>
		/// A list of all AccountUsers
		/// </returns>
		public PageResult<AccountUser> GetAccountUsers(ODataQueryOptions queryOptions, string includes)
		{
			using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                IEntityRepository<AccountUser> entityRepository = unitOfWork.GetRepository<AccountUser, string>();

                // Query the generic repository using any odata query options and includes
                return entityRepository.GetList(queryOptions, includes);
            };
		}


		/// <summary>
		/// Adds the AccountUser to the database.
		/// </summary>
		/// <param name="accountUser">The AccountUser.</param>
		/// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
		/// <returns>The identifier of the accountUser added to database</returns>
		public string InsertAccountUser(AccountUser accountUser, bool updateGraph)
		{
			using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<AccountUser> entityRepository = unitOfWork.GetRepository<AccountUser, string>();

                // Insert the entity
                if (updateGraph)
                {
                    // Update any entities in the graph
                    entityRepository.InsertOrUpdateGraph(accountUser);
                }
                else
                {
                    // Update just the root entity
                    entityRepository.Insert(accountUser);
                }

                // Persist the changes and return Id
                return unitOfWork.Save().GetInsertedEntityKey<string>("AccountUsers");
            }
		}

		/// <summary>
		/// Gets the AccountUser matching the specified Id
		/// </summary>
		/// <param name="accountUserId">The AccountUser unique identifier.</param>
		/// <param name="includes">The includes.</param>
		/// <returns>
		/// The AccountUser matching the id
		/// </returns>
		public AccountUser GetAccountUser(string accountUserId, string includes)
		{
			using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<AccountUser> entityRepository = unitOfWork.GetRepository<AccountUser, string>();

                // GetChangedActionTypes the matching entity
                return entityRepository.GetSingle(t => t.Id == accountUserId, includes.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            }
		}

		/// <summary>
		/// Updates the specified AccountUser.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="accountUser">The AccountUser.</param>
		/// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
		public void UpdateAccountUser(string id, AccountUser accountUser, bool updateGraph)
		{
			using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<AccountUser> entityRepository = unitOfWork.GetRepository<AccountUser, string>();

                // Update the entity
                if (updateGraph)
                {
                    entityRepository.InsertOrUpdateGraph(accountUser);
                }
                else
                {
                    entityRepository.Update(accountUser);
                }

                // Persist the changes
                unitOfWork.Save();
            }
		}

		/// <summary>
		/// Deletes the specified AccountUser.
		/// </summary>
		/// <param name="id">The AccountUser identifier.</param>
		public void DeleteAccountUser(string id)
		{
			using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<AccountUser> entityRepository = unitOfWork.GetRepository<AccountUser, string>();

                // Insert the entity
                entityRepository.Delete(id);

                // Persist the changes
                unitOfWork.Save();
            }
		}   
	}
}

