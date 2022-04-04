using UnityEngine;

public class DragAndDrop_logic
{
    private Input_Handler _inputHandler;
    private Camera _mainCamera;    
    private bool _isDraged;
    private Vector2 _controlPosition; 
    private IDraggable _rememberDraggableObject;
    private IDraggable _holdDraggableObject;
    private IDraggable _currentHighlightObject;
    private Plane _plane;    
    private ControlSelectType _holdControlSelectType;
    private ControlSelectType _selectControlSelectType;

    // Service initialization
    public void Initialization(Input_Handler inputHandler)
    {
        // subscribing to events in Input_Handler
        _inputHandler = inputHandler;
        _inputHandler.ControlInteractionEvent += ControlInteraction;      
        _inputHandler.ControlPositonEvent += ControlPositionUpdate;

        _mainCamera = Camera.main; 
        _plane = new Plane(Vector3.up, Vector3.zero);
    }

    // Service destruction
    private void Destruction()
    {
        // unsubscribing to events in Input_Handler
        _inputHandler.ControlInteractionEvent -= ControlInteraction;       
        _inputHandler.ControlPositonEvent -= ControlPositionUpdate;
    }

    // Update control position
    private void ControlPositionUpdate(Vector3 position)
    {        
        _controlPosition = position;
        CheckObjectUnderControl();

        if (_holdDraggableObject != null)
        {
            if (_holdControlSelectType == ControlSelectType.Connector)
            {
                if (_currentHighlightObject is Platform || _currentHighlightObject is Connector)
                {
                    _holdDraggableObject.Dragging(LogicHelper.GetPointHitPosition(_mainCamera, _controlPosition));
                }

                if (_currentHighlightObject == null)
                {                   
                    _holdDraggableObject.Dragging(LogicHelper.GetPlanePosition(_plane, _mainCamera, _controlPosition));
                }
            }
            else
            {
                _holdDraggableObject.Dragging(LogicHelper.GetPlanePosition(_plane, _mainCamera, _controlPosition));
            }
        }
    }

    // Control interaction tracking
    //Switch between pressed and released controls, for drag tracking
    private void ControlInteraction(bool isControlHold)
    {
        if (isControlHold)
        {
            HoldObject();
        }
        else
        {
            SelectObject();
        }
    }

    // Object highlighting logic under control
    private void CheckObjectUnderControl()
    {
        IDraggable newDraggableHighlightObject = LogicHelper.CheckDraggableObject(_mainCamera, _controlPosition);

        if (newDraggableHighlightObject != null)
        {
            if (_currentHighlightObject != null && _currentHighlightObject != newDraggableHighlightObject)
            {
                _currentHighlightObject.HighlightMode(isUnder: false);
            }

            _currentHighlightObject = newDraggableHighlightObject;

            if (_currentHighlightObject != _holdDraggableObject || _currentHighlightObject is Platform)
            {
                _currentHighlightObject.HighlightMode(isUnder: true);
            }
        }
        else
        {
            if (_currentHighlightObject != null)
            {
                _currentHighlightObject.HighlightMode(isUnder: false);
                _currentHighlightObject = null;
            }
        }    
    }

    // Start (hold) selection object logic
    private void HoldObject()
    {
        _holdDraggableObject = LogicHelper.CheckDraggableObject(_mainCamera, _controlPosition);
        _holdControlSelectType = LogicHelper.ChekControlSelectType(_holdDraggableObject);

        if (_holdControlSelectType == ControlSelectType.Platform || _holdControlSelectType == ControlSelectType.Empty)
        {
            if (_rememberDraggableObject != null)
            {
                _rememberDraggableObject.SelectControl();
                _rememberDraggableObject = null;
            }
        }
        else if (_holdControlSelectType == ControlSelectType.Connector)
        {           
            if (_rememberDraggableObject != null)
            {
                if (_rememberDraggableObject != _holdDraggableObject)
                {
                    _rememberDraggableObject.SelectControl();
                    _holdDraggableObject.SelectControl();
                }
            }
            else
            {
                _holdDraggableObject.SelectControl();
            }
        }
        _holdDraggableObject?.HoldControl(LogicHelper.GetPlanePosition(_plane, _mainCamera, _controlPosition));        
    }

