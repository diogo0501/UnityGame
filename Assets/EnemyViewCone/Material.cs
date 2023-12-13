using UnityEngine;

public class OpacityController : MonoBehaviour
{
    public Material maskMaterial; // Reference to the material with Unlit/Transparent shader

    void Start()
    {
        // Check if the material is assigned
        if (maskMaterial == null)
        {
            Debug.LogError("Material not assigned!");
            return;
        }

        // Create a new material instance to ensure changes don't affect other instances
        Material newMaterial = new Material(maskMaterial);

        // Modify the shader to support transparency
        newMaterial.shader = Shader.Find("Unlit/Transparent");

        // Set the alpha value to 0.5 (50% opacity)
        newMaterial.color = new Color(newMaterial.color.r, newMaterial.color.g, newMaterial.color.b, 0.5f);

        // Apply the updated material to the renderer
        GetComponent<MeshRenderer>().material = newMaterial;
    }
}