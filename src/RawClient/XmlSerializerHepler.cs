﻿using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace RawClient;

public class XmlSerializerHepler
{
    public static T Deserialize<T>(string filePath) where T : class, new()
    {
        T result = default!;

        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                result = xmlSerializer.Deserialize(reader) as T;
            }
        }
        return result;
    }

    /// <summary>
    /// Serialize a serializable object to XML string.
    /// </summary>
    /// <typeparam name="T">Type of object</typeparam>
    /// <param name="xmlObject">Type of object</param>
    /// <param name="useNamespaces">Use of XML namespaces</param>
    /// <returns>XML string</returns>
    public static string SerializeToXmlString<T>(T xmlObject, bool useNamespaces = true)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        MemoryStream memoryStream = new MemoryStream();
        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        xmlTextWriter.Formatting = Formatting.Indented;

        if (useNamespaces)
        {
            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add("", "");
            xmlSerializer.Serialize(xmlTextWriter, xmlObject, xmlSerializerNamespaces);
        }
        else
            xmlSerializer.Serialize(xmlTextWriter, xmlObject);

        string output = Encoding.UTF8.GetString(memoryStream.ToArray());
        string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
        if (output.StartsWith(_byteOrderMarkUtf8))
        {
            output = output.Remove(0, _byteOrderMarkUtf8.Length);
        }

        return output;
    }

    /// <summary>
    /// Serialize a serializable object to XML string and create a XML file.
    /// </summary>
    /// <typeparam name="T">Type of object</typeparam>
    /// <param name="xmlObject">Type of object</param>
    /// <param name="filename">XML filename with .XML extension</param>
    /// <param name="useNamespaces">Use of XML namespaces</param>
    public static void SerializeToXmlFile<T>(T xmlObject, string filename, bool useNamespaces = true)
    {
        try
        {
            File.WriteAllText(filename, SerializeToXmlString(xmlObject, useNamespaces));
        }
        catch
        {
            throw new Exception();
        }
    }

    /// <summary>
    /// Deserialize XML string to an object.
    /// </summary>
    /// <typeparam name="T">Type of object</typeparam>
    /// <param name="xml">XML string</param>
    /// <returns>XML-deserialized object</returns>
    public static T DeserializeFromXmlString<T>(string xml) where T : new()
    {
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.IgnoreWhitespace = false;
        //T xmlObject = default;
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        StringReader stringReader = new StringReader(xml);
        XmlReader reader = XmlReader.Create(stringReader, settings);
        T xmlObject = (T)xmlSerializer.Deserialize(reader);
        return xmlObject;
    }

    /// <summary>
    /// Deserialize XML string from XML file to an object.
    /// </summary>
    /// <typeparam name="T">Type of object</typeparam>
    /// <param name="filename">XML filename with .XML extension</param>
    /// <returns>XML-deserialized object</returns>
    public static T DeserializeFromXmlFile<T>(string filename) where T : new()
    {
        if (!File.Exists(filename))
        {
            throw new FileNotFoundException();
        }

        return DeserializeFromXmlString<T>(File.ReadAllText(filename));
    }
}