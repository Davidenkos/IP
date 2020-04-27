using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Vuforia;

public class TrackableList : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Get the Vuforia StateManager
        StateManager sm = TrackerManager.Instance.GetStateManager();

        // Query the StateManager to retrieve the list of
        // currently 'active' trackables 
        //(i.e. the ones currently being tracked by Vuforia)
        IEnumerable<TrackableBehaviour> activeTrackables = sm.GetActiveTrackableBehaviours();

        // Iterate through the list of active trackables
        Debug.Log("List of trackables currently active (tracked): ");
        foreach (TrackableBehaviour tb in activeTrackables)
        {
            print("Trackable: " + tb.TrackableName);
        }
    }
}