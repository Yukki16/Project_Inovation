using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class StreamUtil
{
    public static void Write(NetworkStream pStream, byte[] pBytes)
    {
        //convert message length to 4 bytes and write those bytes into the stream
        pStream.Write(BitConverter.GetBytes(pBytes.Length), 0, 4);
        //now send the bytes of the message themselves
        pStream.Write(pBytes, 0, pBytes.Length);
    }

    /**
     * Reads the amount of bytes to receive from the stream and then the bytes themselves.
     */
    public static byte[] Read(NetworkStream pStream)
    {
        //get the message size first
        int byteCountToRead = BitConverter.ToInt32(Read(pStream, 4), 0);
        //then read that amount of bytes
        return Read(pStream, byteCountToRead);
    }

    /**
     * Read the given amount of bytes from the stream
     */
    private static byte[] Read(NetworkStream pStream, int pByteCount)
    {
        //create a buffer to hold all the requested bytes
        byte[] bytes = new byte[pByteCount];
        //keep track of how many bytes we read last read operation
        int bytesRead = 0;
        //and keep track of how many bytes we've read in total
        int totalBytesRead = 0;

        try
        {
            //keep reading bytes until we've got what we are looking for or something bad happens.
            while (
                totalBytesRead != pByteCount &&
                (bytesRead = pStream.Read(bytes, totalBytesRead, pByteCount - totalBytesRead)) > 0
            )
            {
                totalBytesRead += bytesRead;
            }
        }
        catch { }

        return (totalBytesRead == pByteCount) ? bytes : null;
    }
}
