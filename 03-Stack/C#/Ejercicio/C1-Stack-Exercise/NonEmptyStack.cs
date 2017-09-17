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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C1_Stack_Exercise
{
    
    public class NonEmptyStack : StackComponent
    {
        public static String STACK_EMPTY_DESCRIPTION = "Stack is Empty";

        protected int numberOfItems;
        protected StackComponent previousTopObject;
        protected Object topItem;

        public NonEmptyStack(Object anObject, int previousNumberOfItems, StackComponent previousTop)
        {
            numberOfItems = previousNumberOfItems + 1;
            previousTopObject = previousTop; 
            topItem = anObject;
        }
        public override NonEmptyStack push(Object anObject)
        {
            return new NonEmptyStack(anObject, numberOfItems+1, this);
        }
        public override StackComponent pop()
        {
            return previousTopObject;
        }
        public override Object top()
        {
            return topItem;
        }
        public override bool isEmpty()
        {
            return false;
        }
        public override int size()
        {
            return numberOfItems;
        }
    }
}
