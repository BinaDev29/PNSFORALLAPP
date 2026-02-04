using Xunit;
using Moq;
using Application.Common.Interfaces;
using Application.Services;
using Domain.Models;
using FluentAssertions;
using System.Threading.Tasks;

namespace PNS.Tests
{
    public class NotificationTests
    {
        [Fact]
        public void SampleTest_AlwaysPasses()
        {
            // Arrange
            var x = 1;
            var y = 1;

            // Act
            var result = x + y;

            // Assert
            result.Should().Be(2);
        }

        [Fact]
        public async Task GetCurrentUserService_ShouldReturnUserId()
        {
             // This is just a placeholder to show how to structure tests
            var mockUserService = new Mock<ICurrentUserService>();
            mockUserService.Setup(s => s.UserId).Returns("test-user-id");

            var userId = mockUserService.Object.UserId;

            userId.Should().Be("test-user-id");
        }
    }
}
