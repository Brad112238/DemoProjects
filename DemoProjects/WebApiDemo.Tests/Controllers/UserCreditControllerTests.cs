using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApiDemo.Application;
using WebApiDemo.Controllers;
using WebApiDemo.Interfaces;
using WebApiDemo.Models.TestDb;
using WebApiDemo.Tests.TestHelpers;
using WebApiDemo.ViewModels;
using WebApiDemo.ViewModels.Ecpay;
using WebApiDemo.ViewModels.UserCredit;
using Xunit;

namespace WebApiDemo.Tests.Controllers
{
    public class UserCreditControllerTests
    {
        private readonly Mock<IUserCreditService> _mockUserCreditService;
        private readonly Mock<IEcpayService> _mockEcpayService;
        private readonly UserCreditController _controller;

        public UserCreditControllerTests()
        {
            _mockUserCreditService = new Mock<IUserCreditService>();
            _mockEcpayService = new Mock<IEcpayService>();

            var application = new UserCreditApplication(
                _mockUserCreditService.Object, _mockEcpayService.Object);

            _controller = new UserCreditController(application);
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithList()
        {
            var testData = new List<UserCredit>
            {
                new() { Id = 1, HostId = 1, UserId = 100, Amount = 500 },
                new() { Id = 2, HostId = 1, UserId = 200, Amount = 300 }
            };
            _mockUserCreditService
                .Setup(s => s.Query())
                .Returns(testData.AsAsyncQueryable());

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var items = Assert.IsAssignableFrom<List<UserCredit>>(okResult.Value);
            Assert.Equal(2, items.Count);
        }

        [Fact]
        public async Task GetById_ExistingId_ReturnsOk()
        {
            var testData = new List<UserCredit>
            {
                new() { Id = 1, HostId = 1, UserId = 100, Amount = 500 },
                new() { Id = 2, HostId = 1, UserId = 200, Amount = 300 }
            };
            _mockUserCreditService
                .Setup(s => s.Query())
                .Returns(testData.AsAsyncQueryable());

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var item = Assert.IsType<UserCredit>(okResult.Value);
            Assert.Equal(1, item.Id);
        }

        [Fact]
        public async Task GetById_NonExistingId_ReturnsNotFound()
        {
            var testData = new List<UserCredit>();
            _mockUserCreditService
                .Setup(s => s.Query())
                .Returns(testData.AsAsyncQueryable());

            var result = await _controller.GetById(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateTradeToken_Success_ReturnsOk()
        {
            var request = new CreateTradeTokenRequest
            {
                UserId = 1,
                PointAmount = "100",
                Email = "test@example.com"
            };
            _mockEcpayService
                .Setup(s => s.CreateTradeTokenAsync(It.IsAny<EcpayViewModel>()))
                .ReturnsAsync(new AppResult<EcpayTokenResponse>
                {
                    Success = true,
                    Code = 1,
                    Message = "Success",
                    Data = new EcpayTokenResponse
                    {
                        MerchantID = "M001",
                        MerchantTradeNo = "T001",
                        Token = "token123",
                        TokenExpireDate = "2026-12-31"
                    }
                });

            var result = await _controller.CreateTradeToken(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var appResult = Assert.IsType<AppResult<CreateTradeTokenResponse>>(okResult.Value);
            Assert.True(appResult.Success);
            Assert.Equal("token123", appResult.Data!.Token);
        }

        [Fact]
        public async Task CreateTradeToken_Failure_ReturnsBadRequest()
        {
            var request = new CreateTradeTokenRequest { UserId = 1 };
            _mockEcpayService
                .Setup(s => s.CreateTradeTokenAsync(It.IsAny<EcpayViewModel>()))
                .ReturnsAsync(new AppResult<EcpayTokenResponse>
                {
                    Success = false,
                    Code = 0,
                    Message = "Error",
                    Data = new EcpayTokenResponse
                    {
                        MerchantID = "",
                        MerchantTradeNo = "",
                        Token = "",
                        TokenExpireDate = ""
                    }
                });

            var result = await _controller.CreateTradeToken(request);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
