// This code is for forking. 
// This does not have encryption.
// If you're gonna fork this, change InputPath ah to your actual file names and change OutpathPath to whatever you want.

using System.Buffers.Text;
using System.ComponentModel.DataAnnotations;
using System.IO.Compression;
using System.Text;
using System.Xml.Serialization;

class Program
{
    static void Main()
    { 

        // Explained at the start.
        string InputPath = "CCGameManager.dat"; 
        string OutputPath = "output.xml"; 

        // Read raw data of file.
        byte[] rawBytes = File.ReadAllBytes(InputPath);
        // XOR bytes with key 11.
        for (int i = 0; i < rawBytes.Length; i++)
        {
            rawBytes[i] ^= 11;
        }

        // Get string from the rawbytes and trim null bytes to not cause problems with base64 decode (if any).
        string base64 = Encoding.ASCII.GetString(rawBytes).TrimEnd('\0');
        // Change from url-safe to non url-safe. This is because base64 decode requests a non-url safe base64 string.
        base64 = base64.Replace("-", "+").Replace("_", "/");
        // Convert the base64 decoded result to bytes.
        byte[] compressedBytes = Convert.FromBase64String(base64);

        // Get the string from the bytes.
        string xml = GZipDecompress(compressedBytes);

        // Write the string to a file with the OutputPath
        File.WriteAllText(OutputPath, xml);
        Console.WriteLine("Over Finally!");
    }

    // Helper function
    static string GZipDecompress(byte[] data)
    {    
        // Create a MemoryStream to store the bytes for the GZipStream to read from
        using (MemoryStream input = new MemoryStream(data))
        // Decompress the data from the memory stream
        using (GZipStream gzip = new GZipStream(input, CompressionMode.Decompress))
        // Read the data as it gets decompressed
        using (StreamReader reader = new StreamReader(gzip, Encoding.UTF8))
        {
            return reader.ReadToEnd();
        }
    }
}

