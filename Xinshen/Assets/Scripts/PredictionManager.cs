using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictionManager : MonoBehaviour
{
    [SerializeField] Transform predictionIndicator;
    static Transform playerTrfm;
    // Start is called before the first frame update
    void Awake()
    {
        playerTrfm = transform;
        playerPositions = new Vector3[10];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (calculateTimer > 0) { calculateTimer--; }
        else
        {
            calculateTimer = 5;
            CalculatePredictedPos();
            predictionIndicator.position = GetPredictedPos(1, false);
        }
    }

    static Vector3[] playerPositions;
    static Vector3 predictedOffset;
    static int addPos, calculateTimer;

    public static Vector3 GetPredictedPos(float seconds, bool verticalTargeting = true) //larger preditions are less accurate (obviously), ~1.5s is the higher end of accurate predictions
    {
        int newestPos = addPos - 3;
        if (newestPos < 0) { newestPos += 10; }
        predictedOffset = (playerTrfm.position - playerPositions[addPos]) * seconds * .5f;
        predictedOffset += (playerTrfm.position - playerPositions[newestPos]) * 4 * seconds * .5f;

        if (verticalTargeting) { return predictedOffset + playerTrfm.position; }
        predictedOffset.y = 0;
        return predictedOffset;
    }
    private void CalculatePredictedPos()
    {
        playerPositions[addPos] = playerTrfm.position;
        addPos++;
        if (addPos > 9) { addPos = 0; }
    }
}
