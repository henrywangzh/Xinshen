using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Shrine : Interactable
{
    [SerializeField] string levelToGoTo;
    [SerializeField] Image blackSquare;
    // Start is called before the first frame update
    void Start()
    {

    }

    override protected void Interact()
    {
        InvokeRepeating("FadeToBlack", 0, .05f);
    }

    void FadeToBlack()
    {
        if (blackSquare.color.a < 1)
        {
            blackSquare.color = new Vector4(blackSquare.color.r, blackSquare.color.g, blackSquare.color.b, blackSquare.color.a + 0.05f);
        } else
        {
            CancelInvoke("FadeToBlack");
            SceneManager.LoadScene(levelToGoTo);
        }
    }
}
