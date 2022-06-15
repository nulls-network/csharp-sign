
using System.Text;
using Nethereum.Signer;
using Multiformats.Hash;
using Multiformats.Hash.Algorithms;
using Org.BouncyCastle.Utilities.Encoders;

public class  PaySign{



    /**
        version = 1.0
    */
    public static  string   Sign(string[] data,string privateKey){
        List<byte[]> bytesdata = new List<byte[]>();
        foreach (string str in data)
        {
            bytesdata.Add(Encoding.ASCII.GetBytes(str));
        }
        var mh = Multihash.Sum<KECCAK_256>(ConvertList(bytesdata));
        var signer  = new MessageSigner();
        var sign = signer.Sign(mh.Digest,new EthECKey(privateKey));
        return sign;
    }


    /**
        version = 1.1
    */
    public static  string   Sign_v1(string[] data,string privateKey){
        List<byte[]> bytesdata = new List<byte[]>();
        foreach (string str in data)
        {
            bytesdata.Add(Encoding.ASCII.GetBytes(str));
        }
        var mh = Multihash.Sum<KECCAK_256>(ConvertList(bytesdata));
        var bytesData = Hex.ToHexString(mh.Digest);

        var signer = new EthereumMessageSigner();
        var sign = signer.EncodeUTF8AndSign("0x" + bytesData, new EthECKey(privateKey));
        return sign;
    }



    public static  string  Recover(string[] data,string sign){
        List<byte[]> bytesdata = new List<byte[]>();
        foreach (string str in data)
        {
            bytesdata.Add(Encoding.ASCII.GetBytes(str));
        }
        var mh = Multihash.Sum<KECCAK_256>(ConvertList(bytesdata));
        var signer  = new MessageSigner();
        var public_key = signer.EcRecover(mh.Digest,sign);
        return public_key;
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