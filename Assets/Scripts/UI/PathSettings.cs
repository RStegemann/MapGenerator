using SFB;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PathSettings : MonoBehaviour
{
    private readonly string FOLDER = "Assets/Textures";

    public InputField minHeight;
    public InputField maxHeight;
    public InputField pathWidth;
    public Button addTexture;
    public Button removePath;
    public Slider optimalHeight;
    public Slider heightWeight;
    public BattleMapSettings mapSettings;
    public PathMenu pathMenu;
    public Toggle active;
    public PathData data;

    public TileMapGeneration mapGenerator;

    public GameObject pointPanelPrefab;
    public GameObject pointPanelParent;

    private AStarPathing pathGenerator;
    private List<BattleMapGround> lastPath = new List<BattleMapGround>();

    public struct PathData
    {
        public Sprite texture;
        public List<BattleMapGround> points;
        public int targetWidth;
        public float minHeight;
        public float maxHeight;
        public float optimalHeight;
        public float heightWeight;
    }

    public void Start()
    {
        data.points = new List<BattleMapGround>();
        minHeight.contentType = InputField.ContentType.DecimalNumber;
        maxHeight.contentType = InputField.ContentType.DecimalNumber;
        pathWidth.contentType = InputField.ContentType.IntegerNumber;
        optimalHeight.value = 0.5f;
        data.optimalHeight = 0.5f;
        heightWeight.value = 1f;
        data.heightWeight = 1f;
        data.minHeight = 0f;
        data.maxHeight = 1f;
        data.targetWidth = 1;
        minHeight.text = data.minHeight.ToString();
        maxHeight.text = data.maxHeight.ToString();
        pathWidth.text = data.targetWidth.ToString();
    }

    public void AddTexture()
    {
        string[] path = StandaloneFileBrowser.OpenFilePanel("Pick Texture", FOLDER, new ExtensionFilter[] { new ExtensionFilter("Image Files", "jpg", "png")}, false);
        if (path.Length == 0) return;

        byte[] fileData = File.ReadAllBytes(path[0]);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(fileData);
        tex = TextureScaling.Resize(tex, mapSettings);
        data.texture = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), tex.width);

        addTexture.image.sprite = data.texture;
        addTexture.GetComponentInChildren<Text>().text = "";
        DrawPath();
    }

    public void SetActive(bool b)
    {
        pathMenu.ActivePath = this;
    }

    public void SetMinHeight(string s)
    {
        float old = data.minHeight;
        try
        {
            data.minHeight = float.Parse(s);
        }
        catch
        {
            data.minHeight = old;
        }
        optimalHeight.minValue = data.minHeight;
        DrawPath();
    }

    public void SetMaxHeight(string s)
    {
        float old = data.maxHeight;
        try
        {
            data.maxHeight = float.Parse(s);
        }
        catch
        {
            data.maxHeight = old;
        }
        optimalHeight.maxValue = data.maxHeight;
        DrawPath();
    }

    public void SetOptimalHeight(float f)
    {
        data.optimalHeight = f;
        DrawPath();
    }

    public void SetHeightWeight(float f)
    {
        data.heightWeight = f;
        DrawPath();
    }

    public void SetPathWidth(string s)
    {
        int old = data.targetWidth;
        try
        {
            data.targetWidth = int.Parse(s);
        }
        catch
        {
            data.targetWidth = old;
        }
        DrawPath();
    }

    public void AddPoint(BattleMapGround g)
    {
        GameObject instance = Instantiate(pointPanelPrefab, pointPanelParent.transform);

        data.points.Add(g);
        instance.GetComponent<PointPanelControl>().SetPoint(g);
        instance.GetComponent<PointPanelControl>().pathSettings = this;
        DrawPath();
    }

    public void RemovePoint(BattleMapGround g)
    {
        data.points.Remove(g);
        DrawPath();
    }

    public void RemovePath()
    {
        mapGenerator.DeletePath(lastPath);
        pathMenu.ActivePath = null;
        Destroy(this.gameObject);
    }

    private void DrawPath()
    {
        Tilemap map = mapGenerator.manager.battleMap;
        if (pathGenerator != null && lastPath != null) mapGenerator.DeletePath(lastPath);
        if (data.points.Count >= 2)
        {
            pathGenerator = new AStarPathing(
                map,
                data.heightWeight,
                data.optimalHeight,
                data.minHeight,
                data.maxHeight);

            lastPath = new List<BattleMapGround>();
            for (int i = 0; i < data.points.Count - 1; i++)
            {
                Stack<BattleMapGround> pathTiles = pathGenerator.FindPath(data.points[i], data.points[i + 1]);
                while (pathTiles != null && pathTiles.Count > 0)
                {
                    BattleMapGround t = pathTiles.Pop();
                    t.sprite = data.texture;
                    lastPath.Add(t);
                    if(data.targetWidth > 1)
                    {
                        int w = data.targetWidth - 1;
                        Vector3Int dir = data.points[i + 1].pos - data.points[i].pos;
                        if(dir.x != 0)
                        {
                            int topTiles = w / 2;
                            int bottomTiles = w - topTiles;
                            for (int offset = 1; offset <= topTiles; offset++)
                            {
                                if ((t.pos.y + offset) < map.size.y)
                                {
                                    BattleMapGround neighbour = (BattleMapGround)map.GetTile(new Vector3Int(t.pos.x, t.pos.y + offset, 0));
                                    if(neighbour.height >= data.minHeight && neighbour.height <= data.maxHeight)
                                    {
                                        neighbour.sprite = data.texture;
                                        lastPath.Add(neighbour);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            for (int offset = 1; offset <= bottomTiles; offset++)
                            {
                                if ((t.pos.y - offset) >= 0)
                                {
                                    BattleMapGround neighbour = (BattleMapGround)map.GetTile(new Vector3Int(t.pos.x, t.pos.y - offset, 0));
                                    if (neighbour.height >= data.minHeight && neighbour.height <= data.maxHeight)
                                    {
                                        neighbour.sprite = data.texture;
                                        lastPath.Add(neighbour);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        if(dir.y != 0)
                        {
                            int leftTiles = w / 2;
                            int rightTiles = w - leftTiles;

                            for (int offset = 1; offset <= leftTiles; offset++)
                            {
                                if ((t.pos.x - offset) >= 0)
                                {
                                    BattleMapGround neighbour = (BattleMapGround)map.GetTile(new Vector3Int(t.pos.x - offset, t.pos.y, 0));
                                    if (neighbour.height >= data.minHeight && neighbour.height <= data.maxHeight)
                                    {
                                        neighbour.sprite = data.texture;
                                        lastPath.Add(neighbour);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            for (int offset = 1; offset <= rightTiles; offset++)
                            {
                                if ((t.pos.x + offset) < map.size.x) 
                                {
                                    BattleMapGround neighbour = (BattleMapGround)map.GetTile(new Vector3Int(t.pos.x + offset, t.pos.y, 0));
                                    if(neighbour.height >= data.minHeight && neighbour.height <= data.maxHeight)
                                    {
                                        neighbour.sprite = data.texture;
                                        lastPath.Add(neighbour);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            map.RefreshAllTiles();
            mapGenerator.manager.objectSettings.UpdateSpawners();
        }
    }
}
