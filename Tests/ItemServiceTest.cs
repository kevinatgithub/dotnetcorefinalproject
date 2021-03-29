using AutoMapper;
using Moq;
using Repositories;
using Services;
using System;
using Xunit;

namespace Tests
{
    public class ItemServiceTests
    {
        private Mock<IMapper> _mapperMock;
        private Mock<IItemRepository> _itemRepositoryMock;
        private ItemService _subject;
        public ItemServiceTests()
        {
            SetupDependencies();
            _subject = CreateSubject();
        }

        private void SetupDependencies()
        {
            _mapperMock = new Mock<IMapper>();
            _itemRepositoryMock = new Mock<IItemRepository>();
        }


        #region Constructor tests
        [Fact]
        public void Instantiation_failed_IItemRepository_null()
        {
            Assert.Throws<ArgumentNullException>(() => new ItemService(null, _mapperMock.Object));
        }

        [Fact]
        public void Instantiation_failed_IMapper_null()
        {
            Assert.Throws<ArgumentNullException>(() => new ItemService(_itemRepositoryMock.Object, null));
        }
        #endregion
        [Fact]
        public void Can_Create_Item()
        {

        }

        [Fact]
        public void Can_Get_Item()
        {

        }

        [Fact]
        public void Can_Delete_Item()
        {

        }

        [Fact]
        public void Can_Update_Item()
        {

        }
        private ItemService CreateSubject()
        {
            var service = new ItemService(_itemRepositoryMock.Object, _mapperMock.Object);
            return service;
        }

    }
}