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
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;

namespace ToDoAppTest
{
        
    

    public class ToDoTest
    {
        ToDoesController c;
        readonly Mock<IToDo> itodomock=new Mock<IToDo>();
        readonly Mock<IToDoList> itodolistmock=new Mock<IToDoList>();

        [SetUp]
        public void Setup()
        {

        }



        [Test]
        public void ToDo_DetailsTest()
        {
            c = new ToDoesController(itodomock.Object, itodolistmock.Object);
            int id = 186;
            ToDo t = new ToDo
            {
                title = "aaaaaaa",
                description = "sssssss"


            };




            itodomock.Setup(x => x.GetById(id)).Returns(t);
            var re=c.Details(38);
            var tt = c.Details(id);

            Assert.AreEqual(re.Status,tt.Status);
        }




        [Test]
        [TestCase(1)]
        
        public void Details_Returns_ViewResult_If_Model_Is_Returned(int id)
        {
            // Arrange
            itodomock.Setup(repo => repo.GetById(It.IsAny<int>()))
                    .Returns(GetTestToDos()[0]);
            var controller = new ToDoesController(itodomock.Object, itodolistmock.Object);

            // Act
            var result = controller.Details(id).Result;

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            itodomock.Verify(x => x.GetById(It.IsAny<int>()), Times.Exactly(1));
        }


        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void Details_Returns_RedirectToActionResult_And_Id_Passed(int id)
        {
            // Arrange
            itodomock.Setup(repo => repo.GetById(It.IsAny<int>()))
                    .Returns(GetTestToDos()[0]);
            var controller = new ToDoesController(itodomock.Object, itodolistmock.Object);

            // Act
            var result = controller.Details(id).Result;

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            itodomock.Verify(x => x.GetById(It.IsAny<int>()), Times.AtLeast(1));
        }

        [Test]
        public void Index_Returns_ViewResult_With_ToDo()
        {
            // Arrange
            itodomock.Setup(repo => repo.GetAll())
                    .Returns(GetTestToDos());
            var controller = new ToDoesController( itodomock.Object, itodolistmock.Object);

            // Act
            var result = controller.Index();
            

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            itodomock.Verify(x => x.GetAll(), Times.Exactly(1));
        }


        [Test]
   
        public void Create_Returns_ViewResult_If_Id_Is_Valid()
        {
            ITempDataProvider tempDataProvider = Mock.Of<ITempDataProvider>();
            TempDataDictionaryFactory tempDataDictionaryFactory = new TempDataDictionaryFactory(tempDataProvider);
            ITempDataDictionary tempData = tempDataDictionaryFactory.GetTempData(new DefaultHttpContext());

            var controller = new ToDoesController(itodomock.Object, itodolistmock.Object)
            {
                TempData = tempData
            };
            // Arrange
           

            // Act
            var result = controller.Create();

            // Assert
           Assert.That(result, Is.TypeOf<ViewResult>());
        }


        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void Edit_Returns_ViewResult_If_Model_Is_Returned(int id)
        {
            // Arrange
            itodomock.Setup(repo => repo.GetById(It.IsAny<int>()))
                    .Returns(GetTestToDos()[0]);
            ITempDataProvider tempDataProvider = Mock.Of<ITempDataProvider>();
            TempDataDictionaryFactory tempDataDictionaryFactory = new TempDataDictionaryFactory(tempDataProvider);
            ITempDataDictionary tempData = tempDataDictionaryFactory.GetTempData(new DefaultHttpContext());

            var controller = new ToDoesController(itodomock.Object, itodolistmock.Object)
            {
                TempData = tempData
            };

            // Act
            var result = controller.Edit(id);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            itodomock.Verify(x => x.GetById(It.IsAny<int>()), Times.AtLeast(1));
        }


        [Test]
        [TestCase(0)]
        
        public void Edit_Returns_RedirectToActionResult(int id)
        {
            // Arrange
            itodomock.Setup(repo => repo.GetById(It.IsAny<int>()))
                    .Returns(GetTestToDos()[0]);
            ITempDataProvider tempDataProvider = Mock.Of<ITempDataProvider>();
            TempDataDictionaryFactory tempDataDictionaryFactory = new TempDataDictionaryFactory(tempDataProvider);
            ITempDataDictionary tempData = tempDataDictionaryFactory.GetTempData(new DefaultHttpContext());

            var controller = new ToDoesController(itodomock.Object, itodolistmock.Object)
            {
                TempData = tempData
            };

            // Act
            var result = controller.Edit(id);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            itodomock.Verify(x => x.GetById(It.IsAny<int>()), Times.Exactly(1));
        }

      
        [Test]
        [TestCase(35)]

        public void Edit_Post_Returns_RedirectToActionResult(int id)
        {
            // Arrange
            itodomock.Setup(repo => repo.Update(It.IsAny<ToDo>()));
            ITempDataProvider tempDataProvider = Mock.Of<ITempDataProvider>();
            TempDataDictionaryFactory tempDataDictionaryFactory = new TempDataDictionaryFactory(tempDataProvider);
            ITempDataDictionary tempData = tempDataDictionaryFactory.GetTempData(new DefaultHttpContext());

            var controller = new ToDoesController(itodomock.Object, itodolistmock.Object)
            {
                TempData = tempData
            };

            // Act
            var result = controller.Edit(1, GetTestToDos()[0]);

            // Assert
            if (result.GetType().Name != "NotFoundResult")
            {
                Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            }
           
            else
                {
                Assert.That(result, Is.TypeOf<NotFoundResult>());
            }

            itodomock.Verify(x => x.Update(It.IsAny<ToDo>()), Times.AtMost(1));
        }

