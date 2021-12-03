using System;
using System.Text;

namespace ChatCoreTest
{
  internal class Program
  {
    private static byte[] m_PacketData;
    private static uint m_Pos;

    public static void Main(string[] args)
    {
      m_PacketData = new byte[1024];
      m_Pos = 0;

      Write(1666);
      Write(109.99f);
      Write("Hello!");

      Console.Write($"Output Byte array(length:{m_Pos}): ");
      for (var i = 0; i < m_Pos; i++)
      {
        Console.Write(m_PacketData[i] + ", ");
      }
      Console.WriteLine("-------作業1:------");
      _Read(m_PacketData, m_Pos);
      Console.WriteLine("------作業2------");
      _Read2(m_PacketData, m_Pos);
      

      
    }

    // write an integer into a byte array
    private static bool Write(int i)
    {
      // convert int to byte array
      var bytes = BitConverter.GetBytes(i);
      _Write(bytes);
      return true;
    }

    // write a float into a byte array
    private static bool Write(float f)
    {
      // convert int to byte array
      var bytes = BitConverter.GetBytes(f);
      _Write(bytes);
      return true;
    }

    // write a string into a byte array
    private static bool Write(string s)
    {
      // convert string to byte array
      var bytes = Encoding.Unicode.GetBytes(s);

      // write byte array length to packet's byte array
      if (Write(bytes.Length) == false)
      {
        return false;
      }

      _Write(bytes);
      return true;
    }

    // write a byte array into packet's byte array
    private static void _Write(byte[] byteData)
    {
      // converter little-endian to network's big-endian
      if (BitConverter.IsLittleEndian)
      {
        Array.Reverse(byteData);
      }

      byteData.CopyTo(m_PacketData, m_Pos);
      m_Pos += (uint)byteData.Length;
    }

    private static void _Read(byte[] m_PacketData, uint m_Pos)
    {
            byte[] ibackByte = new byte[4];
            byte[] fbackByte = new byte[4];
            byte[] sbackByte = new byte[m_Pos-8];
            for (int i=0; i<4; i++)
            {
                ibackByte[i]= m_PacketData[i];
            }
            for (int i = 4; i < 8; i++)
            {
                fbackByte[i-4] = m_PacketData[i];
            }
            for (int i = 8; i < m_Pos; i++)
            {
                sbackByte[i-8] = m_PacketData[i];
            }
            Array.Reverse(ibackByte);
            int a = BitConverter.ToInt32(ibackByte,0);
            Array.Reverse(fbackByte);
            float aa = BitConverter.ToSingle(fbackByte, 0); 
            Array.Reverse(sbackByte);
            string aaa = System.Text.Encoding.Unicode.GetString(sbackByte);
            Console.WriteLine();
            Console.WriteLine("int------>  "+a);
            Console.WriteLine("float---->  "+aa );
            Console.WriteLine("string--->  "+aaa );
    }
        private static void _Read2(byte[] m_PacketData, uint m_Pos)
        {
            byte[] topLength = new byte[1024];
            for (int i= 0;i<m_Pos;i++)
            {
                topLength[i + 1] = m_PacketData[i];
            }
            var dataLength = BitConverter.GetBytes(m_Pos+1);
            //for(int i=0;i<4;i++)
            //{
            //    topLength[i] = dataLength[i];
            //}
            topLength[0] = dataLength[0];
      
            for (int i = 0; i < m_Pos+1; i++)
            {
                Console.Write(topLength[i] + ", ");
            }
            _Read(m_PacketData, m_Pos);

        }
    }
}
