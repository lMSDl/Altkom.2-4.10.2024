using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace Shop.Test
{
    public class OrderProcessorTest
    {
        [Fact]
        public void ProcessOrder_StockIsInsufficient_False()
        {
            // Arrange
            var mockInventoryService = new Mock<IInventoryService>();
            var stubOrderRepository = new Mock<IOrderRepository>();
            var stubPaymentService = new Mock<IPaymentService>();

            var orderProcessor = new OrderProcessor(stubOrderRepository.Object, mockInventoryService.Object, stubPaymentService.Object);

            var fixture = new Fixture();
            var order = fixture.Create<Order>();
            var cardNumber = fixture.Create<string>();
            var lastItem = order.Items.Last();
            var otherItems = order.Items.Take(order.Items.Count - 1).ToList();

            mockInventoryService.Setup(s => s.CheckStock(It.IsIn(otherItems.Select(xx => xx.ProductId)), It.IsIn(otherItems.Select(xx => xx.Quantity)))).Returns(true).Verifiable(Times.Exactly(otherItems.Count));
            mockInventoryService.Setup(s => s.CheckStock(lastItem.ProductId, lastItem.Quantity)).Returns(false).Verifiable(Times.Once);

            // Act
            var result = orderProcessor.ProcessOrder(order, cardNumber);

            // Assert
            using (new AssertionScope())
            {
                mockInventoryService.Verify();
                result.Should().BeFalse();
            }
        }

        [Fact]
        public void ProcessOrder_PaymentFailed_False()
        {
            // Arrange
            var stubInventoryService = new Mock<IInventoryService>();
            var stubOrderRepository = new Mock<IOrderRepository>();
            var mockPaymentService = new Mock<IPaymentService>();

            var orderProcessor = new OrderProcessor(stubOrderRepository.Object, stubInventoryService.Object, mockPaymentService.Object);

            var fixture = new Fixture();
            var order = fixture.Create<Order>();
            var cardNumber = fixture.Create<string>();

            stubInventoryService.Setup(s => s.CheckStock(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
            mockPaymentService.Setup(x => x.ProcessPayment(cardNumber, It.IsAny<decimal>())).Returns(false).Verifiable(Times.Once);

            // Act
            var result = orderProcessor.ProcessOrder(order, cardNumber);

            // Assert
            using (new AssertionScope())
            {
                stubInventoryService.Verify();
                result.Should().BeFalse();
            }
        }

        [Fact]
        public void ProcessOrder_ConditionsAreMet_True()
        {
            // Arrange
            var mockInventoryService = new Mock<IInventoryService>();
            var mockOrderRepository = new Mock<IOrderRepository>();
            var stubPaymentService = new Mock<IPaymentService>();

            var orderProcessor = new OrderProcessor(mockOrderRepository.Object, mockInventoryService.Object, stubPaymentService.Object);

            var fixture = new Fixture();
            var order = fixture.Create<Order>();
            var cardNumber = fixture.Create<string>();

            mockInventoryService.Setup(s => s.CheckStock(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
            stubPaymentService.Setup(x => x.ProcessPayment(cardNumber, It.IsAny<decimal>())).Returns(true);

            mockInventoryService.Setup(x => x.ReserveStock(It.IsIn(order.Items.Select(xx => xx.ProductId)), It.IsIn(order.Items.Select(xx => xx.Quantity)))).Verifiable(Times.Exactly(order.Items.Count));
            mockOrderRepository.Setup(x => x.SaveOrder(order)).Verifiable(Times.Once);

            // Act
            var result = orderProcessor.ProcessOrder(order, cardNumber);

            // Assert
            using (new AssertionScope())
            {
                mockInventoryService.Verify();
                result.Should().BeTrue();
            }
        }


    }
}