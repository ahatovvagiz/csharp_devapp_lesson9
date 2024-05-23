using System;
using System.IO;
using System.Text.Json;
using System.Xml;

namespace JsonToXmlConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            string json = """{"name": "John", "age": 30, "city": "New York"}""";


            // Parse the JSON string
            JsonDocument jsonDoc = JsonDocument.Parse(json);

            // Create XML writer
            using (var stream = new MemoryStream())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true }))
                {
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("root");

                    // Convert JSON to XML
                    ConvertJsonToXml(jsonDoc.RootElement, xmlWriter);

                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();
                }

                // Display the XML
                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    Console.WriteLine(reader.ReadToEnd());
                }
            }
        }

        static void ConvertJsonToXml(JsonElement jsonElement, XmlWriter xmlWriter)
        {
            foreach (var property in jsonElement.EnumerateObject())
            {
                xmlWriter.WriteStartElement(property.Name);

                if (property.Value.ValueKind == JsonValueKind.Object)
                {
                    ConvertJsonToXml(property.Value, xmlWriter);
                }
                else if (property.Value.ValueKind == JsonValueKind.Array)
                {
                    foreach (var element in property.Value.EnumerateArray())
                    {
                        ConvertJsonToXml(element, xmlWriter);
                    }
                }
                else
                {
                    xmlWriter.WriteString(property.Value.ToString());
                }

                xmlWriter.WriteEndElement();
            }
        }
    }
}