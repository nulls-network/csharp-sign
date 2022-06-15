using System.Text;
using Nethereum.Signer;
using Multiformats.Hash;
using Multiformats.Hash.Algorithms;
using Org.BouncyCastle.Utilities.Encoders;
using Libsecp256k1Zkp.Net;
using Flurl.Http;
using System;


public class Program
{
    static async Task Main()
    {
        await order();
        await order_v1();
        Recover();
    }
    static async Task CreateOrder(object json)
    {
        var responseTask = await "https://api-tron-v1.dpay.systems/v1/order/create".PostJsonAsync(json);
        var responseBody = await responseTask.GetStringAsync();
        Console.WriteLine(responseBody);
    }


    static async Task  order(){
         var privateKey = "f78494eb224f875d7e352a2b017304e11e6a3ce94af57b373ae82a73b3496cdd";
         DateTimeOffset now = (DateTimeOffset)DateTime.UtcNow;
        var param = new OrderParam
        {
            out_order_no = now.ToUnixTimeMilliseconds().ToString(),
            pay_chain = "tron",
            pay_token = "TR7NHqjeKQxGTCi8q8ZY4pL8otSzgjLj6t",
            pay_amount = "1345.96",
            notify = "http://vapiv2.dummyuat.com/api/callback/DepositCallbackHandler.ashx",
            pub_key = "0x2143d11B31b319C008F59c2D967eBF0E5ad2791d",
        };
        string[] stringarr = new string[5] { param.out_order_no, param.pay_chain, param.pay_token, param.pay_amount, param.notify };
        var signature = PaySign.Sign(stringarr,privateKey);
        param.signature = signature;
        await CreateOrder(param);
    }


    static async Task  order_v1(){
         var privateKey = "f78494eb224f875d7e352a2b017304e11e6a3ce94af57b373ae82a73b3496cdd";
         DateTimeOffset now = (DateTimeOffset)DateTime.UtcNow;
        var param = new OrderParam
        {
            out_order_no = now.ToUnixTimeMilliseconds().ToString(),
            pay_chain = "tron",
            pay_token = "TR7NHqjeKQxGTCi8q8ZY4pL8otSzgjLj6t",
            pay_amount = "1345.96",
            notify = "http://vapiv2.dummyuat.com/api/callback/DepositCallbackHandler.ashx",
            pub_key = "0x2143d11B31b319C008F59c2D967eBF0E5ad2791d",
            version = "1.1"
        };
        string[] stringarr = new string[5] { param.out_order_no, param.pay_chain, param.pay_token, param.pay_amount, param.notify };
        var signature = PaySign.Sign_v1(stringarr,privateKey);
        param.signature = signature;
        await CreateOrder(param);
    }


    static  string  Recover(){
        var param = new notify
            {
            out_order_no = "1082331632868",
            uuid = "17b6d9c5-a59c-491a-8bd3-ad370d1ea82b",
            merchant_address= "0x2143d11B31b319C008F59c2D967eBF0E5ad2791d",
            type = "order",
            amount = "1.5000",
            amount_hex= "1500000",
            got_amount= "1.5000",
            pay_result= "success",
            token= "TR7NHqjeKQxGTCi8q8ZY4pL8otSzgjLj6t",
        };
        var sign = "0x4cc6c847b0b42483920ed93191504acfd3fb1ad5ff109e672d4f5927a8b8bf47420a9b0461f803b23ad2311a1293c194b4aa4d9d1a424e685c2aaac5a06128fb1b";
        using var secp256K1 = new Secp256k1();
        string[] stringarr;
        stringarr = new string[9] {param.out_order_no, param.uuid, param.merchant_address, param.type, param.amount,param.amount_hex,param.got_amount,param.pay_result,param.token };
        var public_key = PaySign.Recover(stringarr,sign);
        Console.WriteLine(public_key);
        return public_key;
    }
}



public class OrderParam
{
    public string out_order_no { get; set; }
    public string pay_chain { get; set; }
    public string pay_token { get; set; }
    public string pay_amount { get; set; }
    public string notify { get; set; }
    public string signature { get; set; }
    public string pub_key { get; set; }
    public string version { get; set; }
}


public class notify {
    public string out_order_no { get;set; }
    public string uuid {get ; set; }
    public string merchant_address {get;set;}
    public string type {get;set;}
    public string amount {get;set;}

    public string amount_hex {get;set;}

    public string got_amount {get;set;}

    public string pay_result {get;set;}

    public string token{get;set;}
}

