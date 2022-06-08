using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

public class MapManager : MonoBehaviour
{
    public List<GameObject> items = new List<GameObject>();
    public QRGenerator qRGenerator;

    private Logger logger;
    private XmlDocument storedDocument;

    #region  Unity
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
            ClearItemsList();
        }
    }
    #endregion

    #region  IO methods
    public void Save()
    {
        try
        {
            items.Clear();
            items.AddRange(GameObject.FindGameObjectsWithTag("Ground"));

            storedDocument = new XmlDocument();
            XmlElement root = storedDocument.CreateElement("Map");
            root.SetAttribute("Map_Name", "File_Name");

            foreach (GameObject item in items)
            {
                SpriteRenderer sr = item.gameObject.GetComponent<SpriteRenderer>();

                XmlElement element = storedDocument.CreateElement("Ground");
                string name = item.name.Replace("(Clone)", "");
                name = $"{name[0]}{name[1]}{name[2]}";

                element.SetAttribute("name", name);
                element.SetAttribute("x", item.transform.position.x.ToString());
                element.SetAttribute("y", item.transform.position.y.ToString());
                element.SetAttribute("so", sr.sortingOrder.ToString());
                element.SetAttribute("fx", sr.flipX ? "1" : "0");
                element.SetAttribute("fy", sr.flipY ? "1" : "0");

                root.AppendChild(element);
            }

            storedDocument.AppendChild(root);
            storedDocument.Save($"{Application.dataPath}/Map.xml");

            Encoder encoder = new Encoder();
            qRGenerator.EncodeTextToQR(encoder.XMLToString(storedDocument));
            qRGenerator.SetQRVisibility(true);

            logger.LogMessage("File saved successfully to: " + $"{Application.dataPath}/Map.xml", LogType.Warning);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public void Load(XmlDocument document = null)
    {
        qRGenerator.SetQRVisibility(false);
        items.Clear();

        XmlNodeList nodes;

        if (document != null)
        {
            nodes = document.GetElementsByTagName("ground");
        }
        else
        {
            storedDocument = new XmlDocument();
            storedDocument.Load($"{Application.dataPath}/Map.xml");

            nodes = storedDocument.GetElementsByTagName("Ground");
        }

        foreach (XmlNode node in nodes)
        {
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
    #endregion

    #region  Gameplay
    void Connect(Transform pointA, Transform pointB)
    {

    }
    #endregion



    public void ClearItemsList()
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
