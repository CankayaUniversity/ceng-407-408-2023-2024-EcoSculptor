using UnityEngine;

public class ToggleGameObject : MonoBehaviour
{
    public void ActivitySetter()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }
}