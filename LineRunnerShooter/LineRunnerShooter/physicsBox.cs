using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRunnerShooter
{
    /*
     * Warning!! 
     * This class is contains some deseases (bad english), do not touch until it is finished (or at least working)
     * 
     */
    class PhysicsBox
    {
        protected double time; //for update 

        public Vector2 _Position;        //position
        private Vector2 _VelocityChange;
        protected Vector2 _Velocity;
        protected Vector2 _ResetPos;


        public double slow { get; set; }
        public double maxSpeed { get; set; }
        public double gravity { get; set; }

        public Vector2 Location { get { return _Position; } }

        public PhysicsBox(MoveMethod move, Vector2 startPos, Vector2 resetPos)
        {
            _Position = startPos;
            time = 0;
            _ResetPos = resetPos;
            slow = 30;
            _Velocity = new Vector2(0, 0);
            gravity = 9.81;
            maxSpeed = 15;
            _VelocityChange = new Vector2(0, 0);
        }
        public PhysicsBox(MoveMethod move, Vector2 startPos, Vector2 resetPos, int slow, int gravity, int maxSpeed)
        {
            _Position = startPos;
            time = 0;
            _ResetPos = resetPos;
            this.slow = slow;
            _Velocity = new Vector2(0, 0);
            this.gravity = gravity;
            this.maxSpeed = maxSpeed;
            _VelocityChange = new Vector2(0, 0);
        }

        public virtual void Update(GameTime gameTime)
        {
            double totalTime = gameTime.ElapsedGameTime.TotalMilliseconds;
            _Velocity += _VelocityChange;
            _Position += _Velocity;
            _VelocityChange = new Vector2(0, 0);
        }

        public void moveRight(double time)//I WAS HERE888
        {
            _VelocityChange.X += (float)(Math.Abs(_Velocity.X) + time / slow);
            if (_VelocityChange.X > maxSpeed) { _VelocityChange.X = (float)maxSpeed; }
        }
        public void moveLeft(double time)
        {
            _VelocityChange.X -= (float)(Math.Abs(_Velocity.X) + time / slow);
            if (_VelocityChange.X < -maxSpeed) { _VelocityChange.X = (float)(-maxSpeed); }
        }

        public void moveStop(double time)
        {
            _Velocity.X /= 1.2F;
            if (Math.Abs(_Velocity.X) < 0.5f) { _Velocity.X = 0; }
        }

        private void moveUpdate(MoveMethod dir, double time, bool isGrounded)
        {
            switch (dir.Movedir)
            {
                case (0):
                    moveLeft(time);
                    break;
                case (1):
                    moveRight(time);
                    break;
                default:
                    moveStop(time);
                    break;
            }
            if (!isGrounded)
            {
                _VelocityChange.X = _VelocityChange.X * 0.95f;
            }
        }

        public virtual void MoveVertical(double time)
        {
            /*
            if (!false)
            {
                _Velocity.Y += (float)((time / 400.0) * gravity);
            }
            else { _Velocity.Y = 0; }
            if (_Velocity.Y > 20) { _Velocity.Y = 20; }
            */
        }

        public virtual void Reset()
        {
            _Position = _ResetPos;
            _Velocity = new Vector2(0, 0);
        }

        
        
    }
}
