using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxEffectMultiplier;
    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;
    public Transform[] backgrounds; // Array holding the three backgrounds

    void Start()
    {
        lastCameraPosition = cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
    }

    void Update()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier, 0f, 0f);
        lastCameraPosition = cameraTransform.position;

        foreach (Transform background in backgrounds)
        {
            if (cameraTransform.position.x - background.position.x >= textureUnitSizeX)
            {
                // Camera has moved past the right edge of the texture
                float offsetPositionX = (cameraTransform.position.x - background.position.x) % textureUnitSizeX;
                background.position = new Vector3(cameraTransform.position.x + offsetPositionX, background.position.y, background.position.z);
            }
            else if (background.position.x - cameraTransform.position.x >= textureUnitSizeX)
            {
                // Camera has moved past the left edge of the texture
                float offsetPositionX = (background.position.x - cameraTransform.position.x) % textureUnitSizeX;
                background.position = new Vector3(cameraTransform.position.x - offsetPositionX, background.position.y, background.position.z);
            }
        }
    }
}
