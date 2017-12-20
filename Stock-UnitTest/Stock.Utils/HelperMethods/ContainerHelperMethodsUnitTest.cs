using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Stock.Utils;

namespace Stock_UnitTest.Stock.Utils.HelperMethods
{

    [TestClass]
    public class ContainerHelperMethodsUnitTest
    {

        #region HAS_THE_SAME_ITEMS

        [TestMethod]
        public void HasTheSameItems_ReturnsTrue_ForEmptyCollections()
        {
            IEnumerable<string> a = new List<string>();
            IEnumerable<string> b = new List<string>();
            bool areEqual = a.HasTheSameItems(b);
            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void HasTheSameItems_ReturnsTrue_ForExactSameCollectionsOfPrimitives()
        {
            int[] a = new int[] { 1, 2, 3 };
            int[] b = new int[] { 1, 2, 3 };
            bool areEqual = a.HasTheSameItems(b);
            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void HasTheSameItems_ReturnsTrue_ForExactSameCollectionsOfObjects()
        {

            //Arrange.
            List<ContainerUnitTestEntity> a = new List<ContainerUnitTestEntity>();
            List<ContainerUnitTestEntity> b = new List<ContainerUnitTestEntity>();

            ContainerUnitTestEntity objA = new ContainerUnitTestEntity { Id = 1, Value = "a" };
            ContainerUnitTestEntity objB = new ContainerUnitTestEntity { Id = 2, Value = "b" };

            //Act.
            a.Add(objA);
            a.Add(objB);

            b.Add(objA);
            b.Add(objB);

            //Assert.
            bool areEqual = a.HasTheSameItems(b);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void HasTheSameItems_ReturnsFalse_IfComparedCollectionHasClonesOfObjects()
        {

            //Arrange.
            List<ContainerUnitTestEntity> a = new List<ContainerUnitTestEntity>();
            List<ContainerUnitTestEntity> b = new List<ContainerUnitTestEntity>();

            //Act.
            a.Add(new ContainerUnitTestEntity { Id = 1, Value = "a" });
            a.Add(new ContainerUnitTestEntity { Id = 2, Value = "b" });
            b.Add(new ContainerUnitTestEntity { Id = 1, Value = "a" });
            b.Add(new ContainerUnitTestEntity { Id = 2, Value = "b" });

            //Assert.
            bool areEqual = a.HasTheSameItems(b);
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void HasTheSameItems_ReturnsTrue_ForCollectionsWithSameItemsInAnotherOrder()
        {
            int[] a = new int[] { 1, 2, 3 };
            int[] b = new int[] { 3, 1, 2 };
            bool areEqual = a.HasTheSameItems(b);
            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void HasTheSameItems_ReturnsFalse_IfOneCollectionHasDuplicatedItem()
        {
            int[] a = new int[] { 1, 2, 3 };
            int[] b = new int[] { 3, 1, 2, 1 };
            bool areEqual = a.HasTheSameItems(b);
            Assert.IsFalse(areEqual);
        }

        #endregion HAS_THE_SAME_ITEMS


        #region HAS_EQUAL_ITEMS

        [TestMethod]
        public void HasEqualItems_ReturnsTrue_ForEmptyCollections()
        {

            //Arrange.
            IEnumerable<string> a = new List<string>();
            IEnumerable<string> b = new List<string>();

            //Act.
            bool areEqual = a.HasEqualItems(b);

            //Assert.
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void HasEqualItems_ReturnsTrue_ForExactSameCollectionsOfObjects()
        {

            //Arrange.
            List<ContainerUnitTestEntity> a = new List<ContainerUnitTestEntity>();
            List<ContainerUnitTestEntity> b = new List<ContainerUnitTestEntity>();

            ContainerUnitTestEntity objA = new ContainerUnitTestEntity { Id = 1, Value = "a" };
            ContainerUnitTestEntity objB = new ContainerUnitTestEntity { Id = 2, Value = "b" };

            //Act.
            a.Add(objA);
            a.Add(objB);

            b.Add(objA);
            b.Add(objB);

            //Assert.
            bool areEqual = a.HasEqualItems(b);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void HasEqualItems_ReturnsTrue_IfComparedCollectionHasClonesOfObjects()
        {

            //Arrange.
            List<ContainerUnitTestEntity> a = new List<ContainerUnitTestEntity>();
            List<ContainerUnitTestEntity> b = new List<ContainerUnitTestEntity>();

            //Act.
            a.Add(new ContainerUnitTestEntity { Id = 1, Value = "a" });
            a.Add(new ContainerUnitTestEntity { Id = 2, Value = "b" });
            b.Add(new ContainerUnitTestEntity { Id = 1, Value = "a" });
            b.Add(new ContainerUnitTestEntity { Id = 2, Value = "b" });

            //Assert.
            bool areEqual = a.HasEqualItems(b);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void HasEqualItems_ReturnsTrue_ForCollectionsWithSameItemsInAnotherOrder()
        {

            //Arrange.
            List<ContainerUnitTestEntity> a = new List<ContainerUnitTestEntity>();
            List<ContainerUnitTestEntity> b = new List<ContainerUnitTestEntity>();

            //Act.
            a.Add(new ContainerUnitTestEntity { Id = 1, Value = "a" });
            a.Add(new ContainerUnitTestEntity { Id = 2, Value = "b" });
            a.Add(new ContainerUnitTestEntity { Id = 3, Value = "c" });
            b.Add(new ContainerUnitTestEntity { Id = 3, Value = "c" });
            b.Add(new ContainerUnitTestEntity { Id = 2, Value = "b" });
            b.Add(new ContainerUnitTestEntity { Id = 1, Value = "a" });

            //Assert.
            bool areEqual = a.HasEqualItems(b);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void HasEqualItems_ReturnsFalse_IfOneCollectionHasDuplicatedItem()
        {

            //Arrange.
            List<ContainerUnitTestEntity> a = new List<ContainerUnitTestEntity>();
            List<ContainerUnitTestEntity> b = new List<ContainerUnitTestEntity>();

            //Act.
            a.Add(new ContainerUnitTestEntity { Id = 1, Value = "a" });
            a.Add(new ContainerUnitTestEntity { Id = 2, Value = "b" });
            a.Add(new ContainerUnitTestEntity { Id = 3, Value = "c" });

            b.Add(new ContainerUnitTestEntity { Id = 3, Value = "c" });
            b.Add(new ContainerUnitTestEntity { Id = 1, Value = "a" });
            b.Add(new ContainerUnitTestEntity { Id = 2, Value = "b" });
            b.Add(new ContainerUnitTestEntity { Id = 1, Value = "a" });

            //Assert.
            bool areEqual = a.HasEqualItems(b);
            Assert.IsFalse(areEqual);
        }

        #endregion HAS_EQUAL_ITEMS


        #region HAS_EQUAL_ITEMS_IN_THE_SAME_ORDER

        [TestMethod]
        public void HasEqualItemsInTheSameOrder_ReturnsFalse_IfArrayAreOfDifferentLength()
        {

            //Arrange.
            ContainerUnitTestEntity[] a = new ContainerUnitTestEntity[3];
            ContainerUnitTestEntity[] b = new ContainerUnitTestEntity[4];

            //Act.
            bool areEqual = a.HasEqualItemsInTheSameOrder(b);

            //Assert.
            Assert.IsFalse(areEqual);
        }

        [TestMethod]
        public void HasEqualItemsInTheSameOrder_ReturnsFalse_IfSomeItemsAreOnDifferentPlace()
        {

            //Arrange.
            ContainerUnitTestEntity[] a = new ContainerUnitTestEntity[3];
            a[0] = new ContainerUnitTestEntity() { Id = 1, Value = "a" };
            a[1] = new ContainerUnitTestEntity() { Id = 2, Value = "b" };
            a[2] = new ContainerUnitTestEntity() { Id = 3, Value = "c" };

            ContainerUnitTestEntity[] b = new ContainerUnitTestEntity[3];
            b[0] = new ContainerUnitTestEntity() { Id = 1, Value = "a" };
            b[1] = new ContainerUnitTestEntity() { Id = 3, Value = "c" };
            b[2] = new ContainerUnitTestEntity() { Id = 2, Value = "b" };

            //Act.
            bool areEqual = a.HasEqualItemsInTheSameOrder(b);

            //Assert.
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void HasEqualItemsInTheSameOrder_ReturnsFalse_IfSomeItemsAreDifferent()
        {

            //Arrange.
            ContainerUnitTestEntity[] a = new ContainerUnitTestEntity[3];
            a[0] = new ContainerUnitTestEntity() { Id = 1, Value = "a" };
            a[1] = new ContainerUnitTestEntity() { Id = 2, Value = "b" };
            a[2] = new ContainerUnitTestEntity() { Id = 3, Value = "c" };

            ContainerUnitTestEntity[] b = new ContainerUnitTestEntity[3];
            b[0] = new ContainerUnitTestEntity() { Id = 1, Value = "a" };
            b[1] = new ContainerUnitTestEntity() { Id = 2, Value = "b" };
            b[2] = new ContainerUnitTestEntity() { Id = 4, Value = "d" };

            //Act.
            bool areEqual = a.HasEqualItemsInTheSameOrder(b);

            //Assert.
            Assert.IsFalse(areEqual);

        }


        [TestMethod]
        public void HasEqualItemsInTheSameOrder_ReturnsFalse_IfHaveAllTheSameButOneArrayHasEmptySlotAtEnd()
        {

            //Arrange.
            ContainerUnitTestEntity[] a = new ContainerUnitTestEntity[3];
            a[0] = new ContainerUnitTestEntity() { Id = 1, Value = "a" };
            a[1] = new ContainerUnitTestEntity() { Id = 2, Value = "b" };
            a[2] = new ContainerUnitTestEntity() { Id = 3, Value = "c" };

            ContainerUnitTestEntity[] b = new ContainerUnitTestEntity[4];
            b[0] = new ContainerUnitTestEntity() { Id = 1, Value = "a" };
            b[1] = new ContainerUnitTestEntity() { Id = 2, Value = "b" };
            b[2] = new ContainerUnitTestEntity() { Id = 3, Value = "c" };

            //Act.
            bool areEqual = a.HasEqualItemsInTheSameOrder(b);

            //Assert.
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void HasEqualItemsInTheSameOrder_ReturnsTrue_IfAllItemsAreEqual()
        {

            //Arrange.
            ContainerUnitTestEntity[] a = new ContainerUnitTestEntity[3];
            a[0] = new ContainerUnitTestEntity() { Id = 1, Value = "a" };
            a[1] = new ContainerUnitTestEntity() { Id = 2, Value = "b" };
            a[2] = new ContainerUnitTestEntity() { Id = 3, Value = "c" };

            ContainerUnitTestEntity[] b = new ContainerUnitTestEntity[3];
            b[0] = new ContainerUnitTestEntity() { Id = 1, Value = "a" };
            b[1] = new ContainerUnitTestEntity() { Id = 2, Value = "b" };
            b[2] = new ContainerUnitTestEntity() { Id = 3, Value = "c" };

            //Act.
            bool areEqual = a.HasEqualItemsInTheSameOrder(b);

            //Assert.
            Assert.IsTrue(areEqual);

        }

        #endregion HAS_EQUAL_ITEMS_IN_THE_SAME_ORDER


        #region DICTIONARY.HAS.THE.SAME.VALUES

        [TestMethod]
        public void DictionaryHasEqualValues_ReturnsFalse_IfDictionaryHasDifferentNumberOfItems()
        {

            //Arrange.
            Dictionary<int, ContainerUnitTestEntity> a = new Dictionary<int, ContainerUnitTestEntity>();
            Dictionary<int, ContainerUnitTestEntity> b = new Dictionary<int, ContainerUnitTestEntity>();

            //Act.
            a.Add(1, new ContainerUnitTestEntity() { Id = 1, Value = "a" });
            a.Add(2, new ContainerUnitTestEntity() { Id = 2, Value = "b" });

            b.Add(1, new ContainerUnitTestEntity() { Id = 1, Value = "a" });
            b.Add(2, new ContainerUnitTestEntity() { Id = 2, Value = "b" });
            b.Add(3, new ContainerUnitTestEntity() { Id = 3, Value = "c" });

            bool areEqual = a.HasTheSameValues(b);

            //Assert.
            Assert.IsFalse(areEqual);
        }

        [TestMethod]
        public void DictionaryHasEqualValues_ReturnsFalse_IfSomeItemInDictionaryHasDifferentValue()
        {

            //Arrange.
            Dictionary<int, ContainerUnitTestEntity> a = new Dictionary<int, ContainerUnitTestEntity>();
            Dictionary<int, ContainerUnitTestEntity> b = new Dictionary<int, ContainerUnitTestEntity>();

            //Act.
            a.Add(1, new ContainerUnitTestEntity() { Id = 1, Value = "a" });
            a.Add(2, new ContainerUnitTestEntity() { Id = 2, Value = "b" });

            b.Add(1, new ContainerUnitTestEntity() { Id = 1, Value = "a" });
            b.Add(2, new ContainerUnitTestEntity() { Id = 2, Value = "c" });

            bool areEqual = a.HasTheSameValues(b);

            //Assert.
            Assert.IsFalse(areEqual);
        }


        [TestMethod]
        public void DictionaryHasEqualValues_ReturnsFalse_IfDictionariesHaveMixedValues()
        {

            //Arrange.
            Dictionary<int, ContainerUnitTestEntity> a = new Dictionary<int, ContainerUnitTestEntity>();
            Dictionary<int, ContainerUnitTestEntity> b = new Dictionary<int, ContainerUnitTestEntity>();

            //Act.
            a.Add(1, new ContainerUnitTestEntity() { Id = 1, Value = "a" });
            a.Add(2, new ContainerUnitTestEntity() { Id = 2, Value = "b" });

            b.Add(1, new ContainerUnitTestEntity() { Id = 2, Value = "b" });
            b.Add(2, new ContainerUnitTestEntity() { Id = 1, Value = "a" });
            

            bool areEqual = a.HasTheSameValues(b);

            //Assert.
            Assert.IsFalse(areEqual);
        }


        [TestMethod]
        public void DictionaryHasEqualValues_ReturnsTrue_IfDictionariesHaveTheSameKeysWithTheSameValues()
        {

            //Arrange.
            Dictionary<int, ContainerUnitTestEntity> a = new Dictionary<int, ContainerUnitTestEntity>();
            Dictionary<int, ContainerUnitTestEntity> b = new Dictionary<int, ContainerUnitTestEntity>();

            //Act.
            a.Add(1, new ContainerUnitTestEntity() { Id = 1, Value = "a" });
            a.Add(2, new ContainerUnitTestEntity() { Id = 2, Value = "b" });

            b.Add(1, new ContainerUnitTestEntity() { Id = 1, Value = "a" });
            b.Add(2, new ContainerUnitTestEntity() { Id = 2, Value = "b" });

            bool areEqual = a.HasTheSameValues(b);

            //Assert.
            Assert.IsTrue(areEqual);
        }


        #endregion DICTIONARY.HAS.THE.SAME.VALUES

    }

    class ContainerUnitTestEntity
    {
        public int Id { get; set; }
        public string Value { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(ContainerUnitTestEntity)) return false;

            ContainerUnitTestEntity compared = (ContainerUnitTestEntity)obj;
            if ((compared.Id) != Id) return false;
            if (!compared.Value.Equals(Value)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + Id.GetHashCode();
            hash = (hash * 7) + Value.GetHashCode();
            return hash;
        }

    }

}