using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line_Connector : MonoBehaviour
{
    private LineRenderer _lineRenderer;   
    private const int _lineConstPathCount = 2;
    private Platform[] _platforms;

    public Transform[] ConnectorsTransform { get; private set; } 

    private void Awake()
    {        
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = _lineConstPathCount;
        _lineRenderer.enabled = false;
    }

    // Set up the first connector
    public void SetFirstConnect(Transform connectorTransform)
    {
        ConnectorsTransform = new Transform[_lineConstPathCount];
        ConnectorsTransform[0] = connectorTransform;

        _platforms = new Platform[_lineConstPathCount];
        _platforms[0] = connectorTransform.parent.GetComponentInChildren<Platform>();
        _platforms[0].PlatformDraggedEvent += UpdateConnectorsLinePosition;
    }

    // Set up the second connector
    public void SetSecondConnect(Transform connectorTransform)
    {
        ConnectorsTransform[1] = connectorTransform;
        _platforms[1] = connectorTransform.parent.GetComponentInChildren<Platform>();
        _platforms[1].PlatformDraggedEvent += UpdateConnectorsLinePosition;
        UpdateConnectorsLinePosition();
        _lineRenderer.enabled = true;
    }

    // Remove Line Connector and all his event bindings
    public void RemoveLineConnector()
    {
        if (_platforms != null)
        {
            _platforms[0].PlatformDraggedEvent -= UpdateConnectorsLinePosition;
            _platforms[1].PlatformDraggedEvent -= UpdateConnectorsLinePosition;
        }

        _lineRenderer.enabled = false;
        Destroy(gameObject);
    }

    // Assigning a position while dragging platforms
    public void UpdateConnectorsLinePosition()
    {
        Vector3[] connectorsPosition = new Vector3[ConnectorsTransform.Length];
        for (int i = 0; i < ConnectorsTransform.Length; i++)
        {
            connectorsPosition[i] = ConnectorsTransform[i].transform.position;
        }
        _lineRenderer.SetPositions(connectorsPosition);       
    }

    // Assigning a position while dragging line connector
    public void UpdateConnectorsLinePosition(Vector3 startPosition, Vector3 secondPosition)
    {
        _lineRenderer.SetPosition(0, startPosition);
        _lineRenderer.SetPosition(1, secondPosition);
        _lineRenderer.enabled = true;
    }


}
