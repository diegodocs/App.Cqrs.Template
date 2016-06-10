using App.Cqrs.Core.Bus;
using App.Cqrs.Template.Application.Command;
using App.Cqrs.Template.EventSource.Core.Repository;
using App.Cqrs.Template.Infrastructure.Bus;
using App.Cqrs.Template.Test.Unit.Infrastructure;
using App.Template.Domain.Model;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Reflection;

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

            var types = AppDomain
                            .CurrentDomain
                            .GetAssemblies()
                            .SelectMany(n => n.GetReferencedAssemblies())
                            .Select(n => Assembly.Load(n))
                            .SelectMany(n => n.GetTypes())
                                            .Where(n => n.Namespace != null
                                            && n.Namespace.StartsWith("App.")
                                                && (n.Name.EndsWith("EventHandler") || n.Name.EndsWith("CommandHandler")))
                             .ToList();

            types.ForEach(n => { builder.RegisterType(n).AsImplementedInterfaces(); });

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
            var expectedId = Guid.NewGuid();
            var name = "Crunch Chocolate";
            var newName = "Buballo Chiclete";

            var bus = container.Resolve<IBus>();
            var queryService = container.Resolve<IRepositoryForEventSource<InventoryItem>>();

            // Act
            bus.Dispatch(new CreateInventoryItemCommand(expectedId, name));
            bus.Dispatch(new RenameInventoryItemCommand(expectedId, newName, 1));

            // Assert            
            Assert.AreEqual(newName, queryService.GetById(expectedId).Name);
        }
    }
}