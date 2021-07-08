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
    public class Bird
    {
        
        private Texture2D bird;
        private Vector2 OldPosition = Vector2.Zero;
        private List<Texture2D> animation;
        private Vector2 velocity = Vector2.Zero;
        private bool is_Flying = false;
        private float jump_force = 15.7f;
        private float falling_speed = 2.7f;
        private float friction = 0.9f;
        private int current_frame = 0; //animation
        private int changingspeed = 1;
        private KeyboardState keyboard;
        private float FPS = 15;
        private float FPS_Clock = 0;
        private Vector2 origin = new Vector2(size / 2f , size/2f);

        private float angle = 0f;
        private float speed_angle = 0.1f;
        private static  int size = Main.Width / 15;
        private Vector2 Position = new Vector2(Main.Width / 2 - size, Main.Height / 2);
        bool onground = false;

        


        public virtual void LoadContent(ContentManager content)
        {
            animation = new List<Texture2D>
            {
                content.Load<Texture2D>("Assets/sprites/yellowbird-midflap"),
                content.Load<Texture2D>("Assets/sprites/yellowbird-upflap"),
                content.Load<Texture2D>("Assets/sprites/yellowbird-downflap")
            };
        }
        private void Control()
        {
            if(keyboard.IsKeyDown(Keys.Space) && !is_Flying)
            {
                Jump();
                is_Flying = true;
            }else
            {
                Falling();
            }
            if(keyboard.IsKeyUp(Keys.Space))
            {
                is_Flying =false;
            }
        }

        public void FixedUpdate()
        {
            OldPosition = Position;
            Control();
            
            Position += velocity;
            if(Position.Y < 0 )
            {
                Position.Y = 0;
            }
            if(Position.Y > Main.Height)
            {
                Position.Y = Main.Height - size/2;
                this.angle = 0f;
                onground = true;
            }
            velocity *= friction;
        }
        public virtual void Update(GameTime gameTime)
        {
            keyboard = Keyboard.GetState();
            FPS_Clock++;
            bird = animation[current_frame];
            if (FPS_Clock != FPS) return;

            current_frame += changingspeed;
            if(current_frame == 2)
            {
                changingspeed = -1;
            }
            if(current_frame == 0)
            {
                changingspeed = 1;
            }
            bird = animation[current_frame];
            FPS_Clock = 0;
        }
        private void Falling()
        {
            velocity.Y += falling_speed;
            if (onground) velocity.Y = 0;
        }

        private void Jump()
        {
            if(onground == true)
            {
                onground = false;
            }
            velocity.Y = 0;
            velocity.Y -= jump_force;
            angle = -1;
        }
        Vector2 Draw_position;
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)OldPosition.X, (int)OldPosition.Y, (int)OldPosition.X + size, (int)OldPosition.Y + size);
            }
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
           
           
            Draw_position = Vector2.Lerp(OldPosition, Position, Main.ALPHA);
            
            
            origin = new Vector2(bird.Width / 2f, bird.Height / 2f);
            if(angle < 1f && onground == false)
            {
                angle += speed_angle;
            }
            spriteBatch.Draw(bird, new Rectangle((int)Draw_position.X, (int)Draw_position.Y, size, size), null, Color.White, angle, origin, SpriteEffects.None, 0f); 
            spriteBatch.End();
        }


        #region Collision
        bool IsTouchingLeft(Tiles tile)
        {
            return (
                this.Rectangle.Right + this.velocity.X > tile.Rectangle1.Left &&
                this.Rectangle.Left < tile.Rectangle1.Left &&
                this.Rectangle.Bottom > tile.Rectangle1.Top &&
                this.Rectangle.Top < tile.Rectangle1.Bottom 
                ) 
                ||
                (
                this.Rectangle.Right + this.velocity.X > tile.Rectangle2.Left &&
                this.Rectangle.Left < tile.Rectangle2.Left &&
                this.Rectangle.Bottom > tile.Rectangle2.Top &&
                this.Rectangle.Top < tile.Rectangle2.Bottom
                );
        }

        bool IsTouchingRight(Tiles tile)
        {
            return (
                this.Rectangle.Left + this.velocity.X < tile.Rectangle1.Right &&
                this.Rectangle.Right > tile.Rectangle1.Right &&
                this.Rectangle.Bottom > tile.Rectangle1.Top &&
                this.Rectangle.Top < tile.Rectangle1.Bottom
                ) 
                ||
                (
                this.Rectangle.Left + this.velocity.X < tile.Rectangle2.Right &&
                this.Rectangle.Right > tile.Rectangle2.Right &&
                this.Rectangle.Bottom > tile.Rectangle2.Top &&
                this.Rectangle.Top < tile.Rectangle2.Bottom
                );
        }

        bool IsTouchingTop(Tiles tile)
        {
            return (
                this.Rectangle.Bottom + this.velocity.Y > tile.Rectangle1.Top &&
                this.Rectangle.Top < tile.Rectangle1.Top &&
                this.Rectangle.Right > tile.Rectangle1.Left &&
                this.Rectangle.Left < tile.Rectangle1.Right)
                || 
            (
                this.Rectangle.Bottom + this.velocity.Y > tile.Rectangle2.Top &&
                this.Rectangle.Top < tile.Rectangle2.Top &&
                this.Rectangle.Right > tile.Rectangle2.Left &&
                this.Rectangle.Left < tile.Rectangle2.Right
            );
        }

        bool IsTouchingBottom(Tiles tile)
        {
            return (
                this.Rectangle.Top + this.velocity.Y < tile.Rectangle1.Bottom &&
                this.Rectangle.Bottom > tile.Rectangle1.Bottom &&
                this.Rectangle.Right > tile.Rectangle1.Left &&
                this.Rectangle.Left < tile.Rectangle1.Right
                ) 
                ||
                (
                  this.Rectangle.Top + this.velocity.Y < tile.Rectangle2.Bottom &&
                  this.Rectangle.Bottom > tile.Rectangle2.Bottom &&
                  this.Rectangle.Right > tile.Rectangle2.Left &&
                  this.Rectangle.Left < tile.Rectangle2.Right
                );

        }

        #endregion


        public virtual bool  Check_Collision(Tiles first , Tiles second)
        {
             if(IsTouchingLeft(first) || ( second != null&&IsTouchingLeft(second) ))
             {
                return true;
             }
            if (IsTouchingRight(first) || (second != null && IsTouchingRight(second)))
            {
                return true;
            }
            if (IsTouchingTop(first) || (second != null && IsTouchingTop(second)))
            {
                return true;
            }
            if (IsTouchingBottom(first) || (second != null && IsTouchingBottom(second)))
            {
                return true;
            }
            return false;
        }

    }
}
