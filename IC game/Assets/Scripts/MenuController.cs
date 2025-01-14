using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public Transform StartText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(StartText == null)
        {
            Debug.LogError("StartText is not assigned in the inspector");
        }
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
                if(hit.transform == StartText) {
                    // switch scene to game scene
                    SceneManager.LoadScene("game");
                }
            }
        }
    }
}
