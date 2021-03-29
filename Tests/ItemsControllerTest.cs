using AutoMapper;
using FinalProject;
using FinalProject.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Services;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        [Fact]
        public void ItemsController_GetAll_Success()
        {
            _itemServiceMock.Setup(_ => _.GetAll()).Returns(Task.FromResult(createMockItems()));

            var result = (OkObjectResult)_subject.GetAll().Result;
            Assert.True(result.StatusCode == (int)HttpStatusCode.OK);
        }

        [Fact]
        public void ItemsController_GetAll_NullItems_Success()
        {
            _itemServiceMock.Setup(_ => _.GetAll()).Returns(Task.FromResult(createMockNullItems()));

            var result = (OkObjectResult)_subject.GetAll().Result;
            Assert.True(result.StatusCode == (int)HttpStatusCode.OK);
        }

        [Fact]
        public void ItemsController_Get_Success()
        {
            _itemServiceMock.Setup(_ => _.Get(1)).Returns(Task.FromResult(createMockItem()));

            var result = (OkObjectResult)_subject.Get(1).Result;
            var itemDTO = (ItemDTO)result.Value;
            Assert.True(itemDTO.Name == "Sardines");
        }

        #endregion

        private ItemsController CreateSubject()
        {
            var service = new ItemsController(_itemServiceMock.Object, _mapperMock.Object, _loggerMock.Object);
            return service;
        }

        public IList<ItemDTO> createMockItems()
        {
            return new List<ItemDTO>()
            {
                new ItemDTO { Id = 1, Name = "Sardines", Stocks =  5, UnitPrice = 30.0m },
                new ItemDTO { Id = 2, Name = "Lucky Me", Stocks =  4, UnitPrice = 14m }
            };
        }

        public IList<ItemDTO> createMockNullItems()
        {
            return new List<ItemDTO>()
            {
            };
        }

        public ItemDTO createMockItem()
        {
            return new ItemDTO { Id = 1, Name = "Sardines", Stocks = 5, UnitPrice = 30.0m };
        }
    }

}
