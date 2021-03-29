using AutoMapper;
using FinalProject;
using FinalProject.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class ItemsControllerTest
    {
        private Mock<IItemService> _itemServiceMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<ItemsController>> _loggerMock;
        private ItemsController _subject;
        public ItemsControllerTest()
        {
            SetupDependencies();
            _subject = CreateSubject();
        }

        private void SetupDependencies()
        {
            _itemServiceMock = new Mock<IItemService>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<ItemsController>>();
        }


        #region Constructor tests

        [Fact]
        public void Instantiation_failed_item_service_null()
        {
            Assert.Throws<ArgumentNullException>(() => new ItemsController(null, _mapperMock.Object, _loggerMock.Object));
        }

        [Fact]
        public void Instantiation_failed_mapper_null()
        {
            Assert.Throws<ArgumentNullException>(() => new ItemsController(_itemServiceMock.Object, null, _loggerMock.Object));
        }

        [Fact]
        public void Instantiation_failed_logger_null()
        {
            Assert.Throws<ArgumentNullException>(() => new ItemsController(_itemServiceMock.Object, _mapperMock.Object, null));
        }

        #endregion








        private ItemsController CreateSubject()
        {
            var service = new ItemsController(_itemServiceMock.Object, _mapperMock.Object, _loggerMock.Object);
            return service;
        }

    }

}
