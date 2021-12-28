using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class AStarPathing
{
    private List<List<Node>> grid;
    private float minHeight;
    private float maxHeight;

    public AStarPathing(Tilemap map, float heightWeight, float optimalHeight, float minHeight=0, float maxHeight=1)
    {
        this.minHeight = minHeight;
        this.maxHeight = maxHeight;
        CreateGrid(map, optimalHeight, heightWeight);
    }

    private void CreateGrid(Tilemap map, float optimalHeight, float heightWeight)
    {
        grid = new List<List<Node>>();
        for(int x = 0; x < map.size.x; x++)
        {
            grid.Add(new List<Node>());
            for(int y = 0; y < map.size.y; y++)
            {
                BattleMapGround t = (BattleMapGround)map.GetTile(new Vector3Int(x, y, 0));
                Node n = new Node(t, minHeight, maxHeight, Mathf.Abs(t.height - optimalHeight) * heightWeight);
                grid[x].Add(n);
            }
        }
    }

    public Stack<BattleMapGround> FindPath(BattleMapGround start, BattleMapGround end)
    {
        Node startPoint = new Node(start, minHeight, maxHeight);
        Node endPoint = new Node(end, minHeight, maxHeight);

        Stack<BattleMapGround> path = new Stack<BattleMapGround>();
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();
        List<Node> adjacencies;

        Node current = startPoint;

        openList.Add(current);

        while(openList.Count != 0 && !closedList.Exists(x => x.position == endPoint.position))
        {
            current = openList[0];
            openList.Remove(current);
            closedList.Add(current);
            adjacencies = GetAdjacentNodes(current);

            foreach (Node n in adjacencies)
            {
                if(n.traversable)
                {
                    if (!openList.Contains(n))
                    {
                        float oldF = n.GetF();
                        n.distanceToTarget = Mathf.Abs(n.position.pos.x - endPoint.position.pos.x) + Mathf.Abs(n.position.pos.y - endPoint.position.pos.y);
                        float newF = n.distanceToTarget + n.weight + current.cost;
                        if (!closedList.Contains(n) || oldF > newF)
                        {
                            if (closedList.Contains(n)) closedList.Remove(n);
                            n.parent = current;
                            n.cost = n.weight + n.parent.cost;
                            openList.Add(n);
                        }
                        openList = openList.OrderBy(node => node.GetF()).ToList<Node>();
                    }
                }
            }
        }
        // If end was not closed (found) return null
        if(!closedList.Exists(x => x.position == endPoint.position))
        {
            return null;
        }

        // Path exists
        Node temp = closedList[closedList.IndexOf(current)];
        do
        {
            path.Push(temp.position);
            temp = temp.parent;
        } while (temp != startPoint && temp != null);
        path.Push(startPoint.position);
        return path;
    }

    private List<Node> GetAdjacentNodes(Node n)
    {
        List<Node> temp = new List<Node>();

        int row = n.position.pos.y;
        int col = n.position.pos.x;

        if(row + 1 < grid[0].Count)
        {
            temp.Add(grid[col][row + 1]);
        }
        if(row - 1 >= 0)
        {
            temp.Add(grid[col][row - 1]);
        }
        if(col + 1 < grid.Count)
        {
            temp.Add(grid[col + 1][row]);
        }
        if(col - 1 >= 0)
        {
            temp.Add(grid[col - 1][row]);
        }

        return temp;
    }

    private class Node
    {
        public Node parent;
        public BattleMapGround position;
        public int distanceToTarget;
        public bool traversable;
        public float weight;
        public float cost;

        public float GetF()
        {
            return (distanceToTarget != -1 && cost != -1) ? distanceToTarget + cost : -1;
        }

        public Node(BattleMapGround pos, float minHeight, float maxHeight, float weight = 1)
        {
            traversable = pos.height >= minHeight && pos.height <= maxHeight;
            this.weight = weight;
            position = pos;
        }
    }
}
