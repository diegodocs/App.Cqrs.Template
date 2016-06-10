using App.Cqrs.Core.Bus;
using App.Cqrs.Template.ApplicationSvc.Command;
using App.Cqrs.Template.ApplicationSvc.ReadModel;
using App.Cqrs.Template.Core.Repository;
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
    public class CreateEmployeeCommandHandlerTest
    {
        private IContainer container;

        [TestInitialize]
        public void TestInitialize()
        {
            var builder = new ContainerBuilder();

            builder.RegisterGeneric(typeof(RepositoryInMemory<>)).AsImplementedInterfaces().SingleInstance();
            
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
        public void Execute_EmployeeCreate_NewEmployeeCreated()
        {
            // Arrange
            var expectedNumberOfEmployee = 1;
            var name = "Chuck Norris";
            var command = new EmployeeCreateCommand(name, "Architecture", 10, 200);

            var bus = container.Resolve<IBus>();
            var queryServiceEmployee = container.Resolve<IRepository<Employee>>();

            // Act
            bus.Dispatch(command);

            // Assert
            Assert.AreEqual(
                expectedNumberOfEmployee,
                queryServiceEmployee.FindList(x => x.Name == name).Count());

            Assert.AreEqual(name, queryServiceEmployee.All().Single().Name);
        }

        [TestMethod]
        public void Execute_EmployeeCreate_NewReadModelCreated()
        {
            // Arrange
            var expectedNumberOfEmployee = 1;
            var name = "Jet Lee";
            var command = new EmployeeCreateCommand(name, "Scrum Master", 8, 180);

            var queryServiceEmployee = container.Resolve<IRepository<EmployeeReadModel>>();
            var bus = container.Resolve<IBus>();

            // Act
            bus.Dispatch(command);

            // Assert
            Assert.AreEqual(expectedNumberOfEmployee, queryServiceEmployee.FindList(x => x.Name == name).Count());
            Assert.AreEqual(name, queryServiceEmployee.All().Single().Name);
        }
    }
}