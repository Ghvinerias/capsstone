using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using todoapp;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ToDoList.DataModel;
using System.IO;
using todoapp.Controllers;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Interfaces;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;

namespace ToDoAppTest.ToDoListTest
{
    class ToDoLitsTets

    {
        
        private Mock<IToDo> itodomock;
        private Mock<IToDoList> itodolistmock;

        [SetUp]
        public void Setup()
        {
            itodomock = new Mock<IToDo>();
            itodolistmock = new Mock<IToDoList>();
        }



        [Test]
        public void Index_Returns_ViewResult_With_ToDoLists()
        {
            // Arrange
            itodolistmock.Setup(repo => repo.GetAll())
                    .Returns(GetTestToDoLists());
            var controller = new TodolistsController(itodolistmock.Object, itodomock.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            itodolistmock.Verify(x => x.GetAll(), Times.Exactly(1));
        }

        [Test]
        public void Index_Returns_ViewResult_With_GetCountByStatus()
        {
            // Arrange
            itodolistmock.Setup(repo => repo.GetAll())
                    .Returns(GetTestToDoLists());
            var controller = new TodolistsController(itodolistmock.Object, itodomock.Object);

            // Act
            var result = controller.GetCountByStatus();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());

        }

        
        [Test]
        [TestCase(35)]
        [TestCase(14)]
        [TestCase(38)]
        [TestCase(36)]
        [TestCase(37)]
        public void Index_Returns_ViewResult_With_Details(int id)
        {
            // Arrange
            itodomock.Setup(repo => repo.GetById(id))
                    .Returns(GetTestToDo());
    
            ITempDataProvider tempDataProvider = Mock.Of<ITempDataProvider>();
            TempDataDictionaryFactory tempDataDictionaryFactory = new TempDataDictionaryFactory(tempDataProvider);
            ITempDataDictionary tempData = tempDataDictionaryFactory.GetTempData(new DefaultHttpContext());

            TodolistsController controller = new TodolistsController(itodolistmock.Object, itodomock.Object)
            {
                TempData = tempData
            };

            // Act
            var result = controller.Details(id);
            if (result.GetType().Name != "NotFoundResult")
            { 
                Assert.That(result, Is.TypeOf<IActionResult>()); 
            }
            else
            {
                Assert.That(result, Is.TypeOf<NotFoundResult>());
            }
            // Assert


        }


        [Test]
        public void Create_Returns_ViewResult()
        {
            // Arrange
            TodolistsController controller = new TodolistsController(itodolistmock.Object, itodomock.Object);
 
            // Act
             var result = controller.Create();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
        }


        [Test]
        public void Create_Post_Returns_RedirectToActionResult()
        {
            // Arrange
            itodolistmock.Setup(repo => repo.Insert(It.IsAny<Todolist>()));
            ITempDataProvider tempDataProvider = Mock.Of<ITempDataProvider>();
            TempDataDictionaryFactory tempDataDictionaryFactory = new TempDataDictionaryFactory(tempDataProvider);
            ITempDataDictionary tempData = tempDataDictionaryFactory.GetTempData(new DefaultHttpContext());

            TodolistsController controller = new TodolistsController(itodolistmock.Object, itodomock.Object)
            {
                TempData = tempData
            };

            // Act
            var result = controller.Create(GetTestToDoLists()[0]);

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            itodolistmock.Verify(x => x.Insert(It.IsAny<Todolist>()), Times.Exactly(1));
        }


        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void Edit_Returns_ViewResult_If_Model_Is_Returned(int id)
        {
            // Arrange
            itodolistmock.Setup(repo => repo.Insert(It.IsAny<Todolist>()));
            itodolistmock.Setup(repo => repo.GetById(It.IsAny<int>()))
                    .Returns(GetTestToDoLists()[0]);
            ITempDataProvider tempDataProvider = Mock.Of<ITempDataProvider>();
            TempDataDictionaryFactory tempDataDictionaryFactory = new TempDataDictionaryFactory(tempDataProvider);
            ITempDataDictionary tempData = tempDataDictionaryFactory.GetTempData(new DefaultHttpContext());

            TodolistsController controller = new TodolistsController(itodolistmock.Object, itodomock.Object)
            {
                TempData = tempData
            };

            // Act
            var result = controller.Edit(id);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            itodolistmock.Verify(x => x.GetById(It.IsAny<int>()), Times.Exactly(1));
        }

        [Test]
        [TestCase(14)]
        [TestCase(35)]
        public void CopyList_Returns_ViewResult_If_Model_Is_Returned(int id)
        {
            // Arrange
            itodolistmock.Setup(repo => repo.Insert(It.IsAny<Todolist>()));
            itodolistmock.Setup(repo => repo.GetById(It.IsAny<int>()))
                    .Returns(GetTestToDoLists()[0]);
            ITempDataProvider tempDataProvider = Mock.Of<ITempDataProvider>();
            TempDataDictionaryFactory tempDataDictionaryFactory = new TempDataDictionaryFactory(tempDataProvider);
            ITempDataDictionary tempData = tempDataDictionaryFactory.GetTempData(new DefaultHttpContext());

            TodolistsController controller = new TodolistsController(itodolistmock.Object, itodomock.Object)
            {
                TempData = tempData
            };

            // Act
            var result = controller.CopyList(id);

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            itodolistmock.Verify(x => x.GetById(It.IsAny<int>()), Times.Exactly(1));
        }





        [Test]
        [TestCase(14)]
        [TestCase(35)]
        public void Delete_Returns_RedirectToActionResult(int id)
        {
            // Arrange
            itodolistmock.Setup(repo => repo.GetById(It.IsAny<int>()))
                    .Returns(GetTestToDoLists()[0]);
            itodolistmock.Setup(repo => repo.Delete(It.IsAny<int>()));

            ITempDataProvider tempDataProvider = Mock.Of<ITempDataProvider>();
            TempDataDictionaryFactory tempDataDictionaryFactory = new TempDataDictionaryFactory(tempDataProvider);
            ITempDataDictionary tempData = tempDataDictionaryFactory.GetTempData(new DefaultHttpContext());

            TodolistsController controller = new TodolistsController(itodolistmock.Object, itodomock.Object)
            {
                TempData = tempData
            };

            // Act
            var result = controller.Delete(id);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            itodolistmock.Verify(x => x.GetById(It.IsAny<int>()), Times.AtMost(2));
            itodolistmock.Verify(x => x.Delete(It.IsAny<int>()), Times.AtMost(2));
            
        }



        private List<Todolist> GetTestToDoLists()
        {
            return new List<Todolist>()
            {
                new Todolist()
                {
                    id = 1,
                    Name="aaa"
                },

                new Todolist()
                {
                    id = 1,
                    Name="aaa"
                },
                             new Todolist()
                {
                    id = 1,
                    Name="aaa"
                },
            };
        }

        private List<ToDo> GetTestToDos()
        {
            return new List<ToDo>()
            {
                new ToDo()
                {
                    Id = 1,
                   description="desc1",
                   title="title1"
                },
                new ToDo()
                {
                    Id = 2,

                   description="desc1",
                     title="title1"
                },
                new ToDo()
                {
                    Id = 3,
                    
                      title="title1"
                },
            };


        }

        private ToDo GetTestToDo()
        {
            return new ToDo()
            {

                Id = 1,
                description = "desc1",
                title="title1"
                
                

            };
        }
    }
}

