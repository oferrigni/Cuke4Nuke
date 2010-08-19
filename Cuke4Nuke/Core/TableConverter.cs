﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Cuke4Nuke.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cuke4Nuke.Core
{
    public class TableConverter : TypeConverter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            System.Console.WriteLine("Checking convert from with type " + sourceType);
            if (sourceType == typeof(string))
            {
                return true;
            } if (sourceType == typeof(WatiN.Core.Table))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }
        
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                return JsonToTable(value.ToString());
            }
            if (value is WatiN.Core.Table)
            {
                return null;
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            System.Console.WriteLine("Checking convert to with type " + destinationType);
            if (destinationType == typeof(string))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return TableToJsonString(((Table)value));
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public Table JsonToTable(string jsonTable)
        {
            Table table = new Table();
            
            if (jsonTable == null || jsonTable == String.Empty)
            {
                return table;
            }

            ArgumentException invalidDataEx = new ArgumentException("Input not in expected format. Expecting a JSON array of string arrays.");

            JArray rows;
            try
            {
                rows = JArray.Parse(jsonTable);
            }
            catch (Exception)
            {
                throw invalidDataEx;
            }

            if (rows.Count == 0)
            {
                return table;
            }

            foreach (JToken row in rows.Children())
            {
                if (!(row.Type == JTokenType.Array))
                {
                    throw invalidDataEx;
                }

                var values = new List<string>();

                foreach (JToken cell in row.Children())
                {
                    if (!(cell.Type == JTokenType.String))
                    {
                        throw invalidDataEx;
                    }
                    values.Add(cell.Value<string>() as string);
                }

                table.Data.Add(values);
            }
            return table;
        }

        public string TableToJsonString(Table table)
        {
            return JsonConvert.SerializeObject(table.Data, Formatting.None);
        }
    }
}
