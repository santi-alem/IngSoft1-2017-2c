/*
 * Developed by 10Pines SRL
 * License: 
 * This work is licensed under the 
 * Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/ 
 * or send a letter to Creative Commons, 444 Castro Street, Suite 900, Mountain View, 
 * California, 94041, USA.
 *  
 */
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace C1_Stack_Exercise
{
    [TestClass]
    public class StackTest
    {
        [TestMethod]
        public void testStackShouldBeEmptyWhenCreated()
        {
            Stack stack = new Stack();

            Assert.IsTrue(stack.isEmpty());
        }

        [TestMethod]
        public void testPushAddElementsToTheStack()
        {
            Stack stack = new Stack();
            stack.push("Something");

            Assert.IsFalse(stack.isEmpty());
        }

        [TestMethod]
        public void testPopRemovesElementsFromTheStack()
        {
            Stack stack = new Stack();
            stack.push("Something");
            stack.pop();

            Assert.IsTrue(stack.isEmpty());
        }

        [TestMethod]
        public void testPopReturnsLastPushedObject()
        {
            Stack stack = new Stack();
            String pushedObject = "Something";
            stack.push(pushedObject);
            Assert.AreEqual(pushedObject, stack.pop());
        }

        [TestMethod]
        public void testStackBehavesLIFO()
        {
            String firstPushed = "First";
            String secondPushed = "Second";
            Stack stack = new Stack();
            stack.push(firstPushed);
            stack.push(secondPushed);

            Assert.AreEqual(secondPushed, stack.pop());
            Assert.AreEqual(firstPushed, stack.pop());
            Assert.IsTrue(stack.isEmpty());
        }

        [TestMethod]
        public void testTopReturnsLastPushedObject()
        {
            Stack stack = new Stack();
            String pushedObject = "Something";

            stack.push(pushedObject);

            Assert.AreEqual(pushedObject, stack.top());
        }

        [TestMethod]
        public void testTopDoesNotRemoveObjectFromStack()
        {
            Stack stack = new Stack();
            String pushedObject = "Something";

            stack.push(pushedObject);

            Assert.AreEqual(1, stack.size());
            stack.top();
            Assert.AreEqual(1, stack.size());
        }

        [TestMethod]
        public void testCanNotPopWhenThereAreNoObjectsInTheStack()
        {
            Stack stack = new Stack();

            try
            {
                stack.pop();
                Assert.Fail();
            }
            catch (Exception stackIsEmpty)
            {
                Assert.AreEqual(Stack.STACK_EMPTY_DESCRIPTION, stackIsEmpty.Message);
            }
        }

        [TestMethod]
        public void testCanNotTopWhenThereAreNoObjectsInTheStack()
        {
            Stack stack = new Stack();

            try
            {
                stack.top();
                Assert.Fail();
            }
            catch (Exception stackIsEmpty)
            {
                Assert.AreEqual(Stack.STACK_EMPTY_DESCRIPTION, stackIsEmpty.Message);
            }
        }

    }
}
