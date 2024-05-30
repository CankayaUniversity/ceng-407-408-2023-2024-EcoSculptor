using UnityEngine;

public class HexCoordinates : MonoBehaviour
{
    public static float xOffset = 2f;
    public static float yOffset = 1f;
    public static float zOffset = 1.73f;
    
    public Vector3Int GetHexCoords() => offsetCoordinates;
    
    [Header("Offset Coordinates")] [SerializeField]
    private Vector3Int offsetCoordinates;

    private void Awake()
    {
        offsetCoordinates = ConvertPositionToOffset(transform.position);
    }

    private Vector3Int ConvertPositionToOffset(Vector3 position)
    {
        var x = Mathf.CeilToInt(position.x / xOffset);
        var y = Mathf.RoundToInt(position.y / yOffset);
        var z = Mathf.RoundToInt(position.z / zOffset);

        return new Vector3Int(x, y, z);
    }

}
