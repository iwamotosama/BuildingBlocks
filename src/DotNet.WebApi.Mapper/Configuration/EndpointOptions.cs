global using EndpointSpec = (
    System.Predicate<System.Reflection.MethodInfo> Predicate,
    System.Func<System.Reflection.MethodInfo, (string Name, System.Net.Http.HttpMethod HttpMethod)> Method,
    System.Func<object?, Microsoft.AspNetCore.Http.IResult> StatusTransform,
    System.Func<System.Exception, Microsoft.AspNetCore.Http.IResult> ExceptionTransform);

using System.ComponentModel;
using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace Nikuman.BuildingBlocks.DotNet.WebApi.Mapper.Configuration;

/// <summary>
/// Configuration options for mapping services to api endpoints
/// </summary>
public class EndpointOptions
{
    private readonly IReadOnlyDictionary<Type, Func<string, object?>> _converters;
    internal IReadOnlyCollection<EndpointSpec> EndpointSpecifications { get; }

    private EndpointOptions(IReadOnlyCollection<EndpointSpec> endpointSpecifications, IReadOnlyDictionary<Type, Func<string, object?>> converters)
    {
        EndpointSpecifications = endpointSpecifications;
        _converters = converters;
    }
    
    /// <summary>
    /// Initializes an <see cref="EndpointOptionsBuilder"/> to create a new <see cref="EndpointOptions"/>  
    /// </summary>
    /// <returns>A new <see cref="EndpointOptionsBuilder"/> </returns>
    public static EndpointOptionsBuilder Builder()
    {
        return new EndpointOptionsBuilder();
    }

    internal Func<string, object?> GetConverter(Type targetType)
    {
        if (_converters.TryGetValue(targetType, out var converter))
        {
            return converter;
        }

        return TypeDescriptor.GetConverter(targetType).ConvertFromString;
    }

    /// <summary>
    /// Builder class to create <see cref="EndpointOptions"/> instances 
    /// </summary>
    public class EndpointOptionsBuilder
    {
        // hide the constructor
        internal EndpointOptionsBuilder()
        {

        }


        private readonly List<EndpointSpec> _specsBuilder = new();
        private readonly Dictionary<Type, Func<string, object?>> _converters = new();

        /// <summary>
        /// Adds Endpoints via reflection by searchingfor methods whose names starts with the specified <paramref name="prefix"/>
        /// </summary>
        /// <param name="method">The <see cref="HttpMethod"/> for service calls with this prefix</param> 
        /// <param name="prefix">The method prefix</param>
        /// <param name="statusTransform">
        /// Optional transformation to customize response results. The default behavior returns 200/OK status for service calls that
        /// return a value and 204/NoContent for service calls with a null return or voids.
        /// </param>
        /// <param name="exceptionTransform">
        /// Optional transformation to customize response results for exceptions. The default behavior is to delegate all exception 
        /// handling to the ASP.NET pipeline.
        /// </param>
        /// <returns>This <see cref="EndpointOptionsBuilder"/></returns>
        public EndpointOptionsBuilder AddReflectedEndpoints(HttpMethod method, string prefix, Func<object?, IResult>? statusTransform = null, Func<Exception, IResult>? exceptionTransform = null)
        {
            _specsBuilder.Add((mi => mi.Name.StartsWith(prefix), mi => (mi.Name[prefix.Length..], method), statusTransform ?? DefaultStatusTransform, exceptionTransform ?? DefaultExceptionTransform));
            return this;
        }

        /// <summary>
        /// Adds Endpoints via reflection with the selected <see cref="HttpMethod"/> as determined by <paramref name="methodSelector"/> 
        /// </summary>
        /// <param name="methodSelector">
        /// Delegate that returns the name and the <see cref="HttpMethod"/> that a given <see cref="MethodInfo"/> should be mapped as. 
        /// A null return from this delegate indicates that the <see cref="MethodInfo"/> should not be mapped. 
        /// </param>
        /// <param name="statusTransform">
        /// Optional transformation to customize response results. The default behavior returns 200/OK status for service calls that
        /// return a value and 204/NoContent for service calls with a null return or voids.
        /// </param>
        /// <param name="exceptionTransform">
        /// Optional transformation to customize response results for exceptions. The default behavior is to delegate all exception 
        /// handling to the ASP.NET pipeline.
        /// </param>
        /// <returns>This <see cref="EndpointOptionsBuilder"/></returns>
        public EndpointOptionsBuilder AddReflectedEndpoints(Func<MethodInfo, (string, HttpMethod)?> methodSelector, Func<object?, IResult>? statusTransform = null, Func<Exception, IResult>? exceptionTransform = null)
        {
            _specsBuilder.Add((mi => methodSelector(mi).HasValue, mi => methodSelector(mi)!.Value, statusTransform ?? DefaultStatusTransform, exceptionTransform ?? DefaultExceptionTransform));
            return this;
        }

        /// <summary>
        /// Adds the specified conversion function to convert strings to an alternate type
        /// </summary>
        /// <param name="type">The <see cref="Type"/> that the <paramref name="converter"/> is able to convert to</param>
        /// <param name="converter">The delegate to convert a given string</param>
        /// <returns>This <see cref="EndpointOptionsBuilder"/> </returns>
        /// <remarks>
        /// By default, if a conversion delegate is not added for a type the <see cref="TypeConverter"/> registered for 
        /// that type will be used, if available 
        /// </remarks>
        public EndpointOptionsBuilder AddConverter(Type type, Func<string, object?> converter)
        {
            _converters[type] = converter;
            return this;
        }
        /// <summary>
        /// Returns a fully initialized <see cref="EndpointOptions"/> 
        /// </summary>
        /// <returns>A fully initializes <see cref="EndpointOptions"/> </returns>
        public EndpointOptions Build()
        {
            return new EndpointOptions(_specsBuilder, _converters);
        }
    }

    internal static IResult DefaultStatusTransform(object? obj)
    {
        return obj == null ? Results.NoContent() : Results.Ok(obj);
    }

    internal static IResult DefaultExceptionTransform(Exception exception)
    {
        // throw the exception so that the default ASP.NET exception handling does its thing
        throw exception;
    }
}