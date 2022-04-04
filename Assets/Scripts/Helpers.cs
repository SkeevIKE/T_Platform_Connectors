using UnityEngine;

// Interface describing the contract of draggable objects
public interface IDraggable
{    
    void HoldControl(Vector3 position);
    void SelectControl();
    void Dragging(Vector3 position);
    void HighlightMode(bool isUnder);    
}

// Control select type
public enum ControlSelectType
{
    Empty,
    Connector,
    Platform
}

public class LogicHelper
{
    // Replacing the material in the render with the specified material
    public static void ChangeRendererMaterials(Renderer changeableRender, Material newMaterial)
    {
        if (newMaterial != null)
        {
            var newMaterials = new Material[changeableRender.materials.Length];
            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = newMaterial;
            }
            changeableRender.materials = newMaterials;
        }
    }

    // Find a position on a plane in world for dragging object
    public static Vector3 GetPlanePosition(Plane plane, Camera camera, Vector2 position)
    {
        Vector3 planePosition = new Vector3();
        Ray ray = camera.ScreenPointToRay(position);
        float entry;
        if (plane.Raycast(ray, out entry))
        {
            planePosition = ray.GetPoint(entry);
        }
        return planePosition;
    }

    // Find a position on a colider
    public static Vector3 GetPointHitPosition(Camera camera, Vector2 position)
    { 
        Vector3 planePosition = new Vector3();
        Ray ray = camera.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Data_Link.Instance.LayersData.DraggableObjectsMask))
        {
            planePosition = hit.point;
        }
        return planePosition;
    }

    // Checking and returning a draggable object
    public static IDraggable CheckDraggableObject(Camera camera, Vector2 position)
    {
        Ray ray = camera.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Data_Link.Instance.LayersData.DraggableObjectsMask))
        {
            if (hit.collider.TryGetComponent(out IDraggable draggableItem))
            {
                return draggableItem;
            }
        }
        return null;
    }

    // Determining the type of control
    public static ControlSelectType ChekControlSelectType(IDraggable draggableObject)
    {
        ControlSelectType controlSelectType = ControlSelectType.Empty;
        if (draggableObject != null)
        {
            if (draggableObject is Connector)
            {
                controlSelectType = ControlSelectType.Connector;
            }
            else
            {
                controlSelectType = ControlSelectType.Platform;
            }
        }
        return controlSelectType;
    }
}

