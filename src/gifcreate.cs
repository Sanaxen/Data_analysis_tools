using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace GifCreator
{
    public class GifCreator
    {
        public static void CreateAnimatedGif(List<string> gifFiles, int delay, string outputFile)
        {
            BinaryWriter writer = new BinaryWriter(new FileStream(outputFile, FileMode.Create, FileAccess.ReadWrite));
            byte[] gif_Signature = new byte[] { (byte)'G', (byte)'I', (byte)'F', (byte)'8', (byte)'9', (byte)'a' };
            writer.Write(gif_Signature);
            for (int i = 0; i < gifFiles.Count; i++)
            {
                GifClass gif = new GifClass();
                gif.LoadGifPicture(gifFiles[i]);
                if (i == 0)
                    writer.Write(gif.m_ScreenDescriptor.ToArray());
                writer.Write(GifCreator.CreateGraphicControlExtensionBlock(delay));
                writer.Write(gif.m_ImageDescriptor.ToArray());
                writer.Write(gif.m_ColorTable.ToArray());
                writer.Write(gif.m_ImageData.ToArray());
            }
            writer.Write(GifCreator.CreateLoopBlock());
            writer.Write((byte)0x3B); //End file
            writer.Close();
        }
        public static byte[] CreateGraphicControlExtensionBlock(int delay)
        {
            byte[] result = new byte[8];
            // Split the delay into high- and lowbyte
            byte d1 = (byte)(delay % 256);
            byte d2 = (byte)(delay / 256);
            result[0] = (byte)0x21; // Start ExtensionBlock
            result[1] = (byte)0xF9; // GraphicControlExtension
            result[2] = (byte)0x04; // Size of DataBlock (4)
            result[3] = (byte)0x00;
            result[4] = d1;
            result[5] = d2;
            result[6] = (byte)0x00;
            result[7] = (byte)0x00;
            return result;
        }
        public static byte[] CreateLoopBlock()
        { return CreateLoopBlock(0); }
        public static byte[] CreateLoopBlock(int numberOfRepeatings)
        {
            byte rep1 = (byte)(numberOfRepeatings % 256);
            byte rep2 = (byte)(numberOfRepeatings / 256);
            byte[] result = new byte[19];
            result[0] = (byte)0x21; // Start ExtensionBlock
            result[1] = (byte)0xFF; // ApplicationExtension
            result[2] = (byte)0x0B; // Size of DataBlock (11) for NETSCAPE2.0)
            result[3] = (byte)'N';
            result[4] = (byte)'E';
            result[5] = (byte)'T';
            result[6] = (byte)'S';
            result[7] = (byte)'C';
            result[8] = (byte)'A';
            result[9] = (byte)'P';
            result[10] = (byte)'E';
            result[11] = (byte)'2';
            result[12] = (byte)'.';
            result[13] = (byte)'0';
            result[14] = (byte)0x03; // Size of Loop Block
            result[15] = (byte)0x01; // Loop Indicator
            result[16] = (byte)rep1; // Number of repetitions
            result[17] = (byte)rep2; // 0 for endless loop
            result[18] = (byte)0x00;
            return result;
        }
        public enum GIFVersion
        {
            GIF87a,
            GIF89a
        }
        enum GIFBlockType
        {
            ImageDescriptor = 0x2C,
            Extension = 0x21,
            Trailer = 0x3B
        }
    }
}
