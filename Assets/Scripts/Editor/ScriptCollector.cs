using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class ScriptCollector : EditorWindow
{
    private List<string> allFolders = new List<string>();
    private List<bool> folderSelection = new List<bool>();
    private Vector2 scrollPosition;

    [MenuItem("Tools/Append All Scripts to File")]
    public static void ShowWindow()
    {
        ScriptCollector window = GetWindow<ScriptCollector>("Script Collector");
        window.LoadFolders();
        window.Show();
    }

    private void LoadFolders()
    {
        allFolders = Directory.GetDirectories(Application.dataPath, "*", SearchOption.AllDirectories).ToList();
        folderSelection = new List<bool>(new bool[allFolders.Count]);
    }

    private void OnGUI()
    {
        GUILayout.Label("Select Folders to Exclude", EditorStyles.boldLabel);
        GUILayout.Space(5);

        if (allFolders.Count == 0)
        {
            GUILayout.Label("No folders found.");
            return;
        }

        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200));

        for (int i = 0; i < allFolders.Count; i++)
        {
            folderSelection[i] = GUILayout.Toggle(folderSelection[i], allFolders[i].Replace(Application.dataPath, "Assets"));
        }

        GUILayout.EndScrollView();

        GUILayout.Space(10);
        
        GUILayout.Label("Filename", EditorStyles.boldLabel);
        
        GUILayout.Space(5);

        var filename = GUILayout.TextField("AllScripts", 100);
        
        GUILayout.Space(10);

        if (GUILayout.Button("Generate Script File"))
        {
            GenerateScriptFile(filename);
        }
    }

    private void GenerateScriptFile(string filename)
    {
        string outputPath = Path.Combine(Application.dataPath, filename + ".txt");
        HashSet<string> excludedFolders = new HashSet<string>(allFolders.Where((_, index) => folderSelection[index]));

        string[] scriptFiles = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories)
            .Where(file => !excludedFolders.Any(file.StartsWith))
            .ToArray();

        using (StreamWriter writer = new StreamWriter(outputPath, false))
        {
            foreach (string file in scriptFiles)
            {
                writer.WriteLine($"// ===== {Path.GetFileName(file)} =====");
                writer.WriteLine(File.ReadAllText(file));
                writer.WriteLine("\n\n");
            }
        }

        EditorUtility.RevealInFinder(outputPath);
        Debug.Log($"All scripts have been appended to: {outputPath}");
    }
}
