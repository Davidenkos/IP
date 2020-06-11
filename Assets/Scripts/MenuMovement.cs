using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMovement : MonoBehaviour
{

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
}
