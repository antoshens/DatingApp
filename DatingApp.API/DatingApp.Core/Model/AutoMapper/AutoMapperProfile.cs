using AutoMapper;
using System.Reflection;

namespace DatingApp.Core.Model.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        internal enum MapDirection
        {
            MapTo = 1,
            MapFrom = 2,
        }

        internal class AssemblyTypes
        {
            internal Type Type { get; set; }
            internal Type Interface { get; set; }
        }

        internal class MappingType
        {
            internal Type Source { get; set; }
            internal Type Destination { get; set; }
            internal object? ModelInstance { get; set; }
            internal MapDirection Direction { get; set; } = 0;
        }

        public static MapperConfiguration RegisterMappings()
        {
            var assemblyTypes = GetAllTypesImplementingOpenGenericType(typeof(AutoMapperProfile).Assembly);

            var mapstoCreate = GetMappingTypesInfo(assemblyTypes);

            var createMapFrom = typeof(IProfileExpression).GetMethod("CreateMap", new Type[0]);
            var createMapTo = typeof(IProfileExpression).GetMethod("CreateMap", new Type[0]);

            var config = new MapperConfiguration(cfg =>
            {
                foreach (var map in mapstoCreate)
                {
                    if (map.ModelInstance is null)
                    {
                        cfg.CreateMap(map.Source, map.Destination);
                    }
                    else if (map.Direction == MapDirection.MapFrom)
                    {
                        var genericCreateMap = createMapFrom.MakeGenericMethod(map.Source, map.Destination);
                        var mapConfiguration = genericCreateMap.Invoke(cfg, null);

                        var configureMapFrom = typeof(IMapFrom<,>).MakeGenericType(map.Source, map.Destination).GetMethod("ConfigureMapFrom");
                        configureMapFrom.Invoke(map.ModelInstance, new[] { mapConfiguration });
                    }
                    else if (map.Direction == MapDirection.MapTo)
                    {
                        var genericCreateMap = createMapTo.MakeGenericMethod(map.Source, map.Destination);
                        var mapConfiguration = genericCreateMap.Invoke(cfg, null);

                        var configureMapTo = typeof(IMapTo<,>).MakeGenericType(map.Source, map.Destination).GetMethod("ConfigureMapTo");
                        configureMapTo.Invoke(map.ModelInstance, new[] { mapConfiguration });
                    }
                }
            });

            return config;
        }

        private static IEnumerable<AssemblyTypes> GetAllTypesImplementingOpenGenericType(Assembly assembly)
        {
            return from t in assembly.GetTypes()
                   from i in t.GetInterfaces()
                   where i.IsGenericType && !t.IsAbstract && !t.IsInterface
                        && (i.GetGenericTypeDefinition() == typeof(IMapFrom<,>)
                        || i.GetGenericTypeDefinition() == typeof(IMapTo<,>))
                   select new AssemblyTypes { Type = t, Interface = i };
        }

        private static IEnumerable<MappingType> GetMappingTypesInfo(IEnumerable<AssemblyTypes> assemblyTypes)
        {

            var mappingTypes = assemblyTypes.Select(t =>
            {
                var genericTypes = t.Interface.GetGenericArguments();

                return new MappingType
                {
                    Source = genericTypes[0],
                    Destination = genericTypes[1],
                    ModelInstance = Activator.CreateInstance(t.Type),
                    Direction = t.Interface.GetGenericTypeDefinition() == typeof(IMapFrom<,>)
                                        ? MapDirection.MapFrom : MapDirection.MapTo
                };
            });

            return mappingTypes;
        }
    }
}
