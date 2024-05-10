
using UnityEngine;

public class TileClassifier : MonoBehaviour
{
    [SerializeField] private Sprite grass;
    [SerializeField] private Sprite water;
    [SerializeField] private Sprite river;
    [SerializeField] private Sprite dirt;
    [SerializeField] private Sprite sand;
    [SerializeField] private Sprite stone;

    public Sprite GetSprite(string tileTag)
    {
        return tileTag switch
        {
            "Grass" => grass,
            "Water" => water,
            "River" => river,
            "Dirt" => dirt,
            "Sand" => sand,
            "Stone" => stone,
            _ => null
        };
    }
}

