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
        var pub_key = "0x2143d11B31b319C008F59c2D967eBF0E5ad2791d";
        var privateKey = "f78494eb224f875d7e352a2b017304e11e6a3ce94af57b373ae82a73b3496cdd";
        DateTimeOffset now = (DateTimeOffset)DateTime.UtcNow;
        Console.WriteLine(now.ToUnixTimeMilliseconds());
        var param = new OrderParam
        {
            out_order_no = now.ToUnixTimeMilliseconds().ToString(),
            pay_chain = "tron",
            pay_token = "TR7NHqjeKQxGTCi8q8ZY4pL8otSzgjLj6t",
            pay_amount = "1345.96",
            notify = "http://vapiv2.dummyuat.com/api/callback/DepositCallbackHandler.ashx",
            signature = "",
            pub_key = pub_key,
            version = "1.1"
        };

        using var secp256K1 = new Secp256k1();
        string[] stringarr;
        stringarr = new string[5] { param.out_order_no, param.pay_chain, param.pay_token, param.pay_amount, param.notify };
        List<byte[]> bytesdata = new List<byte[]>();
        foreach (string str in stringarr)
        {
            bytesdata.Add(Encoding.ASCII.GetBytes(str));
        }
        var mh = Multihash.Sum<KECCAK_256>(ConvertList(bytesdata));

        var bytesData = Hex.ToHexString(mh.Digest);

        Console.WriteLine("bytedata:" + bytesData);


        var signer = new EthereumMessageSigner();
        var sign = signer.EncodeUTF8AndSign("0x" + bytesData, new EthECKey(privateKey));

        Console.WriteLine("sign: " + sign);

        param.signature = sign;
        await CreateOrder(param);
    }
    static async Task CreateOrder(object json)
    {
        var responseTask = await "https://api-tron-v1.dpay.systems/v1/order/create".PostJsonAsync(json);
        var responseBody = await responseTask.GetStringAsync();
        Console.WriteLine(responseBody);
    }
    static Byte[] ConvertList(List<Byte[]> list)
    {
        List<Byte> tmpList = new List<byte>();
        foreach (Byte[] byteArray in list)
            foreach (Byte singleByte in byteArray)
                tmpList.Add(singleByte);
        return tmpList.ToArray();
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