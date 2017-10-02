using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patterns_Portfolio_Exercise_WithAccountImplementation
{
    class Portfolio: SummarizingAccount
    {

    	public static String ACCOUNT_NOT_MANAGED = "No se maneja esta cuenta";
	    public static String ACCOUNT_ALREADY_MANAGED = "La cuenta ya estÃ¡ manejada por otro portfolio";
	
	    public static Portfolio createWith(SummarizingAccount anAccount1, SummarizingAccount anAccount2) {
    		throw new Exception();
	    }

    	public static Portfolio createWith(List<SummarizingAccount> summarizingAccounts) {
	    	throw new Exception();
	    }
	
    	public double balance() {
	    	throw new Exception();
	    }
	
	    public bool registers(AccountTransaction transaction) {
    		throw new Exception();
    	}

    	public List<AccountTransaction> transactionsOf(SummarizingAccount account) {
	    	throw new Exception();
	    }
	
    	public bool manages(SummarizingAccount account) {
		    throw new Exception();
	    }
	
	    public List<AccountTransaction> transactions() {
		    throw new Exception();
    	}
    }
}
