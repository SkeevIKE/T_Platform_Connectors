using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour, IDraggable
{
    private Line_Connector _dragginglineConnector;

    public List<Line_Connector> CurrentConnectionsList { get; set; } = new List<Line_Connector>();

    public Renderer Renderer { get; private set; }
    public Materials_Data MaterialsData { get; private set; }    

    public State_Machine ConnectorStateMachine { get; set; }
    public NormalState_Connector NormalStateConnector { get; set; }    
    public ReadyForConnectState_Connector ReadyForConnectStateConnector { get; set; }
    public SelectedState_Connector SelectedStateConnector { get; set; }
       
    public void Awake()
    {
        Renderer = GetComponent<Renderer>();
        MaterialsData = Data_Link.Instance.MaterialsData;
        LogicHelper.ChangeRendererMaterials(Renderer, MaterialsData.DefaultMaterial);

        NormalStateConnector = new NormalState_Connector(connector: this);        
        ReadyForConnectStateConnector = new ReadyForConnectState_Connector(connector: this);
        SelectedStateConnector = new SelectedState_Connector(connector: this);

        ConnectorStateMachine = new State_Machine(startingState: NormalStateConnector);        
    }

    public void HoldControl(Vector3 position)
    {
        _dragginglineConnector = Data_Link.Instance.SceneBuilder.SpawnLineConnector();
    }

    public void SelectControl()
    {
        ConnectorStateMachine.CurrentState.SelectConector();
    }

    public void Dragging(Vector3 position)
    {
        if (_dragginglineConnector != null)
        {            
            _dragginglineConnector.UpdateConnectorsLinePosition(transform.position, position);
        }
    }

    // Removing the line connector when dragging is used
    public void ResetDraggLineConnector()
    {
        if (_dragginglineConnector != null)
        {
            _dragginglineConnector.RemoveLineConnector();
        }
    }

    public void HighlightMode(bool isUnder)
    {
        ConnectorStateMachine.CurrentState.HighlightMode(isUnder);
    }
}
