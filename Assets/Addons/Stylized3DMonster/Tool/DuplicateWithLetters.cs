using UnityEngine;
using UnityEditor;
using System.IO;

public class DuplicateWithLetters : Editor
{
    // Assign shortcut (&d = Alt+D) to a single menu item to avoid conflicts.
    // The function automatically detects whether an asset or a hierarchy object is selected.
    [MenuItem("Edit/Duplicate With Letters &d")]
    static void DuplicateGlobal()
    {
        ExecuteDuplicate();
    }

    // Keep the context menu in Assets for convenience, without an extra shortcut assignment.
    [MenuItem("Assets/Duplicate With Letters")]
    static void DuplicateFromAssets()
    {
        ExecuteDuplicate();
    }

    static void ExecuteDuplicate()
    {
        Object selected = Selection.activeObject;
        if (selected == null) return;

        string path = AssetDatabase.GetAssetPath(selected);

        // Check if the selection is an Asset in the Project window
        if (!string.IsNullOrEmpty(path) && File.Exists(path))
        {
            DuplicateProjectAsset(path);
        }
        // Check if the selection is a GameObject in the Hierarchy
        else if (selected is GameObject go)
        {
            DuplicateHierarchyObject(go);
        }
    }

    static void DuplicateProjectAsset(string path)
    {
        string directory = Path.GetDirectoryName(path);
        string fileName = Path.GetFileNameWithoutExtension(path);
        string extension = Path.GetExtension(path);

        char lastChar = fileName[fileName.Length - 1];
        string baseName = fileName;
        char nextLetter = 'a';

        // Check if the file name already ends with a lowercase letter
        if (lastChar >= 'a' && lastChar < 'z')
        {
            baseName = fileName.Substring(0, fileName.Length - 1);
            nextLetter = (char)(lastChar + 1);
        }

        string newPath = directory + "/" + baseName + nextLetter + extension;

        // Iterate to find the next available letter if the file already exists
        while (File.Exists(newPath))
        {
            if (nextLetter >= 'z') break;
            nextLetter = (char)(nextLetter + 1);
            newPath = directory + "/" + baseName + nextLetter + extension;
        }

        AssetDatabase.CopyAsset(path, newPath);
        AssetDatabase.Refresh();

        Object newAsset = AssetDatabase.LoadAssetAtPath<Object>(newPath);
        Selection.activeObject = newAsset;
    }

    static void DuplicateHierarchyObject(GameObject selected)
    {
        string name = selected.name;
        char lastChar = name[name.Length - 1];
        string baseName = name;
        char nextLetter = 'a';

        // Check if the object name already ends with a lowercase letter
        if (lastChar >= 'a' && lastChar < 'z')
        {
            baseName = name.Substring(0, name.Length - 1);
            nextLetter = (char)(lastChar + 1);
        }

        // Instantiate the duplicate as a child of the same parent
        GameObject newObj = Instantiate(selected, selected.transform.parent);
        newObj.name = baseName + nextLetter;
        Selection.activeGameObject = newObj;

        // Register for Undo system
        Undo.RegisterCreatedObjectUndo(newObj, "Duplicate With Letters");
    }
}