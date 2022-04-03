using UnityEngine;

public class SelectedState_Connector : State
{
    public SelectedState_Connector(Connector connector) : base(connector)
    {
    }

    public override void Enter()
    {
        LogicHelper.ChangeRendererMaterials(_connector.Renderer, _connector.MaterialsData.SelectedMaterial);
        Data_Link.Instance.CurentConnector = _connector;
        ChangeOtherConnectorsToReadyConnect(isReady: true);
    }

    public override void Exit()
    {
        Data_Link.Instance.CurentConnector = null;
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
        var otherConnectors = Data_Link.Instance.ConnectorPlatformsInScene;
        foreach (var connector in otherConnectors)
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
