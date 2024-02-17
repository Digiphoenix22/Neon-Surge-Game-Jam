using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxEffectMultiplierX;
    public float parallaxEffectMultiplierY;
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
        foreach (Transform background in backgrounds)
        {
            // Parallax effect for x-axis
            float parallaxX = deltaMovement.x * parallaxEffectMultiplierX;
            float backgroundTargetPosX = background.position.x + parallaxX;

            // Parallax effect for y-axis
            float parallaxY = deltaMovement.y * parallaxEffectMultiplierY;
            float backgroundTargetPosY = background.position.y + parallaxY;

            // Set the new position with parallax effect applied
            background.position = new Vector3(backgroundTargetPosX, backgroundTargetPosY, background.position.z);

            // Looping logic for x-axis
            if (cameraTransform.position.x - background.position.x >= textureUnitSizeX)
            {
                float offsetPositionX = (cameraTransform.position.x - background.position.x) % textureUnitSizeX;
                background.position = new Vector3(cameraTransform.position.x + offsetPositionX, background.position.y, background.position.z);
            }
            else if (background.position.x - cameraTransform.position.x >= textureUnitSizeX)
            {
                float offsetPositionX = (background.position.x - cameraTransform.position.x) % textureUnitSizeX;
                background.position = new Vector3(cameraTransform.position.x - offsetPositionX, background.position.y, background.position.z);
            }
        }

        // Update the last camera position
        lastCameraPosition = cameraTransform.position;
    }
}
