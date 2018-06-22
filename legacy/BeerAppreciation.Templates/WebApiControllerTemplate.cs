namespace EventSauce.Web.Controllers
{
	using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.OData;
    using System.Web.Http.OData.Query;
	using EventSauce.Core.Services;
	using EventSauce.Domain;

	/// <summary>
    /// WebApi controller exposing functionality relating to AccountUsers
    /// </summary>
	public class AccountUsersController : ApiController
	{

		/// <summary>
        /// The AccountUser service
        /// </summary>
        private readonly IAccountUserService accountUserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountUsersController" /> class.
        /// </summary>
        /// <param name="accountUserService"></param>
        public AccountUsersController(IAccountUserService accountUserService)
        {
            this.accountUserService = accountUserService;
        }

		/// <summary>
        /// Gets a list of AccountUsers
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A list of AccountUsers
        /// </returns>
        /// <remarks>
        /// GET: api/accountusers
        /// </remarks>
        public PageResult<AccountUser> Get(ODataQueryOptions<AccountUser> queryOptions, string includes = "")
        {
            return accountUserService.GetAccountUsers(queryOptions, includes);
        }

		/// <summary>
        /// Gets an AccountUser matching the identifier
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// The matching AccountUser
        /// </returns>
        /// <remarks>
        /// GET: api/accountusers/{id}
        /// </remarks>
        public AccountUser Get(string id, string includes = "")
        {
            return accountUserService.GetAccountUser(id, includes);
        }

		/// <summary>
        /// Adds a new AccountUser
        /// </summary>
        /// <param name="accountUser">The AccountUser.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the POST
        /// </returns>
        /// <remarks>
        /// POST: api/AccountUsers
        /// </remarks>
        public HttpResponseMessage Post(AccountUser accountUser, bool updateGraph = false)
        {
            string accountUserId = this.accountUserService.InsertAccountUser(accountUser, updateGraph);

            // Success, return a uri to the created resource in the response header
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);
            response.Headers.Location = new Uri(this.Request.RequestUri.AbsoluteUri + "/" + accountUserId.ToString());

            return response;
        }

		/// <summary>
        /// Updates the specified AccountUser
        /// </summary>
        /// <param name="id">The AccountUser identifier.</param>
        /// <param name="accountUser">The accountUser.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the PUT
        /// </returns>
        /// <remarks>
        /// PUT: api/accountusers/{id}
        /// </remarks>
        public HttpResponseMessage Put(string id, AccountUser accountUser, bool updateGraph = false)
        {
            this.accountUserService.UpdateAccountUser(id, accountUser, updateGraph);

            // Success
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

		/// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A HttpResponseMessage containing the result of the DELETE</returns>
        /// <remarks>
        /// DELETE: api/accountusers/{id}
        /// </remarks>
        public HttpResponseMessage Delete(string id)
        {
            this.accountUserService.DeleteAccountUser(id);

            // Success
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
		
	}
}
