using Celeste.Player;
using UnityEngine;

namespace Examples.Celeste.Player
{
    [RequireComponent (typeof (PlayerCollisionChecker), typeof (Animator))]
    public class PlayerAnimator : MonoBehaviour {

        Animator _animator;
        SpriteRenderer _spriteRenderer;

        public GameObject groundedFlipAnim;
        private static readonly int IsRunning = Animator.StringToHash("isRunning");
        private static readonly int VerticalVelocity = Animator.StringToHash("verticalVelocity");

        private void Start() {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void UpdateAnimation(Vector3 velocity, PlayerCollisionChecker playerCollisionChecker) {
            _animator.SetBool(IsRunning, Mathf.Abs(velocity.x) > 0.1f);

            _animator.SetFloat(VerticalVelocity, velocity.y);

            var shouldFlipX = (playerCollisionChecker.FaceDirection == -1);

            if (_spriteRenderer.flipX == shouldFlipX) return;
            _spriteRenderer.flipX = shouldFlipX;

            if(playerCollisionChecker.CollisionData.Below)
            {
                PlayGroundedFlipAnim(playerCollisionChecker.FaceDirection);
            }
        }

        private void PlayGroundedFlipAnim(int faceDirection) 
        {
            var position = this.transform.position;
            position.x -= 0.7f * faceDirection;
            position.y -= 0.55f;
            var rotation = faceDirection == -1 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
            Instantiate(groundedFlipAnim, position, rotation);
        }
    }
}
