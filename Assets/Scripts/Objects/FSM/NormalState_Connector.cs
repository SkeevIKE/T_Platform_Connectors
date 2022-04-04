using UnityEngine;

public class NormalState_Connector : State
{
    public NormalState_Connector(Connector connector) : base(connector)
    {
    }

    public override void Enter()
    {
        LogicHelper.ChangeRendererMaterials(_connector.Renderer, _connector.MaterialsData.DefaultMaterial);
    }

    public override void Exit()
    {
        
    }

    public override void SelectConector()
    {
        _connector.ConnectorStateMachine.ChangeState(_connector.SelectedStateConnector);
    }

    public override void HighlightMode(bool isUnder)
    {
        if (isUnder)
        {
            LogicHelper.ChangeRendererMaterials(_connector.Renderer, _connector.MaterialsData.HighlightMaterial);
        }
        else
        {
            LogicHelper.ChangeRendererMaterials(_connector.Renderer, _connector.MaterialsData.DefaultMaterial);
        }
    }
}
