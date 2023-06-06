using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RespawnManager : MonoBehaviour
{
    static string lastScene;
    static Vector3 lastPosition;
    static bool respawnQued;
    [SerializeField] Image blackSquare;

    static RespawnManager self;
    static bool firstStart;

    public static void SetRespawn()
    {
        lastScene = SceneManager.GetActiveScene().name;
        lastPosition = PredictionManager.playerTrfm.position;
        Debug.Log("set respawn position: " + lastPosition);
    }

    public static void LoadScene()
    {
        respawnQued = true;
        SceneManager.LoadScene(lastScene);
    }

    public static void Respawn()
    {
        self.InvokeRepeating("FadeToBlack", 0, .05f);
    }

    private void Awake()
    {
        self = GetComponent<RespawnManager>();
    }

    void Start()
    {
        if (!firstStart)
        {
            SetRespawn();
            firstStart = true;
        }

        if (respawnQued)
        {
            Debug.Log("respawning player: " + lastPosition);
            PredictionManager.playerTrfm.position = lastPosition;

            respawnQued = false;
        }
    }

    void FadeToBlack()
    {
        if (blackSquare.color.a < 2f)
        {
            blackSquare.color = new Vector4(blackSquare.color.r, blackSquare.color.g, blackSquare.color.b, blackSquare.color.a + 0.03f);
        }
        else
        {
            LoadScene();
        }
    }
}
