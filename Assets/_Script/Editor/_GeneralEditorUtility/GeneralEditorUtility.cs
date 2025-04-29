using UnityEngine;
using UnityEditor;
using System.IO;
using Unity.Collections;

public static partial class GeneralEditorUtility
{
    public static string ConvertGUIDToName(this string guid, bool withExtension = false)
    {
        string path = ConvertGUIDToPath(guid);
        
        return ConvertPathToName(path, withExtension);
    }
    public static string ConvertGUIDToPath(this string guid)
    {
        return AssetDatabase.GUIDToAssetPath(guid);
    }
    public static string ConvertPathToName(this string path, bool withExtension = false)
    {
        return withExtension ?
            Path.GetFileName(path) :
            Path.GetFileNameWithoutExtension(path);
    }
    public static string ConvertInstanceIDtoStr(this int instanceID)
    {
        NativeArray<int> instanceIDs = new NativeArray<int>(1, Allocator.Persistent);
        instanceIDs[0] = instanceID;
        NativeArray<GUID> outResult = new NativeArray<GUID>(1, Allocator.Persistent);
        AssetDatabase.InstanceIDsToGUIDs(instanceIDs, outResult);

        string result = outResult[0].ToString();
        
        instanceIDs.Dispose();
        outResult.Dispose();
        return result;
    }
}
