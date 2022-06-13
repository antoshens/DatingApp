namespace DatingApp.Business.CQRS
{
    public interface IQueryByParameterHandler<TResult, TParam>
    {
        TResult HandleQuery(TParam parameter);
    }
}
