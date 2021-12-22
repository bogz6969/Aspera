using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;
using SFML.Graphics;
using SFML.System;
using TiledCS;

namespace Aspera.Environment
{
    public class TileLayerProperties
    {
        public bool Folliage { get; set; } = false;
        public Vector2f Offset { get; set; } = new Vector2f();
        public bool Collidable { get; set; } = false;
        public bool Foreground { get; set; } = false;
        public bool Invisible { get; set; } = false;
        public int Id { get; internal set; }
        public bool Ground { get; internal set; }
    }

    public struct Tile
    {
        public static Tile None => new Tile { ID = 0 };

        public float Noise { get; }
        public int ID { get; set; }
        public Tile(int id, float noise)
        {
            this.Noise = noise;
            this.ID = id;
        }
    }
    public class Tileset
    {
        public TiledTileset Set { get; private set; }
        public Texture Texture { get; private set; }
        public int FirstGid { get; private set; }

        public Tileset(TiledMapTileset source)
        {
            FirstGid = source.firstgid;
            Set = new TiledTileset("assets/map/" + source.source);
            Texture = new Texture("assets/world/" + Set.Image.source.Split('/').Last());
        }
    }

    public class WorldTileLayer
    {
        public TileLayerProperties Properties { get; private set; } = new TileLayerProperties();
        public Tile[,] Tiles { get; set; }

        public WorldTileLayer(GameWorld w, TiledLayer layer)
        {
            var noise = new Noise();
            Tiles = new Tile[w.Tilemap.Width, w.Tilemap.Height];
            Properties.Id = layer.id;
            Properties.Offset = new Vector2f(layer.offsetX * GameWorld.Scale, layer.offsetY * GameWorld.Scale);

            foreach (var item in layer.properties)
            {
                switch (item.name.ToLower())
                {
                    case "ground":
                        Properties.Ground = bool.Parse(item.value);
                        break;
                    case "folliage":
                        Properties.Folliage = bool.Parse(item.value);
                        break;
                    case "collidable":
                        Properties.Collidable = bool.Parse(item.value);
                        break;
                    case "foreground":
                        Properties.Foreground = bool.Parse(item.value);
                        break;
                    case "invisible":
                        Properties.Invisible = bool.Parse(item.value);
                        break;
                }
            }

            for (int x = 0; x < w.Tilemap.Width; x++)
            {
                for (int y = 0; y < w.Tilemap.Height; y++)
                {
                    int index = x * w.Tilemap.Width + y;
                    var tile = new Tile(layer.data[index], (float)noise.GetSimplex(y * 32 + 16 + (layer.offsetX / GameWorld.TileSize) * 32, x * 32 + 16 + (layer.offsetY / GameWorld.TileSize) * 32));

                    if (!Properties.Invisible)
                    {
                        Tiles[y, x] = tile;
                    }
                    if (tile.ID > 0 && Properties.Collidable)
                    {
                        var _groundBody = BodyFactory.CreateRectangle(w, GameWorld.TileSize * GameWorld.Scale, GameWorld.TileSize * GameWorld.Scale, 1f);
                        _groundBody.BodyType = BodyType.Static;
                        _groundBody.Position = new Microsoft.Xna.Framework.Vector2(y * GameWorld.TileSize * GameWorld.Scale + (GameWorld.TileSize * GameWorld.Scale) / 2, x * GameWorld.TileSize * GameWorld.Scale + (GameWorld.TileSize * GameWorld.Scale) / 2);
                    }
                }
            }
        }

        public Tile this[int x, int y]
        {
            get
            {
                if (x < 0 || y < 0 || x >= Tiles.GetLength(0) || y >= Tiles.GetLength(1)) return Tile.None;
                return Tiles[x, y];
            }
        }
    }

    public class GameWorld : Genbox.VelcroPhysics.Dynamics.World
    {
        public static int TileSize = 32;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public TiledMap Tilemap { get; set; }
        public Dictionary<string, Tileset> Tilesets;
        public List<WorldTileLayer> Layers { get; set; } = new List<WorldTileLayer>();

        public static float Scale { get; set; } = 2.0f;

        public GameWorld()
            : base(new Microsoft.Xna.Framework.Vector2(0, 0))
        {
        }

        public void Load()
        {
            Tilesets = new Dictionary<string, Tileset>();
            Tilemap = new TiledMap("assets/map/main.tmx");
            TileSize = Tilemap.TileWidth;

            foreach (var set in Tilemap.Tilesets)
                Tilesets.Add(set.source, new Tileset(set));
            foreach (var layer in Tilemap.Layers)
                Layers.Add(new WorldTileLayer(this, layer));
        }

        public void Update(float delta)
        {
            Step(delta);
        }
    }
}