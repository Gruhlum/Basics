using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using HexTecGames.Basics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace HexTecGames.Basics.Editor
{
    public class GitHelper : EditorWindow
    {
        private static string packageFolder = "C:\\Users\\Patrick\\Documents\\Projects\\Unity\\_Misc\\Packages";

        private static List<string> folderPaths = new List<string>();
        private static List<string> hasChangesPaths = new List<string>();

        [MenuItem("Tools/Git Helper")]
        public static void ShowWindow()
        {
            GetWindow(typeof(GitHelper));
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Path");
            packageFolder = EditorGUILayout.TextField(label: string.Empty, packageFolder);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Total Folders:", folderPaths.Count.ToString());
            EditorGUILayout.LabelField("Has Changes:", hasChangesPaths.Count.ToString());

            if (folderPaths.Count <= 0 || hasChangesPaths.Count <= 0)
            {
                if (GUILayout.Button("Check", GUILayout.Height(30)))
                {
                    GetSubFolders();
                }
            }
            else
            {
                if (GUILayout.Button("Start", GUILayout.Height(30)))
                {
                    string result  = Run();
                    EditorGUILayout.HelpBox(result, MessageType.Info);
                }
            }
            
        }

        private static void GetSubFolders()
        {
            folderPaths = System.IO.Directory.GetDirectories(packageFolder).ToList();
            hasChangesPaths = new List<string>();


            foreach (var path in folderPaths)
            {
                // Run cmd and check with git status if any changes are needed
                // If not, continue
                // Else run git add and open an input prompt where the user can enter a commit msg
                // Increment the version number
                // run git push
                if (HasChanges(path))
                {
                    hasChangesPaths.Add(path);
                }
            }
        }

        public static string Run()
        {
            

            


            if (hasChangesPaths.Count <= 0)
            {
                return $"{folderPaths.Count} files checked in {packageFolder}";
            }

            foreach (var path in hasChangesPaths)
            {
                StartCommit(path);
            }
            return $"{folderPaths.Count} files checked in {packageFolder}";
        }

        private static void StartCommit(string path)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/K \"cd /d {path} && git status\"",
                UseShellExecute = true,
                CreateNoWindow = false,
            };

            var cmdProcess = Process.Start(psi);
            cmdProcess.WaitForExit();
        }
        private static bool HasChanges(string path)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c cd {path} && git status",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var cmdProcess = Process.Start(psi);
            string output = cmdProcess.StandardOutput.ReadToEnd();
            cmdProcess.WaitForExit();
            string lastLine = output.Trim().Split('\n').Last();
            if (string.IsNullOrEmpty(lastLine))
            {
                return false;
            }
            //Debug.Log(path + " - " + lastLine);
            return lastLine != "nothing to commit, working tree clean";
        }
    }
}