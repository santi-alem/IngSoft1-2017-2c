using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patterns_Portfolio_Exercise_WithAccountImplementation
{
    interface SummarizingAccount
    {

    	double balance(); 
    	bool registers(AccountTransaction transaction);
	    bool manages(SummarizingAccount account);
	    List<AccountTransaction> transactions();
    }
}
