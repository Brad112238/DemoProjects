using Microsoft.AspNetCore.Mvc;
using WebApiDemo.Controllers;
using Xunit;

namespace WebApiDemo.Tests.Controllers
{
    public class HealthControllerTests
    {
        private readonly HealthController _controller;

        public HealthControllerTests()
        {
            _controller = new HealthController();
        }

        [Fact]
        public void Get_ReturnsOkResult()
        {
            var result = _controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Get_ReturnsHealthyStatus()
        {
            var result = _controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = okResult.Value!;

            var statusProp = value.GetType().GetProperty("status");
            var timestampProp = value.GetType().GetProperty("timestamp");

            Assert.NotNull(statusProp);
            Assert.NotNull(timestampProp);
            Assert.Equal("Healthy", statusProp.GetValue(value));
            Assert.NotNull(timestampProp.GetValue(value));
        }

        [Fact]
        public void Get_TimestampIsValidIso8601()
        {
            var result = _controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = okResult.Value!;
            var timestamp = value.GetType().GetProperty("timestamp")!.GetValue(value) as string;

            Assert.True(DateTime.TryParse(timestamp, out _));
        }
    }
}
