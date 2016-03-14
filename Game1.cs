using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace PotatoClickerMono
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        int potatocount;
        double currentTime;
        double previousTime;
        double timeDifference;
        bool isbuttonpressed = false;
        Rectangle button;
        Rectangle progressbar;
        Texture2D buttonPressed;
        Texture2D buttonNotPressed;
        Texture2D progressBar;
        KeyboardState currentKBState;
        KeyboardState previousKBState;
        MouseState currentMouseState;
        MouseState previousMouseState;
        string messageBuffer;

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
            IsMouseVisible = true;
            button.Width = 100;
            button.Height = 30;
            button.X = 200;
            button.Y = 200;
            progressbar.Width = 0;
            progressbar.Height = 10;
            progressbar.X = 200;
            progressbar.Y = 400;
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
            font = Content.Load<SpriteFont>("kooten");
            buttonPressed = Content.Load<Texture2D>("potatobuttonpressed");
            buttonNotPressed = Content.Load<Texture2D>("potatobutton");
            progressBar = Content.Load<Texture2D>("pbar");

            if(!File.Exists("Game1.sav"))
            {
                using (StreamWriter saveFile = new StreamWriter("Game1.sav"))
                {
                    saveFile.WriteLine("0");
                    saveFile.WriteLine(potatocount);
                }

            }
            else
            {
                using (StreamReader saveFile = new StreamReader("Game1.sav"))
                {
                    saveFile.ReadLine();
                    int.TryParse(saveFile.ReadLine(), out potatocount);
                    messageBuffer = "Save game loaded!";
                }
            }

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            if(!File.Exists("Game1.sav"))
            {
                using (StreamWriter saveFile = new StreamWriter("Game1.sav"))
                {
                    saveFile.WriteLine("0");
                    saveFile.WriteLine(potatocount);
                }
            }

            using (StreamWriter saveFile = new StreamWriter("Game1.sav", false))
            {
                saveFile.WriteLine("0");
                saveFile.WriteLine(potatocount);
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            currentKBState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
            currentTime = gameTime.TotalGameTime.Seconds;
            timeDifference = currentTime - previousTime;



            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if(currentKBState.IsKeyDown(Keys.P) && !previousKBState.IsKeyDown(Keys.P))
            {
                potatocount += 100;
            }

            if(currentKBState.IsKeyDown(Keys.S) && !previousKBState.IsKeyDown(Keys.S))
            {
                if (!File.Exists("Game1.sav"))
                {
                    using (StreamWriter saveFile = new StreamWriter("Game1.sav"))
                    {
                        saveFile.WriteLine("0");
                        saveFile.WriteLine(potatocount);
                    }
                }
                using (StreamWriter saveFile = new StreamWriter("Game1.sav", false))
                {
                    saveFile.WriteLine("0");
                    saveFile.WriteLine(potatocount);
                }
                messageBuffer = "Game Saved!";
                //add saving stuff here.
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (button.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                {
                    if (!isbuttonpressed)
                    {
                        isbuttonpressed = true;
                        progressbar.Width++;
                    }
        
                }
            }
            if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                //add mouse button release stuff here
                isbuttonpressed = false;
            }


            if (timeDifference >= 1)
            {
                messageBuffer = null;

                if (progressbar.Width < 100)
                {

                    progressbar.Width++;
                }

                else
                {
                    progressbar.Width = 0;
                    potatocount++;

                }

            }

            previousKBState = currentKBState;
            previousMouseState = currentMouseState;
           previousTime = currentTime;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(progressBar, progressbar, Color.White);

            if (!isbuttonpressed)
            {
                spriteBatch.Draw(buttonNotPressed, button, Color.White);
            }
            else
                spriteBatch.Draw(buttonPressed, button, Color.White);
            spriteBatch.DrawString(font, "Potatoes: " + potatocount, new Vector2(100, 100), Color.White);
            if(messageBuffer != null)
            {
                spriteBatch.DrawString(font, messageBuffer, new Vector2(100, 0), Color.White);
            }

            spriteBatch.End();                
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
