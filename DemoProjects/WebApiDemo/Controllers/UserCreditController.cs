using Microsoft.AspNetCore.Mvc;
using WebApiDemo.Application;
using WebApiDemo.ViewModels.UserCredit;

namespace WebApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCreditController : ControllerBase
    {
        private readonly UserCreditApplication _userCreditApplication;

        public UserCreditController(UserCreditApplication userCreditApplication)
        {
            _userCreditApplication = userCreditApplication;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _userCreditApplication.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _userCreditApplication.GetByIdAsync(id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost("trade-token")]
        public async Task<IActionResult> CreateTradeToken([FromBody] CreateTradeTokenRequest request)
        {
            var result = await _userCreditApplication.CreateTradeToken(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
