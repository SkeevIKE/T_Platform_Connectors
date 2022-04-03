using UnityEngine;

public class ReadyForConnectState_Connector : State
{
    public ReadyForConnectState_Connector(Connector connector) : base(connector)
    {
    }

    public override void Enter()
    {
        LogicHelper.ChangeRendererMaterials(_connector.Renderer, _connector.MaterialsData.ActiveMaterial); 
    }

    public override void Exit()
    {
        
    }

    public override void SelectConector()
    {
       // _connector.ConnectorStateMachine.ChangeState(_connector.NormalStateConnector);
    }

    public override void HighlightMode(bool isUnder)
    {
        if (isUnder)
        {
            LogicHelper.ChangeRendererMaterials(_connector.Renderer, _connector.MaterialsData.HighlightMaterial);
        }
        else
        {
            LogicHelper.ChangeRendererMaterials(_connector.Renderer, _connector.MaterialsData.ActiveMaterial);
        }
    }
}
