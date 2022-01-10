using AutoMapper;

namespace DatingApp.Core.Model.AutoMapper
{
    public interface IMapTo<TFrom, TResult>
    {
        void ConfigureMapTo(IMappingExpression<TFrom, TResult> mapping);
    }
}
