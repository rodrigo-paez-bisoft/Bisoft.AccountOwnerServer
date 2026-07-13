using Contracts;
using Entities.models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;



namespace Bisoft.AccountOwnerServer.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        [HttpGet]
        public IActionResult GetAccountsForOwner(Guid ownerId, [FromQuery] AccountParameters parameters)
        {

            var accounts = _repository.Account.GetAccountsByOwner(ownerId, parameters);

            var metadata = new
            {
                accounts.TotalCount,
                accounts.PageSize,
                accounts.CurrentPage,
                accounts.TotalPages,
                accounts.HasNext,
                accounts.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            _logger.LogInfo($"Returned {accounts.TotalCount} owners from database.");

            return Ok(accounts);
        }
    }
}
