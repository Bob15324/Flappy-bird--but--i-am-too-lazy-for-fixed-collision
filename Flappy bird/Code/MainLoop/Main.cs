#region include
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion
namespace Flappy_bird.Code
{
    public class Main : Game
    {

        Random r = new Random();
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static int Width = 500;
        public static int Height = 800;
        public static float game_speed = 0.5f;
        public static bool game_over = false;
        public static float FixedDeltaTime = (int) 1000 / (float)30;

        public static float MaxFrame = 250;

        public static float previousT = 0;
        public static float accumulator = 0 ;

        public static int spaces = Width / 4;
        private int turn = 1;
        private Texture2D background;
        

        private Vector2 backgroundPosition1 = new Vector2(0, 0);
        private Vector2 backgroundPosition2 = new Vector2(Width , 0 );

        public Bird Player = new Bird();

        public static float ALPHA = 0;
        private Tiles FirstObject;
        private Tiles SecondObject;
        bool first = true;
        bool second = false;
        public void Contruct1()
        {
            int height = r.Next(200, Height - 200);
            FirstObject = new Tiles(new Vector2(Width + 100+Width/4, 0), Width / 5, height);
        }

        public void Contruct2()
        {
            int height = r.Next(200, Height - 200);
            SecondObject = new Tiles(new Vector2(Width + 100 + Width/4, 0), Width / 5, height);
        }

        

         
        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = Width;
            _graphics.PreferredBackBufferHeight = Height;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("Assets/sprites/background-night");
            Contruct1();
            Player.LoadContent(this.Content);
            FirstObject.LoadContent(this.Content);
            
        }
        bool game_start = true;
        protected override void Update(GameTime gameTime)
        {
           
            Player.Update(gameTime);
            if((int)FirstObject.Get_Pos().X <Width/2 && first)
            {
                Contruct2();
                SecondObject.LoadContent(this.Content);
                first = false;
                second = true;
            }
            if(SecondObject != null && (int)SecondObject.Get_Pos().X < Width/2 && second)
            {
                Contruct1();
                FirstObject.LoadContent(this.Content);
                second = false;
                first = true;
            }
            
            

            FirstObject.Update(gameTime);
            if(SecondObject != null)
                 SecondObject.Update(gameTime);
            if (previousT == 0)
            {
                previousT = (float)gameTime.TotalGameTime.TotalMilliseconds;
            }



            float now = (float)gameTime.TotalGameTime.TotalMilliseconds;
            float frameTime = now - previousT;
            if(frameTime  > MaxFrame)
            {
                frameTime = MaxFrame;
            }
            previousT = now;

            accumulator += frameTime;

            while(accumulator >= FixedDeltaTime)
            {
                Player.FixedUpdate();
                FirstObject.FixedUpdate();
                if(SecondObject != null)
                    SecondObject.FixedUpdate();
                accumulator -= FixedDeltaTime;
            }

            ALPHA = (accumulator / FixedDeltaTime );

            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            Matrix view = Matrix.Identity;

            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Width + 500, Height, 0, 0,1);
            _spriteBatch.Begin();
            if (-Width < backgroundPosition1.X)
            {
                backgroundPosition1.X -= game_speed;
                backgroundPosition2.X -= game_speed;
            }else
            {

                backgroundPosition1.X = 0;
                backgroundPosition2.X = Width;
            }
            
            _spriteBatch.Draw(background, new Rectangle((int)backgroundPosition1.X, (int)backgroundPosition1.Y, Width , Height), Color.White);
            _spriteBatch.Draw(background, new Rectangle((int)backgroundPosition2.X, (int)backgroundPosition2.Y, Width, Height), Color.White);

            _spriteBatch.End();

            Player.Draw(_spriteBatch);

            FirstObject.Draw(_spriteBatch);
            if(SecondObject != null)            
               SecondObject.Draw(_spriteBatch);

            base.Draw(gameTime);
        }
    }
}
