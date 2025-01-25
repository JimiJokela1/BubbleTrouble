using System;
using System.Linq;
using UnityEngine;

public class CleaningTest : MonoBehaviour
{
    public RenderTexture renderTexture;
    public Material cleanMat;
    MeshRenderer meshRenderer;

    Texture2D texture;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        texture = new Texture2D(renderTexture.width, renderTexture.height);
        Color[] pixels = Enumerable.Repeat(Color.black, renderTexture.width * renderTexture.height).ToArray();
        texture.SetPixels(pixels);
        texture.Apply();
    }

    public void Clean(Vector3 shotPos, Vector3 hitPoint, int radius)
    {
        RenderTexture.active = renderTexture;
        Ray ray = new Ray(shotPos, hitPoint - shotPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.Log(hit.textureCoord);
            int x = (int)Mathf.Lerp(0, renderTexture.width, hit.textureCoord.x);
            int y = (int)Mathf.Lerp(0, renderTexture.height, hit.textureCoord.y);
            // texture.SetPixel(x, y, new Color(1, 0, 0));
            DrawCircle(texture, new Color(1, 1, 1), x, y, radius);
        }

        texture.Apply();
        cleanMat.SetTexture("_RenderTexture", texture);
        RenderTexture.active = null;
    }

    public static void DrawCircle( Texture2D tex, Color color, int x, int y, int radius = 10)
    {
        float rSquared = radius * radius;

        for (int u = x - radius; u < x + radius + 1; u++)
        for (int v = y - radius; v < y + radius + 1; v++)
            if ((x - u) * (x - u) + (y - v) * (y - v) < rSquared)
                tex.SetPixel(u, v, color);
    }

    public void Dirtify(Vector3 shotPos, Vector3 hitPoint, int radius)
    {
        RenderTexture.active = renderTexture;
        Ray ray = new Ray(shotPos, hitPoint - shotPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.Log(hit.textureCoord);
            int x = (int)Mathf.Lerp(0, renderTexture.width, hit.textureCoord.x);
            int y = (int)Mathf.Lerp(0, renderTexture.height, hit.textureCoord.y);
            // texture.SetPixel(x, y, new Color(1, 0, 0));
            DrawCircle(texture, new Color(0, 0, 0), x, y, radius);
        }

        texture.Apply();
        cleanMat.SetTexture("_RenderTexture", texture);
        RenderTexture.active = null;
    }
}