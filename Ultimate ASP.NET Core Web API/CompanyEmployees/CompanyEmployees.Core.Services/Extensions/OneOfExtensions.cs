namespace CompanyEmployees.Core.Services.Extensions;
using OneOf;

public static class OneOfExtensions
{
    public static void Deconstruct<T0, T1>(this OneOf<T0, T1> oneOf, out T0? val0, out T1? val1) 
        => oneOf.TryPickT0(out val0, out val1);
}