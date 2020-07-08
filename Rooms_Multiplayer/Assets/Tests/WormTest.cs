using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class WormTest
    {
        // Testing GenerateVertices method in Worm class.
        [Test]
        public void WormTestGenerateVertices()
        {
            Worm worm = new Worm(0, 50, 0, 50, false);
            Vector3[] vertices = worm.GetVertices();
            // checking that the vectors are all initially 0, 0, 0
            for (int i = 0; i < vertices.Length; i++)
            {
                Assert.AreEqual(vertices[i], new Vector3(0, 0, 0));
            }
            worm.GenerateVertices();
            vertices = worm.GetVertices();
            // after building the worms new vertices, check that the
            // initial point still starts at 0, 0, 0
            Assert.AreEqual(vertices[0], new Vector3(0, 0, 0));
            // check that the rest of the vertices have changed from 0, 0, 0
            for (int i = 1; i < vertices.Length; i++)
            {
                Assert.AreNotEqual(vertices[i], new Vector3(0, 0, 0));
            }
        }
    }
}
