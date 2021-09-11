using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ForceDepthNormals : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // enable the generation of the depth normals texture
        var camera = GetComponent<Camera>();
        camera.depthTextureMode = camera.depthTextureMode | DepthTextureMode.DepthNormals;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
