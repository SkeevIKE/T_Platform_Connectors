using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [Tooltip("Spawning platforms count")]
    [SerializeField] [Range(1, 20)]
    private int _platformsCount = 10;

    [Space]
    [Tooltip("Platform spawn radius")]
    [SerializeField] [Range(0.1f, 20f)]
    private float _radius = 10;

    // Scene initialization
    private void Awake()
    {      
        var sceneBuilder = new Scene_Builder();       
        sceneBuilder.BuildScene();
        sceneBuilder.BuildSceneObjects(_platformsCount, _radius);
        Data_Link.Instance.SceneBuilder = sceneBuilder;
    }
}
