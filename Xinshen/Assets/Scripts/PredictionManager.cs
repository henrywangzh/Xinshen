using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictionManager : MonoBehaviour
{
    [SerializeField] Transform predictionIndicator;
    static Transform playerTrfm;
    static PredictionManager self;
    // Start is called before the first frame update
    void Awake()
    {
        self = GetComponent<PredictionManager>();
        playerTrfm = transform;
        playerPositions = new Vector3[10];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (calculateTimer > 0) { calculateTimer--; }
        else
        {
            calculateTimer = 4;
            CalculatePredictedPos();
            predictionIndicator.position = GetPredictedPos(1, false);
        }
    }

    [SerializeField] Vector3[] playerPositions;
    [SerializeField] Vector3 predictedPosition;
    [SerializeField] Vector3 positionDelta;
    static int addPos, calculateTimer;

    public static Vector3 GetPredictedPos(float seconds, bool verticalTargeting = true) //larger preditions are less accurate (obviously), ~1.5s is the higher end of accurate predictions
    {
        int latestPos1 = addPos - 2;
        int latestPos2 = addPos - 3;
        if (latestPos1 < 0) { latestPos1 += 10; }
        if (latestPos2 < 0) { latestPos2 += 10; }

        self.positionDelta = (playerTrfm.position - self.playerPositions[latestPos2]) * 2 * seconds;
        self.positionDelta += (playerTrfm.position - self.playerPositions[latestPos1]) * 6 * seconds;
        self.predictedPosition = playerTrfm.position + self.positionDelta;

        if (verticalTargeting) { return self.predictedPosition; }
        self.predictedPosition.y = 0;
        return self.predictedPosition;
    }
    private void CalculatePredictedPos()
    {
        playerPositions[addPos] = playerTrfm.position;
        addPos++;
        if (addPos > 9) { addPos = 0; }
    }
}
