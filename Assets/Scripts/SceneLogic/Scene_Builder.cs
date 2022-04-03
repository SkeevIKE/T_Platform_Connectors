using UnityEngine;

public class Scene_Builder 
{
    private const string _connectableGroup_Name = "Connectable Group";
    private const string _lineConnectorGroup_Name = "Line Connector Group";
    private const string _inputHandlerGameObject_Name = "Input Handler";
    private Transform _connectableGroup;
    private Transform _lineConnectorGroup;

    // Creating and configuring management services
    public void BuildScene()
    {
        var inputHandler = new GameObject(_inputHandlerGameObject_Name).AddComponent<Input_Handler>();

        var dragAndDropLogic = new DragAndDrop_logic();
        dragAndDropLogic.Initialization(inputHandler);
        Data_Link.Instance.DragAndDropLogic = dragAndDropLogic;
    }

    // Creating and configuring interactive scene objects
    public void BuildSceneObjects(int count, float radius)
    {
        _connectableGroup = new GameObject(_connectableGroup_Name).transform;
        var connectors = new Connector[count];

        int newTransfomsCount = 0;
        while (newTransfomsCount < count)
        {
            var floatX = Random.Range(-radius, radius);
            var floatZ = Random.Range(-radius, radius);
            var spawnPosition = new Vector3(floatX, 0, floatZ);

            var newObject = Object.Instantiate(Data_Link.Instance.SpawnGameObject, spawnPosition, Quaternion.identity, _connectableGroup.transform);
            connectors[newTransfomsCount] = newObject.GetComponentInChildren<Connector>();

            newTransfomsCount++;
        }

        Data_Link.Instance.ConnectorPlatformsInScene = connectors;        
    }

    // Create and configuring a connection object
    public Line_Connector SpawnLineConnector(Transform[] connectorsTransform)
    {
        if (_lineConnectorGroup == null)
        {
            _lineConnectorGroup = new GameObject(_lineConnectorGroup_Name).transform;
            _lineConnectorGroup.SetParent(_connectableGroup);
        }

        var lineConnector = Object.Instantiate(Data_Link.Instance.LineConnectorSpawnGameObject, _lineConnectorGroup.transform);

        Vector3[] connectorsPosition = new Vector3[2];
        for (int i = 0; i < connectorsPosition.Length; i++)
        {
            connectorsPosition[i] = connectorsTransform[i].transform.position;
        }

        //lineConnector.SetPathConnection(connectorsPosition);
        lineConnector.SetFirstConnect(connectorsTransform[0]);
        lineConnector.SetSecondConnect(connectorsTransform[1]);        

        return lineConnector;
    }

    // Create and configuring a connection object for dragg line connection
    public Line_Connector SpawnLineConnector()
    {
        if (_lineConnectorGroup == null)
        {
            _lineConnectorGroup = new GameObject(_lineConnectorGroup_Name).transform;
            _lineConnectorGroup.SetParent(_connectableGroup);
        }

        var lineConnector = Object.Instantiate(Data_Link.Instance.LineConnectorSpawnGameObject, _lineConnectorGroup.transform);

        return lineConnector;
    }
}
