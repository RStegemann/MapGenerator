using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureScaling
{
    public static Texture2D Resize(Texture2D texture2D, BattleMapSettings settings)
    {
        RenderTexture rt = new RenderTexture(settings.pixelsPerField, settings.pixelsPerField, 24);
        RenderTexture.active = rt;
        Graphics.Blit(texture2D, rt);
        Texture2D result = new Texture2D(settings.pixelsPerField, settings.pixelsPerField);
        result.ReadPixels(new Rect(0, 0, settings.pixelsPerField, settings.pixelsPerField), 0, 0);
        result.Apply();
        return result;
    }

    public static Texture2D Resize(Texture2D texture2D, BattleMapSettings settings, float widthMult, float heightMult)
    {
        int w = (int)(settings.pixelsPerField * widthMult);
        int h = (int)(settings.pixelsPerField * heightMult);
        RenderTexture rt = new RenderTexture(w, h, 24);
        RenderTexture.active = rt;
        Graphics.Blit(texture2D, rt);
        Texture2D result = new Texture2D(w, h);
        result.ReadPixels(new Rect(0, 0, w, h), 0, 0);
        result.Apply();
        return result;
    }
}
