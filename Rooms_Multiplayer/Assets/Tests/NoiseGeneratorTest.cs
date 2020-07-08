using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class NoiseGeneratorTest
    {
        [Test]
        public void NoiseGeneratorConstructorExceptions()
        {
            int[] seed = new int[]{0, 10, 20, 30, 40, 50, 60, 70};
            // invalid xMin/xMax
            try
            {
                NoiseGenerator n = new NoiseGenerator(seed, 0, 0, -20, 20, -500, 500, 2, 10);
                Assert.Fail("Should have thrown exception before this");
            }
            catch (System.ArgumentException e)
            {
                Assert.AreEqual("xMin should be less than xMax", e.Message);
            }
            try
            {
                NoiseGenerator n = new NoiseGenerator(seed, 0, -1, -20, 20, -500, 500, 2, 10);
                Assert.Fail("Should have thrown exception before this");
            }
            catch (System.ArgumentException e)
            {
                Assert.AreEqual("xMin should be less than xMax", e.Message);
            }

            // invalid yMin/yMax
            try
            {
                NoiseGenerator n = new NoiseGenerator(seed, -500, 500, 0, 0, -500, 500, 2, 10);
                Assert.Fail("Should have thrown exception before this");
            }
            catch (System.ArgumentException e)
            {
                Assert.AreEqual("yMin should be less than yMax", e.Message);
            }
            try
            {
                NoiseGenerator n = new NoiseGenerator(seed, -500, 500, 0, -1, -500, 500, 2, 10);
                Assert.Fail("Should have thrown exception before this");
            }
            catch (System.ArgumentException e)
            {
                Assert.AreEqual("yMin should be less than yMax", e.Message);
            }

            // invalid zMin/zMax
            try
            {
                NoiseGenerator n = new NoiseGenerator(seed, -500, 500, -20, 20, 0, 0, 2, 10);
                Assert.Fail("Should have thrown exception before this");
            }
            catch (System.ArgumentException e)
            {
                Assert.AreEqual("zMin should be less than zMax", e.Message);
            }
            try
            {
                NoiseGenerator n = new NoiseGenerator(seed, -500, 500, -20, 20, 0, -1, 2, 10);
                Assert.Fail("Should have thrown exception before this");
            }
            catch (System.ArgumentException e)
            {
                Assert.AreEqual("zMin should be less than zMax", e.Message);
            }

            // invalid numStartWorms
            try
            {
                NoiseGenerator n = new NoiseGenerator(seed, -500, 500, -20, 20, -500, 500, -1, 10);
                Assert.Fail("Should have thrown exception before this");
            }
            catch (System.ArgumentException e)
            {
                Assert.AreEqual("numStartWorms must be non-negative", e.Message);
            }

            //invalid numWorms
            try
            {
                NoiseGenerator n = new NoiseGenerator(seed, -500, 500, -20, 20, -500, 500, 2, -1);
                Assert.Fail("Should have thrown exception before this");
            }
            catch (System.ArgumentException e)
            {
                Assert.AreEqual("numWorms must be non-negative", e.Message);
            }
        }
        // Testing Boundaries of Noise Generator
        [Test]
        public void NoiseGeneratorNoiseBoundaries()
        {
            int[] seed = new int[]{0, 10, 20, 30, 40, 50, 60, 70};
            NoiseGenerator n = new NoiseGenerator(seed, -500, 500, -20, 20, -500, 500, 2, 10);
            // check when x is near boundaries
            Assert.AreEqual(n.Noise(-491, 0, 0), -1);
            Assert.AreEqual(n.Noise(-495, 0, 0), -1);
            Assert.AreEqual(n.Noise(-500, 0, 0), -1);
            Assert.AreEqual(n.Noise(-510, 0, 0), -1);
            Assert.AreEqual(n.Noise(491, 0, 0), -1);
            Assert.AreEqual(n.Noise(495, 0, 0), -1);
            Assert.AreEqual(n.Noise(500, 0, 0), -1);
            Assert.AreEqual(n.Noise(510, 0, 0), -1);
            // check when z is near boundaries
            Assert.AreEqual(n.Noise(0, 0, -491), -1);
            Assert.AreEqual(n.Noise(0, 0, -495), -1);
            Assert.AreEqual(n.Noise(0, 0, -500), -1);
            Assert.AreEqual(n.Noise(0, 0, -510), -1);
            Assert.AreEqual(n.Noise(0, 0,  491), -1);
            Assert.AreEqual(n.Noise(0, 0,  495), -1);
            Assert.AreEqual(n.Noise(0, 0,  500), -1);
            Assert.AreEqual(n.Noise(0, 0,  510), -1);
            // mix of x and z's outside of boundary
            Assert.AreEqual(n.Noise(495, 0, -491), -1);
            Assert.AreEqual(n.Noise(500, 0, -495), -1);
            Assert.AreEqual(n.Noise(491, 0, -500), -1);
            Assert.AreEqual(n.Noise(-600, 0, -510), -1);
            Assert.AreEqual(n.Noise(-491, 0,  491), -1);
            Assert.AreEqual(n.Noise(-500, 0,  495), -1);
            Assert.AreEqual(n.Noise(-510, 0,  500), -1);
            Assert.AreEqual(n.Noise(510, 0,  510), -1);
        }
    }
}
