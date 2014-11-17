using System;
using System.ComponentModel;
using System.Globalization;

namespace StatisticsCollector.Common
{
    internal class SensorIdConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                return new SensorId(value as string);
            }

            return base.ConvertFrom(value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return (value as SensorId).ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}