using System.ComponentModel;
using System.Reflection;

namespace MyDotNetBase.Domain.Shared.Utilities;

public static class EnumExtensions
{
    public static string GetDescription<TEnum>(this TEnum value) where TEnum : struct, Enum
    {
        var fieldInfo = typeof(TEnum).GetField(value.ToString());
        return fieldInfo?
            .GetCustomAttribute<DescriptionAttribute>(false)?
            .Description ?? value.ToString();
    }
}
