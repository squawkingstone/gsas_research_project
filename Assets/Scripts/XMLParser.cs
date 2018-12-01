using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

public class XMLParser {
	public static Dictionary<string, List<string>> xmlParse(string date){
		string context = System.IO.File.ReadAllText(string.Format("Assets\\Scripts\\{0}.xml", date));
		XmlDocument document = new XmlDocument();
		document.LoadXml(context);
		Dictionary<string, List<string>> dialogue = new Dictionary<string, List<string>>();
		XmlNodeList nodes = document.GetElementsByTagName("node");
		foreach(XmlNode node in nodes){
			List<string> r_dialogue = new List<string>();
			string id = "";
			XmlNodeList children = node.ChildNodes;
			foreach(XmlNode child in children){
				if(child.Name == "id"){
					id = child.InnerText;
				}
				if(child.Name == "text"){
					r_dialogue.Add(child.InnerText);
				}
			}
			dialogue.Add(id,r_dialogue);
		}
		return dialogue;
	}
}
