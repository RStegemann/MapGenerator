using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointPanelControl : MonoBehaviour
{
    public Text col;
    public Text row;
    public Button delete;
    public PathSettings pathSettings;
    private BattleMapGround ground;

    public void SetPoint(BattleMapGround g)
    {
        ground = g;
        col.text = g.pos.x.ToString();
        row.text = g.pos.y.ToString();
    }

    public void Delete()
    {
        pathSettings.RemovePoint(ground);
        Destroy(this.gameObject);
    }
}
