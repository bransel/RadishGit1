using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceSwap : MonoBehaviour
{
    public Material[] faces;
   SkinnedMeshRenderer renderer;
    Material currentFace;
    public int FaceIndex; 
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SkinnedMeshRenderer>();

        

    }

    // Update is called once per frame
    void Update()
    {
        switch (FaceIndex)
        {
            case 2:
               
                currentFace = faces[2];
                renderer.material = currentFace;

                break;

            case 1:

                currentFace = faces[1];
                renderer.material = currentFace;
                break;
                
            default:

                currentFace = faces[0];
                renderer.material = currentFace;
                break;

        }


        

    }
}
