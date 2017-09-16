#
# Developed by 10Pines SRL
# License: 
# This work is licensed under the 
# Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License. 
# To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/ 
# or send a letter to Creative Commons, 444 Castro Street, Suite 900, Mountain View, 
# California, 94041, USA.
#  

import unittest


class Cointainer:
    def push(self, anObject):
        self.shouldBeImplementedBySubclass()

    def top(self):
        self.shouldBeImplementedBySubclass()

    def pop(self):
        self.shouldBeImplementedBySubclass()

    def isEmpty(self):
        self.shouldBeImplementedBySubclass()

    def shouldBeImplementedBySubclass(self):
        raise NotImplemented("Subclass task to implement")


class StackContainer(Cointainer):
    def __init__(self, anObject, previousContainer):
        self._cointainedObject = anObject
        self._previousObject = previousContainer

    def push(self, anObject):
        return StackContainer(anObject, self)

    def pop(self):
        return self._previousObject

    def top(self):
        return self._cointainedObject

    def size(self):
        return self._previousObject.size() + 1

    def isEmpty(self):
        return False


class EmptyStackContainer(Cointainer):
    def push(self, anObject):
        return StackContainer(anObject, self)

    def pop(self):
        raise self.stackIsEmpty()

    def top(self):
        raise self.stackIsEmpty()

    def stackIsEmpty(self):
        return Exception(Stack.STACK_EMPTY_DESCRIPTION)

    def isEmpty(self):
        return True

    def size(self):
        return 0


class Stack:
    def __init__(self):
        self.items = []
        self.topObject = EmptyStackContainer()

    STACK_EMPTY_DESCRIPTION = 'Stack is empty'

    def push(self, anObject):
        self.topObject = self.topObject.push(anObject)

    def pop(self):
        container = self.topObject
        self.topObject = container.pop()
        return container.top()

    def top(self):
        return self.topObject.top()

    def isEmpty(self):
        return self.topObject.isEmpty()

    def size(self):
        return self.topObject.size()

    def stackIsEmpty(self):
        return Exception(self.STACK_EMPTY_DESCRIPTION)


class StackTest(unittest.TestCase):
    def testStackShouldBeEmptyWhenCreated(self):
        stack = Stack()

        self.assertTrue(stack.isEmpty())

    def testPushAddElementsToTheStack(self):
        stack = Stack()
        stack.push('something')

        self.assertFalse(stack.isEmpty())

    def testPopRemovesElementsFromTheStack(self):
        stack = Stack()
        stack.push("Something")
        stack.pop()

        self.assertTrue(stack.isEmpty())

    def testPopReturnsLastPushedObject(self):
        stack = Stack()
        pushedObject = "Something"
        stack.push(pushedObject)
        self.assertEquals(pushedObject, stack.pop())

    def testStackBehavesLIFO(self):
        firstPushed = "First"
        secondPushed = "Second"
        stack = Stack()
        stack.push(firstPushed)
        stack.push(secondPushed)

        self.assertEquals(secondPushed, stack.pop())
        self.assertEquals(firstPushed, stack.pop())
        self.assertTrue(stack.isEmpty())

    def testTopReturnsLastPushedObject(self):
        stack = Stack()
        pushedObject = "Something"

        stack.push(pushedObject)

        self.assertEquals(pushedObject, stack.top())

    def testTopDoesNotRemoveObjectFromStack(self):
        stack = Stack()
        pushedObject = "Something"

        stack.push(pushedObject)

        self.assertEquals(1, stack.size())
        stack.top()
        self.assertEquals(1, stack.size())

    def testCanNotPopWhenThereAreNoObjectsInTheStack(self):
        stack = Stack()

        try:
            stack.pop()
            self.fail()
        except Exception as stackIsEmpty:
            self.assertEquals(Stack.STACK_EMPTY_DESCRIPTION, stackIsEmpty.message)

    def testCanNotTopWhenThereAreNoObjectsInTheStack(self):
        stack = Stack()

        try:
            stack.top()
            self.fail()
        except Exception as stackIsEmpty:
            self.assertEquals(Stack.STACK_EMPTY_DESCRIPTION, stackIsEmpty.message)


if __name__ == "__main__":
    unittest.main()
