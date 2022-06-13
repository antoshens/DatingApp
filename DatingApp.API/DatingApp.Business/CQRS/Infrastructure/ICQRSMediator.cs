namespace DatingApp.Business.CQRS
{
    public interface ICQRSMediator
    {
        TResult QueryByParameter<TQuery, TResult, TParam>(TParam parameter) where TQuery : IQueryByParameterHandler<TResult, TParam>;
        TResult QueryByTwoParameters<TQuery, TResult, TParam1, TParam2>(TParam1 parameter1, TParam2 parameter2) where TQuery : IQueryByTwoParametersHandler<TResult, TParam1, TParam2>;

        void CommandByParameter<TCommand, TParam>(TParam parameter) where TCommand : ICommandByParameterHandler<TParam>;
        TResult CommandByParameter<TCommand, TResult, TParam>(TParam parameter) where TCommand : ICommandByParameterHandler<TResult, TParam>;
        void CommandByTwoParameters<TCommand, TParam1, TParam2>(TParam1 parameter1, TParam2 parameter2) where TCommand : ICommandByTwoParametersHandler<TParam1, TParam2>;
        TResult CommandByTwoParameters<TCommand, TResult, TParam1, TParam2>(TParam1 parameter1, TParam2 parameter2) where TCommand : ICommandByTwoParametersHandler<TResult, TParam1, TParam2>;
        void CommandByIdAndTwoParameters<TCommand, TParam1, TParam2>(int id, TParam1 parameter1, TParam2 parameter2) where TCommand : ICommandByIdAndTwoParametersHandler<TParam1, TParam2>;
        TResult CommandByIdAndTwoParameters<TCommand, TResult, TParam1, TParam2>(int id, TParam1 parameter1, TParam2 parameter2) where TCommand : ICommandByIdAndTwoParametersHandler<TResult, TParam1, TParam2>;
    }
}
