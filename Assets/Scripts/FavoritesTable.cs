using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.IO;


public class FavoritesTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> historyEntryTransformList;
    [SerializeField] private History _History;
    private List<HistoryEntry> historyEntryList;


    private void Awake()
    {
        entryContainer = transform.Find("HistoryEntryContainer");
        entryTemplate = entryContainer.Find("HistoryEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        //AddHistoryEntry(12222222, "asd");
        //Poate fi decomentat
        /*if (!System.IO.File.Exists(Application.persistentDataPath + "/HistoryData.json"))
        {
            AddHistoryEntry(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), "anamaria");
        }*/


        if (System.IO.File.Exists(Application.persistentDataPath + "/FavoritesData.json"))
        {

            string jsonString = System.IO.File.ReadAllText(Application.persistentDataPath + "/HistoryData.json");
            History history = JsonUtility.FromJson<History>(jsonString);



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


        entryTransform.Find("dateText").GetComponent<Text>().text = historyEntry.date;

        //string name = highScoreEntry.name;
        entryTransform.Find("nameText").GetComponent<Text>().text = historyEntry.name;

        //Set background visible odds and evens
        entryTransform.Find("Background").gameObject.SetActive(rank % 2 == 1);


        transformList.Add(entryTransform);
    }


    [System.Serializable]
    private class History
    {
        public List<HistoryEntry> historyEntryList;
    }

    [System.Serializable]
    private class HistoryEntry
    {
        public string date;
        public string name;
    }
}
