using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class CustomPreference
{
    [SettingsProvider]
    private static SettingsProvider CP_Hierachy()
    {
        return new SettingsProvider("CustomPreference/Hierachy", SettingsScope.User)
        {

        };
    }

}
