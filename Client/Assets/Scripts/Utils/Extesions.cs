using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

public static class Extesions
{
    public static string Deserialize(this byte[] buffer)
    {
        return Encoding.UTF8.GetString(buffer);
    }
    public static byte[] Serialize(this string message)
    {
        return Encoding.UTF8.GetBytes(message);
    }
    public static byte[] Serialize(this object message)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (MemoryStream mStream = new MemoryStream())
        {
            formatter.Serialize(mStream, message);
            return mStream.GetBuffer();
        }
    }
    public static T DeserializeObject<T>(this byte[] message)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        try
        {
            using (MemoryStream mStream = new MemoryStream(message))
            {
                T obj = (T)formatter.Deserialize(mStream);
                return obj;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Falha ao deserilizar {ex.Message}");
            return default;
        }
    }
}