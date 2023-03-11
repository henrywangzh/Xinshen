using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaytestingManager : MonoBehaviour
{
    [SerializeField] bool isBossRoom = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            if (isBossRoom)
                SceneManager.LoadScene("FirstLevel");
            else
                SceneManager.LoadScene("MingTest");
        } else if (Input.GetKeyDown(KeyCode.Minus))
        {
            if (isBossRoom)
                SceneManager.LoadScene("MingTest");
            else
                SceneManager.LoadScene("FirstLevel");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }
}
