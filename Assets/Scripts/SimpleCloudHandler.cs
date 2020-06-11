using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.IO;

public class SimpleCloudHandler : MonoBehaviour, IObjectRecoEventHandler
{

    [Serializable]
    public class Movie
    {
        public string name;
        public string url;
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

    public bool scanned = false;
    public ImageTargetBehaviour ImageTargetTemplate;
    private CloudRecoBehaviour mCloudRecoBehaviour;
    private bool mIsScanning = false;
    private string mTargetMetadata = "";
    private string URL = "https://www.google.com/";
    private string btnName;
    public Movie movie = new Movie();

    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> historyEntryTransformList;
    [SerializeField] private History _History;
    private List<HistoryEntry> historyEntryList;


    // Use this for initialization 
    void Start()
    {
        // register this event handler at the cloud reco behaviour 
        mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();

        if (mCloudRecoBehaviour)
        {
            mCloudRecoBehaviour.RegisterEventHandler(this);
        }
    }


    public void OnInitialized(TargetFinder targetFinder)
    {
        Debug.Log("Cloud Reco initialized");
    }
    public void OnInitError(TargetFinder.InitState initError)
    {
        Debug.Log("Cloud Reco init error " + initError.ToString());
    }
    public void OnUpdateError(TargetFinder.UpdateState updateError)
    {
        Debug.Log("Cloud Reco update error " + updateError.ToString());
    }

    public void OnStateChanged(bool scanning)
    {
        mIsScanning = scanning;
        if (scanning)
        {
            // clear all known trackables
            var tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            tracker.GetTargetFinder<ImageTargetFinder>().ClearTrackables(false);
        }

        scanned = false;
    }

    // Here we handle a cloud target recognition event
    public void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult)
    {
        TargetFinder.CloudRecoSearchResult cloudRecoSearchResult =
            (TargetFinder.CloudRecoSearchResult)targetSearchResult;
        // do something with the target metadata
        mTargetMetadata = cloudRecoSearchResult.MetaData;
        // stop the target finder (i.e. stop scanning the cloud)
        mCloudRecoBehaviour.CloudRecoEnabled = false;

        // Build augmentation based on target 
        if (ImageTargetTemplate)
        {
            // enable the new result with the same ImageTargetBehaviour: 
            ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            tracker.GetTargetFinder<ImageTargetFinder>().EnableTracking(targetSearchResult, ImageTargetTemplate.gameObject);
        }
        movie = JsonUtility.FromJson<Movie>(mTargetMetadata);
        mTargetMetadata = movie.name;
        URL = movie.url;
        Debug.Log("Ioana");
        AddHistoryEntry(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), movie.name);
        scanned = true;
    }

    private void AddHistoryEntry(string date, string name)
    {
        //Create HighScoreEntry
        HistoryEntry historyEntry = new HistoryEntry { date = date, name = name };
        if (!System.IO.File.Exists(Application.persistentDataPath + "/HistoryData.json"))
        {
            historyEntryList = new List<HistoryEntry>()
            {
                new HistoryEntry{ date = date, name = name}
            };

            _History = new History { historyEntryList = historyEntryList };

            string potion = JsonUtility.ToJson(_History);

            System.IO.File.WriteAllText(Application.persistentDataPath + "/HistoryData.json", potion);

        }
        else
        {
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



    }

    void OnGUI()
    {
        // Display current 'scanning' status
        GUI.Box(new Rect(100, 100, 200, 50), mIsScanning ? "Scanning" : "Not scanning");
        // Display metadata of latest detected cloud-target
        GUI.Box(new Rect(100, 200, 200, 50), "Metadata: " + mTargetMetadata);
        // If not scanning, show button
        // so that user can restart cloud scanning
        if (!mIsScanning)
        {
            if (GUI.Button(new Rect(100, 300, 100, 50), "Restart Scan"))
            {
                // Restart TargetFinder                
                mCloudRecoBehaviour.CloudRecoEnabled = true;
            }
            if (GUI.Button(new Rect(200, 300, 100, 50), "Movie Trailer"))
            {
                Application.OpenURL(URL);
            }
        }
    }

   
   void Update()
   {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit Hit;
                if (Physics.Raycast(ray, out Hit))
                {
                    btnName = Hit.transform.name;

                    switch (btnName)
                    {
                        case "YesButton":
                        {
                            Application.OpenURL(URL);
                            break;
                        }
                        case "NoButton":
                        {
                                Application.OpenURL("https://www.google.com/");
                            break;
                        }
                    }
                }
            }
        }
    }