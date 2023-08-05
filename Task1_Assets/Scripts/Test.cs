using System;
using System.Text;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Test : MonoBehaviour
{
    string apiUrl;
    Uri address;
    JsonData jsonData;

    [SerializeField] private GameObject dropdown;
    private TMP_Dropdown m_dropdown;

    void Start()
    {
        apiUrl = "https://qa2.sunbasedata.com/sunbase/portal/api/assignment.jsp?cmd=client_data";
        address = new Uri(apiUrl);

        WebRequest request = WebRequest.Create(address);
        request.Method = "GET";
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        string jsonString = null;
        using Stream stream = response.GetResponseStream();
        StreamReader streamReader = new StreamReader(stream);
        jsonString = streamReader.ReadToEnd();
        jsonString = jsonString.Trim();
        streamReader.Close();

        Debug.Log(jsonString);

        // database = JsonUtility.FromJson<Database>(jsonString);
        // Dictionary<int, DataEntry> dataDictionary = new Dictionary<int, DataEntry>();
        
        // if (database.data !=null)
        // foreach (var entry in database.data)
        // {
        //     dataDictionary.Add(entry.Key, entry.Value);
        // }

        // foreach (Client client in database.clients)
        // {
        //     Debug.Log("Client ID: " + client.id + ", Label: " + client.label);
        // }
        // foreach (KeyValuePair<int, DataEntry> entry in database.data)
        // {
        //     Debug.Log("DataEntry Entry ID: " + entry.Key + ", Name: " + entry.Value.name + ", Address: " + entry.Value.address + ", Points: " + entry.Value.points);
        // }
        // Debug.Log("Label: " + database.label);

        jsonData = JsonUtility.FromJson<JsonData>(jsonString);

        // Convert the data entries list to a dictionary
        Dictionary<int, DataEntry> dataDictionary = jsonData.data.ToDictionary();

        // Access the data
        foreach (Client client in jsonData.clients)
        {
            Debug.Log("Client ID: " + client.id + ", Label: " + client.label);
        }

        foreach (KeyValuePair<int, DataEntry> entry in dataDictionary)
        {
            Debug.Log("Data Entry Points: " + entry.Key + ", Name: " + entry.Value.name + ", Address: " + entry.Value.address + ", Points: " + entry.Value.points);
        }

        Debug.Log("Label: " + jsonData.label);

        m_dropdown = dropdown.GetComponent<TMP_Dropdown>();
        m_dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(m_dropdown);
        });
    }

    private void DropdownValueChanged(TMP_Dropdown m_dropdown)
    {
        Debug.Log(m_dropdown.options[m_dropdown.value].text);

        string option = m_dropdown.options[m_dropdown.value].text;
        switch(option)
        {
            case "All Clients":
            foreach (Client client in jsonData.clients)
            {
                Debug.Log(client.label);
            }
            break;
            case "Managers only":
            foreach (Client client in jsonData.clients)
            {
                if (client.isManager) Debug.Log(client.label);
            }
            break;
            case "Non Managers":
            foreach (Client client in jsonData.clients)
            {
                if (!client.isManager) Debug.Log(client.label);
            }
            break;
            default:
            break;
        }
    }
}

[System.Serializable]
public class Client
{
    public bool isManager;
    public int id;
    public string label;
}

[System.Serializable]
public class DataEntry
{
    public string address;
    public string name;
    public int points;
}

[System.Serializable]
public class JsonData
{
    public List<Client> clients;
    public DataEntryList data;
    public string label;
}

[System.Serializable]
public class DataEntryList
{
    public List<DataEntry> entries;

    // Custom method to get the data as a dictionary
    public Dictionary<int, DataEntry> ToDictionary()
    {
        Dictionary<int, DataEntry> dictionary = new Dictionary<int, DataEntry>();
        if (entries != null)
        {
            foreach (DataEntry entry in entries)
            {
                dictionary.Add(entry.points, entry);
            }
        }
        return dictionary;
    }
}
