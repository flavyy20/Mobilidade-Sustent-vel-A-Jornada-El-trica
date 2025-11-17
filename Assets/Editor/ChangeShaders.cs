using UnityEngine;
using UnityEditor;

public class ChangeShaders : MonoBehaviour
{
    [MenuItem("Tools/Fix Pink Materials to Standard")]
    public static void FixMaterials()
    {
        // Busca todos os materiais no projeto
        string[] materialGUIDs = AssetDatabase.FindAssets("t:Material");

        int fixedCount = 0;

        foreach (string guid in materialGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (mat != null)
            {
                // Se o material está usando shader quebrado (rosa) ou obsoleto
                if (mat.shader == null || mat.shader.name == "Hidden/InternalErrorShader")
                {
                    mat.shader = Shader.Find("Standard"); // troca pro Standard
                    EditorUtility.SetDirty(mat);
                    fixedCount++;
                }
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Corrigidos {fixedCount} materiais para Standard.");
    }
}
