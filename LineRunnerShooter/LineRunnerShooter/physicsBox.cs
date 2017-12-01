using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRunnerShooter
{
    class PhysicsBox
    {
        protected double time; //for update 

        public Vector2 _Position;        //position
        protected Vector2 _Velocity;
        protected Vector2 _ResetPos;


        public double slow { get; set; }
        public double maxSpeed { get; set; }
        public double gravity { get; set; }
        public bool isGrounded { get; set; }

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
        }

        public virtual void Update(GameTime gameTime)
        {
            double totalTime = gameTime.ElapsedGameTime.TotalMilliseconds;
            _Position += _Velocity;
        }

        public virtual void UpdateFI(double dt)
        {
            MoveHorizontal(dt);
            MoveVertical(dt);
            _Position += _Velocity;
        }

        public void moveRight(double time)
        {
            _Velocity.X += (float)(Math.Abs(_Velocity.X) + time / slow);
            if (_Velocity.X > maxSpeed) { _Velocity.X = (float)maxSpeed; }
        }
        public void moveLeft(double time)
        {
            _Velocity.X -= (float)(Math.Abs(_Velocity.X) + time / slow);
            if (_Velocity.X < -maxSpeed) { _Velocity.X = (float)(-maxSpeed); }
        }

        public void moveStop(double time)
        {
            _Velocity.X /= 1.2F;
            if (Math.Abs(_Velocity.X) < 0.5f) { _Velocity.X = 0; }
        }

        private void moveHorizontal(MoveMethod dir, double time)
        {
            double speed = Math.Abs(_Velocity.X) + time / slow;
            if (speed > maxSpeed) { speed = maxSpeed; }
            switch (dir.Movedir)
            {
                case (0):
                    _Velocity.X = -(float)(speed);
                    break;
                case (1):
                    _Velocity.X = (float)(speed);
                    break;
                default:
                    if (Math.Abs(_Velocity.X) < 1)
                    {
                        _Velocity.X = 0;
                    }
                    else
                    {
                        _Velocity.X /= 1.2F;
                    }
                    break;
            }
            if (!isGrounded)
            {
                _Velocity.X = _Velocity.X * 0.95f;
            }
        }

        public virtual void MoveVertical(double time)
        {
            if (!isGrounded)
            {
                _Velocity.Y += (float)((time / 400.0) * gravity);
            }
            else { _Velocity.Y = 0; }
            if (_Velocity.Y > 20) { _Velocity.Y = 20; }

        }

        public virtual void Reset()
        {
            _Position = _ResetPos;
            _Velocity = new Vector2(0, 0);
        }

        
        
    }
}
