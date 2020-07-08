using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class CubeTest
    {
        // Test for GetVertices method in Cube class, and vertex placement.
        [Test]
        public void CubeTestConstructorException()
        {
            try
            {
                Cube failedCube = new Cube(0, 0, 0, 0);
                Assert.Fail("Exception should have been thrown");
            }
            catch (System.ArgumentException e)
            {
                Assert.AreEqual("edge length must be greater than 0", e.Message);
            }
            try
            {
                Cube failedCube = new Cube(-1, 0, 0, 0);
                Assert.Fail("Exception should have been thrown");
            }
            catch (System.ArgumentException e)
            {
                Assert.AreEqual("edge length must be greater than 0", e.Message);
            }
        }
        // Test for GetVertices method in Cube class, and vertex placement.
        [Test]
        public void CubeTestGetVertices()
        {
            Cube cube = new Cube(5, 0, 0, 0);
            Vertex[] vertices = cube.GetVertices();
            Assert.AreEqual(vertices.Length, 8);
            Assert.AreEqual(vertices[0], new Vertex(0, 0, 0));
            Assert.AreEqual(vertices[1], new Vertex(5, 0, 0));
            Assert.AreEqual(vertices[2], new Vertex(5, 0, 5));
            Assert.AreEqual(vertices[3], new Vertex(0, 0, 5));
            Assert.AreEqual(vertices[4], new Vertex(0, 5, 0));
            Assert.AreEqual(vertices[5], new Vertex(5, 5, 0));
            Assert.AreEqual(vertices[6], new Vertex(5, 5, 5));
            Assert.AreEqual(vertices[7], new Vertex(0, 5, 5));
            Cube cube2 = new Cube(2.5f, 2.5f, 2.5f, 2.5f);
            Vertex[] vertices2 = cube2.GetVertices();
            Assert.AreEqual(vertices2.Length, 8);
            Assert.AreEqual(vertices2[0], new Vertex(2.5f, 2.5f, 2.5f));
            Assert.AreEqual(vertices2[1], new Vertex(5, 2.5f, 2.5f));
            Assert.AreEqual(vertices2[2], new Vertex(5, 2.5f, 5));
            Assert.AreEqual(vertices2[3], new Vertex(2.5f, 2.5f, 5));
            Assert.AreEqual(vertices2[4], new Vertex(2.5f, 5, 2.5f));
            Assert.AreEqual(vertices2[5], new Vertex(5, 5, 2.5f));
            Assert.AreEqual(vertices2[6], new Vertex(5, 5, 5));
            Assert.AreEqual(vertices2[7], new Vertex(2.5f, 5, 5));
        }
    }
}
