namespace App.Cqrs.Core.Command
{
    public interface ICommandDispatcher
    {
        void Dispatch<TParameter>(TParameter command) where TParameter : ICommand;
    }
}
