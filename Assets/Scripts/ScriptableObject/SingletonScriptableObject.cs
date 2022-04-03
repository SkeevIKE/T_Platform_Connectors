using UnityEditor;
using UnityEngine;

public class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                var assets = Resources.LoadAll<T>("Database");
                if (assets == null)
                {
                    Debug.LogWarning($"No Singleton ScriptableObject found in Resources/Database");
                } 
                else
                {
                    bool isInstanceFinde = false;
                    for (int i = 0; i < assets.Length; i++)
                    {
                        if (assets[i] is T t)
                        {
                            if (!isInstanceFinde)
                            {
                                _instance = t;
                                isInstanceFinde = true;
                            }
                            else
                            {
                                Debug.LogWarning($" Duplication of a singleton {typeof(T)}, must be in a single instance");                                
                                break;
                            }
                        }
                    }
                }


                if (_instance == null)
                {
                    Debug.LogWarning($"Singleton ScriptableObject in Resources/Database - {typeof(T)} not found");
                }
            }
            return _instance;
        }
    }
}
