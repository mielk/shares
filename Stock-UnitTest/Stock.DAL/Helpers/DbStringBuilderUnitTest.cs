using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.DAL.Helpers;

namespace Stock_UnitTest
{
    [TestClass]
    public class DbStringBuilderUnitTest
    {

        [TestMethod]
        public void getValue_returnProperValue_ForExistingItem()
        {
            DbStringBuilder builder = new DbStringBuilder();
            string key = "a";
            int value = 1;
            string expected = "1";
            builder.Add(key, value);
            string actual = builder.getValue(key);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void getValue_returnEmptyString_ForNonexistingItem()
        {
            DbStringBuilder builder = new DbStringBuilder();
            string key = "a";
            string actual = builder.getValue(key);
            Assert.IsNull(actual);

        }
        
        [TestMethod]
        public void afterAddingElementByKeyAndStringValue_DictionaryHasThisElement()
        {
            DbStringBuilder builder = new DbStringBuilder();
            string key = "key";
            string added = "abc";
            string expected = "'abc'";
            builder.Add(key, added);
            string value = builder.getValue(key);
            Assert.AreEqual(expected, value);
        }

        [TestMethod]
        public void afterAddingElementByKeyAndDoubleValue_DictionaryHasThisElement()
        {
            DbStringBuilder builder = new DbStringBuilder();
            string key = "key";
            double added = 1.23456789;
            string expected = "1.23457";
            builder.Add(key, added);
            string value = builder.getValue(key);
            Assert.AreEqual(expected, value);
        }

        [TestMethod]
        public void afterAddingElementByKeyAndIntValue_DictionaryHasThisElement()
        {
            DbStringBuilder builder = new DbStringBuilder();
            string key = "key";
            int added = 1;
            string expected = "1";
            builder.Add(key, added);
            string value = builder.getValue(key);
            Assert.AreEqual(expected, value);
        }

        [TestMethod]
        public void afterAddingElementByKeyAndBooleanValue_DictionaryHasThisElement()
        {
            DbStringBuilder builder = new DbStringBuilder();
            string key = "key";
            bool added = true;
            string expected = "1";
            builder.Add(key, added);
            string value = builder.getValue(key);
            Assert.AreEqual(expected, value);
        }

        [TestMethod]
        public void afterAddingElementByKeyAndDatetimeValue_DictionaryHasThisElement()
        {
            DbStringBuilder builder = new DbStringBuilder();
            string key = "key";
            DateTime added = new DateTime(2017, 4, 21);
            string expected = "'2017-04-21 00:00:00'";
            builder.Add(key, added);
            string value = builder.getValue(key);
            Assert.AreEqual(expected, value);
        }


        [TestMethod]
        public void afterAddingElementByKeyAndExpression_DictionaryHasThisElement()
        {
            DbStringBuilder builder = new DbStringBuilder();
            string key = "key";
            string expression = "NOW()";
            string expected = "NOW()";
            builder.AddExpression(key, expression);
            string value = builder.getValue(key);
            Assert.AreEqual(expected, value);
        }


        [TestMethod]
        public void afterAddingWhere_HasAddedConstraint_WhereAddedWithStringValue()
        {
            DbStringBuilder builder = new DbStringBuilder();
            string key = "Id";
            string value = "abc";
            builder.AddWhere(key, value);

            string expected = "Id = 'abc'";
            Assert.IsTrue(builder.HasWhere(expected));
        }

        [TestMethod]
        public void afterAddingWhere_HasAddedConstraint_WhereAddedWithDoubleValue()
        {
            DbStringBuilder builder = new DbStringBuilder();
            string key = "Id";
            double value = 1.234567;
            builder.AddWhere(key, value);

            string expected = "Id = 1.23457";
            Assert.IsTrue(builder.HasWhere(expected));
        }

        [TestMethod]
        public void afterAddingWhere_HasAddedConstraint_WhereAddedWithIntValue()
        {
            DbStringBuilder builder = new DbStringBuilder();
            string key = "Id";
            int value = 1;
            builder.AddWhere(key, value);

            string expected = "Id = 1";
            Assert.IsTrue(builder.HasWhere(expected));
        }

        [TestMethod]
        public void afterAddingWhere_HasAddedConstraint_WhereAddedWithDateValue()
        {
            DbStringBuilder builder = new DbStringBuilder();
            string key = "Id";
            DateTime value = new DateTime(2017, 4, 21);
            builder.AddWhere(key, value);

            string expected = "Id = '2017-04-21 00:00:00'";
            Assert.IsTrue(builder.HasWhere(expected));
        }

        [TestMethod]
        public void afterAddingWhere_HasAddedConstraint_WhereAddedWithBooleanValue()
        {
            DbStringBuilder builder = new DbStringBuilder();
            string key = "Id";
            bool value = true;
            builder.AddWhere(key, value);

            string expected = "Id = 1";
            Assert.IsTrue(builder.HasWhere(expected));
        }

        [TestMethod]
        public void afterClearing_dictionaryIsEmpty()
        {
            DbStringBuilder builder = new DbStringBuilder();
            builder.Add("key", "1");
            builder.Clear();
            Assert.AreEqual(0, builder.CountElements());            
        }

        [TestMethod]
        public void generateInsertSqlString_ReturnsProperString_ForSingleElement()
        {
            DbStringBuilder builder = new DbStringBuilder();
            builder.DbAppendix = "fx";
            builder.Add("Value", 1);

            string expected = "INSERT INTO fx.{0}(Value) VALUES(1);";
            Assert.AreEqual(expected, builder.GenerateInsertSqlString());

        }

        [TestMethod]
        public void generateInsertSqlString_ReturnsProperString_ForSingleElementAndEmptyDbAppendix()
        {
            DbStringBuilder builder = new DbStringBuilder();
            builder.DbAppendix = string.Empty;
            builder.Add("Value", 1);

            string expected = "INSERT INTO {0}(Value) VALUES(1);";
            Assert.AreEqual(expected, builder.GenerateInsertSqlString());

        }


        [TestMethod]
        public void generateInsertSqlString_ReturnsProperString_ForManyElements()
        {
            DbStringBuilder builder = new DbStringBuilder();
            builder.DbAppendix = "fx";
            builder.Add("Value", 1);
            builder.Add("Name", "abc");
            builder.Add("Date", new DateTime(2017, 3, 12));
            builder.Add("IsActive", true);

            string expected = "INSERT INTO fx.{0}(Value, Name, Date, IsActive) VALUES(1, 'abc', '2017-03-12 00:00:00', 1);";
            Assert.AreEqual(expected, builder.GenerateInsertSqlString());

        }

        [TestMethod]
        public void generateInsertSqlString_ReturnsProperString_ForManyElementsAndTimestamp()
        {
            DbStringBuilder builder = new DbStringBuilder();
            builder.DbAppendix = "fx";
            builder.Add("Value", 1);
            builder.Add("Name", "abc");
            builder.Add("Date", new DateTime(2017, 3, 12));
            builder.Add("IsActive", true);
            builder.AddTimestamp();

            string expected = "INSERT INTO fx.{0}(Value, Name, Date, IsActive, Timestamp) VALUES(1, 'abc', '2017-03-12 00:00:00', 1, NOW());";
            Assert.AreEqual(expected, builder.GenerateInsertSqlString());

        }





        [TestMethod]
        public void generateUpdateSqlString_ReturnsProperString_ForSingleElement()
        {
            DbStringBuilder builder = new DbStringBuilder();
            builder.DbAppendix = "fx";
            builder.Add("Value", 1);
            builder.AddWhere("Id", 1);

            string expected = "UPDATE fx.{0} SET Value = 1 WHERE Id = 1;";
            Assert.AreEqual(expected, builder.GenerateUpdateSqlString());

        }

        [TestMethod]
        public void generateUpdateSqlString_ReturnsProperString_ForSingleElementAndEmptyDbAppendix()
        {
            DbStringBuilder builder = new DbStringBuilder();
            builder.DbAppendix = string.Empty;
            builder.Add("Value", 1);
            builder.AddWhere("Id", 1);

            string expected = "UPDATE {0} SET Value = 1 WHERE Id = 1;";
            string actual = builder.GenerateUpdateSqlString();
            Assert.AreEqual(expected, actual);

        }




        [TestMethod]
        public void generateUpdateSqlString_ReturnsProperString_ForManyElements()
        {
            DbStringBuilder builder = new DbStringBuilder();
            builder.Add("Value", 1);
            builder.Add("Name", "abc");
            builder.Add("Date", new DateTime(2017, 3, 12));
            builder.Add("IsActive", true);
            builder.AddWhere("Id", 1);

            string expected = "UPDATE {0} SET Value = 1, Name = 'abc', Date = '2017-03-12 00:00:00', IsActive = 1 WHERE Id = 1;";
            string actual = builder.GenerateUpdateSqlString();
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void generateUpdateSqlString_ReturnsProperString_ForManyElementsAndTimestamp()
        {
            DbStringBuilder builder = new DbStringBuilder();
            builder.Add("Value", 1);
            builder.Add("Name", "abc");
            builder.Add("Date", new DateTime(2017, 3, 12));
            builder.Add("IsActive", true);
            builder.AddTimestamp();
            builder.AddWhere("Id", 1);

            string expected = "UPDATE {0} SET Value = 1, Name = 'abc', Date = '2017-03-12 00:00:00', IsActive = 1, Timestamp = NOW() WHERE Id = 1;";
            string actual = builder.GenerateUpdateSqlString();
            Assert.AreEqual(expected, actual);

        }

    }
}
