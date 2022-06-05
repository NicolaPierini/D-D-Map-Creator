using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;

public class Encoder
{
    public XmlDocument TextToXML(string text)
    {
        XmlDocument document = new XmlDocument();

        return document;
    }

    public string XMLToString(XmlDocument document)
    {
        try
        {
            string result = string.Empty;

            XmlNodeList nodes = document.GetElementsByTagName("Ground");
            foreach (XmlNode node in nodes)
            {
                if (node.Name == "Ground")
                {
                    result += "g/";
                }

                string name = node.Attributes["name"].Value;
                string x = node.Attributes["x"].Value;
                string y = node.Attributes["y"].Value;
                string so = node.Attributes["so"].Value;
                string fx = node.Attributes["fx"].Value;
                string fy = node.Attributes["fy"].Value;

                result += $"{name}/{x}/{y}/{so}/{fx}/{fy}-";
            }
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
