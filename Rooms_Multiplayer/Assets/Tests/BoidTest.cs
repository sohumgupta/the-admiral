using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;

namespace Tests
{
    public class BoidTest
    {
        GameObject g = new GameObject();
        Boid boid;

        [Test]
        //Test the scale setting method for a boid
        public void testScale()
        {
            try {
                boid = g.AddComponent(typeof(Boid)) as Boid;            
                boid.setScale(0.5f);
                Assert.IsTrue(boid.transform.localScale[0] <= .8);
                Assert.IsTrue(boid.transform.localScale[0] >= .2);
            } catch (System.ArgumentException e) {
                Assert.Fail("Exception");
            }

            try
            {
                boid.setScale(0f);
                Assert.Fail("Exception should have been thrown");
            }
            catch (System.ArgumentException e)
            {
                Assert.AreEqual("Scale mean was too low!", e.Message);
            }
        }

        [Test]
        public void testId()
        {
            boid = g.AddComponent(typeof(Boid)) as Boid;            
            boid.setId(1);
            Assert.AreEqual(boid.id, 1);
        }

        
        //checking velocity functionality (clamping is working correctly)
        [Test]
        public void testVel()
        {
            boid = g.AddComponent(typeof(Boid)) as Boid;            
            boid.changeVelocity(new Vector3(50, 50, 100));
            Assert.IsTrue(boid.getVelocity()[0] >= 10);
            Assert.IsTrue(boid.getVelocity()[1] >= 6); //yVel has seperate clamp
            Assert.IsTrue(boid.getVelocity()[2] >= 10);
            boid.clamp();
            Assert.IsTrue(boid.getVelocity()[0] <= 10);
            Assert.IsTrue(boid.getVelocity()[1] <= 6); //yVel has seperate clamp
            Assert.IsTrue(boid.getVelocity()[2] <= 10);
        }

    }
}
