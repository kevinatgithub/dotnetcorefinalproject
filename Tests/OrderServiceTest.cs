using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Repositories;
using Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Tests
{
    public class OrderServiceTests
    {
        private Mock<IMapper> _mapperMock;
        private Mock<IItemRepository> _itemRepositoryMock;
        private Mock<IOrderRepository> _orderRepositoryMock;
        private Mock<ILogger<OrderService>> _loggerMock;
        private OrderService _subject;
        public OrderServiceTests()
        {
            SetupDependencies();
            _subject = CreateSubject();
        }

        private void SetupDependencies()
        {
            _mapperMock = new Mock<IMapper>();
            _itemRepositoryMock = new Mock<IItemRepository>();
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _loggerMock = new Mock<ILogger<OrderService>>();
        }


        #region Constructor tests

        [Fact]
        public void Instantiation_failed_IOrderRepository_null()
        {
            Assert.Throws<ArgumentNullException>(() => new OrderService(null, _itemRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object));
        }

        [Fact]
        public void Instantiation_failed_IItemRepository_null()
        {
            Assert.Throws<ArgumentNullException>(() => new OrderService(_orderRepositoryMock.Object, null, _mapperMock.Object, _loggerMock.Object));
        }

        [Fact]
        public void Instantiation_failed_IMapper_null()
        {
            Assert.Throws<ArgumentNullException>(() => new OrderService(_orderRepositoryMock.Object, _itemRepositoryMock.Object, null, _loggerMock.Object));
        }

        [Fact]
        public void Instantiation_failed_ILogger_null()
        {
            Assert.Throws<ArgumentNullException>(() => new OrderService(_orderRepositoryMock.Object, _itemRepositoryMock.Object, _mapperMock.Object, null));
        }
        #endregion








        private OrderService CreateSubject()
        {
            var service = new OrderService(_orderRepositoryMock.Object, _itemRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
            return service;
        }

    }
}
