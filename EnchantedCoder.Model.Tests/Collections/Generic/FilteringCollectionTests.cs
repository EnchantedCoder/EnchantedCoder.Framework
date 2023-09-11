﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using EnchantedCoder.Model.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnchantedCoder.Model.Tests.Collections.Generic
{
	[TestClass]
	public class FilteringCollectionTests
	{
		[TestMethod]
		public void FilteringCollection_List_Add()
		{
			// Arrange
			var list = new List<int>();
			var filteringCollection = new FilteringCollection<int>(list, i => i % 2 == 0);

			// Act
			filteringCollection.Add(1);
			filteringCollection.Add(2);

			// Assert
			Assert.IsTrue(list.Contains(1));
			Assert.IsTrue(list.Contains(2));
		}

		[TestMethod]
		public void FilteringCollection_List_AddRange()
		{
			// Arrange
			var list = new List<int>();
			var filteringCollection = new FilteringCollection<int>(list, i => i % 2 == 0);

			// Act
			filteringCollection.AddRange(new List<int> { 1, 2 });

			// Assert
			Assert.IsTrue(list.Contains(1));
			Assert.IsTrue(list.Contains(2));
		}

		[TestMethod]
		public void FilteringCollection_List_Clear()
		{
			// Arrange
			var list = new List<int>() { 1, 2, 3, 4 };
			var filteringCollection = new FilteringCollection<int>(list, i => i % 2 == 0);

			// Act
			filteringCollection.Clear();

			// Assert
			Assert.AreEqual(0, list.Count);
		}
		[TestMethod]
		public void FilteringCollection_List_Contains()
		{
			// Arrange
			var filteringCollection = new FilteringCollection<int>(new List<int> { 1, 2, 3, 4 }, i => i % 2 == 0);

			// Act + Assert
			Assert.IsFalse(filteringCollection.Contains(1));
			Assert.IsTrue(filteringCollection.Contains(2));
		}

		[TestMethod]
		public void FilteringCollection_List_Count()
		{
			// Arrange
			var filteringCollection = new FilteringCollection<int>(new List<int> { 1, 2, 3, 4 }, i => i % 2 == 0);

			// Act + Assert
			Assert.AreEqual(2, filteringCollection.Count);
		}


		[TestMethod]
		public void FilteringCollection_List_ForEach()
		{
			// Arrange
			var filteringCollection = new FilteringCollection<int>(new List<int> { 1, 2, 3, 4 }, i => i % 2 == 0);

			// Act
			int sum = 0;
			filteringCollection.ForEach(i => sum += i);

			// Assert
			Assert.AreEqual(6, sum);
		}

		[TestMethod]
		public void FilteringCollection_List_Remove()
		{
			// Arrange
			var list = new List<int>() { 1, 2, 3, 4 };
			var filteringCollection = new FilteringCollection<int>(list, i => i % 2 == 0);

			// Act
			filteringCollection.Remove(1);
			filteringCollection.Remove(2);

			// Assert
			Assert.IsFalse(list.Contains(1));
			Assert.IsFalse(list.Contains(2));
			Assert.IsTrue(list.Contains(3));
			Assert.IsTrue(list.Contains(4));
		}

		[TestMethod]
		public void FilteringCollection_List_RemoveAll()
		{
			// Arrange
			var list = new List<int>() { 1, 2, 3, 4 };
			var filteringCollection = new FilteringCollection<int>(list, i => i % 2 == 0);

			// Act
			var result = filteringCollection.RemoveAll(i => i < 3);

			// Assert
			Assert.AreEqual(2, result);
			Assert.IsFalse(list.Contains(1));
			Assert.IsFalse(list.Contains(2));
			Assert.IsTrue(list.Contains(3));
			Assert.IsTrue(list.Contains(4));
		}

		[TestMethod]
		public void FilteringCollection_ObservableCollection_Add()
		{
			// Arrange
			var list = new ObservableCollection<int>();
			var filteringCollection = new FilteringCollection<int>(list, i => i % 2 == 0);

			// Act
			filteringCollection.Add(1);
			filteringCollection.Add(2);

			// Assert
			Assert.IsTrue(list.Contains(1));
			Assert.IsTrue(list.Contains(2));
		}

		[TestMethod]
		public void FilteringCollection_ObservableCollection_AddRange()
		{
			// Arrange
			var list = new ObservableCollection<int>();
			var filteringCollection = new FilteringCollection<int>(list, i => i % 2 == 0);

			// Act
			filteringCollection.AddRange(new ObservableCollection<int> { 1, 2 });

			// Assert
			Assert.IsTrue(list.Contains(1));
			Assert.IsTrue(list.Contains(2));
		}

		[TestMethod]
		public void FilteringCollection_ObservableCollection_Clear()
		{
			// Arrange
			var list = new ObservableCollection<int>() { 1, 2, 3, 4 };
			var filteringCollection = new FilteringCollection<int>(list, i => i % 2 == 0);

			// Act
			filteringCollection.Clear();

			// Assert
			Assert.AreEqual(0, list.Count);
		}
		[TestMethod]
		public void FilteringCollection_ObservableCollection_Contains()
		{
			// Arrange
			var filteringCollection = new FilteringCollection<int>(new ObservableCollection<int> { 1, 2, 3, 4 }, i => i % 2 == 0);

			// Act + Assert
			Assert.IsFalse(filteringCollection.Contains(1));
			Assert.IsTrue(filteringCollection.Contains(2));
		}

		[TestMethod]
		public void FilteringCollection_ObservableCollection_Count()
		{
			// Arrange
			var filteringCollection = new FilteringCollection<int>(new ObservableCollection<int> { 1, 2, 3, 4 }, i => i % 2 == 0);

			// Act + Assert
			Assert.AreEqual(2, filteringCollection.Count);
		}


		[TestMethod]
		public void FilteringCollection_ObservableCollection_ForEach()
		{
			// Arrange
			var filteringCollection = new FilteringCollection<int>(new ObservableCollection<int> { 1, 2, 3, 4 }, i => i % 2 == 0);

			// Act
			int sum = 0;
			filteringCollection.ForEach(i => sum += i);

			// Assert
			Assert.AreEqual(6, sum);
		}

		[TestMethod]
		public void FilteringCollection_ObservableCollection_Remove()
		{
			// Arrange
			var list = new ObservableCollection<int>() { 1, 2, 3, 4 };
			var filteringCollection = new FilteringCollection<int>(list, i => i % 2 == 0);

			// Act
			filteringCollection.Remove(1);
			filteringCollection.Remove(2);

			// Assert
			Assert.IsFalse(list.Contains(1));
			Assert.IsFalse(list.Contains(2));
			Assert.IsTrue(list.Contains(3));
			Assert.IsTrue(list.Contains(4));
		}

		[TestMethod]
		public void FilteringCollection_ObservableCollection_RemoveAll()
		{
			// Arrange
			var list = new ObservableCollection<int>() { 1, 2, 3, 4 };
			var filteringCollection = new FilteringCollection<int>(list, i => i % 2 == 0);

			// Act
			var result = filteringCollection.RemoveAll(i => i < 3);

			// Assert
			Assert.AreEqual(2, result);
			Assert.IsFalse(list.Contains(1));
			Assert.IsFalse(list.Contains(2));
			Assert.IsTrue(list.Contains(3));
			Assert.IsTrue(list.Contains(4));
		}

	}
}
