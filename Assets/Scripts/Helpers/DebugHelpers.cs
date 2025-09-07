/*****************************************************************************
// File Name : DebugHelpers.cs
// Author : Brandon Koederitz
// Creation Date : September 7, 2025
// Last Modified : September 7, 2025
//
// Brief Description : Set of static helper functions for debugging
// that have common utility across multiple projects.
*****************************************************************************/
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DebugHelpers
{
    /// <summary>
    /// Prints a collection of values to the console.
    /// </summary>
    /// <typeparam name="T">The type of the collection.</typeparam>
    /// <param name="collection">The collection to print.</param>
    /// <param name="collectionName">A specific name that can be used to identify the collection.</param>
    public static void LogCollection<T>(IEnumerable<T> collection, string collectionName = "")
    {
        T[] array = collection.ToArray();
        if (collectionName == "")
        {
            collectionName = collection.ToString();
        }
        for (int i = 0; i < array.Length; i++)
        {
            Debug.Log($"Collection {collectionName} contains item {array[i]} at index {i}.");
        }
    }
}
