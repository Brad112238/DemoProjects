using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebApiDemo.Application;
using WebApiDemo.Controllers;
using WebApiDemo.Interfaces;
using WebApiDemo.Models.TestDb;
using WebApiDemo.Services;
using Xunit;

namespace WebApiDemo.Tests.Controllers
{
    public class UserCreditControllerTests : IDisposable
    {
        private readonly TestContext _dbContext;
        private readonly Mock<IEcpayService> _mockEcpayService;
        private readonly UserCreditController _controller;

        public UserCreditControllerTests()
        {
            // 建立 In-Memory Database（每個測試用獨立的 DB 名稱）
            var options = new DbContextOptionsBuilder<TestContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new TestContext(options);

            // 用真的 Service + 真的 DbContext
            var userCreditService = new UserCreditService(_dbContext);
            var userTopUpService = new UserTopUpService(_dbContext);

            _mockEcpayService = new Mock<IEcpayService>();

            var application = new UserCreditApplication(userCreditService, userTopUpService, _mockEcpayService.Object);
            _controller = new UserCreditController(application);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        [Fact]
        public async Task GetAll_EmptyDb_ReturnsEmptyList()
        {
            // DB 是空的，不用塞資料

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var items = Assert.IsAssignableFrom<List<UserCredit>>(okResult.Value);
            Assert.Empty(items);
        }

        [Fact]
        public async Task GetAll_WithData_ReturnsAllItems()
        {
            // Arrange — 塞兩筆真實資料進 In-Memory DB
            _dbContext.UserCredits.AddRange(
                new UserCredit { Id = 1, HostId = 1, UserId = 100, Amount = 500, CreateTime = DateTime.Now },
                new UserCredit { Id = 2, HostId = 1, UserId = 200, Amount = 300, CreateTime = DateTime.Now }
            );
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var items = Assert.IsAssignableFrom<List<UserCredit>>(okResult.Value);
            Assert.Equal(2, items.Count);
        }

        [Fact]
        public async Task GetById_ExistingId_ReturnsCorrectItem()
        {
            // Arrange
            _dbContext.UserCredits.Add(
                new UserCredit { Id = 1, HostId = 1, UserId = 100, Amount = 500, CreateTime = DateTime.Now }
            );
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var item = Assert.IsType<UserCredit>(okResult.Value);
            Assert.Equal(1, item.Id);
            Assert.Equal(100, item.UserId);
            Assert.Equal(500, item.Amount);
        }

        [Fact]
        public async Task GetById_NonExistingId_ReturnsNotFound()
        {
            // DB 是空的，找不到 Id=999

            var result = await _controller.GetById(999);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
