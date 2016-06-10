using Autofac;

namespace App.Cqrs.Core.Command
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IContainer container;

        public CommandDispatcher(IContainer container)
        {
            this.container = container;
        }

        public void Dispatch<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handler = container.Resolve<ICommandHandler<TCommand>>();
            handler.Handle(command);
        }
    }
}