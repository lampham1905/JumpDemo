using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColorAI : MonoBehaviour
{
    public Color colorAI;
    public Color blackColorAI;
    public string colorProperty = "_Color";
    public string blackTintProperty = "_Black";
    MaterialPropertyBlock block;
    MeshRenderer meshRenderer;
    // Start is called before the first frame update
    
    IEnumerator  Start() {
         block = new MaterialPropertyBlock();
        meshRenderer = GetComponent<MeshRenderer>();
        while(true){
            yield return new WaitForSeconds(0f);
            block.SetColor(colorProperty, colorAI);
            block.SetColor(blackTintProperty, blackColorAI);
            meshRenderer.SetPropertyBlock(block);
        }
    }

   
}
