using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D3Sharp.Test
{
    public class Tests
    {
        public static void AreValuesEqual(double[,] expected, double[,] actual)
        {
            try
            {
                if (expected == actual)
                    return;
                var exValues = new List<double>();
                var acValues = new List<double>();

                foreach (var item in expected)
                    exValues.Add(item);
                foreach (var item in actual)
                    acValues.Add(item);

                for (int i = 0; i < exValues.Count; i++)
                {
                    if (exValues[i] != acValues[i])
                        throw new Exception($" {exValues[i]}!={acValues[i]}");
                }
            }
            catch (Exception ex)
            {
                throw new AssertFailedException("double[,] not equal", ex);
            }

        }

        public static void AreValuesEqual<T>(List<T> expected, List<T> actiual)
        {
            try
            {
                for (int i = 0; i < expected.Count; i++)
                {
                    if (!expected[i].Equals(actiual[i]))
                        throw new Exception($" {expected[i]}!={actiual[i]}");
                }
            }
            catch (Exception ex)
            {
                throw new AssertFailedException("double[,] not equal", ex);
            }
        }

        public static void AreValuesEqual<T>(T[] expected, T[] actiual)
        {
            try
            {
                for (int i = 0; i < expected.Length; i++)
                {
                    if (!expected[i].Equals(actiual[i]))
                        throw new Exception($" {expected[i]}!={actiual[i]}");
                }
            }
            catch (Exception ex)
            {
                throw new AssertFailedException("double[,] not equal", ex);
            }
        }
    }
}
