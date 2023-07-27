using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace HexTecGames.Basics.Editor.Util
{
    public class PackageGenerator : EditorWindow
    {
        static string author;
        static string displayName;
        static string path;

        static bool documentationDirectory;
        static bool editorDirectory = true;
        static bool testDirectory;
        static int samples;

        private string authorNoSpace;
        private string displayNameNoSpace;
        private string infoText;
        private bool overwrite;

        [MenuItem("Tools/Package Generator")]
        public static void ShowWindow()
        {
            GetWindow(typeof(PackageGenerator));
        }

        private void OnGUI()
        {
            if (string.IsNullOrEmpty(author))
            {
                author = Application.companyName;
            }
            if (string.IsNullOrEmpty(displayName))
            {
                displayName = Application.productName;
            }
            if (string.IsNullOrEmpty(path))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            GUILayout.Label("Generate Package", EditorStyles.boldLabel);
            author = EditorGUILayout.TextField("Author", author);
            displayName = EditorGUILayout.TextField("Package Name", displayName);
            path = EditorGUILayout.TextField("Path", path);

            GUILayout.Label("Include Folders", EditorStyles.boldLabel);
            editorDirectory = EditorGUILayout.Toggle("Editor", editorDirectory);
            testDirectory = EditorGUILayout.Toggle("Tests", testDirectory);
            documentationDirectory = EditorGUILayout.Toggle("Documentation", documentationDirectory);
            samples = EditorGUILayout.IntField("Samples: ", samples);

            if (GUILayout.Button(overwrite ? "Overwrite" : "Generate"))
            {
                infoText = GenerateProjectFiles();
            }
            if (string.IsNullOrEmpty(infoText) == false)
            {
                EditorGUILayout.HelpBox(infoText, MessageType.Info);
            }          
        }

        private string GenerateProjectFiles()
        {
            authorNoSpace = author.Replace(" ", string.Empty);
            displayNameNoSpace = displayName.Replace(" ", string.Empty);

            if (Directory.Exists(path) == false)
            {
                return "Directory doesn't exist: " + path;
            }
            if (overwrite == false && Directory.Exists(Path.Combine(path, displayNameNoSpace)))
            {
                overwrite = true;
                return "Package with name '" + displayNameNoSpace + "' already exists, click again to override.";
            }
            else overwrite = false;
            Directory.CreateDirectory(Path.Combine(path, displayNameNoSpace));

            File.CreateText(Path.Combine(path, displayNameNoSpace, "README.md")).Close();
            File.CreateText(Path.Combine(path, displayNameNoSpace, "LICENSE.md")).Close();
            File.CreateText(Path.Combine(path, displayNameNoSpace, "CHANGELOG.md")).Close();

            GeneratePackageJSON();

            Directory.CreateDirectory(Path.Combine(path, displayNameNoSpace, "Runtime"));
            GenerateASMDEF(Path.Combine("Runtime"), "", false);

            if (editorDirectory)
            {
                Directory.CreateDirectory(Path.Combine(path, displayNameNoSpace, "Editor"));
                GenerateASMDEF(Path.Combine("Editor"), ".Editor" , true);
            }

            if (testDirectory)
            {
                Directory.CreateDirectory(Path.Combine(path, displayNameNoSpace, "Tests"));
                Directory.CreateDirectory(Path.Combine(path, displayNameNoSpace, "Tests", "Runtime"));
                GenerateASMDEF(Path.Combine("Tests", "Runtime"), ".Tests.Runtime", true);
                if (editorDirectory)
                {
                    Directory.CreateDirectory(Path.Combine(path, displayNameNoSpace, "Tests", "Editor"));
                    GenerateASMDEF(Path.Combine("Tests", "Editor"), ".Tests.Editor", true);
                }
            }
            if (documentationDirectory)
            {
                Directory.CreateDirectory(Path.Combine(path, displayNameNoSpace, "Documentation"));
            }
            if (samples > 0)
            {
                Directory.CreateDirectory(Path.Combine(path, displayNameNoSpace, "Samples"));
                for (int i = 1; i <= samples; i++)
                {
                    Directory.CreateDirectory(Path.Combine(path, displayNameNoSpace, "Samples", "Example " + i));
                }
            }           
            return "Success";
        }
        private void GeneratePackageJSON()
        {
            using (StreamWriter sw = File.CreateText(Path.Combine(path, displayNameNoSpace, "package.json")))
            {
                sw.WriteLine("{");
                sw.WriteLine("  \"name\": \"com.{0}.{1}\",", authorNoSpace.ToLower(), displayNameNoSpace.ToLower());
                sw.WriteLine("  \"version\": \"1.0.0\",");
                sw.WriteLine("  \"displayName\": \"{0}\",", displayName);
                sw.WriteLine("  \"description\": \"My Package\",");
                sw.WriteLine("  \"unity\": \"2019.1\",");
                sw.WriteLine("  \"dependencies\": {},");
                sw.WriteLine("  \"author\": {");
                sw.WriteLine("    \"name\": \"{0}\"", author);
                sw.WriteLine("  },");
                if (samples > 0)
                {
                    sw.WriteLine("  \"hideInEditor\": \"false\",");
                }
                else sw.WriteLine("  \"hideInEditor\": \"false\"");
                if (samples > 0)
                {
                    sw.WriteLine("  \"samples\": [");

                    for (int i = 1; i <= samples; i++)
                    {
                        sw.WriteLine("    {");
                        sw.WriteLine("      \"displayName\": \"Example {0}\",", i);
                        sw.WriteLine("      \"description\": \"Example {0} description\",", i);
                        sw.WriteLine("      \"path\": \"Samples/Example {0}\"", i);
                        if (i < samples)
                        {
                            sw.WriteLine("    },");
                        }
                        else sw.WriteLine("    }");
                    }
                    sw.WriteLine("  ]");
                }
                sw.WriteLine("}");
            }
        }
        private void GenerateASMDEF(string dir, string suffix, bool editor)
        {
            using (StreamWriter sw = File.CreateText(Path.Combine(path, displayNameNoSpace, dir, authorNoSpace + "." + displayNameNoSpace + suffix + ".asmdef")))
            {
                sw.WriteLine("{");
                sw.WriteLine("    \"name\": \"{0}.{1}{2}\",", authorNoSpace, displayNameNoSpace, suffix);
                sw.WriteLine("    \"references\": [");
                sw.WriteLine("    ],");
                sw.WriteLine("    \"includePlatforms\": [");
                if (editor)
                {
                    sw.WriteLine("        \"Editor\"");
                }
                sw.WriteLine("    ],");
                sw.WriteLine("    \"excludePlatforms\": [],");
                sw.WriteLine("    \"allowUnsafeCode\": false,");
                sw.WriteLine("    \"overrideReferences\": false,");
                sw.WriteLine("    \"precompiledReferences\": [],");
                sw.WriteLine("    \"autoReferenced\": true,");
                sw.WriteLine("    \"defineConstraints\": [],");
                sw.WriteLine("    \"versionDefines\": [],");
                sw.WriteLine("    \"noEngineReferences\": false");
                sw.WriteLine("}");
            }
        }
    }
}