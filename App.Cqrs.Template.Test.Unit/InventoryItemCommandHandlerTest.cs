using App.Cqrs.Core.Bus;
using App.Cqrs.Core.Event;
using App.Cqrs.Template.Application.Command;
using App.Cqrs.Template.Application.CommandHandler;
using App.Cqrs.Template.Application.EventHandler;
using App.Cqrs.Template.Application.ReadModel;
using App.Cqrs.Template.Core.Repository;
using App.Cqrs.Template.EventSource.Core.Repository;
using App.Cqrs.Template.Infrastructure.Bus;
using App.Cqrs.Template.Test.Unit.Infrastructure;
using App.Template.Domain.Model;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace App.Cqrs.Template.Test.Unit
{
    [TestClass]
    public class InventoryItemCommandHandlerTest
    {
        private IContainer container;

        [TestInitialize]
        public void TestInitialize()
        {
            var builder = new ContainerBuilder();
            builder.RegisterSource(new Autofac.Features.Variance.ContravariantRegistrationSource());
            builder.RegisterGeneric(typeof(RepositoryInMemory<>)).AsImplementedInterfaces().SingleInstance();
            builder.RegisterGeneric(typeof(RepositoryForEventSource<>)).AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<EventStore>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType(typeof(FakeBus)).AsImplementedInterfaces();
            builder.RegisterType<CreateInventoryItemCommandHandler>().AsImplementedInterfaces();
            builder.RegisterType<EmployeeCreateCommandHandler>().AsImplementedInterfaces();
            builder.RegisterType<RenameInventoryItemCommandHandler>().AsImplementedInterfaces();
            builder.RegisterType<EmployeeUserAccountCreatedEventHandler>().Named<IEventHandler<IEvent>>("EmployeeCreated");
            builder.RegisterType<InventoryItemCreatedEventHandler>().Named<IEventHandler<IEvent>>("InventoryItemCreated");
            builder.RegisterType<InventoryItemRenamedEventHandler>().Named<IEventHandler<IEvent>>("InventoryItemRenamed");

            container = builder.Build();
        }

        [TestMethod]
        public void Execute_InvetoryItemCreate_NewCreated()
        {
            // Arrange
            var expectedId = Guid.NewGuid();
            var name = "Crunch Chocolate";
            var command = new CreateInventoryItemCommand(expectedId, name);

            var bus = container.Resolve<IBus>();
            var queryService = container.Resolve<IRepositoryForEventSource<InventoryItem>>();

            // Act
            bus.Dispatch(command);

            // Assert
            Assert.AreEqual(name, queryService.GetById(expectedId).Name);
        }

        [TestMethod]
        public void Execute_InventoryItemRename_Renamed()
        {
            // Arrange
            var expectedNumberOfObjects = 1;
            var expectedId = Guid.NewGuid();
            var name = "Crunch Chocolate";
            var newName = "Buballo Chiclete";

            var bus = container.Resolve<IBus>();
            var queryService = container.Resolve<IRepositoryForEventSource<InventoryItem>>();
            var queryServiceReadModel = container.Resolve<IRepository<InventoryItemReadModel>>();

            // Act
            bus.Dispatch(new CreateInventoryItemCommand(expectedId, name));
            bus.Dispatch(new RenameInventoryItemCommand(expectedId, newName, 1));

            // Assert
            Assert.AreEqual(newName, queryService.GetById(expectedId).Name);
            Assert.AreEqual(expectedNumberOfObjects, queryServiceReadModel.All().Count());
            Assert.AreEqual(newName, queryServiceReadModel.All().Single().Name);
        }
    }
}