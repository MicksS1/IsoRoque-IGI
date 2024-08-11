using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePositiion : MonoBehaviour
{
    public Camera cam;
    public LayerMask Layers;
    public Ray ray;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycasthit, float.MaxValue, Layers))
            transform.position = raycasthit.point;
    }
}
