using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;

namespace ECS.Framework.Extensions
{
    public static class EnumExtensions
    {
        public static int ObterTipo(Type enumType, string tipo)
        {
            var enumValue = enumType
                                   .GetFields()
                                   .Select(x => new
                                   {
                                       att = x.GetCustomAttributes(false)
                                                    .OfType<DescriptionAttribute>()
                                                    .FirstOrDefault(),
                                       member = x
                                   })
                                   .Where(x => x.att != null && x.att.Description == tipo)
                                   .Select(x => (int)x.member.GetValue(null))
                                   .FirstOrDefault();

            return enumValue;

        }

        public static int GetValueByDescription<T>(this string obj)
        {
            if (string.IsNullOrEmpty(obj))
                return 0;

            var type = typeof(T);

            if (!type.IsEnum) throw new InvalidOperationException();

            foreach (var field in type.GetFields())
            {
                var attribute =
                    Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

                if (attribute != null)
                {
                    if (attribute.Description == obj)
                        return (int)field.GetValue(null);

                    if (obj.Contains(attribute.Description))
                        return (int)field.GetValue(null);

                    if (field.Name == obj)
                        return (int)field.GetValue(null);
                }

            }

            return 0;
        }

        public static string GetDescriptionByValue(this int value, Type tipo)
        {
            return ((Enum)Enum.Parse(tipo, value.ToString())).GetEnumDescription();
        }

        public static string GetEnumDescription(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            if (fi == null)
                return "";
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            var enumMemberAttributes = (EnumMemberAttribute[])fi.GetCustomAttributes(typeof(EnumMemberAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description :
                enumMemberAttributes.Length > 0 && !string.IsNullOrEmpty(enumMemberAttributes[0].Value)
                    ? enumMemberAttributes[0].Value : value.ToString();
        }
    }
}
