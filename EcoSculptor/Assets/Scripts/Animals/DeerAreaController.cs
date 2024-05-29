using UnityEngine;

public class DeerAreaController : MonoBehaviour
{
    [SerializeField] private PreyAnimal parent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hunter")||other.CompareTag("AlphaHunter"))
        {
            if (parent != null)
            {
                parent.OnHunterEnter();
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Hunter")||other.CompareTag("AlphaHunter"))
        {
            if (parent != null)
            {
                parent.OnHunterStay();
            }
        }
    }
}