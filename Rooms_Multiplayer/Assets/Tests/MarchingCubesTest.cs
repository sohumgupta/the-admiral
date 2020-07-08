using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class MarchingCubesTest
    {
        // Testing Constructor exceptions in MarchingCubes.
        [Test]
        public void MarchingCubesTestConstructorExceptions()
        {
            // invalid xMin/xMax
            try
            {
                MarchingCubes badMC = new MarchingCubes(0, 0, 0, 500, 0, 500, 5, 0, null);
                Assert.Fail("Should have errored before this");
            }
            catch (System.ArgumentException e)
            {
                Assert.AreEqual("xMin should be less than xMax", e.Message);
            }
            try
            {
                MarchingCubes badMC = new MarchingCubes(0, -1, 0, 500, 0, 500, 5, 0, null);
                Assert.Fail("Should have errored before this");
            }
            catch (System.ArgumentException e)
            {
                Assert.AreEqual("xMin should be less than xMax", e.Message);
            }

            // invalid yMin/yMax
            try
            {
                MarchingCubes badMC = new MarchingCubes(0, 500, 0, 0, 0, 500, 5, 0, null);
                Assert.Fail("Should have errored before this");
            }
            catch (System.ArgumentException e)
            {
                Assert.AreEqual("yMin should be less than yMax", e.Message);
            }
            try
            {
                MarchingCubes badMC = new MarchingCubes(0, 500, 0, -1, 0, 500, 5, 0, null);
                Assert.Fail("Should have errored before this");
            }
            catch (System.ArgumentException e)
            {
                Assert.AreEqual("yMin should be less than yMax", e.Message);
            }

            // invalid xMin/xMax
            try
            {
                MarchingCubes badMC = new MarchingCubes(0, 500, 0, 500, 0, 0, 5, 0, null);
                Assert.Fail("Should have errored before this");
            }
            catch (System.ArgumentException e)
            {
                Assert.AreEqual("zMin should be less than zMax", e.Message);
            }
            try
            {
                MarchingCubes badMC = new MarchingCubes(0, 500, 0, 500, 0, -1, 5, 0, null);
                Assert.Fail("Should have errored before this");
            }
            catch (System.ArgumentException e)
            {
                Assert.AreEqual("zMin should be less than zMax", e.Message);
            }

            // invalid edge length
            try
            {
                MarchingCubes badMC = new MarchingCubes(0, 500, 0, 500, 0, 500, 0, 0, null);
                Assert.Fail("Should have errored before this");
            }
            catch (System.ArgumentException e)
            {
                Assert.AreEqual("edge length should be less than 0", e.Message);
            }
            try
            {
                MarchingCubes badMC = new MarchingCubes(0, 500, 0, 500, 0, 500, -1, 0, null);
                Assert.Fail("Should have errored before this");
            }
            catch (System.ArgumentException e)
            {
                Assert.AreEqual("edge length should be less than 0", e.Message);
            }
        }
    }
}
