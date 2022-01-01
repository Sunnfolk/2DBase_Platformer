using System.Collections.Generic;
using UnityEngine;

namespace Examples.Kinematic
{
    public class PhysicsObject : MonoBehaviour
    {
        public float minGroundNormalY = .65f;
        public float gravityModifier = 1f;

        protected Vector2 targetVelocity;
        protected bool Grounded;
        protected Vector2 GroundNormal;
    
        protected Rigidbody2D Rigidbody2D;
        protected Vector2 Velocity;
        protected ContactFilter2D ContactFilter2D;
        protected RaycastHit2D[] Hitbuffer = new RaycastHit2D[16];
        protected List<RaycastHit2D> HitBufferList = new(16);

        protected const float MinMoveDistance = 0.001f;
        protected const float ShellRadius = 0.01f;

        private void OnEnable()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            ContactFilter2D.useTriggers = false;
            ContactFilter2D.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
            ContactFilter2D.useLayerMask = true;
            GroundNormal = new Vector2 (0f, 1f); 
        }

        private void Update()
        {
            targetVelocity = Vector2.zero;
            ComputeVelocity();
        }

        protected virtual void ComputeVelocity()
        {
        
        }

        private void FixedUpdate()
        {
            Velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
            Velocity.x = targetVelocity.x;
        
            Grounded = false;
        
            var deltaPosition = Velocity * Time.deltaTime;
        
            var moveAlongGround = new Vector2(GroundNormal.y, -GroundNormal.x);
        
            Vector2 move = moveAlongGround * deltaPosition.x;
            Movement(move, false);
        
            move = Vector2.up * deltaPosition.y;
            Movement(move, true);
        }

        public void Movement(Vector2 move, bool yMovement)
        {
            var distance = move.magnitude;
            if (distance > MinMoveDistance)
            {
                int count = Rigidbody2D.Cast(move, ContactFilter2D, Hitbuffer, distance+ShellRadius);
                HitBufferList.Clear();
            
                for (int i = 0; i < count; i++)
                {
                    HitBufferList.Add(Hitbuffer[i]);
                }

                foreach (var hitBuffer in HitBufferList)
                {
                    var currentNormal = hitBuffer.normal;
                
                    if (currentNormal.y > minGroundNormalY)
                    {
                        Grounded = true;
                        if (yMovement)
                        {
                            GroundNormal = currentNormal;
                            currentNormal.x = 0f;
                        }
                    }

                    var projection = Vector2.Dot(Velocity, currentNormal);
                    if (projection < 0)
                    {
                        Velocity -= projection * currentNormal;
                    }

                    var modifiedDistance = hitBuffer.distance - ShellRadius;
                    distance = modifiedDistance < distance ? modifiedDistance : distance;
                }
                
                /*
                for (int i = 0; i < HitBufferList.Count; i++)
                {
                    var currentNormal = HitBufferList[i].normal;
                
                    if (currentNormal.y > minGroundNormalY)
                    {
                        Grounded = true;
                        if (yMovement)
                        {
                            GroundNormal = currentNormal;
                            currentNormal.x = 0f;
                        }
                    }

                    var projection = Vector2.Dot(Velocity, currentNormal);
                    if (projection < 0)
                    {
                        Velocity -= projection * currentNormal;
                    }

                    var modifiedDistance = HitBufferList[i].distance - ShellRadius;
                    distance = modifiedDistance < distance ? modifiedDistance : distance;
                }
                 * 
                 */
            }
            Rigidbody2D.position += move.normalized * distance;
        }
    }
}