namespace DatingApp.Business.CQRS
{
    public interface IQueryByTwoParametersHandler<TResult, TParam1, TParam2>
    {
        TResult HandleQuery(TParam1 parameter1, TParam2 parameter2);
    }
}
