using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Platform : MonoBehaviour, IDraggable
{
    private const int _positionUpdateDelayCount = 20;
    private Renderer _renderer;
    private Materials_Data _materialsData;
    private Vector3 _dragStartPosition;
    private Vector3 _startPosition;
    private Vector3 _oldRigidbodyPosition;

    private bool _isDragging;
    private bool _isPositionChange;
    
    private Rigidbody _rigidbody;

    public event Action PlatformDraggedEvent;

    public void Awake()
    {
        _rigidbody = GetComponentInParent<Rigidbody>();
        _oldRigidbodyPosition = transform.position;
        _renderer = GetComponent<Renderer>();
        _materialsData = Data_Link.Instance.MaterialsData;
        LogicHelper.ChangeRendererMaterials(_renderer, _materialsData.DefaultMaterial);        
    }

    public void HoldControl(Vector3 position)
    {
        _dragStartPosition =  position;
        _startPosition = _rigidbody.position;      
        _isDragging = true;
    }

    public void SelectControl()
    {
        LogicHelper.ChangeRendererMaterials(_renderer, _materialsData.DefaultMaterial);
        _isDragging = false;        
    }

    public void Dragging(Vector3 position)
    {
        _rigidbody.position = _startPosition + (position - _dragStartPosition);       
        PlatformDraggedEvent?.Invoke();
    }

    private void Update()
    {
        // Checking and triggering a delay to update line connector
        if (PlatformDraggedEvent != null && !_isPositionChange)
        {           
            if (_oldRigidbodyPosition != _rigidbody.position)
            {
                _isPositionChange = true;                
                StartCoroutine(UpdatePosition());               
            }            
        }    
    }

    // Line connector update cycle
    IEnumerator UpdatePosition()
    {
        int waitCount = 0;
        while (waitCount < _positionUpdateDelayCount)
        {
            waitCount++;            
            PlatformDraggedEvent?.Invoke();
            yield return null;
        }
        
        _oldRigidbodyPosition = _rigidbody.position;
        _isPositionChange = false;
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
