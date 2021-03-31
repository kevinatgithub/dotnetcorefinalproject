using AutoMapper;
using FinalProject.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Services;
using System;
using Xunit;

namespace Tests
{
    public class OrdersControllerTest
    {

        private Mock<IOrderService> _orderServiceMock;
        private Mock<IUserStore<IdentityUser>>  _userStoreMock;
        private Mock<UserManager<IdentityUser>> _userManagerMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IAuthorizationService> _authServiceMock;
        private Mock<ILogger<OrdersController>> _loggerMock;
        private OrdersController _subject;
        public OrdersControllerTest()
        {
            SetupDependencies();
            _subject = CreateSubject();
        }

        private void SetupDependencies()
        {
            _orderServiceMock = new Mock<IOrderService>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<OrdersController>>();
            _userStoreMock = new Mock<IUserStore<IdentityUser>>();
            _userManagerMock = new Mock<UserManager<IdentityUser>>(_userStoreMock.Object,
                null, null, null, null, null, null, null, null);
            //_userManagerMock = new Mock<UserManager<IdentityUser>>(null);
        }



        #region Constructor tests

        [Fact]
        public void Instantiation_failed_item_service_null()
        {
            Assert.Throws<ArgumentNullException>(() => new OrdersController(null, _userManagerMock.Object, _mapperMock.Object, _authServiceMock.Object, _loggerMock.Object));
        }
        [Fact]
        public void Instantiation_failed_userManager_null()
        {
            Assert.Throws<ArgumentNullException>(() => new OrdersController(_orderServiceMock.Object, null, _mapperMock.Object, _authServiceMock.Object, _loggerMock.Object));
        }

        [Fact]
        public void Instantiation_failed_mapper_null()
        {
            Assert.Throws<ArgumentNullException>(() => new OrdersController(_orderServiceMock.Object, _userManagerMock.Object, null, _authServiceMock.Object, _loggerMock.Object));
        }

        [Fact]
        public void Instantiation_failed_logger_null()
        {
            Assert.Throws<ArgumentNullException>(() => new OrdersController(_orderServiceMock.Object, _userManagerMock.Object, _mapperMock.Object, _authServiceMock.Object, null));
        }

        #endregion








        private OrdersController CreateSubject()
        {
            var service = new OrdersController(_orderServiceMock.Object, _userManagerMock.Object, _mapperMock.Object, _authServiceMock.Object, _loggerMock.Object);
            return service;
        }

    }

}
