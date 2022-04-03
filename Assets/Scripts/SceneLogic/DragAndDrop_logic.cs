using UnityEngine;

public class DragAndDrop_logic
{
    private Input_Handler _inputHandler;
    private Camera _mainCamera;    
    private bool _isDraged;
    private Vector2 _controlPosition;    
    private IDraggable _currentDraggableObject;
    private IDraggable _holdDraggableObject;
    private IDraggable _curentHighlightObject;
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
            _holdDraggableObject.Dragging(LogicHelper.GetPlanePosition(_plane, _mainCamera, _controlPosition));            
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
            if (_curentHighlightObject != null && _curentHighlightObject != newDraggableHighlightObject)
            {
                _curentHighlightObject.HighlightMode(isUnder: false);
            }

            _curentHighlightObject = newDraggableHighlightObject;

            if (_curentHighlightObject != _holdDraggableObject || _curentHighlightObject is Platform)
            {
                _curentHighlightObject.HighlightMode(isUnder: true);
            }
        }
        else
        {
            if (_curentHighlightObject != null)
            {
                _curentHighlightObject.HighlightMode(isUnder: false);
            }
        }    
    }

    // Start (hold) selection object logic
    private void HoldObject()
    {
        IDraggable holdDraggableObject = LogicHelper.CheckDraggableObject(_mainCamera, _controlPosition);
        if (holdDraggableObject != null)
        {
            _holdDraggableObject = holdDraggableObject;
        }

        _holdControlSelectType = LogicHelper.ChekControlSelectType(holdDraggableObject);
        if (_holdControlSelectType == ControlSelectType.Platform)
        {
            if (_currentDraggableObject != null)
            {
                _currentDraggableObject.SelectControl();
                _currentDraggableObject = null;
            }
        }
        else if (_holdControlSelectType == ControlSelectType.Connector)
        {
            _holdDraggableObject?.SelectControl();
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
                    if (_currentDraggableObject != null)
                    {
                        _currentDraggableObject.SelectControl();
                        _currentDraggableObject = null;
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
                    _holdDraggableObject.SelectControl();

                    if (_currentDraggableObject != null)
                    {
                        _currentDraggableObject.SelectControl();
                        _currentDraggableObject = null;
                    }
                }
                break;           
        }
        _holdDraggableObject = null;        
    }

    // Connector selection/unselection logic
    private void SetSelectConnector(IDraggable selectDraggableObject)
    {
        switch (_selectControlSelectType)
        {
            case ControlSelectType.Empty:
                {
                    ((Connector)_holdDraggableObject).ResetLineConnector();
                    _holdDraggableObject.SelectControl();
                    if (_currentDraggableObject != null)
                    {
                        _currentDraggableObject.SelectControl();
                        _currentDraggableObject = null;
                    }
                }
                break;

            case ControlSelectType.Connector:
                {
                    if (_currentDraggableObject != null)
                    {                        
                        // deselect if the same connector
                        if (selectDraggableObject == _currentDraggableObject)
                        {                            
                            _currentDraggableObject = null;
                        }
                        else  // configure the connection if the connector is different
                        {
                            Line_Connector checkLineConnector = CheckExistingConnections((Connector)_currentDraggableObject, (Connector)selectDraggableObject);
                            if (checkLineConnector == null)
                            {                                
                                CreatConnection((Connector)_currentDraggableObject, (Connector)selectDraggableObject);
                            }
                            else
                            {
                                ((Connector)_currentDraggableObject).CurrentConnectionsList.Remove(checkLineConnector);
                                ((Connector)selectDraggableObject).CurrentConnectionsList.Remove(checkLineConnector);
                                checkLineConnector.RemoveLineConnector();
                            }
                            _currentDraggableObject.SelectControl();
                            _currentDraggableObject = null;
                            ((Connector)selectDraggableObject).ResetLineConnector();
                        }
                    }
                    else
                    {
                        if (selectDraggableObject != _holdDraggableObject)
                        {
                            Line_Connector checkLineConnector = CheckExistingConnections((Connector)_holdDraggableObject, (Connector)selectDraggableObject);
                            if (checkLineConnector == null)
                            {
                                CreatConnection((Connector)_holdDraggableObject, (Connector)selectDraggableObject);                                
                                _holdDraggableObject.SelectControl();
                            }
                            else
                            {
                                ((Connector)_holdDraggableObject).CurrentConnectionsList.Remove(checkLineConnector);
                                ((Connector)selectDraggableObject).CurrentConnectionsList.Remove(checkLineConnector);
                                checkLineConnector.RemoveLineConnector();
                                _holdDraggableObject.SelectControl();
                            }
                        }

                        if (selectDraggableObject == _holdDraggableObject)
                        {
                            _currentDraggableObject = selectDraggableObject;  
                        }                        
                    }
                    ((Connector)_holdDraggableObject).ResetLineConnector();
                }
                break;

            case ControlSelectType.Platform:
                {
                    ((Connector)_holdDraggableObject).ResetLineConnector();
                    _holdDraggableObject.SelectControl();
                    if (_currentDraggableObject != null)
                    {
                        _currentDraggableObject.SelectControl();                       
                        _currentDraggableObject = null;
                    }
                }
                break;
        }
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

    // Creating a connection between connectors
    private void CreatConnection(Connector firstConnectorObject, Connector secondConnectorObject)
    {
        var connectorsTransfom = new Transform[2] { firstConnectorObject.transform, secondConnectorObject.transform };
        var newLineConnector = Data_Link.Instance.SceneBuilder.SpawnLineConnector(connectorsTransfom);

        firstConnectorObject.CurrentConnectionsList.Add(newLineConnector);
        secondConnectorObject.CurrentConnectionsList.Add(newLineConnector);
    }
}
