using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class MapManager : MonoBehaviour
{
    public List<GameObject> items = new List<GameObject>();
    private Logger logger;

    void Start()
    {
        logger = new Logger();
    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Save();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Clear();
        }
    }

    public void Save()
    {
        items.Clear();
        items.AddRange(GameObject.FindGameObjectsWithTag("Ground"));

        XmlDocument document = new XmlDocument();
        XmlElement root = document.CreateElement("Map");
        root.SetAttribute("Map_Name", "File_Name");

        foreach (GameObject item in items)
        {
            SpriteRenderer sr = item.gameObject.GetComponent<SpriteRenderer>();

            XmlElement element = document.CreateElement("ground");
            element.SetAttribute("name", item.name.Replace("(Clone)", ""));
            element.SetAttribute("x", item.transform.position.x.ToString());
            element.SetAttribute("y", item.transform.position.y.ToString());
            element.SetAttribute("so", sr.sortingOrder.ToString());
            element.SetAttribute("fx", sr.flipX ? "1" : "0");
            element.SetAttribute("fy", sr.flipY ? "1" : "0");

            root.AppendChild(element);
        }

        document.AppendChild(root);
        document.Save($"{Application.dataPath}/Map.xml");

        logger.LogMessage("File saved successfully to: " + $"{Application.dataPath}/Map.xml", LogType.Warning);
    }

    public void Load()
    {

        items.Clear();

        XmlDocument document = new XmlDocument();
        document.Load($"{Application.dataPath}/Map.xml");

        XmlNodeList nodes = document.GetElementsByTagName("ground");

        foreach (XmlNode node in nodes)
        {
            var pippo = node.Attributes["name"].Value;
            var prefab = Resources.Load($"Prefabs/{node.Attributes["name"].Value}") as GameObject;

            Vector3 position = new Vector3(float.Parse(node.Attributes["x"].Value),
                                           float.Parse(node.Attributes["y"].Value), 0);

            SpriteRenderer sr = prefab.GetComponent<SpriteRenderer>();
            sr.flipX = int.Parse(node.Attributes["fx"].Value) == 0 ? false : true;
            sr.flipY = int.Parse(node.Attributes["fy"].Value) == 0 ? false : true;
            sr.sortingOrder = int.Parse(node.Attributes["so"].Value);

            GameObject go = Instantiate(prefab, position, Quaternion.identity) as GameObject;
            go.name.Replace("(Clone)", ""); // TODO: Better way to do this?

            items.Add(prefab);
        }
    }

    public void Clear()
    {

        List<GameObject> list = new List<GameObject>();
        list.AddRange(GameObject.FindGameObjectsWithTag("Ground"));

        foreach (var i in list)
        {
            Destroy(i);
        }

        items.Clear();
    }
}
