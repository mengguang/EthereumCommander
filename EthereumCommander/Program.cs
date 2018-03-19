using System;
using System.IO;
using System.Threading.Tasks;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
namespace EthereumCommander
{
    class Program
    {
        static async Task Main(string[] args)
        {
            
            var password = "123qwe";
            var accountFile = @"C:\Users\mengguang\Desktop\eth_account.json";

            var eth = new Ethereum();
            eth.LoadAccountFromFile(accountFile, password);

            var key = eth.CreateAccount(password);
            Console.WriteLine("Key text of new account: ");
            Console.WriteLine(key);

            var destAddress = eth.GetAddress(key, password);
            Console.WriteLine("Address of new account: " + destAddress);

            var balance = await eth.GetBalanceAsync(destAddress);
            Console.WriteLine("Balance of new account: " + balance);


            balance = await eth.GetBalanceAsync();
            Console.WriteLine("Balance of main account: " + balance);

            var result = await eth.PayAsync(destAddress, new decimal(3.6));
            Console.WriteLine("Transaction data: " + result);

            balance = await eth.GetBalanceAsync();
            Console.WriteLine("Balance of main account: " + balance);

            balance = await eth.GetBalanceAsync(destAddress);
            Console.WriteLine("Balance of new account: " + balance);
        }

    }
}
