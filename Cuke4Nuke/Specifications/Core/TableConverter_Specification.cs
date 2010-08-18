using System;
using System.Collections.Generic;
using System.ComponentModel;
using NUnit.Framework;
using Cuke4Nuke.Core;
using Cuke4Nuke.Framework;
using Moq;

namespace Cuke4Nuke.Specifications.Core
{
    public class TableConverter_Specification
    {
        private TableConverter converter;
        [SetUp]
        public void SetUp()
        {
            TypeConverterAttribute attr = new TypeConverterAttribute(typeof(TableConverter));
            TypeDescriptor.AddAttributes(typeof(Table), new Attribute[] { attr });
            converter = TypeDescriptor.GetConverter(typeof(Table)) as TableConverter;
        }

        [Test]
        public void ShouldConvertFromWatinTable()
        {
            var watinTable = new Mock<WatiN.Core.Table>();
            converter = TypeDescriptor.GetConverter(typeof(WatiN.Core.Table)) as TableConverter;

            Assert.DoesNotThrow(delegate
            {
                Table table = (Table)converter.ConvertFrom(watinTable);
            });
        }

        [Test]
        public void WatinToTable_ShouldConvertWithNoRowsInTable()
        {
            throw new NotImplementedException();
        }


        [Test]
        public void WatinToTable_ShouldConvertWithOneRowAndOneCell()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void WatinToTable_ShouldConvertWithOneRowAndMultipleCells()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void ShouldConvertFromString()
        {
            string serializedTable = "[[\"foo\",\"1\"],[\"bar\",\"2\"]]";
            Assert.DoesNotThrow(delegate {                
                Table table = (Table) converter.ConvertFromString(serializedTable);
            });
        }

        [Test]
        public void JsonToTable_ShouldConvertFromString_EmptyString()
        {
            string serializedTable = "";
            Table table = converter.JsonToTable(serializedTable);
            Assert.That(table.Hashes().Count, Is.EqualTo(0));
        }

        [Test]
        public void JsonToTable_ShouldThrowForInvalidData_NotAnArray()
        {
            string serializedTable = "{\"foo\":\"bar\"}";
            Assert.Throws<ArgumentException>(delegate {
                Table table = converter.JsonToTable(serializedTable);
            });
        }

        [Test]
        public void JsonToTable_ShouldThrowForInvalidData_NoInternalArrays()
        {
            string serializedTable = "[{\"foo\":\"1\"},{\"bar\":\"2\"}]";
            Assert.Throws<ArgumentException>(delegate
            {
                Table table = converter.JsonToTable(serializedTable);
            });
        }

        [Test]
        public void JsonToTable_ShouldThrowForInvalidData_NonStringDataInInternalArray()
        {
            string serializedTable = "[[2,1],[42,2]]";
            //converter.JsonToTable(serializedTable);
            Assert.Throws<ArgumentException>(delegate
            {
                Table table = converter.JsonToTable(serializedTable);
            });
        }

        [Test]
        public void JsonToTable_ShouldConvertFromString_HeaderRowOnly()
        {
            string serializedTable = "[[\"item\",\"count\"]]";
            Table table = converter.JsonToTable(serializedTable);
            Assert.That(table.Hashes().Count, Is.EqualTo(0));
        }

        [Test]
        public void JsonToTable_ShouldConvertFromString_HeaderAndDataRows()
        {
            string serializedTable = "[[\"item\",\"count\"],[\"cucumbers\",\"3\"],[\"bananas\",\"5\"],[\"tomatoes\",\"2\"]]";
            Table table = converter.JsonToTable(serializedTable);
            Assert.That(table.Data.Count, Is.EqualTo(4));
        }

        [Test]
        public void TableToJsonString_ShouldConvertFromTable_EmptyTable()
        {
            Table table = new Table();
            string expectedJsonString = "[]";
            string actualJsonString = converter.TableToJsonString(table);
            Assert.That(actualJsonString, Is.EqualTo(expectedJsonString));
        }

        [Test]
        public void TableToJsonString_ShouldConvertFromTable_HeaderRowOnly()
        {
            Table table = new Table();
            table.Data.Add(new List<string>(new string[] { "item", "count" }));
            string expectedJsonString = "[[\"item\",\"count\"]]";
            string actualJsonString = converter.TableToJsonString(table);
            Assert.That(actualJsonString, Is.EqualTo(expectedJsonString));
        }

        [Test]
        public void TableToJsonString_ShouldConvertFromTable_HeaderAndDataRows()
        {
            Table table = new Table();
            table.Data.Add(new List<string>(new string[] { "item", "count" }));
            table.Data.Add(new List<string>(new string[] { "cucumbers", "3" }));
            table.Data.Add(new List<string>(new string[] { "bananas", "5" }));
            table.Data.Add(new List<string>(new string[] { "tomatoes", "2" }));
            string expectedJsonString = "[[\"item\",\"count\"],[\"cucumbers\",\"3\"],[\"bananas\",\"5\"],[\"tomatoes\",\"2\"]]";
            string actualJsonString = converter.TableToJsonString(table);
            Assert.That(actualJsonString, Is.EqualTo(expectedJsonString));
        }
        
        [Test]
        public void ShouldConvertToString()
        {
            Table table = new Table();
            table.Data.Add(new List<string>(new string[] { "foo", "1" }));
            table.Data.Add(new List<string>(new string[] { "bar", "2" }));
            string expectedJsonString = "[[\"foo\",\"1\"],[\"bar\",\"2\"]]";
            string actualJsonString = null;
            Assert.DoesNotThrow(delegate {                
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(Table));
                actualJsonString = (string)converter.ConvertToString(table);
            });
            Assert.That(actualJsonString, Is.EqualTo(expectedJsonString));
        }
    }
}
