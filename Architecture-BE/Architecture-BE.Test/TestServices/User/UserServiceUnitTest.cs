using Architecture_BE.Business.Services;
using Architecture_BE.DAL.Entities;
using Architecture_BE.DAL.UnitOfWork;
using Architecture_BE.Models.Dto;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Architecture_BE.Test.TestServices
{
    public class UserServiceUnitTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly IUserService _userService;

        public UserServiceUnitTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _userService = new UserService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Active_ExistingGuidPassed_ReturnsTrueResult()
        {
            // Arrange
            Guid testGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");

            _mockUnitOfWork.Setup(x => x.UserRepository.GetUserActiveById(testGuid))
                           .ReturnsAsync(new User
                           {
                               Id = testGuid,
                               UserName = "Test"
                           });

            _mockUnitOfWork.Setup(x => x.CommitAsync())
                           .ReturnsAsync(true);

            // Act
            bool okResult = await _userService.Active(testGuid);

            // Assert
            _mockUnitOfWork.Verify(x => x.UserRepository.Delete(It.IsAny<User>()), Times.Once);

            Assert.True(okResult);
        }

        [Fact]
        public async Task Active_ExistingGuidPassed_CommitAysncFail_ReturnsFalseResult()
        {
            // Arrange
            Guid testGuid = Guid.NewGuid();

            _mockUnitOfWork.Setup(x => x.UserRepository.GetUserActiveById(testGuid))
                       .ReturnsAsync(new User
                       {
                           Id = testGuid,
                           UserName = "Test"
                       });

            _mockUnitOfWork.Setup(x => x.CommitAsync())
                           .ReturnsAsync(false);

            // Act
            bool okResult = await _userService.Active(testGuid);

            // Assert
            _mockUnitOfWork.Verify(x => x.UserRepository.Delete(It.IsAny<User>()), Times.Once);

            Assert.False(okResult);
        }

        [Fact]
        public async Task Active_UserIsNull_ReturnsFalseResult()
        {
            // Arrange
            Guid testGuid = Guid.NewGuid();

            _mockUnitOfWork.Setup(x => x.UserRepository.GetUserActiveById(testGuid))
                       .ReturnsAsync((User)null);

            //_mockUnitOfWork.Setup(x => x.CommitAsync())
            //               .ReturnsAsync(false);

            // Act
            bool okResult = await _userService.Active(testGuid);

            // Assert
             _mockUnitOfWork.Verify(x => x.UserRepository.Delete(It.IsAny<User>()), Times.Never);

            Assert.False(okResult);
        }

        [Fact]
        public async Task GetUsers_UserExsits_ReturnsUserDtoListResult()
        {
            // Arrange
            List<User> users = new List<User>()
            {
                new User() { Id = Guid.NewGuid(), UserName = "Test"},
                new User() { Id = Guid.NewGuid(), UserName = "Test2"}
            };

            _mockUnitOfWork.Setup(x => x.UserRepository.GetUsers())
                           .ReturnsAsync(users);


            // Act
            var result = await _userService.GetUsers();

            // Assert
            Assert.IsType<List<UserDto>>(result);
        }

        [Fact]
        public async Task GetUsers_UsersIsEmpty_ReturnsUserDtoListResult()
        {
            // Arrange
            List<User> users = new List<User>();

            _mockUnitOfWork.Setup(x => x.UserRepository.GetUsers())
                           .ReturnsAsync(users);


            // Act
            var result = await _userService.GetUsers();

            // Assert
            Assert.IsType<List<UserDto>>(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task UserExsits_ReturnsResult(bool expected)
        {
            // Arrange
            string userName = "Test";

            _mockUnitOfWork.Setup(x => x.UserRepository.UserExists(userName))
                           .ReturnsAsync(expected);


            // Act
            var result = await _userService.UserExists(userName);

            // Assert
            Assert.Equal<bool>(result, expected);
        }

        [Fact]
        public async Task VerifyAccount_UserExsits_ReturnsUserDtoResult()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.UserRepository.VerifyAccount(It.IsAny<string>(), It.IsAny<string>()))
                           .ReturnsAsync(new User
                           {
                               Id = Guid.NewGuid()
                           });

            // Act
            LoginUserDto loginUserDto = new LoginUserDto
            {
                UserName = "Test",
                Password = "123"
            };

            var result = await _userService.VerifyAccount(loginUserDto);

            // Assert
            Assert.IsType<UserDto>(result);
        }

        [Fact]
        public async Task VerifyAccount_UserNotExsits_ReturnsNullResult()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.UserRepository.VerifyAccount(It.IsAny<string>(), It.IsAny<string>()))
                           .ReturnsAsync((User)null);

            // Act
            LoginUserDto loginUserDto = new LoginUserDto
            {
                UserName = "Test",
                Password = "123"
            };

            var result = await _userService.VerifyAccount(loginUserDto);

            // Assert
            Assert.Null(result);
        }
    }
}
