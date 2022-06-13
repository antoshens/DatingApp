namespace DatingApp.Business.CQRS
{
    public interface ICommandByIdAndTwoParametersHandler<TParam1, TParam2>
    {
        void HandleCommand(int id, TParam1 parameter1, TParam2 parameter2);
    }

    public interface ICommandByIdAndTwoParametersHandler<TResult, TParam1, TParam2>
    {
        TResult HandleCommand(int id, TParam1 parameter1, TParam2 parameter2);
    }
}
