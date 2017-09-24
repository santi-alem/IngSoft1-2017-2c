using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C1_Stack_Exercise
{
    public class EmptyStack : StackComponent
    {
        public override NonEmptyStack push(Object anObject)
        {
            return new NonEmptyStack(anObject, 0, this);
        }
        public override StackComponent pop()
        {
            throw new Exception(STACK_EMPTY_DESCRIPTION);
        }
        public override Object top()
        {
            throw new Exception(STACK_EMPTY_DESCRIPTION);
        }
        public override bool isEmpty()
        {
            return true;
        }
        public override int size()
        {
            return 0;
        }
    }
}
