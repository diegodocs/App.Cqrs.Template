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

        public void Dispatch<TParameter>(TParameter command) where TParameter : ICommand
        {
            var handler = container.Resolve<ICommandHandler<TParameter>>();
            handler.Execute(command);
        }
    }
}