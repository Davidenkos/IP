using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class MenuMovement : MonoBehaviour
{

    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> historyEntryTransformList;
    [SerializeField] private History _History;
    private List<HistoryEntry> historyEntryList;

    public GameObject menuOriginalPos;
    public GameObject menuActivePos;
    public GameObject menuPanel;
    public SimpleCloudHandler scanner;

    public bool movePanel;
    public bool movePanelBack;

    private float speed = 10;
    // Start is called before the first frame update
    void Start()
    {
        // make sure that the panel is hidden
        menuPanel.transform.position = menuOriginalPos.transform.position;
        scanner = GameObject.FindGameObjectWithTag("CloudRecognition").GetComponent<SimpleCloudHandler>();

    }

    // Update is called once per frame
    void Update()
    {
        if (movePanel)
        {
            menuPanel.transform.position = Vector3.Lerp(menuPanel.transform.position, menuActivePos.transform.position, speed * Time.deltaTime);
        }
        else if (movePanelBack)
        {
            menuPanel.transform.position = Vector3.Lerp(menuPanel.transform.position, menuOriginalPos.transform.position, speed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && movePanel)
        {
            MovePanelBack();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void MovePanel()
    {
        movePanelBack = false;
        movePanel = true;
    }

    public void MovePanelBack()
    {
        movePanel = false;
        movePanelBack = true;
    }

    public void HistoryButton()
    {
        SceneManager.LoadSceneAsync("HistoryTableResponsive");

    }

    public void FavoritesButton()
    {
        SceneManager.LoadSceneAsync("FavoritesTableResponsive");

    }

    public void NewFav()
    {
        //scanner.scanned = false;
        Debug.Log("Ioana");
        Debug.Log(scanner.scanned);
        if (scanner.scanned)
        {
            HistoryEntry historyEntry = new HistoryEntry { date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), name = scanner.movie.name };
            if (System.IO.File.Exists(Application.persistentDataPath + "/FavoritesData.json"))
            {
                //Load saved HighScores
                string jsonString = System.IO.File.ReadAllText(Application.persistentDataPath + "/FavoritesData.json");
                History highScores = JsonUtility.FromJson<History>(jsonString);

                var element = highScores.historyEntryList.Find(x => x.name == scanner.movie.name);

                //Add new entry
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
        AddFavoritesEntry(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), "Test");

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
            Debug.Log("Ioana");
            Debug.Log(element);

            //Add new entry
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
