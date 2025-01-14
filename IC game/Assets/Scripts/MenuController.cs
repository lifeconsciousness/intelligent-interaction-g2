using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public Transform StartText;
    public Transform calibrateText;
    public SceneAsset gameScene;

    public GameObject MainMenu;
    public GameObject Calibrationmenu;
    // There are 2 possible menu states Main and Calibrate depending on which state currently exists we need to toggle the corresponding game objects

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(StartText == null)
        {
            Debug.LogError("StartText is not assigned in the inspector");
        }

        if(gameScene == null)
        {
            Debug.LogError("gameScene is not assigned in the inspector");
        }

        if(MainMenu == null)
        {
            Debug.LogError("MainMenu is not assigned in the inspector");
        }

        if(Calibrationmenu == null)
        {
            Debug.LogError("Calibrationmenu is not assigned in the inspector");
        }

        // Initially we want to show the main menu and hide the calibration menu
        MainMenu.SetActive(true);
        Calibrationmenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                switch (hit.transform)
                {
                    case Transform t when t == StartText:
                        Debug.Log("StartText was clicked");
                        break;
                    
                    case Transform t when t == calibrateText:
                        Debug.Log("calibrateText was clicked");
                        MainMenu.SetActive(false);
                        Calibrationmenu.SetActive(true);
                        break;
                }

                if(hit.transform == StartText) {
                    // switch scene to game scene
                    SceneManager.LoadScene(gameScene.name);
                }
            }
        }
    
        // if escape is hit go back to the main menu
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            MainMenu.SetActive(true);
            Calibrationmenu.SetActive(false);
        }
    }
}
