using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class BuildSetting : IPostprocessBuildWithReport
{
    public int callbackOrder => 0;
    private const string m_strBurstDebugInformation_DoNotShip = "BurstDebugInformation_DoNotShip";

    public void OnPostprocessBuild(BuildReport report)
    {
        BuildSummary summary = report.summary;
        BuildTarget  platform = summary.platform;

        if (summary.options.HasFlag(BuildOptions.Development))
        {
            return;
        }

        if (platform != BuildTarget.StandaloneWindows
            && platform != BuildTarget.StandaloneWindows64
            && platform != BuildTarget.Android
            && platform != BuildTarget.iOS)
        {
            return;
        }

        string outputPath = summary.outputPath;
        string outputDirectoryPath = Path.GetDirectoryName(outputPath);

        string outputFileName = Path.GetFileNameWithoutExtension(outputPath);

        string backUpThisFolderPath = $"{outputDirectoryPath}/{outputFileName}_{m_strBurstDebugInformation_DoNotShip}";

        if (!Directory.Exists(backUpThisFolderPath))
        {
            return;
        }

        Directory.Delete(backUpThisFolderPath, true);
    }
}
