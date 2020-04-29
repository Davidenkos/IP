using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.IO;


public class HistoryTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> historyEntryTransformList;
    [SerializeField] private History _History;


    private void Awake()
    {
        entryContainer = transform.Find("HistoryEntryContainer");
        entryTemplate = entryContainer.Find("HistoryEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        //AddHistoryEntry(12222222, "asd");

        
        Debug.Log(System.IO.File.Exists(Application.persistentDataPath + "/HistoryData.json"));
        Debug.Log(Application.persistentDataPath);
        Debug.Log(System.IO.File.Exists(Application.persistentDataPath + "/HistoryData.json"));
        if (System.IO.File.Exists(Application.persistentDataPath + "/HistoryData.json"))
        {

            string jsonString = System.IO.File.ReadAllText(Application.persistentDataPath + "/HistoryData.json");
            History history = JsonUtility.FromJson<History>(jsonString);

            Debug.Log("Ioana");

            Debug.Log(history);
            for (int i = 0; i < history.historyEntryList.Count; i++)
            {
                for (int j = i + 1; j < history.historyEntryList.Count; j++)
                {
                    if (history.historyEntryList[j].date > history.historyEntryList[i].date)
                    {
                        HistoryEntry temp = history.historyEntryList[i];
                        history.historyEntryList[i] = history.historyEntryList[j];
                        history.historyEntryList[j] = temp;
                    }
                }

            }
            historyEntryTransformList = new List<Transform>();
            var index = 0;
            foreach (HistoryEntry historyEntry in history.historyEntryList)
            {
                index += 1;
                if (index <= 15)
                {
                    CreateHistoryEntryTemplate(historyEntry, entryContainer, historyEntryTransformList);
                }
            }


        }


    }
   

    private void CreateHistoryEntryTemplate(HistoryEntry historyEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 70f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;


        entryTransform.Find("dateText").GetComponent<Text>().text = historyEntry.date.ToString();

        //string name = highScoreEntry.name;
        entryTransform.Find("nameText").GetComponent<Text>().text = historyEntry.name;

        //Set background visible odds and evens
        entryTransform.Find("Background").gameObject.SetActive(rank % 2 == 1);

       
        transformList.Add(entryTransform);
    }
   

    private void AddHistoryEntry(int date, string name)
    {
        //Create HighScoreEntry
        HistoryEntry historyEntry = new HistoryEntry { date = date, name = name };

        //Load saved HighScores
        string jsonString = System.IO.File.ReadAllText(Application.persistentDataPath + "/HistoryData.json");
        History highScores = JsonUtility.FromJson<History>(jsonString);

        //Add new entry
        highScores.historyEntryList.Add(historyEntry);


        _History = new History { historyEntryList = highScores.historyEntryList };

        string list = JsonUtility.ToJson(_History);
        Debug.Log(list);

        System.IO.File.WriteAllText(Application.persistentDataPath + "/HistoryData.json", list);

    }

    [System.Serializable]
    private class History
    {
        public List<HistoryEntry> historyEntryList;
    }

    [System.Serializable]
    private class HistoryEntry
    {
        public int date;
        public string name;
    }
}
