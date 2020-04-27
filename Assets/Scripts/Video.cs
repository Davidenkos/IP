using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Video : MonoBehaviour
{
    public AudioClip aClips;

    public AudioSource myAudioSource;

    string btnName;



    // Use this for initialization

    void Start()
    {

        myAudioSource = GetComponent<AudioSource>();

    }



    // Update is called once per frame

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
                        Application.OpenURL("https://www.google.com/");
                        break;
                    }
                    case "NoButton":
                    {
                        myAudioSource.clip = aClips;
                        myAudioSource.Play();
                        break;
                    }
                }
            }
        }
    }
}
