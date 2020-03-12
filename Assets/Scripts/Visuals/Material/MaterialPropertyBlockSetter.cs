using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PropertyMaterial
{
    public string name;
    public Color color;
}

public class MaterialPropertyBlockSetter : MonoBehaviour
{
    public Renderer[] rends;
    public PropertyMaterial[] materialProperties;

    public MaterialPropertyBlock block;

    private void Start()
    {
        UpdatePropertyBlock();
    }

    private void OnDrawGizmos()
    {
        UpdatePropertyBlock();
    }

    public void UpdatePropertyBlock()
    {
        if (block == null) block = new MaterialPropertyBlock();
        for (int i = 0; i < materialProperties.Length; i++)
        {
            block.SetColor(materialProperties[i].name, materialProperties[i].color);
        }

        if (rends != null)

            for (int i = 0; i < rends.Length; i++)
            {
                rends[i].SetPropertyBlock(block);
            }
    }
}
