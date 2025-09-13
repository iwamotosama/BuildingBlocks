namespace Nikuman.BuildingBlocks.DotNet.Essentials.Reflection;

/// <summary>
/// Extensions to <see cref="Type"/> 
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// Determines if the <paramref name="givenType"/> can be assigned to a variable with specified open generic type
    /// </summary>
    /// <param name="givenType">The <see cref="Type"/> to compare</param>
    /// <param name="genericType">The open generic <see cref="Type"/> to compare <paramref name="givenType"/> against</param>
    /// <returns>True if <paramref name="givenType"/> can be assigned, False if not.</returns>
    public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
    {
        var interfaceTypes = givenType.GetInterfaces();

        foreach (var it in interfaceTypes)
        {
            if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }
        }

        if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
        {
            return true;
        }

        return givenType.BaseType?.IsAssignableToGenericType(genericType) ?? false;
    }
}

