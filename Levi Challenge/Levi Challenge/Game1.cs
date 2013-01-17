using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Levi_Challenge
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        BackgroundManager backgroundManager = new BackgroundManager();
        SpawnManager spawnManager = new SpawnManager();
        ProjectileManager projectileManager = new ProjectileManager();
        CollisionManager collisionManager = new CollisionManager();
        Player player = new Player();
        HudManager hudManager = new HudManager();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            XMLEngine.PhraseWeaponXML();
            XMLEngine.PhraseShipXML();
            backgroundManager.Initialize(Content, GraphicsDevice, @"Clouds\Cloud-Red-1", @"Clouds\Cloud-Red-2");
            spawnManager.Initialize(GraphicsDevice, Content);
            projectileManager.Initialize();
            player.Initialize();
            hudManager.Initialize();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            foreach (Ship ship in XMLEngine.PlayerShips)
            {
                ship.ShipTexture = (Content.Load<Texture2D>(ship.ShipTexturePath));
                foreach (Weapon weapon in ship.myHardpoints)
                {
                    weapon.ProjectileTexture = (Content.Load<Texture2D>(weapon.ProjectileTexturePath));
                }
            }

            foreach (Ship ship in XMLEngine.EnemyShips)
            {
                ship.ShipTexture = (Content.Load<Texture2D>(ship.ShipTexturePath));
                foreach (Weapon weapon in ship.myHardpoints)
                {
                    weapon.ProjectileTexture = (Content.Load<Texture2D>(weapon.ProjectileTexturePath));
                }
            }

            

            player.LoadContent(Content, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            hudManager.LoadContent(Content, GraphicsDevice);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            hudManager.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            backgroundManager.Update();
            spawnManager.Update(gameTime, projectileManager);
            player.Update(gameTime, projectileManager);
            projectileManager.Update(GraphicsDevice.Viewport);
            collisionManager.Update(player, spawnManager.enemyManager.Enemies, spawnManager.enemyManager.Astroids, projectileManager.Projectiles);
            hudManager.Update(GraphicsDevice);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            backgroundManager.Draw(spriteBatch);
            spriteBatch.Begin();
            projectileManager.Draw(spriteBatch);
            spawnManager.Draw(spriteBatch);
            player.Draw(spriteBatch);
            spriteBatch.End();
            hudManager.Draw(spriteBatch);
            base.Draw(gameTime);
        }
    }
}
