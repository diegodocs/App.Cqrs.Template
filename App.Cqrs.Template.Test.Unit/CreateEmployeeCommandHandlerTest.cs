using App.Cqrs.Core.Command;
using App.Cqrs.Core.Event;
using App.Cqrs.Template.ApplicationSvc.Command;
using App.Cqrs.Template.Core.Repository;
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
    public class CreateEmployeeCommandHandlerTest
    {
        private IContainer container;

        [TestInitialize]
        public void TestInitialize()
        {
            var builder = new ContainerBuilder();

            builder.RegisterGeneric(typeof(RepositoryInMemory<>)).AsImplementedInterfaces().SingleInstance();
            builder.RegisterGeneric(typeof(EventPublisher<>)).As(typeof(IEventPublisher<>));

            var types = AppDomain
                            .CurrentDomain
                            .GetAssemblies().SelectMany(n=>n.GetReferencedAssemblies()).Select(n=> Assembly.Load(n))
                            .SelectMany(n => n.GetTypes())
                                            .Where(n => n.Namespace != null && n.Namespace.StartsWith("App.") && (
                                                n.Name.EndsWith("EventHandler") ||
                                                n.Name.EndsWith("CommandHandler"))).ToList();

            types.ForEach(n => { builder.RegisterType(n).AsImplementedInterfaces(); });

            container = builder.Build();
        }

        [TestMethod]
        public void Execute_EmployeeCreate_NewEmployeeCreated()
        {
            // Arrange
            var expectedNumberOfEmployee = 1;
            var name = "Chuck Norris";
            var command = new EmployeeCreateCommand(name, "Architecture", 10, 200);

            var queryServiceEmployee = container.Resolve<IRepository<Employee>>();
            var handler = container.Resolve<ICommandHandler<EmployeeCreateCommand>>();

            // Act
            handler.Execute(command);

            // Assert
            Assert.AreEqual(expectedNumberOfEmployee, queryServiceEmployee.FindList(x => x.Name == name).Count());
            Assert.AreEqual(name, queryServiceEmployee.All().Single().Name);
        }
    }
}