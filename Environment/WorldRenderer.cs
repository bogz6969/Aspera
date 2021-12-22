using Genbox.VelcroPhysics.Collision.Shapes;
using SFML.Graphics;
using SFML.System;

namespace Aspera.Environment
{
    public class WorldRenderer
    {
        public WorldRenderer(GameWorld world)
        {
            this.World = world;

            TextureAtlas = new Sprite();
            TextureAtlas.Position = new SFML.System.Vector2f(0, 0);
            rect = new RectangleShape(new Vector2f(512 * GameWorld.Scale, 512 * GameWorld.Scale));
            shaderTexture = new RenderTexture(512, 512);
            shaderSprite = new Sprite(shaderTexture.Texture);
            shaderSprite.Scale = new Vector2f(GameWorld.Scale, GameWorld.Scale);

            folliageShader = new Shader("shaders/waves.vert", null, null);
            noise = new Noise();
        }


        private RectangleShape rect;
        private RenderTexture shaderTexture;
        private Sprite shaderSprite;
        private Shader folliageShader;
        private Noise noise;
        private List<(Tileset set, WorldTileLayer layer, Vector2f pos, IntRect bounds, Vector2i tilePos)> foregroundTileRenderQueue = new List<(Tileset set, WorldTileLayer layer, Vector2f pos, IntRect bounds, Vector2i tilePos)>();

        public Sprite TextureAtlas { get; private set; }
        public GameWorld World { get; private set; }
        public void Draw(RenderTarget worldTarget, View camera, Player.PlayerObject player)
        {
            foregroundTileRenderQueue.Clear();

            foreach (var item in World.Layers)
                drawLayer(item, worldTarget, camera);
            worldTarget.Draw(player);
            drawForegroundTiles(worldTarget, camera);

                if (false)
            foreach (var item in this.World.BodyList)
            {
                item.GetTransform(out Genbox.VelcroPhysics.Shared.Transform xf);
                foreach (var f in item.FixtureList)
                {
                    switch (f.Shape.ShapeType)
                    {
                        case ShapeType.Polygon:
                            VertexArray array = new VertexArray(PrimitiveType.Quads);
                            PolygonShape poly = (PolygonShape)f.Shape;
                            int vertexCount = poly.Vertices.Count;

                            for (int i = 0; i < vertexCount; ++i)
                            {
                                var v = poly.Vertices[i] + item.Position;
                                array.Append(new Vertex(v.Convert(), new Color(255, 255, 255, 100)));
                            }
                            worldTarget.Draw(array);
                            break;
                    }
                }
            }
        }

        private void drawForegroundTiles(RenderTarget worldTarget, View camera)
        {
            foreach (var tile in foregroundTileRenderQueue)
            {
                DrawTile(tile.set, worldTarget, tile.layer, tile.pos, tile.bounds, tile.tilePos);
            }
        }

        private void DrawTile(Tileset set, RenderTarget worldTarget, WorldTileLayer layer, Vector2f pos, IntRect rect, Vector2i tilePos)
        {
            TextureAtlas.Texture = set.Texture;
            TextureAtlas.TextureRect = rect;
            shaderTexture.Clear(Color.Transparent);
            shaderTexture.Draw(TextureAtlas);
            shaderTexture.Display();

            shaderSprite.Position = pos;
            var states = RenderStates.Default;

            if (layer.Properties.Folliage)
            {
                folliageShader.SetUniform("iTime", Game.Instance.GameTime + 10 * layer[tilePos.X, tilePos.Y].Noise);
                states.Shader = folliageShader;
            }

            worldTarget.Draw(shaderSprite, states);
        }

        private void drawLayer(WorldTileLayer layer, RenderTarget worldTarget, View camera)
        {
            float leftBounds = (int)((camera.Center.X - camera.Size.X / 2) - GameWorld.TileSize * GameWorld.Scale);
            float rightBounds = (int)((camera.Center.X + camera.Size.X / 2));

            float topBounds = (int)((camera.Center.Y - camera.Size.Y / 2) - GameWorld.TileSize * GameWorld.Scale);
            float bottomBounds = (int)((camera.Center.Y + camera.Size.Y / 2));

            for (int x = 0; x < World.Tilemap.Width; x++)
            {
                float xPos = x * GameWorld.TileSize * GameWorld.Scale + layer.Properties.Offset.X;
                if (xPos > rightBounds || xPos < leftBounds) continue;

                for (int y = 0; y < World.Tilemap.Height; y++)
                {
                    float yPos = y * GameWorld.TileSize * GameWorld.Scale + layer.Properties.Offset.Y;
                    if (yPos > bottomBounds || yPos < topBounds) continue;

                    if (layer[x, y].ID > 0)
                    {
                        var set = World.Tilesets[World.Tilemap.GetTiledMapTileset(layer[x, y].ID).source];


                        int ty = (layer[x, y].ID - set.FirstGid) / set.Set.Columns;
                        int tx = (layer[x, y].ID - set.FirstGid) % set.Set.Columns;

                        Vector2f pos = new Vector2f(x * set.Set.TileWidth * GameWorld.Scale + layer.Properties.Offset.X, y * set.Set.TileHeight * GameWorld.Scale + layer.Properties.Offset.Y);
                        var rect = new IntRect(tx * set.Set.TileWidth, ty * set.Set.TileHeight, set.Set.TileWidth, set.Set.TileHeight);

                        if (pos.Y + (set.Set.TileWidth * GameWorld.Scale) / 2.0f > camera.Center.Y - 10 && layer.Properties.Id > 1 && ! layer.Properties.Ground || layer.Properties.Foreground)
                        {
                            foregroundTileRenderQueue.Add(new (set, layer, pos, rect, new Vector2i(x, y)));
                        }
                        else
                        {
                            DrawTile(set, worldTarget, layer, pos, rect, new Vector2i(x, y));
                        }
                    }
                }
            }

        }


    }
}