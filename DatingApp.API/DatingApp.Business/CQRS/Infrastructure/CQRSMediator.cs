using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.Business.CQRS
{
    public class CQRSMediator : ICQRSMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public CQRSMediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void CommandByIdAndTwoParameters<TCommand, TParam1, TParam2>(int id, TParam1 parameter1, TParam2 parameter2) where TCommand : ICommandByIdAndTwoParametersHandler<TParam1, TParam2>
        {
            TCommand commandHandler = _serviceProvider.GetRequiredService<TCommand>();

            commandHandler.HandleCommand(id, parameter1, parameter2);
        }

        public TResult CommandByIdAndTwoParameters<TCommand, TResult, TParam1, TParam2>(int id, TParam1 parameter1, TParam2 parameter2) where TCommand : ICommandByIdAndTwoParametersHandler<TResult, TParam1, TParam2>
        {
            TCommand commandHandler = _serviceProvider.GetRequiredService<TCommand>();

            var commandResult = commandHandler.HandleCommand(id, parameter1, parameter2);

            return commandResult;
        }

        public void CommandByParameter<TCommand, TParam>(TParam parameter) where TCommand : ICommandByParameterHandler<TParam>
        {
            TCommand commandHandler = _serviceProvider.GetRequiredService<TCommand>();

            commandHandler.HandleCommand(parameter);
        }

        public TResult CommandByParameter<TCommand, TResult, TParam>(TParam parameter) where TCommand : ICommandByParameterHandler<TResult, TParam>
        {
            TCommand commandHandler = _serviceProvider.GetRequiredService<TCommand>();

            var commandResult = commandHandler.HandleCommand(parameter);

            return commandResult;
        }

        public void CommandByTwoParameters<TCommand, TParam1, TParam2>(TParam1 parameter1, TParam2 parameter2) where TCommand : ICommandByTwoParametersHandler<TParam1, TParam2>
        {
            TCommand commandHandler = _serviceProvider.GetRequiredService<TCommand>();

            commandHandler.HandleCommand(parameter1, parameter2);
        }

        public TResult CommandByTwoParameters<TCommand, TResult, TParam1, TParam2>(TParam1 parameter1, TParam2 parameter2) where TCommand : ICommandByTwoParametersHandler<TResult, TParam1, TParam2>
        {
            TCommand commandHandler = _serviceProvider.GetRequiredService<TCommand>();

            var commandResult = commandHandler.HandleCommand(parameter1, parameter2);

            return commandResult;
        }

        public TResult QueryByParameter<TQuery, TResult, TParam>(TParam parameter) where TQuery : IQueryByParameterHandler<TResult, TParam>
        {
            TQuery queryHandler = _serviceProvider.GetRequiredService<TQuery>();
            
            var queryResult = queryHandler.HandleQuery(parameter);

            return queryResult;
        }

        public TResult QueryByTwoParameters<TQuery, TResult, TParam1, TParam2>(TParam1 parameter1, TParam2 parameter2) where TQuery : IQueryByTwoParametersHandler<TResult, TParam1, TParam2>
        {
            TQuery queryHandler = _serviceProvider.GetRequiredService<TQuery>();

            var queryResult = queryHandler.HandleQuery(parameter1, parameter2);

            return queryResult;
        }
    }
}
