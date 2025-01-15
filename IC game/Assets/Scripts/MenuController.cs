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

    private FaceTrackerReceiver faceTracker; // Add this line

    void Start()
    {
        faceTracker = FaceTrackerReceiver.Instance; // Add this block
        if (faceTracker == null)
        {
            Debug.LogError("FaceTrackerReceiver instance not found.");
        }

        if (StartText == null)
        {
            Debug.LogError("StartText is not assigned in the inspector");
        }

        if (gameScene == null)
        {
            Debug.LogError("gameScene is not assigned in the inspector");
        }

        if (MainMenu == null)
        {
            Debug.LogError("MainMenu is not assigned in the inspector");
        }

        if (Calibrationmenu == null)
        {
            Debug.LogError("Calibrationmenu is not assigned in the inspector");
        }

        // Initially we want to show the main menu and hide the calibration menu
        MainMenu.SetActive(true);
        Calibrationmenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                switch (hit.transform)
                {
                    case Transform t when t == StartText:
                        Debug.Log("StartText was clicked");
                        // switch scene to game scene
                        SceneManager.LoadScene(gameScene.name);
                        break;

                    case Transform t when t == calibrateText:
                        Debug.Log("calibrateText was clicked");
                        MainMenu.SetActive(false);
                        Calibrationmenu.SetActive(true);
                        faceTracker.StartCalibration(); // Start calibration
                        break;
                }
            }
        }

        // If escape is hit, go back to the main menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MainMenu.SetActive(true);
            Calibrationmenu.SetActive(false);
            faceTracker.StopCalibration(); // Stop calibration
        }
    }

    // Call this method when the user clicks the 'Done' button in the calibration menu
    public void OnCalibrationDone()
    {
        MainMenu.SetActive(true);
        Calibrationmenu.SetActive(false);
        faceTracker.StopCalibration(); // Stop calibration
    }
}