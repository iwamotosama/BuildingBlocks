using System.Collections.Immutable;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Nikuman.BuildingBlocks.DotNet.Essentials.Collections;
using Nikuman.BuildingBlocks.DotNet.WebApi.Mapper.Configuration;
using AspMapExt = Microsoft.AspNetCore.Builder.EndpointRouteBuilderExtensions;

namespace Nikuman.BuildingBlocks.DotNet.WebApi.Mapper;

/// <summary>
/// Extensions to <see cref="IEndpointRouteBuilder"/> 
/// </summary>
public static class EndpointRouteBuilderExtensions
{
    private static readonly HashSet<string> _excludedMethodNames =
    [
        nameof(object.ToString),
        nameof(object.Equals),
        nameof(object.GetHashCode),
        nameof(object.GetType)
    ];

    /// <summary>
    /// Maps the service as an web API endpoint
    /// </summary>
    /// <typeparam name="TService">The <see cref="Type"/> of the service to map</typeparam>
    /// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to add this service to</param>
    /// <param name="rootPath">
    /// The root path tree to map this service under, with each path segement as an element, or empty collection to map to root
    /// </param>
    /// <param name="options"><see cref="EndpointOptions"/></param> 
    public static void MapApi<TService>(this IEndpointRouteBuilder builder, ImmutableList<string> rootPath, EndpointOptions options)
    {
        MapInternal(typeof(TService), rootPath, o => o);

        void MapInternal(Type currentServiceType, ImmutableList<string> path, Func<object?, object?> accessor)
        {
            var pathSequence = string.Empty.One().Concat(path);

            foreach (var candidateMethod in currentServiceType
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(mi => !_excludedMethodNames.Contains(mi.Name)))
            {
                var spec = options.EndpointSpecifications
                    .Cast<EndpointSpec?>()
                    .FirstOrDefault(ss => ss!.Value.Predicate(candidateMethod));

                if (!spec.HasValue)
                {
                    continue;
                }
                var endpointSpec = spec.Value;
                var endpoint = new ReflectionEndpoint(candidateMethod, endpointSpec.StatusTransform, endpointSpec.ExceptionTransform);

                var (endpointName, httpMethod) = endpointSpec.Method(candidateMethod);
                var endpointSequence = pathSequence.Append(endpointName);

                var parameters = candidateMethod.GetParameters();
                if (parameters.Length > 0)
                {
                    GetMapper(httpMethod)(builder, string.Join('/', endpointSequence.Append("{pathParameter}")),
                        async ([FromServices] TService service, string pathParameter, HttpContext context) =>
                        {
                            var paramsBuilder = new List<object?>(parameters.Length);

                            void AddParameter(ParameterInfo pi, StringValues value)
                            {
                                if (StringValues.IsNullOrEmpty(value))
                                {
                                    if (pi.HasDefaultValue)
                                    {
                                        paramsBuilder.Add(pi.DefaultValue);
                                    }
                                    else
                                    {
                                        throw new BadHttpRequestException($"Required parameter {pi.Name} was not provided from query string");
                                    }
                                }
                                else if (value.Count == 1)
                                {
                                    paramsBuilder.Add(options.GetConverter(pi.ParameterType)(value.First() ?? string.Empty));
                                }
                                else
                                {
                                    var elemType = pi.ParameterType.GetGenericArguments()[0];
                                    paramsBuilder.Add(value.Select(s => options.GetConverter(elemType)).ToImmutableList());
                                }
                            }

                            AddParameter(parameters[0], pathParameter);
                            foreach (var param in parameters.Skip(1))
                            {
                                AddParameter(param, context.Request.Query[param.Name!]);
                            }

                            return await endpoint.Invoke(accessor(service), paramsBuilder.ToArray());
                        });
                }
                else
                {
                    GetMapper(httpMethod)(builder, string.Join('/', endpointSequence),
                        async ([FromServices] TService service) =>
                        {
                            return await endpoint.Invoke(accessor(service), null);
                        });
                }
            }

            foreach (var subApi in SubApis(currentServiceType))
            {
                MapInternal(subApi.PropertyType, path.Add(subApi.Name), o => subApi.GetMethod!.Invoke(accessor(o), null));
            }
        }
    }

    private static IEnumerable<PropertyInfo> SubApis(Type service)
    {
        return service.GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }

    private static Func<IEndpointRouteBuilder, string, Delegate, RouteHandlerBuilder> GetMapper(HttpMethod httpMethod)
    {
        if (httpMethod == HttpMethod.Get)
        {
            return AspMapExt.MapGet;
        }
        else if (httpMethod == HttpMethod.Patch)
        {
            return AspMapExt.MapPatch;
        }
        else if (httpMethod == HttpMethod.Post)
        {
            return AspMapExt.MapPost;
        }
        else if (httpMethod == HttpMethod.Put)
        {
            return AspMapExt.MapPut;
        }
        else if (httpMethod == HttpMethod.Delete)
        {
            return AspMapExt.MapDelete;
        }
        else
        {
            return AspMapExt.Map;
        }
    }
}
