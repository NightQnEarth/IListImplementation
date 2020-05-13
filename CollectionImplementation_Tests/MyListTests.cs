using System;
using System.Collections.Generic;
using System.Linq;
using CollectionImplementation;
using FluentAssertions;
using NUnit.Framework;

namespace CollectionImplementation_Tests
{
    [TestFixture]
    public class MyListTests
    {
        private MyList<int> myList;

        [SetUp]
        public void SetUp() => myList = new MyList<int>();

        [Test]
        public void IsReadOnly_Always_False() => myList.IsReadOnly.Should().BeFalse();

        [Test]
        public void Count_InNewInstance_IsZero() => myList.Count.Should().Be(0);

        [Test]
        public void Add_OneItem_MyListShouldContainsThisItem()
        {
            myList.Add(1);

            myList.Should().ContainSingle(item => item == 1);
        }

        [Test]
        public void Count_AfterAddingFirstItem_IsOne()
        {
            myList.Add(1);

            myList.Should().HaveCount(1);
        }

        [Test]
        public void Add_ItemsRange_MyListShouldContainsThisRange()
        {
            var randomSequence = GetRandomSequence(TestContext.CurrentContext.Random).ToArray();

            foreach (var item in randomSequence)
                myList.Add(item);

            myList.Should().Equal(randomSequence);
        }

        [Test]
        public void Clear_ItemsRange_MyListShouldBeEmpty()
        {
            foreach (var item in GetRandomSequence(TestContext.CurrentContext.Random))
                myList.Add(item);

            myList.Clear();

            myList.Should().BeEmpty();
        }

        [Test]
        public void Contains_AddedItem_ShouldReturnTrue()
        {
            for (int i = 0; i < 100; i++)
                myList.Add(i);

            myList.Contains(40).Should().BeTrue();
        }

        [Test]
        public void CopyTo_ThrowsArgumentNullException_OnNullDestinationArray()
        {
            foreach (var item in GetRandomSequence(TestContext.CurrentContext.Random))
                myList.Add(item);

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => myList.CopyTo(null, 0));
        }

        [Test]
        public void CopyTo_ThrowsArgumentOutOfRangeException_OnNegativeArrayIndex()
        {
            foreach (var item in GetRandomSequence(TestContext.CurrentContext.Random))
                myList.Add(item);

            var destinationArray = new int[myList.Count];

            Assert.Throws<ArgumentOutOfRangeException>(() => myList.CopyTo(destinationArray, -1));
        }

        [Test]
        public void CopyTo_ThrowsArgumentException_OnNotEnoughAvailableSpace()
        {
            foreach (var item in GetRandomSequence(TestContext.CurrentContext.Random))
                myList.Add(item);

            var destinationArray = new int[myList.Count];

            Assert.Throws<ArgumentException>(() => myList.CopyTo(destinationArray, 1));
        }

        [Test]
        public void CopyTo_ItemsRange()
        {
            foreach (var item in GetRandomSequence(TestContext.CurrentContext.Random))
                myList.Add(item);

            var destinationArray = new int[myList.Count];

            myList.CopyTo(destinationArray, 0);

            myList.Should().BeEquivalentTo(destinationArray);
        }

        [Test]
        public void Remove_SomeItems_MyListNotContainsRemovedItems()
        {
            for (int i = 0; i < 100; i++)
                myList.Add(i);

            var itemsToRemove = new[] { 0, 4, 29, 47, 100 };

            foreach (var item in itemsToRemove)
                myList.Remove(item);

            myList.Should().NotContain(itemsToRemove);
        }

        [Test]
        public void IndexOf_SomeOfItems()
        {
            for (int i = 10; i < 100; i++)
                myList.Add(i);

            var randomItem = TestContext.CurrentContext.Random.Next(10, 100);

            myList.IndexOf(randomItem).Should().Be(randomItem - 10);
        }

        [Test]
        public void IndexOf_InvalidItem_ShouldBeNegative()
        {
            for (int i = 0; i < 100; i++)
                myList.Add(i);

            myList.IndexOf(100).Should().BeNegative();
        }

        [Test]
        public void Insert_NewItem_SequenceShouldShiftToTheRight()
        {
            for (int i = 0; i < 10; i++)
                myList.Add(i);

            myList.Insert(5, 1618);

            myList.Should().Equal(0, 1, 2, 3, 4, 1618, 5, 6, 7, 8, 9);
        }

        [Test]
        public void Insert_ThrowsArgumentOutOfRangeException_OnInvalidIndex([Values(-1, 1618)] int invalidIndex)
        {
            for (int i = 0; i < 10; i++)
                myList.Add(i);

            Assert.Throws<ArgumentOutOfRangeException>(() => myList.Insert(10, invalidIndex));
        }

        [Test]
        public void RemoveAt_SomeItems_MyListNotContainsRemovedItems()
        {
            for (int i = 10; i < 100; i++)
                myList.Add(i);

            var randomIndex = TestContext.CurrentContext.Random.Next(10, 90);

            myList.RemoveAt(randomIndex);

            myList.Should().NotContain(randomIndex + 10);
        }

        [Test]
        public void RemoveAt_ThrowsArgumentOutOfRangeException_OnInvalidIndex([Values(-1, 1618)] int invalidIndex)
        {
            for (int i = 0; i < 10; i++)
                myList.Add(i);

            Assert.Throws<ArgumentOutOfRangeException>(() => myList.RemoveAt(invalidIndex));
        }

        private static IEnumerable<int> GetRandomSequence(Random randomizer, int from = 2, int to = 500)
        {
            var randomCount = randomizer.Next(from, to);

            while (randomCount > 0)
            {
                yield return randomizer.Next();
                randomCount--;
            }
        }
    }
}