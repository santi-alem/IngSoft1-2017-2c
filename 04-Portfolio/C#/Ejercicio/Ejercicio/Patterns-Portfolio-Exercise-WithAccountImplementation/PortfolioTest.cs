using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Patterns_Portfolio_Exercise_WithAccountImplementation
{
    [TestClass]
    public class PortfolioTest
    {
        [TestMethod]
        public void test01ReceptiveAccountHaveZeroAsBalanceWhenCreated()
        {
            ReceptiveAccount account = new ReceptiveAccount();

            Assert.AreEqual(0.0, account.balance());
        }

        [TestMethod]
        public void test02DepositIncreasesBalanceOnTransactionValue()
        {
            ReceptiveAccount account = new ReceptiveAccount();
            Deposit.registerForOn(100, account);

            Assert.AreEqual(100.0, account.balance());

        }

        [TestMethod]
        public void test03WithdrawDecreasesBalanceOnTransactionValue()
        {
            ReceptiveAccount account = new ReceptiveAccount();
            Deposit.registerForOn(100, account);
            Withdraw.registerForOn(-50, account);

            Assert.AreEqual(50.0, account.balance());
        }

        [TestMethod]
        public void test04PortfolioBalanceIsSumOfManagedAccountsBalance()
        {
            ReceptiveAccount account1 = new ReceptiveAccount();
            ReceptiveAccount account2 = new ReceptiveAccount();
            Portfolio complexPortfolio = Portfolio.createWith(account1, account2);

            Deposit.registerForOn(100, account1);
            Deposit.registerForOn(200, account2);

            Assert.AreEqual(300.0, complexPortfolio.balance());
        }

        [TestMethod]
        public void test05PortfolioCanManagePortfolios()
        {
            ReceptiveAccount account1 = new ReceptiveAccount();
            ReceptiveAccount account2 = new ReceptiveAccount();
            ReceptiveAccount account3 = new ReceptiveAccount();
            Portfolio complexPortfolio = Portfolio.createWith(account1, account2);
            Portfolio composedPortfolio = Portfolio.createWith(complexPortfolio, account3);

            Deposit.registerForOn(100, account1);
            Deposit.registerForOn(200, account2);
            Deposit.registerForOn(300, account3);
            Assert.AreEqual(600.0, composedPortfolio.balance());
        }

        [TestMethod]
        public void test06ReceptiveAccountsKnowsRegisteredTransactions()
        {
            ReceptiveAccount account = new ReceptiveAccount();
            Deposit deposit = Deposit.registerForOn(100, account);
            Withdraw withdraw = Withdraw.registerForOn(-50, account);

            Assert.IsTrue(account.registers(deposit));
            Assert.IsTrue(account.registers(withdraw));
        }

        [TestMethod]
        public void test07ReceptiveAccountsDoNotKnowNotRegisteredTransactions()
        {
            ReceptiveAccount account = new ReceptiveAccount();
            Deposit deposit = new Deposit(100);
            Withdraw withdraw = new Withdraw(-50);

            Assert.IsFalse(account.registers(deposit));
            Assert.IsFalse(account.registers(withdraw));
        }

        [TestMethod]
        public void test08PortofoliosKnowsTransactionsRegisteredByItsManagedAccounts()
        {
            ReceptiveAccount account1 = new ReceptiveAccount();
            ReceptiveAccount account2 = new ReceptiveAccount();
            ReceptiveAccount account3 = new ReceptiveAccount();
            Portfolio complexPortfolio = Portfolio.createWith(account1, account2);
            Portfolio composedPortfolio = Portfolio.createWith(complexPortfolio, account3);

            Deposit deposit1 = Deposit.registerForOn(100, account1);
            Deposit deposit2 = Deposit.registerForOn(200, account2);
            Deposit deposit3 = Deposit.registerForOn(300, account3);

            Assert.IsTrue(composedPortfolio.registers(deposit1));
            Assert.IsTrue(composedPortfolio.registers(deposit2));
            Assert.IsTrue(composedPortfolio.registers(deposit3));
        }

        [TestMethod]
        public void test09PortofoliosDoNotKnowTransactionsNotRegisteredByItsManagedAccounts()
        {
            ReceptiveAccount account1 = new ReceptiveAccount();
            ReceptiveAccount account2 = new ReceptiveAccount();
            ReceptiveAccount account3 = new ReceptiveAccount();
            Portfolio complexPortfolio = Portfolio.createWith(account1, account2);
            Portfolio composedPortfolio = Portfolio.createWith(complexPortfolio, account3);

            Deposit deposit1 = new Deposit(100);
            Deposit deposit2 = new Deposit(200);
            Deposit deposit3 = new Deposit(300);

            Assert.IsFalse(composedPortfolio.registers(deposit1));
            Assert.IsFalse(composedPortfolio.registers(deposit2));
            Assert.IsFalse(composedPortfolio.registers(deposit3));
        }

        [TestMethod]
        public void test10ReceptiveAccountManageItSelf()
        {
            ReceptiveAccount account1 = new ReceptiveAccount();

            Assert.IsTrue(account1.manages(account1));
        }

        [TestMethod]
        public void test11ReceptiveAccountDoNotManageOtherAccount()
        {
            ReceptiveAccount account1 = new ReceptiveAccount();
            ReceptiveAccount account2 = new ReceptiveAccount();

            Assert.IsFalse(account1.manages(account2));
        }

        [TestMethod]
        public void test12PortfolioManagesComposedAccounts()
        {
            ReceptiveAccount account1 = new ReceptiveAccount();
            ReceptiveAccount account2 = new ReceptiveAccount();
            ReceptiveAccount account3 = new ReceptiveAccount();
            Portfolio complexPortfolio = Portfolio.createWith(account1, account2);

            Assert.IsTrue(complexPortfolio.manages(account1));
            Assert.IsTrue(complexPortfolio.manages(account2));
            Assert.IsFalse(complexPortfolio.manages(account3));
        }

        [TestMethod]
        public void test13PortfolioManagesComposedAccountsAndPortfolios()
        {
            ReceptiveAccount account1 = new ReceptiveAccount();
            ReceptiveAccount account2 = new ReceptiveAccount();
            ReceptiveAccount account3 = new ReceptiveAccount();
            Portfolio complexPortfolio = Portfolio.createWith(account1, account2);
            Portfolio composedPortfolio = Portfolio.createWith(complexPortfolio, account3);

            Assert.IsTrue(composedPortfolio.manages(account1));
            Assert.IsTrue(composedPortfolio.manages(account2));
            Assert.IsTrue(composedPortfolio.manages(account3));
            Assert.IsTrue(composedPortfolio.manages(complexPortfolio));
        }

        [TestMethod]
        public void test14AccountsKnowsItsTransactions()
        {
            ReceptiveAccount account1 = new ReceptiveAccount();

            Deposit deposit1 = Deposit.registerForOn(100, account1);

            Assert.AreEqual(1, account1.transactions().Count);
            Assert.IsTrue(account1.transactions().Contains(deposit1));
        }

        [TestMethod]
        public void test15PortfolioKnowsItsAccountsTransactions()
        {
            ReceptiveAccount account1 = new ReceptiveAccount();
            ReceptiveAccount account2 = new ReceptiveAccount();
            ReceptiveAccount account3 = new ReceptiveAccount();
            Portfolio complexPortfolio = Portfolio.createWith(account1, account2);
            Portfolio composedPortfolio = Portfolio.createWith(complexPortfolio, account3);

            Deposit deposit1 = Deposit.registerForOn(100, account1);
            Deposit deposit2 = Deposit.registerForOn(200, account2);
            Deposit deposit3 = Deposit.registerForOn(300, account3);

            Assert.AreEqual(3, composedPortfolio.transactions().Count);
            Assert.IsTrue(composedPortfolio.transactions().Contains(deposit1));
            Assert.IsTrue(composedPortfolio.transactions().Contains(deposit2));
            Assert.IsTrue(composedPortfolio.transactions().Contains(deposit3));
        }

        [TestMethod]
        public void test16PortofolioKnowsItsAccountsTransactions()
        {
            ReceptiveAccount account1 = new ReceptiveAccount();
            ReceptiveAccount account2 = new ReceptiveAccount();
            ReceptiveAccount account3 = new ReceptiveAccount();
            Portfolio complexPortfolio = Portfolio.createWith(account1, account2);
            Portfolio composedPortfolio = Portfolio.createWith(complexPortfolio, account3);

            Deposit deposit1 = Deposit.registerForOn(100, account1);

            Assert.AreEqual(1, composedPortfolio.transactionsOf(account1).Count);
            Assert.IsTrue(composedPortfolio.transactionsOf(account1).Contains(deposit1));
        }

        [TestMethod]
        public void test17PortofolioKnowsItsPortfoliosTransactions()
        {
            ReceptiveAccount account1 = new ReceptiveAccount();
            ReceptiveAccount account2 = new ReceptiveAccount();
            ReceptiveAccount account3 = new ReceptiveAccount();
            Portfolio complexPortfolio = Portfolio.createWith(account1, account2);
            Portfolio composedPortfolio = Portfolio.createWith(complexPortfolio, account3);

            Deposit deposit1 = Deposit.registerForOn(100, account1);
            Deposit deposit2 = Deposit.registerForOn(100, account2);
            Deposit.registerForOn(100, account3);

            Assert.AreEqual(2, composedPortfolio.transactionsOf(complexPortfolio).Count);
            Assert.IsTrue(composedPortfolio.transactionsOf(complexPortfolio).Contains(deposit1));
            Assert.IsTrue(composedPortfolio.transactionsOf(complexPortfolio).Contains(deposit2));
        }

        [TestMethod]
        public void test18PortofolioCanNotAnswerTransactionsOfNotManagedAccounts()
        {
            ReceptiveAccount account1 = new ReceptiveAccount();
            ReceptiveAccount account2 = new ReceptiveAccount();
            ReceptiveAccount account3 = new ReceptiveAccount();
            Portfolio complexPortfolio = Portfolio.createWith(account1, account2);

            try
            {
                complexPortfolio.transactionsOf(account3);
                Assert.Fail();
            }
            catch (Exception accountNotManaged)
            {
                Assert.AreEqual(Portfolio.ACCOUNT_NOT_MANAGED, accountNotManaged.Message);
            }
        }

        [TestMethod]
        public void test19CanNotCreatePortfoliosWithRepeatedAccount()
        {
            ReceptiveAccount account1 = new ReceptiveAccount();
            try
            {
                Portfolio.createWith(account1, account1);
                Assert.Fail();
            }
            catch (Exception invalidPortfolio)
            {
                Assert.AreEqual(Portfolio.ACCOUNT_ALREADY_MANAGED, invalidPortfolio.Message);
            }

        }

        [TestMethod]
        public void test20CanNotCreatePortfoliosWithAccountsManagedByOtherManagedPortfolio()
        {
            ReceptiveAccount account1 = new ReceptiveAccount();
            ReceptiveAccount account2 = new ReceptiveAccount();
            Portfolio complexPortfolio = Portfolio.createWith(account1, account2);
            try
            {
                Portfolio.createWith(complexPortfolio, account1);
                Assert.Fail();
            }
            catch (Exception invalidPortfolio)
            {
                Assert.AreEqual(Portfolio.ACCOUNT_ALREADY_MANAGED, invalidPortfolio.Message);
            }
        }

    }
}
