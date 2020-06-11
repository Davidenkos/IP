using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FavoritesSwapIcon : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Button defaultIconBtn;
    [SerializeField] private Button favoriteIconBtn;
    [SerializeField] private SimpleCloudHandler cloudHandler;

    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> historyEntryTransformList;
    [SerializeField] private History _History;
    private List<HistoryEntry> historyEntryList;

    private void Awake()
    {
        SetFavorite(false);

        defaultIconBtn.onClick.AddListener(OnDefaultClick);
        favoriteIconBtn.onClick.AddListener(OnFavoriteClick);

    }

    void Start()
    {
    }

    private void OnFavoriteClick()
    {
        if (!string.IsNullOrEmpty(cloudHandler.movie.name))
        {
            DeleteFavoritesEntry(cloudHandler.movie.name);
            SetFavorite(false);
        }
    }

    private void OnDefaultClick()
    {   
        if (!string.IsNullOrEmpty(cloudHandler.movie.name))
        {
            AddFavoritesEntry(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), cloudHandler.movie.name);
            SetFavorite(true);
        }
    }

    public void SetFavorite(bool state)
    {
        defaultIconBtn.gameObject.SetActive(!state);
        favoriteIconBtn.gameObject.SetActive(state);
    }

    // Update is called once per frame
    void Update()
    {
    }


    private void DeleteFavoritesEntry(string name)
    {
        if (System.IO.File.Exists(Application.persistentDataPath + "/FavoritesData.json"))
        {
            string jsonString = System.IO.File.ReadAllText(Application.persistentDataPath + "/FavoritesData.json");
            History highScores = JsonUtility.FromJson<History>(jsonString);

            var element = highScores.historyEntryList.Find(x => x.name == name);
            highScores.historyEntryList.Remove(element);
        }
    }
    private void AddFavoritesEntry(string date, string name)
    {
        //Create HighScoreEntry
        HistoryEntry historyEntry = new HistoryEntry { date = date, name = name };
        if (!System.IO.File.Exists(Application.persistentDataPath + "/FavoritesData.json"))
        {
            historyEntryList = new List<HistoryEntry>()
            {
                new HistoryEntry{ date = date, name = name}
            };

            _History = new History { historyEntryList = historyEntryList };

            string potion = JsonUtility.ToJson(_History);

            System.IO.File.WriteAllText(Application.persistentDataPath + "/FavoritesData.json", potion);

        }
        else
        {
            //Load saved HighScores
            string jsonString = System.IO.File.ReadAllText(Application.persistentDataPath + "/FavoritesData.json");
            History highScores = JsonUtility.FromJson<History>(jsonString);

            //Add new entry
            var element = highScores.historyEntryList.Find(x => x.name == name);
            if (element == null)
            {
                highScores.historyEntryList.Add(historyEntry);
            }

            _History = new History { historyEntryList = highScores.historyEntryList };

            string list = JsonUtility.ToJson(_History);
            Debug.Log(list);

            System.IO.File.WriteAllText(Application.persistentDataPath + "/FavoritesData.json", list);
        }



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
