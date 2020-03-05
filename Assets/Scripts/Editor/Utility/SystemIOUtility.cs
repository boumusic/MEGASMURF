using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public static class SystemIOUtility
{
    public static string SaveTextureToFile(Texture2D texture, string fileName)
    {
        byte[] bytes = texture.EncodeToPNG();
        FileStream file = File.Open(fileName, FileMode.Create);
        BinaryWriter binary = new BinaryWriter(file);
        binary.Write(bytes);
        file.Close();
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        return fileName;
    }
}
