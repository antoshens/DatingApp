using AutoMapper;

namespace DatingApp.Core.Model.AutoMapper
{
    public interface IMapFrom<TFrom, TResult>
    {
        void ConfigureMapFrom(IMappingExpression<TFrom, TResult> mapping);
    }
}