    // Object selection/unselection logic
    private void SelectObject()
    {
        switch (_holdControlSelectType)
        {
            case ControlSelectType.Empty:
                {
                    if (_rememberDraggableObject != null)
                    {
                        _rememberDraggableObject.SelectControl();
                        _rememberDraggableObject = null;
                    }                    
                }
                break;

            case ControlSelectType.Connector:
                {
                    IDraggable selectDraggableObject = LogicHelper.CheckDraggableObject(_mainCamera, _controlPosition);
                    _selectControlSelectType = LogicHelper.ChekControlSelectType(selectDraggableObject);
                    SetSelectConnector(selectDraggableObject);
                }
                break;

            case ControlSelectType.Platform:
                {
                    if (_rememberDraggableObject != null)
                    {
                        _rememberDraggableObject.SelectControl();
                        _rememberDraggableObject = null;
                    }
                    _holdDraggableObject.SelectControl();
                    _holdDraggableObject = null;
                }
                break;            
        }       
    }

    // Connector selection/unselection logic
    private void SetSelectConnector(IDraggable selectDraggableObject)
    {
        switch (_selectControlSelectType)
        {
            case ControlSelectType.Empty:
                {
                    if (_rememberDraggableObject != null)
                    {                        
                        _rememberDraggableObject = null;
                    }
                    _holdDraggableObject.SelectControl();                   
                }
                break;

            case ControlSelectType.Connector:
                {
                    if (_rememberDraggableObject != null)
                    {                        
                        // deselect if the same connector
                        if (_rememberDraggableObject == selectDraggableObject)
                        {
                            selectDraggableObject.SelectControl();
                            _rememberDraggableObject = null;
                        }
                        else  // create the connection if the connector is different
                        {
                           if (selectDraggableObject == _holdDraggableObject)
                           {
                                SwitchCreateOrDestroyConnection((Connector)_rememberDraggableObject, (Connector)selectDraggableObject);                                
                           }
                           else
                           {
                                SwitchCreateOrDestroyConnection((Connector)selectDraggableObject, (Connector)_holdDraggableObject);                                
                           }

                            _holdDraggableObject.SelectControl();                            
                            _rememberDraggableObject = null;
                        }
                    }
                    else
                    {
                        // create connection or while dragging or select an connector
                        if (selectDraggableObject != _holdDraggableObject)
                        {
                            SwitchCreateOrDestroyConnection((Connector)selectDraggableObject, (Connector)_holdDraggableObject);
                            _holdDraggableObject.SelectControl();
                        }
                        else 
                        {
                            _rememberDraggableObject = selectDraggableObject;                           
                        }                        
                    }                               
                }
                break;

            case ControlSelectType.Platform:
                {
                    if (_rememberDraggableObject != null)
                    {                        
                        _rememberDraggableObject = null;
                    }
                    _holdDraggableObject.SelectControl();                   
                }
                break;
        }
        ((Connector)_holdDraggableObject).ResetDraggLineConnector();
    }

    // Finding existing connections
    private Line_Connector CheckExistingConnections(Connector firstDraggableObject, Connector secondDraggableObject)
    {
        Line_Connector findLineConnector = null;
        foreach (var Connection in firstDraggableObject.CurrentConnectionsList)
        {
            if (Connection.ConnectorsTransform[0] == firstDraggableObject.transform &&
                Connection.ConnectorsTransform[1] == secondDraggableObject.transform ||
                Connection.ConnectorsTransform[1] == firstDraggableObject.transform &&
                Connection.ConnectorsTransform[0] == secondDraggableObject.transform)
            {
                findLineConnector = Connection;
                break;
            }
        }

        return findLineConnector;
    }

    // Switch create or destroy connection
    private void SwitchCreateOrDestroyConnection(Connector firstConnectorObject, Connector secondConnectorObject)
    {
        Line_Connector checkLineConnector = CheckExistingConnections(firstConnectorObject, secondConnectorObject);
        if (checkLineConnector == null)
        {
            CreatConnection(firstConnectorObject, secondConnectorObject);
        }
        else
        {
            firstConnectorObject.CurrentConnectionsList.Remove(checkLineConnector);
            secondConnectorObject.CurrentConnectionsList.Remove(checkLineConnector);
            checkLineConnector.RemoveLineConnector();
        }
    }

    // Creating a connection between connectors
    private void CreatConnection(Connector firstConnectorObject, Connector secondConnectorObject)
    {
        var connectorsTransfom = new Transform[2] { firstConnectorObject.transform, secondConnectorObject.transform };
        var newLineConnector = Data_Link.Instance.SceneBuilder.SpawnLineConnector(connectorsTransfom);

        firstConnectorObject.CurrentConnectionsList.Add(newLineConnector);
        secondConnectorObject.CurrentConnectionsList.Add(newLineConnector);
    }
}
