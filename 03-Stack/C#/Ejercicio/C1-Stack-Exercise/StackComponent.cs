using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C1_Stack_Exercise
{
    public abstract class StackComponent
    {
        public static String STACK_EMPTY_DESCRIPTION = "Stack is Empty";
        public abstract NonEmptyStack push(Object anObject);
        public abstract StackComponent pop();
        public abstract Object top();
        public abstract bool isEmpty();
        public abstract int size();
    }

}
