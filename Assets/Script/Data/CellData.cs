using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Cell", menuName = "Scriptable Objects/New Cell")]
public class CellData : ScriptableObject
{
    [SerializeField] private BaseCellEffect cellEffect;
    [SerializeField] private Tile backTile;
    [SerializeField] private Tile frontTile;
    [SerializeField] private int cost;
    [SerializeField] private int frequency;
    [SerializeField] private GroupCost[] groupsCost;


    public BaseCellEffect CellEffect => cellEffect;
    public Tile BackTile => backTile;
    public Tile FrontTile => frontTile;
    public int Cost => cost;
    public GroupCost[] GroupsCost => groupsCost;

    public int Frequency => frequency;

    [Serializable]
    public struct GroupCost
    {
        [SerializeField] private BugGroup group;
        [SerializeField] private int cost;

        public BugGroup Group => group;
        public int Cost => cost;
    }

}