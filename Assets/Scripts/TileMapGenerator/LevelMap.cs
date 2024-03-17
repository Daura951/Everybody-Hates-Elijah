using System.Collections.Generic;

class LevelMap
{
    public int tileSize { get; set; }
    public int mapHeight { get; set; }
    public int mapWidth { get; set; }

    public LevelLayer[] layers { get; set; }
}