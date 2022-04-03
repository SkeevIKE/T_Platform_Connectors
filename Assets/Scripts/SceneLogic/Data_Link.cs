using System.Collections.Generic;
using UnityEngine;

// Data_Link storage class
[CreateAssetMenu(fileName = "New Data_Link", menuName = "Database/Data Link", order = 51)]
public class Data_Link : SingletonScriptableObject<Data_Link>
{  
    [SerializeField]
    private GameObject _spawnGameObject;
    public GameObject SpawnGameObject => _spawnGameObject;

    [SerializeField]
    private Line_Connector _lineConnectorSpawnGameObject;
    public Line_Connector LineConnectorSpawnGameObject => _lineConnectorSpawnGameObject;

    [SerializeField]
    private Materials_Data _materialsData;
    public Materials_Data MaterialsData => _materialsData;

    [SerializeField]
    private Layers_Data _layersData;
    public Layers_Data LayersData => _layersData;

    public Connector CurentConnector { get; set; }
    public Connector[] ConnectorPlatformsInScene { get; set; }

    public Scene_Builder SceneBuilder { get; set; }
    public DragAndDrop_logic DragAndDropLogic { get; set; }
}
