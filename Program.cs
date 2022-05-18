using Nethereum.Web3;
using Nethereum.Util;
using System.Collections.Generic;
using System.Text;
using Nethereum.Signer;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.ABI.Encoders;
using BinaryEncoding;
using Multiformats.Hash;
using Multiformats.Hash.Algorithms;
using Org.BouncyCastle.Utilities.Encoders;
using Libsecp256k1Zkp.Net;
using System;


public class Program
{
    public static void Main()
    {
        Program p = new Program();
		using var secp256K1 = new Secp256k1();
        string[] stringarr;
        stringarr = new string[5] { "1652858514000", "tron", "TR7NHqjeKQxGTCi8q8ZY4pL8otSzgjLj6t", "1345.96", "http://vapiv2.dummyuat.com/api/callback/DepositCallbackHandler.ashx" };
        List<byte[]> bytesdata = new List<byte[]>();
        foreach (string str in stringarr)
        {
            bytesdata.Add(Encoding.ASCII.GetBytes(str));
        }
        // byte[] bytes = Multihash.Encode(p.ConvertList(bytesdata), HashType.KECCAK_256);
        var mh = Multihash.Sum<KECCAK_256>(p.ConvertList(bytesdata));

        var hash = Hex.Decode("e26f1a76086517f32724836852c0bbd5d95364133168ebca879dfeb3ef878b81");

        Console.WriteLine("bytes equal: " + hash.SequenceEqual(mh.Digest));


        var privateKey = "f78494eb224f875d7e352a2b017304e11e6a3ce94af57b373ae82a73b3496cdd";
		var sig = secp256K1.Sign(hash, Hex.Decode(privateKey));
        var sign = "0x1eaa66635b9b7d238e85234366f1e60898885ca0a66b6bca4095c618e2a3c71b60894502f7b818acf1d692e6073ea5fc745211886284c729061ef873d22ef5bc1b";
        

         var actualPubKey = secp256K1.CreatePublicKey(Hex.Decode(privateKey));
         Console.WriteLine(Hex.ToHexString(actualPubKey));

		// Console.WriteLine("Sign equal: " + (signature1 == sign).ToString());
		Console.WriteLine(Hex.ToHexString(sig));
    }
    public Byte[] ConvertList(List<Byte[]> list)
    {
        List<Byte> tmpList = new List<byte>();
        foreach (Byte[] byteArray in list)
            foreach (Byte singleByte in byteArray)
                tmpList.Add(singleByte);
        return tmpList.ToArray();
    }
}