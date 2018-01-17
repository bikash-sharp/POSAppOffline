using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BestariTerrace;

namespace BestariTerrace
{
    public static class PosExt
    {
        public static void Enlarged(this BinaryWriter bw, string text)
        {
            bw.Write(AsciiControlChars.Escape);
            bw.Write((byte)33);
            bw.Write((byte)32);
            bw.Write(System.Text.Encoding.Unicode.GetBytes(text));
            bw.Write(AsciiControlChars.Newline);
        }
        public static void High(BinaryWriter bw, string text)
        {
            bw.Write(AsciiControlChars.Escape);
            bw.Write((byte)33);
            bw.Write((byte)16);
            bw.Write(System.Text.Encoding.Unicode.GetBytes(text)); //Width,enlarged
            bw.Write(AsciiControlChars.Newline);
        }
        public static void LargeText(BinaryWriter bw, string text)
        {
            bw.Write(AsciiControlChars.Escape);
            bw.Write((byte)33);
            bw.Write((byte)48);
            bw.Write(System.Text.Encoding.Unicode.GetBytes(text));
            bw.Write(AsciiControlChars.Newline);
        }
        public static void FeedLines(this BinaryWriter bw, int lines)
        {
            bw.Write(AsciiControlChars.Newline);
            if (lines > 0)
            {
                bw.Write(AsciiControlChars.Escape);
                bw.Write('d');
                bw.Write((byte)lines - 1);
            }
        }
        public static void CutPaper(this BinaryWriter bw)
        {
            bw.Write(AsciiControlChars.GroupSeparator);
            bw.Write('V');
            bw.Write((byte)66);
            bw.Write((byte)3);
        }
        public static void FinishLines(this BinaryWriter bw)
        {
            //bw.FeedLines(1);
            //Center(bw, "---  Thank You, Come Again ---");
            //NormalFont(bw, "Goods sold are non-refundable");
            //NormalFont(bw, "All exchanges and returns must be made");
            //FeedLines(bw, 1);
            bw.FeedLines(1);
            bw.Center("---  Thank You, Come Again ---");
            bw.NormalFont("Goods sold are non-refundable");
            //bw.NormalFont("All exchanges and returns must be made");
            bw.FeedLines(1);
            bw.Write(AsciiControlChars.Newline);
        }

        public static void NormalFont(this BinaryWriter bw, string text, bool line = true)
        {
            bw.Write(AsciiControlChars.Escape);
            bw.Write((byte)33);
            bw.Write((byte)8);
            bw.Write(System.Text.Encoding.Unicode.GetBytes(text));
            if (line)
                bw.Write(AsciiControlChars.Newline);
        }

        public static void LeftJustify(this BinaryWriter bw, string text, bool line = true)
        {
            bw.Write(AsciiControlChars.Escape);
            bw.Write((byte)97);
            bw.Write((byte)0);
            bw.Write(System.Text.Encoding.Unicode.GetBytes(text));
            if (line)
                bw.Write(AsciiControlChars.Newline);
        }
        public static void Center(this BinaryWriter bw, string text, bool line = true)
        {
            bw.Write(AsciiControlChars.Escape);
            bw.Write((byte)97);
            bw.Write((byte)1);
            bw.Write(System.Text.Encoding.Unicode.GetBytes(text));
            if (line)
                bw.Write(AsciiControlChars.Newline);
        }

        public static void RightJustify(this BinaryWriter bw, string text, bool line = true)
        {
            bw.Write(AsciiControlChars.Escape);
            bw.Write((byte)97);
            bw.Write((byte)2);
            bw.Write(System.Text.Encoding.Unicode.GetBytes(text));
            if (line)
                bw.Write(AsciiControlChars.Newline);
        }

        public static void Justify(this BinaryWriter bw, string text, bool line = true)
        {
            bw.Write(AsciiControlChars.Escape);
            bw.Write((byte)97);
            bw.Write((byte)2);
            bw.Write(System.Text.Encoding.Unicode.GetBytes(text));
            if (line)
                bw.Write(AsciiControlChars.Newline);
        }
        public static void BoldON(this BinaryWriter bw, string text, bool line = true)
        {
            bw.Write(AsciiControlChars.Escape);
            bw.Write((byte)69);
            bw.Write(System.Text.Encoding.Unicode.GetBytes(text));
            if (line)
                bw.Write(AsciiControlChars.Newline);
        }
        public static void BoldOFF(this BinaryWriter bw, string text, bool line = true)
        {
            bw.Write(AsciiControlChars.Escape);
            bw.Write((byte)70);
            bw.Write(System.Text.Encoding.Unicode.GetBytes(text));
            if (line)
                bw.Write(AsciiControlChars.Newline);
        }

        public static void ColorRed(this BinaryWriter bw, string text, bool line = true)
        {
            bw.Write(AsciiControlChars.Escape);
            bw.Write((byte)114);
            bw.Write((byte)5);
            bw.Write(System.Text.Encoding.Unicode.GetBytes(text));
            if (line)
                bw.Write(AsciiControlChars.Newline);
        }

