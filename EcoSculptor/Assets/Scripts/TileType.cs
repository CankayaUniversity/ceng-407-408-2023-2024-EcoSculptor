
using DefaultNamespace;
using UnityEngine;

public class TileType : MonoBehaviour
{
    [SerializeField] private TileTypeEnum type;

    public TileTypeEnum Type => type;
}