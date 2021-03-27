using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.API.Controllers;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Domain.Services;

namespace WinterWorkShop.Cinema.Tests.Controllers
{
    [TestClass]
    public class Levi9PaymentControllerTests
    {
        private Mock<ILevi9PaymentService> _paymentService;

        [TestMethod]
        public void Post_Async_Return_Payment_Response_Model_Ok_Result()
        {
            //Arrange
            int expectedStatusCode = 200;
            PaymentResponse paymentResult = new PaymentResponse();
            paymentResult.IsSuccess = true;
            Task<PaymentResponse> responseTask = Task.FromResult(paymentResult);

            _paymentService = new Mock<ILevi9PaymentService>();
            _paymentService.Setup(x => x.MakePayment()).Returns(responseTask);
            Levi9PaymentController levi9PaymentController = new Levi9PaymentController(_paymentService.Object);

            //Act
            var result = levi9PaymentController.Post().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var objectResult = ((OkObjectResult)result).Value;
            PaymentResponseModel model = (PaymentResponseModel)objectResult;

            //Assert
            Assert.IsNotNull(objectResult);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void Post_Async_Return_Payment_Response_Not_SuccessFull_Bad_Request()
        {
            //Arrange
            int expectedStatusCode = 400;
            PaymentResponse paymentResult = new PaymentResponse();
            paymentResult.IsSuccess = false;
            Task<PaymentResponse> responseTask = Task.FromResult(paymentResult);

            _paymentService = new Mock<ILevi9PaymentService>();
            _paymentService.Setup(x => x.MakePayment()).Returns(responseTask);
            Levi9PaymentController levi9PaymentController = new Levi9PaymentController(_paymentService.Object);

            //Act
            var result = levi9PaymentController.Post().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var objectResult = ((BadRequestObjectResult)result).Value;
            PaymentResponseModel model = (PaymentResponseModel)objectResult;

            //Assert
            Assert.IsNotNull(objectResult);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, ((BadRequestObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void Post_Async_PaymentCreation_Error_Return_Bad_Request()
        {
            //Arrange
            int expectedStatusCode = 400;
            PaymentResponse paymentResult = new PaymentResponse();
            paymentResult.Message = "Connection error.";
            Task<PaymentResponse> responseTask = Task.FromResult(paymentResult);

            _paymentService = new Mock<ILevi9PaymentService>();
            _paymentService.Setup(x => x.MakePayment()).Returns(responseTask);
            Levi9PaymentController levi9PaymentController = new Levi9PaymentController(_paymentService.Object);

            //Act
            var result = levi9PaymentController.Post().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var objectResult = ((BadRequestObjectResult)result).Value;
            ErrorResponseModel model = (ErrorResponseModel)objectResult;

            //Assert
            Assert.IsNotNull(objectResult);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, ((BadRequestObjectResult)result).StatusCode);
        }

    }
}
