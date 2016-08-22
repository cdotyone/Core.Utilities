using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Civic.Core.Framework.Helpers
{
    public static class EnumHelper
    {
        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(Description), false);
                if (attrs.Length > 0)
                    return ((Description)attrs[0]).Text;
            }

            return string.Empty;
        }

        public static Enum GetEnum(Type type, string description)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            Array enumValues = Enum.GetValues(type);
            foreach (Enum item in enumValues)
            {
                if (GetDescription(item).Equals(description))
                    return item;
            }
            return null;
        }

        public static IList GetDescriptiveList(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            ArrayList list = new ArrayList();
            Array enumValues = Enum.GetValues(type);
            foreach (Enum item in enumValues)
            {
                list.Add(new KeyValuePair<int, string>((int)Enum.Parse(type, item.ToString()), GetDescription(item)));
            }

            return list;
        }
    }

    public class Description : Attribute
    {
        public string Text;

        public Description(string text)
        {
            Text = text;
        }
    }
}
