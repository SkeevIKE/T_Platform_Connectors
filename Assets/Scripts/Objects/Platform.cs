using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Platform : MonoBehaviour, IDraggable
{
    private Renderer _renderer;
    private Materials_Data _materialsData;
    private Vector3 _dragStartPosition;
    private Vector3 _startPosition;
    private bool _isDragging;

    public event Action PlatformDraggedEvent;

    public void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _materialsData = Data_Link.Instance.MaterialsData;
        LogicHelper.ChangeRendererMaterials(_renderer, _materialsData.DefaultMaterial);        
    }

    public void HoldControl(Vector3 position)
    {
        _dragStartPosition =  position;
        _startPosition = transform.parent.position;
        _isDragging = true;
    }

    public void SelectControl()
    {
        LogicHelper.ChangeRendererMaterials(_renderer, _materialsData.DefaultMaterial);
        _isDragging = false;        
    }

    public void Dragging(Vector3 position)
    {
        transform.parent.position = _startPosition + (position - _dragStartPosition);
        PlatformDraggedEvent?.Invoke();
    }

    public void HighlightMode(bool isUnder)
    {
        if (_isDragging)
        {
            LogicHelper.ChangeRendererMaterials(_renderer, _materialsData.SelectedMaterial);            
        }
        else
        {
            if (isUnder)
            {
                LogicHelper.ChangeRendererMaterials(_renderer, _materialsData.HighlightMaterial);
            }
            else
            {
                LogicHelper.ChangeRendererMaterials(_renderer, _materialsData.DefaultMaterial);
            }
        }
    }
}
