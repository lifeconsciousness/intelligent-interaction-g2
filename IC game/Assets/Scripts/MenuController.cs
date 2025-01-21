using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Transform StartText;
    public Transform calibrateText;
    public SceneAsset gameScene;

    public GameObject MainMenu;
    public GameObject Calibrationmenu;

    private FaceTrackerReceiver faceTracker;
    public Button BackButton;

    void Start()
    {
        faceTracker = FaceTrackerReceiver.Instance;
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
        BackButton.gameObject.SetActive(false);

        BackButton.onClick.AddListener(OnCalibrationDone);
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
                        BackButton.gameObject.SetActive(true);
                        faceTracker.StartCalibration();
                        break;
                }
            }
        }

        // If escape is hit, go back to the main menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnCalibrationDone();
        }
    }

    // This method is called when the user is done in the calibration menu
    public void OnCalibrationDone()
    {
        MainMenu.SetActive(true);
        Calibrationmenu.SetActive(false);
        faceTracker.StopCalibration();
        BackButton.gameObject.SetActive(false);
    }
}