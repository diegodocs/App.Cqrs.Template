namespace App.Cqrs.Core.Command
{
    public interface ICommandHandler<in TParameter> where TParameter : ICommand
    {
        void Execute(TParameter command);
    }

}
