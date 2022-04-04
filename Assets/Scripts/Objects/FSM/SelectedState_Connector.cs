using UnityEngine;

public class SelectedState_Connector : State
{
    public SelectedState_Connector(Connector connector) : base(connector)
    {
    }

    public override void Enter()
    {
        LogicHelper.ChangeRendererMaterials(_connector.Renderer, _connector.MaterialsData.SelectedMaterial);       
        ChangeOtherConnectorsToReadyConnect(isReady: true);
    }

    public override void Exit()
    {        
        ChangeOtherConnectorsToReadyConnect(isReady: false);
    }

    public override void SelectConector()
    {
        _connector.ConnectorStateMachine.ChangeState(_connector.NormalStateConnector);
    }

    public override void HighlightMode(bool isUnder)
    {
        if (isUnder)
        {
            LogicHelper.ChangeRendererMaterials(_connector.Renderer, _connector.MaterialsData.HighlightMaterial);
        }
        else
        {
            LogicHelper.ChangeRendererMaterials(_connector.Renderer, _connector.MaterialsData.SelectedMaterial);
        }
    }

    // Changing the color of connectors for a connection
    private void ChangeOtherConnectorsToReadyConnect(bool isReady)
    {       
        foreach (var connector in Data_Link.Instance.ConnectorPlatformsInScene)
        {
            if (connector != _connector)
            {
                if (isReady)
                {
                    connector.ConnectorStateMachine.ChangeState(connector.ReadyForConnectStateConnector);
                }
                else
                {
                    connector.ConnectorStateMachine.ChangeState(connector.NormalStateConnector);
                }
            }
        }
    }
}
