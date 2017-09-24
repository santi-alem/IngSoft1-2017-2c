using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C1_Stack_Exercise
{
    public class Stack
    {
        public static String STACK_EMPTY_DESCRIPTION = "Stack is Empty";
        protected StackComponent StackComponent;

        public Stack()
        {
            StackComponent = new EmptyStack();
        }

        public void push(Object anObject)
        {
            StackComponent = StackComponent.push(anObject);
        }

        public Object pop()
        {
            var topElement = StackComponent.top();
            StackComponent = StackComponent.pop();
            return topElement;
        }

        public Object top()
        {
            return StackComponent.top();
        }

        public bool isEmpty()
        {
            return StackComponent.isEmpty();
        }

        public int size()
        {
            return StackComponent.size();
        }
    }
}
