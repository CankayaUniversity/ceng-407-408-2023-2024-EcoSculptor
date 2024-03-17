using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private HexGrid hexGrid;

    protected Hex _selectedHex;
    
    public LayerMask selectionMask;

    protected Hex SelectedHex
    {
        get => _selectedHex;
        set => _selectedHex = value;
    }

    protected virtual void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    public virtual void GetHexAndOutline(Vector3 mousePosition)
    {
        GameObject result;
        if (FindTarget(mousePosition, out result))
        {
            if(_selectedHex)
                _selectedHex.Outline.enabled = false;
            
            _selectedHex = result.GetComponent<Hex>();
            _selectedHex.Outline.enabled = true;

            if (Input.GetKeyDown(KeyCode.R))
            {
                var rotation = new Vector3(0, 60, 0);
                _selectedHex.transform.Rotate(rotation);
            }
        }
    }

    private bool FindTarget(Vector3 mousePosition, out GameObject result)
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out hit, 100, selectionMask))
        {
            result = hit.collider.gameObject;
            return true; 
        }

        result = null;
        return false;
    }
}