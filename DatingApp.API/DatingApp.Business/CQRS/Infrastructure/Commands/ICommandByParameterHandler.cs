namespace DatingApp.Business.CQRS
{
    public interface ICommandByParameterHandler<TParam>
    {
        void HandleCommand(TParam parameter);
    }

    public interface ICommandByParameterHandler<TResult, TParam>
    {
        TResult HandleCommand(TParam parameter);
    }
}
