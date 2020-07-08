using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class VertexTest
    {
        // Test Equals method in Vertex class.
        [Test]
        public void VertexTestEquals()
        {
            Vertex v = new Vertex(0, 0, 0);
            Assert.AreEqual(v, v);
            Vertex v2 = new Vertex(0, 0, 0);
            Assert.AreEqual(v, v2);
            v2.SetDensity(5);
            Assert.AreEqual(v, v2);
        }

        // Test GetMidPoint method in Vertex class.
        [Test]
        public void VertexTestGetMidPoint()
        {
            Vertex v = new Vertex(0, 0, 0);
            Vertex v2 = new Vertex(2, 2, 2);
            Vector3 vector12 = v.GetMidPoint(v2);
            Assert.AreEqual(vector12, new Vector3(1, 1, 1));
            Vertex v3 = new Vertex(3, 3, 3);
            Vector3 vector13 = v.GetMidPoint(v3);
            Assert.AreEqual(vector13, new Vector3(1.5f, 1.5f, 1.5f));
        }
    }
}
