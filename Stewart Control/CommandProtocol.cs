using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Globalization;
 
namespace Stewart_Control
{

    static public class CommandProtocol
    {
        public enum Cmd
        {
            Empty = 0,
            setX = 10, setY, setZ, setRoll, setPitch, setYaw,
            setInx, setZmienna,
            getX = 127, getY, getZ, getRoll, getPitch, getYaw,
            getZmienna
        };

        public static byte[] NewSimple(Cmd command, float value)
        {
            string str = ((int)command).ToString();
            str += '=';
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            str += value.ToString(nfi);
            str += ';';
            return Encoding.ASCII.GetBytes(str);
        }

        public static byte[] NewComplex(Cmd[] commands, float[] values)
        {
            if (commands.Length != values.Length)
            {
                throw new Exception("Wrong sizes of commands or values array!");
            }
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            string str = string.Empty;
            for (int i=0; i<commands.Length; i++)
            {
                str += ((int)commands[i]).ToString();
                str += '=';
                str += values[i].ToString(nfi);
                str += ';';
            }
            
            return Encoding.ASCII.GetBytes(str);
        }


    }
}