using UnityEngine;

// Layers storage class, for interactive visualization of objects
[CreateAssetMenu(fileName = "New Layers_Data", menuName = "Database/Layers Data", order = 51)]
public class Layers_Data : ScriptableObject
{    
    [SerializeField]
    private LayerMask _draggableObjectsMask;
    public LayerMask DraggableObjectsMask => _draggableObjectsMask;
}
