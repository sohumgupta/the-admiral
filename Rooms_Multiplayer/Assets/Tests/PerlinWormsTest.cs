using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PerlinWormsTest
    {
        // Test Constructor exceptions for PerlinWorms class.
        [Test]
        public void PerlinWormsTestConstructorExceptions()
        {
            int[] seed = new int[]{500, 500, 500, 500, 500, 500, 500, 500};

            // invalid xMin/xMax
            try
            {
                PerlinWorms pw = new PerlinWorms(seed, 0, 0, -500, 500, 2, 5);
                Assert.Fail("Should have thrown exception before this");
            }
            catch (System.ArgumentException e)
            {
                Assert.AreEqual("xMin should be less than xMax", e.Message);
            }
            try
            {
                PerlinWorms pw = new PerlinWorms(seed, 0, -1, -500, 500, 2, 5);
                Assert.Fail("Should have thrown exception before this");
            }
            catch (System.ArgumentException e)
            {
                Assert.AreEqual("xMin should be less than xMax", e.Message);
            }

            // invalid zMin/zMax
            try
            {
                PerlinWorms pw = new PerlinWorms(seed, -500, 500, 0, 0, 2, 5);
                Assert.Fail("Should have thrown exception before this");
            }
            catch (System.ArgumentException e)
            {
                Assert.AreEqual("zMin should be less than zMax", e.Message);
            }
            try
            {
                PerlinWorms pw = new PerlinWorms(seed, -500, 500, 0, -1, 2, 5);
                Assert.Fail("Should have thrown exception before this");
            }
            catch (System.ArgumentException e)
            {
                Assert.AreEqual("zMin should be less than zMax", e.Message);
            }

            // invalid numStartWorms
            try
            {
                PerlinWorms pw = new PerlinWorms(seed, -500, 500, -500, 500, -1, 5);
                Assert.Fail("Should have thrown exception before this");
            }
            catch (System.ArgumentException e)
            {
                Assert.AreEqual("numStartWorms must be non-negative", e.Message);
            }

            // invalid numWorms
            try
            {
                PerlinWorms pw = new PerlinWorms(seed, -500, 500, -500, 500, 2, -1);
                Assert.Fail("Should have thrown exception before this");
            }
            catch (System.ArgumentException e)
            {
                Assert.AreEqual("numWorms must be non-negative", e.Message);
            }
        }

        // Test the creation of worms in the PerlinWorms class.
        [Test]
        public void PerlinWormsTestWormCreation()
        {
            int[] seed = new int[]{500, 500, 500, 500, 500, 500, 500, 500};
            PerlinWorms pw = new PerlinWorms(seed, -500, 500, -500, 500, 2, 5);
            Worm[] worms = pw.GetWorms();
            // ensure that each worm starts at 0, 0, 0, and the rest of the
            // vertices for each worm are not 0, 0, 0
            for (int i = 0; i < worms.Length; i++)
            {
                Worm worm = worms[i];
                Vector3[] vertices = worm.GetVertices();
                Assert.AreEqual(vertices[0], new Vector3(0, 0, 0));
                for (int j = 1; j < vertices.Length; j++)
                {
                    Assert.AreNotEqual(vertices[j], new Vector3(0, 0, 0));
                }
            }
        }

        // Test the NoiseTable functionality.
        [Test]
        public void PerlinWormsTestNoiseTable()
        {
            int[] seed = new int[]{0, 20, 40, 60, 80, 100, 120, 140};
            PerlinWorms pw = new PerlinWorms(seed, -500, 500, -500, 500, 2, 5);
            int[,] noiseTable = pw.NoiseTable();
            int xDim = noiseTable.GetLength(0);
            int yDim = noiseTable.GetLength(1);
            Assert.AreEqual(xDim, 1000);
            Assert.AreEqual(yDim, 1000);
            // 500, 500 is where the worm should have started
            Assert.AreEqual(noiseTable[500, 500], 1);

            // testing different boundaries
            pw = new PerlinWorms(seed, 0, 250, -100, 1000, 2, 5);
            noiseTable = pw.NoiseTable();
            xDim = noiseTable.GetLength(0);
            yDim = noiseTable.GetLength(1);
            Assert.AreEqual(xDim, 250);
            Assert.AreEqual(yDim, 1100);
            // 0, 100 is where the worm should have started
            Assert.AreEqual(noiseTable[0, 100], 1);
        }
    }
}
