using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace XmlToJsonRigSoftware
{
    class Program
    {
        private static readonly List<string> currentElements = new List<string>();
        private static int counter = 0;
        private static string lastNodeName = "";
        private static List<KeyValueTypeJson> jsonObjects = new List<KeyValueTypeJson>();

        static void Main()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"Xml_2019-09-10.xml");

            StringBuilder sb = new StringBuilder();
            TraverseElements(doc.ChildNodes, sb);

            var result = "{" + sb.ToString().Remove(sb.ToString().LastIndexOf(',')) + "}";
            var json = JsonConvert.DeserializeObject<object>(result);
            Console.WriteLine(json);
        }

        private static void TraverseElements(XmlNodeList children, StringBuilder sb)
        {
            if (children.Count != 0)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    if (children.Item(i).ChildNodes.Count != 0)
                    {
                        currentElements.Add(children.Item(i).Name);
                        lastNodeName = children.Item(i).ParentNode?.Name;
                        TraverseElements(children.Item(i).ChildNodes, sb);
                    }
                }

                string value = children.Item(0).Value;

                if (children.Item(0).NodeType != XmlNodeType.XmlDeclaration)
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        var fullPropertyName = string.Join(".", currentElements);
                        var propertyValue = children.Item(0).Value;

                        var keyValueTypeJson = new KeyValueTypeJson
                        {
                            PropertyName = fullPropertyName,
                            Value = propertyValue,
                            Type = 0
                        };

                        jsonObjects.Add(keyValueTypeJson);

                        string stringLine = ("\"" + fullPropertyName + "\"" + ":" + "\"" + children.Item(0).Value + "\",");
                        sb.AppendLine(stringLine);
                    }

                    if (currentElements.Count != 0)
                    {
                        currentElements.RemoveAt(currentElements.Count - 1);
                        counter++;
                    }

                    if (currentElements.Count == counter && currentElements.Count != 0)
                    {
                        for (int j = 0; j < currentElements.Count - counter; j++)
                        {
                            currentElements.RemoveAt(currentElements.Count - 1);
                        }
                    }
                }
            }
        }
    }
}
