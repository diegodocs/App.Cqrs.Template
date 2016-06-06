using System;
using Ninject;

namespace App.Cqrs.Core.Command
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IKernel _kernel;

        public CommandDispatcher(IKernel kernel)
        {
            if (kernel == null)
            {
                throw new ArgumentNullException("kernel");
            }
            _kernel = kernel;
        }

        public void Dispatch<TParameter>(TParameter command) where TParameter : ICommand
        {
            var handler = _kernel.Get<ICommandHandler<TParameter>>();
            handler.Execute(command);
        }

    }
}