        public static void PrintLogo(this BinaryWriter bw, BestariTerrace.Forms.BitmapData data, BitArray dots, bool line = true)
        {
            byte[] width = BitConverter.GetBytes(data.Width);
            int offset = 0;
            MemoryStream stream = new MemoryStream();
            bw = new BinaryWriter(stream);

            bw.Write((char)0x1B);
            bw.Write('@');

            bw.Write((char)0x1B);
            bw.Write('3');
            bw.Write((byte)24);

            while (offset < data.Height)
            {
                bw.Write((char)0x1B);
                bw.Write('*');         // bit-image mode
                bw.Write((byte)33);    // 24-dot double-density
                bw.Write(width[0]);  // width low byte
                bw.Write(width[1]);  // width high byte

                for (int x = 0; x < data.Width; ++x)
                {
                    for (int k = 0; k < 3; ++k)
                    {
                        byte slice = 0;
                        for (int b = 0; b < 8; ++b)
                        {
                            int y = (((offset / 8) + k) * 8) + b;
                            //Calculate the location of the pixel we want in the bit array.
                            //It'll be at (y * width) + x.
                            int i = (y * data.Width) + x;

                            //If the image is shorter than 24 dots, pad with zero.
                            bool v = false;
                            if (i < dots.Length)
                            {
                                v = dots[i];
                            }
                            slice |= (byte)((v ? 1 : 0) << (7 - b));
                        }

                        bw.Write(slice);
                    }
                }
                offset += 24;
                bw.Write((char)0x0A);
            }
            //Restore the line spacing to the default of 30 dots.
            bw.Write((char)0x1B);
            bw.Write('3');
            bw.Write((byte)30);

            bw.Flush();
            if (line)
                bw.Write(AsciiControlChars.Newline);
        }
        /*
27 33 0     ESC ! NUL    Master style: pica                              ESC/P
27 33 1     ESC ! SOH    Master style: elite                             ESC/P
27 33 2     ESC ! STX    Master style: proportional                      ESC/P
27 33 4     ESC ! EOT    Master style: condensed                         ESC/P
27 33 8     ESC ! BS     Master style: emphasised                        ESC/P
27 33 16    ESC ! DLE    Master style: enhanced (double-strike)          ESC/P
27 33 32    ESC ! SP     Master style: enlarged (double-width)           ESC/P
27 33 64    ESC ! @      Master style: italic                            ESC/P
27 33 128   ESC ! ---    Master style: underline                         ESC/P
                     Above values can be added for combined styles.
         
        bw.Write(AsciiControlChars.Escape);
        bw.Write((byte)33);
        bw.Write((byte)0);
        bw.Write("test"); //Default, Pica
        bw.Write(AsciiControlChars.Newline);
 
        bw.Write(AsciiControlChars.Escape);
        bw.Write((byte)33);
        bw.Write((byte)4);
        bw.Write("test"); //condensed
        bw.Write(AsciiControlChars.Newline);
        bw.Write(AsciiControlChars.Escape);
        bw.Write((byte)33);
        bw.Write((byte)8);
        bw.Write("test"); //emphasised
        bw.Write(AsciiControlChars.Newline);
        bw.Write(AsciiControlChars.Escape);
        bw.Write((byte)33);
        bw.Write((byte)16);
        bw.Write("test"); //Height,enhanced
        bw.Write(AsciiControlChars.Newline);
        bw.Write(AsciiControlChars.Escape);
        bw.Write((byte)33);
        bw.Write((byte)32);
        bw.Write("test"); //Width,enlarged
        bw.Write(AsciiControlChars.Newline);
        bw.Write(AsciiControlChars.Escape);
        bw.Write((byte)33);
        bw.Write((byte)48);
        bw.Write("test");   //WxH
        bw.Write(AsciiControlChars.Newline);
             */
    }

    public static class AsciiControlChars
    {
        /// <summary>
        /// Usually indicates the end of a string.
        /// </summary>
        public const char Nul = (char)0x00;

        /// <summary>
        /// Meant to be used for printers. When receiving this code the 
        /// printer moves to the next sheet of paper.
        /// </summary>
        public const char FormFeed = (char)0x0C;

        /// <summary>
        /// Starts an extended sequence of control codes.
        /// </summary>
        public const char Escape = (char)0x1B;

        /// <summary>
        /// Advances to the next line.
        /// </summary>
        public const char Newline = (char)0x0A;

        /// <summary>
        /// Defined to separate tables or different sets of data in a serial
        /// data storage system.
        /// </summary>
        public const char GroupSeparator = (char)0x1D;

        /// <summary>
        /// A horizontal tab.
        /// </summary>
        public const char HorizontalTab = (char)0x09;


        /// <summary>
        /// Vertical Tab
        /// </summary>
        public const char VerticalTab = (char)0x11;


        /// <summary>
        /// Returns the carriage to the start of the line.
        /// </summary>
        public const char CarriageReturn = (char)0x0D;

        /// <summary>
        /// Cancels the operation.
        /// </summary>
        public const char Cancel = (char)0x18;

        /// <summary>
        /// Indicates that control characters present in the stream should
        /// be passed through as transmitted and not interpreted as control
        /// characters.
        /// </summary>
        public const char DataLinkEscape = (char)0x10;

        /// <summary>
        /// Signals the end of a transmission.
        /// </summary>
        public const char EndOfTransmission = (char)0x04;

        /// <summary>
        /// In serial storage, signals the separation of two files.
        /// </summary>
        public const char FileSeparator = (char)0x1C;

    }
}
