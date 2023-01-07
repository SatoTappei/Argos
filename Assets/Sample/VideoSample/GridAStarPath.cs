using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAStarPath
{
    public readonly Vector3[] LookPoints;
    public readonly GridAStarLine[] TurnBoundaries;
    public readonly int FinishLineIndex;
    public readonly int SlowDownIndex;

    public GridAStarPath(Vector3[] waypoints, Vector3 startPos, float turnDst, float stoppingDst)
    {
        LookPoints = waypoints;
        TurnBoundaries = new GridAStarLine[LookPoints.Length];
        FinishLineIndex = TurnBoundaries.Length - 1;

        Vector2 previousPoint = V3ToV2(startPos);

        for(int i = 0; i < LookPoints.Length; i++)
        {
            Vector2 currentPoint = V3ToV2(LookPoints[i]);
            Vector2 dirToCurrentPoint = (currentPoint - previousPoint).normalized;
            Vector2 turnBoundaryPoint = currentPoint - dirToCurrentPoint * turnDst;
            TurnBoundaries[i] = new GridAStarLine(turnBoundaryPoint, previousPoint);
            previousPoint = turnBoundaryPoint;
        }

        float dstFromEndPoint = 0;
        for(int i = LookPoints.Length - 1; i > 0; i--)
        {
            dstFromEndPoint += Vector3.Distance(LookPoints[i], LookPoints[i - 1]);
            if (dstFromEndPoint > stoppingDst)
            {
                SlowDownIndex = i;
                break;
            }
        }
    }

    Vector2 V3ToV2(Vector3 v3)
    {
        return new Vector2(v3.x, v3.z);
    }
}
