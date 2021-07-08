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
    public class Tiles
    {
        private Texture2D Up ;
        private Texture2D Down;
        private float friction = 0.9f;
        private Vector2 pos ;
        private Vector2 oldPos = Vector2.Zero;

        private Vector2 Velocity = Vector2.Zero;


        


        private bool firstTime = true;
        
        private int size_x ;

        private int size_y;


        


        public Tiles(Vector2 pos , int size_x , int size_y )
        {
            this.pos = pos;
            this.size_x = size_x;
            this.size_y = size_y;
        }


        public virtual void FixedUpdate()
        {
            oldPos = pos;
            Velocity.X -= Main.game_speed;
            pos += Velocity;
            Velocity *= friction;
        }

        public virtual void LoadContent(ContentManager content)
        {
            Up = content.Load<Texture2D>("Assets/sprites/Up");
            Down = content.Load<Texture2D>("Assets/sprites/Down");
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }

        Vector2 Draw_pos  = Vector2.Zero;
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if(firstTime)
            {
                firstTime = false;
                return ; 
            }

            spriteBatch.Begin();
            Draw_pos = Vector2.Lerp(oldPos , pos , Main.ALPHA);
            spriteBatch.Draw(Up , new Rectangle((int)Draw_pos.X , (int)Draw_pos.Y , size_x , size_y) , Color.White);
            spriteBatch.Draw(Down , new Rectangle((int)Draw_pos.X , (int)size_y + Main.spaces , size_x , Main.Height - size_y - Main.spaces ),Color.White);
            spriteBatch.End();
        }

        public Rectangle Rectangle2
        {
            get
            {
                return new Rectangle((int)oldPos.X, (int)size_y + Main.spaces, size_x, Main.Height - size_y - Main.spaces);
            }
        }
        public Rectangle Rectangle1
        {
            get
            {

                return new Rectangle((int)oldPos.X, (int)oldPos.Y, size_x, size_y);
            }
        }

        public Vector2 Get_Pos()
        {
            return this.pos;
        }

        ~Tiles()
        {

        }
    }
}