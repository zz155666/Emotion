using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XamlAnimatedGif.Extensions;

namespace XamlAnimatedGif.Decoding
{
    internal static class GifHelpers
    {
        public static async Task<string> ReadStringAsync(Stream stream, int length)
        {
            byte[] bytes = new byte[length];
            await stream.ReadAllAsync(bytes, 0, length).ConfigureAwait(false);
            return GetString(bytes);
        }

        public static async Task<byte[]> ReadDataBlocksAsync(Stream stream, bool discard)
        {
            MemoryStream ms = discard ? null : new MemoryStream();
            using (ms)
            {
                int len;
                while ((len = stream.ReadByte()) > 0)
                {
                    byte[] bytes = new byte[len];
                    await stream.ReadAsync(bytes, 0, len).ConfigureAwait(false);
                    ms?.Write(bytes, 0, len);
                }
                return ms?.ToArray();
            }
        }

        public static async Task CopyDataBlocksToStreamAsync(Stream sourceStream, Stream targetStream, CancellationToken cancellationToken = default(CancellationToken))
        {
            int len;
            while ((len = await sourceStream.ReadByteAsync(cancellationToken)) > 0)
            {
                byte[] bytes = new byte[len];
                await sourceStream.ReadAsync(bytes, 0, len, cancellationToken).ConfigureAwait(false);
                await targetStream.WriteAsync(bytes, 0, len, cancellationToken);
            }
        }


        public static async Task<GifColor[]> ReadColorTableAsync(Stream stream, int size)
        {
            int length = 3 * size;
            byte[] bytes = new byte[length];
            await stream.ReadAllAsync(bytes, 0, length).ConfigureAwait(false);
            GifColor[] colorTable = new GifColor[size];
            for (int i = 0; i < size; i++)
            {
                byte r = bytes[3 * i];
                byte g = bytes[3 * i + 1];
                byte b = bytes[3 * i + 2];
                colorTable[i] = new GifColor(r, g, b);
            }
            return colorTable;
        }

        public static bool IsNetscapeExtension(GifApplicationExtension ext)
        {
            return ext.ApplicationIdentifier == "NETSCAPE"
                && GetString(ext.AuthenticationCode) == "2.0";
        }

        public static ushort GetRepeatCount(GifApplicationExtension ext)
        {
            if (ext.Data.Length >= 3)
            {
                return BitConverter.ToUInt16(ext.Data, 1);
            }
            return 1;
        }

        public static Exception UnexpectedEndOfStreamException()
        {
            return new UnexpectedEndOfStreamException("Unexpected end of stream before trailer was encountered");
        }

        public static Exception UnknownBlockTypeException(int blockId)
        {
            return new UnknownBlockTypeException("Unknown block type: 0x" + blockId.ToString("x2"));
        }

        public static Exception UnknownExtensionTypeException(int extensionLabel)
        {
            return new UnknownExtensionTypeException("Unknown extension type: 0x" + extensionLabel.ToString("x2"));
        }

        public static Exception InvalidBlockSizeException(string blockName, int expectedBlockSize, int actualBlockSize)
        {
            return new InvalidBlockSizeException(
                $"Invalid block size for {blockName}. Expected {expectedBlockSize}, but was {actualBlockSize}");
        }

        public static Exception InvalidSignatureException(string signature)
        {
            return new InvalidSignatureException("Invalid file signature: " + signature);
        }

        public static Exception UnsupportedVersionException(string version)
        {
            return new UnsupportedGifVersionException("Unsupported version: " + version);
        }

        public static string GetString(byte[] bytes)
        {
            return GetString(bytes, 0, bytes.Length);
        }

        public static string GetString(byte[] bytes, int index, int count)
        {
            return Encoding.UTF8.GetString(bytes, index, count);
        }
    }
}
