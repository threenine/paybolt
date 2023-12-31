using System.Globalization;

namespace PayBolt;

public static class ByteExtensions
{
    private const string DASH = "-";
    private const string COLON = ":";
    public static string ToBitString(this byte[] source)
    {
      if(source.Length <= 0) return  string.Empty;

        return BitConverter.ToString(source)
            .Replace(DASH, string.Empty)
            .ToLower(CultureInfo.InvariantCulture);
    }
    
    
    public static byte[] HexStringToByteArray(this string source)
    {
        if (string.IsNullOrEmpty(source)) return Array.Empty<byte>();
       

        source = source.Replace(COLON, string.Empty);

        var numberChars = source.Length;
        var bytes = new byte[numberChars / 2];
        for (var i = 0; i < numberChars; i += 2)
            bytes[i / 2] = Convert.ToByte(source.Substring(i, 2), 16);
        return bytes;
    }
}