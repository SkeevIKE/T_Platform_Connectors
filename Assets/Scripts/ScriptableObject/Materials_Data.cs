using UnityEngine;

// Material storage class, for interactive visualization of objects
[CreateAssetMenu(fileName = "New Materials_Data", menuName = "Database/Materials Data", order = 51)]
public class Materials_Data : ScriptableObject
{
    [SerializeField]
    private Material _defaultMaterial;
    public Material DefaultMaterial => _defaultMaterial;

    [SerializeField]
    private Material _selectedMaterial;
    public Material SelectedMaterial => _selectedMaterial;

    [SerializeField]
    private Material _activeMaterial;
    public Material ActiveMaterial => _activeMaterial;

    [SerializeField]
    private Material _inactiveMaterial;
    public Material InactiveMaterial => _inactiveMaterial;

    [SerializeField]
    private Material _highlightMaterial;
    public Material HighlightMaterial => _highlightMaterial;
}
