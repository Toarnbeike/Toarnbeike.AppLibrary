using System.Text;

namespace Toarnbeike.AppLibrary.Extensions;

/// <summary>
/// Provides extension members for creating display-friendly representations of <see cref="Type"/> instances.
/// </summary>
/// <remarks>Use these extensions to centralize and standardize type name formatting across an application.</remarks>
public static class TypeDisplayExtensions
{
    extension(Type type)
    {
        /// <summary>
        /// Gets a C#-style display name for the current type, including aliases and generic type arguments.
        /// </summary>
        /// <remarks>Generic type arity suffixes are removed from type names before formatting generic arguments.</remarks>
        /// <returns>A readable type name that uses predefined aliases when available; 
        /// otherwise formats arrays with rank-appropriate brackets, nullable value types with ?, 
        /// generic types with angle-bracketed argument names, 
        /// and nested types with dot notation.</returns>
        public string ReadableName()
        {
            if (_aliases.TryGetValue(type, out var alias))
            {
                return alias;
            }

            if (type.IsArray)
            {
                var ranks = new StringBuilder();
                var elementType = type;
                do
                {
                    ranks.Append('[').Append(',', elementType.GetArrayRank() - 1).Append(']');
                    elementType = elementType.GetElementType()!;
                }
                while (elementType.IsArray);

                return ranks.Insert(0, elementType.ReadableName()).ToString();
            }

            if (Nullable.GetUnderlyingType(type) is { } underlying)
            {
                return $"{underlying.ReadableName()}?";
            }

            if (type.IsGenericType)
            {
                var name = type.Name;
                var tick = name.IndexOf('`');
                if (tick >= 0)
                {
                    name = name[..tick];
                }

                var arguments = string.Join(", ", type.GetGenericArguments().Select(ReadableName));

                return $"{name}<{arguments}>";
            }

            return type.Name.Replace('+', '.');
        }
    }

    private static readonly Dictionary<Type, string> _aliases = new()
    {
        [typeof(bool)] = "bool",
        [typeof(byte)] = "byte",
        [typeof(sbyte)] = "sbyte",
        [typeof(short)] = "short",
        [typeof(ushort)] = "ushort",
        [typeof(int)] = "int",
        [typeof(uint)] = "uint",
        [typeof(long)] = "long",
        [typeof(ulong)] = "ulong",
        [typeof(float)] = "float",
        [typeof(double)] = "double",
        [typeof(decimal)] = "decimal",
        [typeof(char)] = "char",
        [typeof(string)] = "string",
        [typeof(object)] = "object",
        [typeof(void)] = "void",
        [typeof(nint)] = "nint",
        [typeof(nuint)] = "nuint"
    };
}
