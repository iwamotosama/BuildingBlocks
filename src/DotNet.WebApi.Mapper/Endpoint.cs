using System.Reflection;
using Microsoft.AspNetCore.Http;
using Nikuman.BuildingBlocks.DotNet.Essentials.Reflection;

namespace Nikuman.BuildingBlocks.DotNet.WebApi.Mapper;

internal class Endpoint<TService>(MethodInfo method, Func<object?, IResult> statusTransform, Func<Exception, IResult> exceptionTransform)
{
    private readonly MethodInfo _method = method;
    private readonly Func<object?, IResult> _statusTransform = statusTransform;
    private readonly Func<Exception, IResult> _exceptionTransform = exceptionTransform;

    public virtual async Task<IResult> Invoke(TService instance, object?[]? arguments)
    {
        try
        {
            var type = _method.ReturnType;
            var val = _method.Invoke(instance, arguments);

            object? returnValue;
            if (type.IsAssignableTo(typeof(Task)) && val != null)
            {
                // if the method returned a Task, use some dynamic hackary to await it and get its result (if it exists)
                dynamic awaitable = val;
                await awaitable;

                returnValue = type.IsAssignableToGenericType(typeof(Task<>)) ? awaitable.GetAwaiter().GetResult() : null;
            }
            else
            {
                returnValue = val;
            }

            return _statusTransform(returnValue);
        }
        catch (Exception ex)
        {
            return _exceptionTransform(ex);
        }
    }
}