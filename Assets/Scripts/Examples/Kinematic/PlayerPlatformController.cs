using UnityEngine;

namespace Examples.Kinematic
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D), typeof(Input))]
    public class PlayerPlatformController : PhysicsObject
    {
        private Input _input;
        private void Awake() => _input = GetComponent<Input>();

        public float maxSpeed = 7f;
        public float jumpTakeOffSpeed = 7f;

        public float velocityTest;
        protected override void ComputeVelocity()
        {
            velocityTest = Rigidbody2D.velocity.magnitude;
            var move = Vector2.zero;
            move.x = _input.MoveVector.x;

            HandleJump();

            targetVelocity = move * maxSpeed;
        }

        private void HandleJump()
        {
            if (_input.Jump.IsPressed() && Grounded)
            {
                Velocity.y = jumpTakeOffSpeed;
            }
            else if (_input.Jump.WasReleasedThisFrame())
            {
                if (Velocity.y > 0f)
                {
                    Velocity.y *= .5f;
                }
            }
        }
    }
}
