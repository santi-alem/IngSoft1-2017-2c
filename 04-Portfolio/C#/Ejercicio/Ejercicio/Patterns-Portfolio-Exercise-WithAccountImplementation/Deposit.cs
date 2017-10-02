using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patterns_Portfolio_Exercise_WithAccountImplementation
{
    class Deposit: AccountTransaction 
    {
    	private double m_value;
	
	    public static Deposit registerForOn(double value, ReceptiveAccount account) {
		    Deposit deposit = new Deposit(value);
		    account.register(deposit);
		
		    return deposit;
	    }

	    public Deposit (double value) {
		    m_value = value;
    	}
	
	    public double value(){
    		return m_value;
	    }
    }
}
