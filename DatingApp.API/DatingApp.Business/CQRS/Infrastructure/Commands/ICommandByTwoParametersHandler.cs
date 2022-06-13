namespace DatingApp.Business.CQRS
{
    public interface ICommandByTwoParametersHandler<TParam1, TParam2>
    {
        void HandleCommand(TParam1 parameter1, TParam2 parameter2);
    }

    public interface ICommandByTwoParametersHandler<TResult, TParam1, TParam2>
    {
        TResult HandleCommand(TParam1 parameter1, TParam2 parameter2);
    }
}
