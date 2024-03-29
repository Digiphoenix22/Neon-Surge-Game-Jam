using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float length;
    public float startPos;
    public Camera cam;
    [SerializeField] private float parallaxEffect;

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float distance = (cam.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if(temp > startPos + length)
        {
            startPos += length;
        }
        else if (temp < startPos - length) 
        { 
            startPos -= length;
        }
    }
}
