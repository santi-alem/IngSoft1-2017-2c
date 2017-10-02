using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patterns_Portfolio_Exercise_WithAccountImplementation
{
    class ReceptiveAccount: SummarizingAccount
    {

    	private List<AccountTransaction> m_transactions = new List<AccountTransaction>();
	
	    public double balance() {
            return m_transactions.Sum( transaction => transaction.value() );
	    }

	    public void register(AccountTransaction transaction) {
		    m_transactions.Add(transaction);
	    }
	
	    public bool registers(AccountTransaction transaction) {
		    return m_transactions.Contains(transaction);
	    }

	    public bool manages(SummarizingAccount account) {
		    return this == account;
	    }
	
	    public List<AccountTransaction> transactions() {
		    return new List<AccountTransaction>(m_transactions);
	    }    
    }
}
