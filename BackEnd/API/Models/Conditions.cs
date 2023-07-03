using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace PectraForms.WebApplication.BackEnd.API.Models
{
    public class Condition
    {
        public string field { get; set; }
        public object value { get; set; }
    }

    [TypeConverter(typeof(ConditionsConverter))]
    public class ConditionsRequest
    {
        public ConditionsRequest()
        {
            conditionValues = new List<Condition>();
        }

        public List<Condition> conditionValues { get; set; }

        public static bool TryParse(string s, out ConditionsRequest result)
        {
            result = null;

            var parts = s.Split(',');
            if (parts == null || parts.Length < 1)
                return false;

            result = new ConditionsRequest();

            foreach (var cond in parts)
            {
                var conds = cond.Split(':');
                if (conds == null || conds.Length != 2)
                    return false;

                result.conditionValues.Add(new Condition()
                {
                    field = conds[0],
                    value = conds[1]
                });
            }

            return true;
        }
    }

    public class ConditionsConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                ConditionsRequest conditions;
                if (ConditionsRequest.TryParse((string)value, out conditions))
                    return conditions;
            }
            if (context != null)
                return base.ConvertFrom(context, culture, value);
            else
                return null;
        }
    }




}