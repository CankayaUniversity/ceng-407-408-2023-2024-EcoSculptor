
using System.Collections.Generic;
using UnityEngine;

public static class Direction
{
    public static List<Vector3Int> DirectionsOffsetOdd = new List<Vector3Int>()
    {
        new Vector3Int(-1, 0, 1), //N1
        new Vector3Int(0, 0, 1), //N2
        new Vector3Int(1, 0, 0), //E
        new Vector3Int(0, 0, -1), //S2
        new Vector3Int(-1, 0, -1), //S1
        new Vector3Int(-1, 0, 0) //W
    };

    public static readonly List<Vector3Int> DirectionsOffsetEven = new List<Vector3Int>()
    {
        new Vector3Int(-1, 0, 2), //N1
        new Vector3Int(-2, 0, 0), //N2
        new Vector3Int(-1, 0, -2), //E
        new Vector3Int(1, 0, -2), //S2
        new Vector3Int(2, 0, 0), //S1
        new Vector3Int(1, 0, 2), //W
    };

    public static List<Vector3Int> GetDirectionList(int z) 
        => z % 2 == 0 ? DirectionsOffsetEven : DirectionsOffsetOdd;
}
