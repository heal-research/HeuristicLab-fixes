﻿#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2010 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
 *
 * This file is part of HeuristicLab.
 *
 * HeuristicLab is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * HeuristicLab is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with HeuristicLab. If not, see <http://www.gnu.org/licenses/>.
 */
#endregion

using Microsoft.VisualStudio.TestTools.UnitTesting;
using HeuristicLab.Encodings.IntegerVectorEncoding;
using HeuristicLab.Data;

namespace HeuristicLab.Encodings.IntegerVectorEncoding_33.Tests {
  /// <summary>
  ///This is a test class for UniformOnePositionManipulator and is intended
  ///to contain all UniformOnePositionManipulator Unit Tests
  ///</summary>
  [TestClass()]
  public class UniformOnePositionManipulatorTest {


    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext {
      get {
        return testContextInstance;
      }
      set {
        testContextInstance = value;
      }
    }

    #region Additional test attributes
    // 
    //You can use the following additional attributes as you write your tests:
    //
    //Use ClassInitialize to run code before running the first test in the class
    //[ClassInitialize()]
    //public static void MyClassInitialize(TestContext testContext)
    //{
    //}
    //
    //Use ClassCleanup to run code after all tests in a class have run
    //[ClassCleanup()]
    //public static void MyClassCleanup()
    //{
    //}
    //
    //Use TestInitialize to run code before running each test
    //[TestInitialize()]
    //public void MyTestInitialize()
    //{
    //}
    //
    //Use TestCleanup to run code after each test has run
    //[TestCleanup()]
    //public void MyTestCleanup()
    //{
    //}
    //
    #endregion


    /// <summary>
    ///A test for Apply
    ///</summary>
    [TestMethod()]
    public void UniformOnePositionManipulatorApplyTest() {
      TestRandom random = new TestRandom();
      IntegerVector parent, expected;
      IntValue min, max;
      // The following test is not based on published examples
      random.Reset();
      random.IntNumbers = new int[] { 3, 3 };
      parent = new IntegerVector(new int[] { 2, 2, 3, 5, 1 });
      expected = new IntegerVector(new int[] { 2, 2, 3, 3, 1 });
      min = new IntValue(2);
      max = new IntValue(7);
      UniformOnePositionManipulator.Apply(random, parent, min, max);
      Assert.IsTrue(Auxiliary.IntVectorIsEqualByPosition(expected, parent));
    }

    /// <summary>
    ///A test for UniformOnePositionManipulator Constructor
    ///</summary>
    [TestMethod()]
    public void UniformOnePositionManipulatorConstructorTest() {
      UniformOnePositionManipulator target = new UniformOnePositionManipulator();
    }
  }
}
