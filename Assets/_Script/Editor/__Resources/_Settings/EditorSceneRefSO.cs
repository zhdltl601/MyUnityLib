using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "EditorSceneRef", menuName = "Scriptable Objects/EditorSceneRef")]
public class EditorSceneRefSO : ScriptableObject
{
    [SerializeField] private SceneAsset[] sceneRef;
    public IReadOnlyList<SceneAsset> GetSceneRef => sceneRef;
}