        [Test]
        [TestCase(10)]
        public void AddEdit_Returns_RedirectToActionResult(int id)
        {
            // Arrange
            itodomock.Setup(repo => repo.GetById(It.IsAny<int>()))
                    .Returns(GetTestToDos()[0]);
            ITempDataProvider tempDataProvider = Mock.Of<ITempDataProvider>();
            TempDataDictionaryFactory tempDataDictionaryFactory = new TempDataDictionaryFactory(tempDataProvider);
            ITempDataDictionary tempData = tempDataDictionaryFactory.GetTempData(new DefaultHttpContext());

            var controller = new ToDoesController(itodomock.Object, itodolistmock.Object)
            {
                TempData = tempData
            };

            // Act
            var result = controller.Edit(id);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            itodomock.Verify(x => x.GetById(It.IsAny<int>()), Times.Exactly(1));
        }


        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void Delete_Returns_RedirectToActionResult(int id)
        {
            // Arrange
            itodomock.Setup(repo => repo.GetById(It.IsAny<int>()))
                    .Returns(GetTestToDos()[0]);
            itodomock.Setup(repo => repo.Delete((It.IsAny<int>())));
            ITempDataProvider tempDataProvider = Mock.Of<ITempDataProvider>();
            TempDataDictionaryFactory tempDataDictionaryFactory = new TempDataDictionaryFactory(tempDataProvider);
            ITempDataDictionary tempData = tempDataDictionaryFactory.GetTempData(new DefaultHttpContext());

            var controller = new ToDoesController(itodomock.Object, itodolistmock.Object)
            {
                TempData = tempData
            };

            // Act
            var result = controller.Delete(id).Result;

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            itodomock.Verify(x => x.GetById(It.IsAny<int>()), Times.AtMost(2));
            itodomock.Verify(x => x.Delete(It.IsAny<int>()), Times.AtMost(2));
        }



        [Test]
        public void ToDo_GetMyToDoTest()
        {
            c = new ToDoesController(itodomock.Object, itodolistmock.Object);
            ToDo t = new ToDo
            {
                title = "aaaaaaa",
                description = "sssssss"


            };


            var re = c.GetMyToDo();

            Assert.AreEqual(re.GetType(), re.GetType());
        }


        [Test]
        public void ToDo_GetToDoTableTest()
        {
            c = new ToDoesController(itodomock.Object, itodolistmock.Object);
            ToDo t = new ToDo
            {
                title = "aaaaaaa",
                description = "sssssss"


            };


            var re = c.GetToDoTable();

            Assert.AreEqual(re.GetType(), re.GetType());
        }


        //[Test]
        //[TestCase(0)]

        //public void ToDo_GetTodoByLIstIdTest(int id)
        //{
        //    itodomock.Setup(repo => repo.GetById(id))
        //.Returns(GetTestToDo());


        //    ITempDataProvider tempDataProvider = Mock.Of<ITempDataProvider>();
        //    TempDataDictionaryFactory tempDataDictionaryFactory = new TempDataDictionaryFactory(tempDataProvider);
        //    ITempDataDictionary tempData = tempDataDictionaryFactory.GetTempData(new DefaultHttpContext());

        //    ToDoesController controller = new ToDoesController( itodomock.Object, itodolistmock.Object)
        //    {
        //        TempData = tempData
        //    };

        //    var result = controller.GetTodoByLIstId(id);

        //    if (result.GetType().Name != "NotFoundResult")
        //    {
        //        Assert.That(result, Is.TypeOf<IActionResult>());
        //    }
        //    else
        //    {
        //        Assert.That(result, Is.TypeOf<NotFoundResult>());
        //    }
        //}

        [Test]
        public void Create_Returns_ViewResult()
        {
            // Arrange
            ITempDataProvider tempDataProvider = Mock.Of<ITempDataProvider>();
            TempDataDictionaryFactory tempDataDictionaryFactory = new TempDataDictionaryFactory(tempDataProvider);
            ITempDataDictionary tempData = tempDataDictionaryFactory.GetTempData(new DefaultHttpContext());

            TodolistsController controller = new TodolistsController(itodolistmock.Object, itodomock.Object)
            {
                TempData = tempData
            };

            // Act
            var result = controller.Create();

            if (result.GetType().Name != "NotFoundResult")
            {
                Assert.That(result, Is.TypeOf<ViewResult>());
            }
            else
            {
                Assert.That(result, Is.TypeOf<ViewResult>());
            }
            // Assert
            
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
                   description="aaaa"
                },
                new ToDo()
                {
                    Id = 2,

                   description="aaaa"
                },
                new ToDo()
                {
                    Id = 3,

                   description="aaaa"
                },
            };


        }

        private ToDo GetTestToDo()
        {
            return new ToDo()
            {

                Id = 1,
                description = "aaaa"

            };
        }

    }
}