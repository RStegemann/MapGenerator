using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using System;
using SFB;

/// <summary>
/// Class to export Tilemap to PNG
/// From: https://github.com/leocub58/Tilemap-to-PNG-Unity
/// Adapted to no longer be an Editor function and to merge multiple Tilemap Layers into one png
/// Also removed some unnecessary stuff
/// </summary>
public class MapExport : MonoBehaviour
{
    public Tilemap ground;
    public Tilemap objects;
    public Tilemap outline;

    public SpriteRenderer renderHelper;
    public ComputeShader blendShader;
    public RenderTexture rt;

    public BattleMapSettings settings;

    private int gridSize;

    private Texture2D resultMap;

    private void Pack()
    {
        gridSize = settings.pixelsPerField;
        int xSize = gridSize * ground.size.x;
        int ySize = gridSize * ground.size.y;

        Texture2D resultingImage = new Texture2D(xSize, ySize);
        Texture2D groundImage = new Texture2D(xSize, ySize);
        Texture2D objectImage = new Texture2D(xSize, ySize);
        Texture2D gridImage = new Texture2D(xSize, ySize);

        Color[] invisible = new Color[resultingImage.width * resultingImage.height];
        for (int i = 0; i < invisible.Length; i++)
        {
            invisible[i] = new Color(0f, 0f, 0f, 0f);
        }

        resultingImage.SetPixels(0, 0, resultingImage.width, resultingImage.height, invisible);
        groundImage.SetPixels(0, 0, resultingImage.width, resultingImage.height, invisible);
        objectImage.SetPixels(0, 0, resultingImage.width, resultingImage.height, invisible);
        gridImage.SetPixels(0, 0, resultingImage.width, resultingImage.height, invisible);
        groundImage.Apply();
        objectImage.Apply();
        gridImage.Apply();

        Color[] invisTexColors = new Color[gridSize * gridSize];
        for(int i = 0; i < invisTexColors.Length; i++)
        {
            invisTexColors[i] = new Color(0f, 0f, 0f, 0f);
        }
        Texture2D invisTexture = new Texture2D(gridSize, gridSize);
        invisTexture.SetPixels(0, 0, gridSize, gridSize, invisTexColors);
        invisTexture.Apply();

        // Assign pixels based on sprite locations
        for (int x = 0; x <= ground.size.x; x++)
        {
            for (int y = 0; y <= ground.size.y; y++)
            {
                Sprite currentGround = ground.GetSprite(new Vector3Int(x, y, 0));
                Sprite currentObject = objects.GetSprite(new Vector3Int(x, y, 0));
                Sprite currentOutline = outline.GetSprite(new Vector3Int(x, y, 0));

                if(currentGround != null)
                {
                    groundImage.SetPixels(x * gridSize,
                        y * gridSize,
                        currentGround.texture.width,
                        currentGround.texture.height,
                        GetCurrentSprite(currentGround).GetPixels());
                }

                if(currentObject != null && currentObject.texture != null)
                {
                    objectImage.SetPixels(x * gridSize,
                        y * gridSize,
                        currentObject.texture.width,
                        currentObject.texture.height,
                        GetCurrentSprite(currentObject).GetPixels());
                }

                if(currentOutline != null)
                {
                    gridImage.SetPixels(x * gridSize,
                        y * gridSize,
                        currentOutline.texture.width,
                        currentOutline.texture.height,
                        GetCurrentSprite(currentOutline).GetPixels());
                }

            }
        }

        groundImage.Apply();
        objectImage.Apply();
        gridImage.Apply();

        rt = new RenderTexture(groundImage.width, groundImage.height, 24);
        rt.enableRandomWrite = true;
        rt.Create();

        blendShader.SetTexture(0, "Result", rt);
        blendShader.SetTexture(0, "Grid", gridImage);
        blendShader.SetTexture(0, "Object", objectImage);
        blendShader.SetTexture(0, "Ground", groundImage);
        blendShader.Dispatch(0, rt.width / 8, rt.height / 8, 1);

        RenderTexture.active = rt;
        resultingImage.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        resultingImage.Apply();

        resultMap = resultingImage; // Store image texture
    }

    /// <summary>
    /// Get pixel block from sprite
    /// </summary>
    /// <param name="sprite"></param>
    /// <returns></returns>
    private Texture2D GetCurrentSprite(Sprite sprite)
    {
        var pixels = sprite.texture.GetPixels(0, 0, sprite.texture.width, sprite.texture.height);

        Texture2D texture = new Texture2D(sprite.texture.width,
                                         sprite.texture.height);

        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }

    /// <summary>
    /// Export to png
    /// </summary>
    /// <param name="name"> Filename </param>
    public void ExportAsPng(string name)
    {
        var dirPath = StandaloneFileBrowser.SaveFilePanel("Save Map", Application.dataPath + "/Exported Tilemaps/", "map", "png");
        if (dirPath != "")
        {
            Pack();
            byte[] bytes = resultMap.EncodeToPNG();
            File.WriteAllBytes(dirPath, bytes);
            resultMap = null;
        }
    }

}