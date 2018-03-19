using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System.Numerics;
using Nethereum.KeyStore;

namespace EthereumCommander
{
    class Ethereum
    {
        private Account account;
        private Web3 web3;
        private string RpcUrl = "http://47.91.208.241:8601/";

        public bool LoadAccountFromFile(string filePath,string password)
        {
            try
            {
                var accountData = File.ReadAllText(filePath);
                account = Account.LoadFromKeyStore(accountData, password);
                web3 = new Web3(account, RpcUrl);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }

        public string GetAddress(string keyText, string password)
        {
            try
            {
                var account = Account.LoadFromKeyStore(keyText, password);
                return account.Address;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "";
            }
        }

        public string CreateAccount(string password)
        {
            //Generate a private key pair using SecureRandom
            var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
            var address = ecKey.GetPublicAddress();

            var service = new KeyStoreService();
            var encryptedKey = service.EncryptAndGenerateDefaultKeyStoreAsJson(password, ecKey.GetPrivateKeyAsBytes(), address);

            return encryptedKey;
        }

        public async Task<BigDecimal> GetBalanceAsync()
        {
            var balance = await web3.Eth.GetBalance.SendRequestAsync(account.Address);
            return UnitConversion.Convert.FromWeiToBigDecimal(balance, UnitConversion.EthUnit.Ether);
        }
        public async Task<BigDecimal> GetBalanceAsync(string address)
        {
            var balance = await web3.Eth.GetBalance.SendRequestAsync(address);
            return UnitConversion.Convert.FromWeiToBigDecimal(balance, UnitConversion.EthUnit.Ether);
        }
        public async Task<string> PayAsync(string address, decimal value)
        {
            var wei = UnitConversion.Convert.ToWei(new BigDecimal(value));
            var amount = new Nethereum.Hex.HexTypes.HexBigInteger(wei);
            var data = await web3.Eth.TransactionManager.SendTransactionAsync(account.Address, address,amount);
            var result = await web3.Eth.TransactionManager.TransactionReceiptService.PollForReceiptAsync(data);
            var blocknumber = result.BlockNumber;
            Console.WriteLine("block number: " + blocknumber.Value);
            return data;

        }

    }
}
