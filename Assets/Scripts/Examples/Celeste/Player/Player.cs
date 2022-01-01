using System.Collections;
using Cinemachine;
using Examples.Celeste.level;
using UnityEngine;

namespace Examples.Celeste.Player
{
	[RequireComponent (typeof (PlayerCollisionChecker), typeof (PlayerAnimator), typeof (PlayerInput))]
	public class Player : MonoBehaviour {

		[SerializeField] private float moveSpeed = 8;
		[SerializeField] private float wallSlideSpeedMax = 3;
		[SerializeField] private float dashDistance = 5;
		[SerializeField] private float dashTime = .2f;
		[SerializeField] private float wallStickTime = .25f;
		[SerializeField] private float deathDelay = 2.0f;
		[SerializeField] private float maxJumpHeight = 4;
		[SerializeField] private float minJumpHeight = 1;
		[SerializeField] private float timeToJumpApex = .4f;
		[SerializeField] private Vector2 wallJumpClimb = new(6, 24);
		[SerializeField] private Vector2 wallJump = new(16, 24);
		[SerializeField] private float accelAirborne = .2f;
		[SerializeField] private float accelGrounded = .1f;

		private float _timeToWallUnstick;
		private float _gravity;
		private float _maxJumpVelocity;
		private float _minJumpVelocity;
		private float _velocityXSmoothing;

		private bool _isDashing;
		private bool _isWallSliding;
		private bool _isDead;

		private CinemachineImpulseSource _impulseSource;

		private Vector2 _directionalInput;
		private Vector3 _velocity;

		private PlayerCollisionChecker _playerCollisionChecker;
		private PlayerAnimator _animator;
		private PlayerInput _playerInput;

		public delegate void PlayerDelegate();
		public event PlayerDelegate JumpEvent;
		public event PlayerDelegate DashEvent;
		public event PlayerDelegate DeathEvent;

		public delegate void GateDelegate(Gate gate);
		public event GateDelegate GateEvent;

		private void Start() {
			_playerCollisionChecker = GetComponent<PlayerCollisionChecker> ();
			_animator = GetComponent<PlayerAnimator>();

			_playerInput = GetComponent<PlayerInput> ();
			_playerInput.directionalInputEvent += OnDirectionalInputUpdate;
			_playerInput.jumpPressedEvent += OnJumpPressed;
			_playerInput.jumpReleasedEvent += OnJumpReleased;
			_playerInput.dashPressedEvent += OnDashPressed;

			_impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();

			_gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
			_maxJumpVelocity = Mathf.Abs(_gravity) * timeToJumpApex;
			_minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (_gravity) * minJumpHeight);
		}

		private void Update() {
		
			CalculateVelocity ();
			_isWallSliding = (_playerCollisionChecker.CollisionData.Left || _playerCollisionChecker.CollisionData.Right) && !_playerCollisionChecker.CollisionData.Below && _velocity.y < 0;

			if (_isWallSliding) {
				HandleWallSliding ();
			}

			HandleMove();
			_animator.UpdateAnimation(_velocity, _playerCollisionChecker);
		}

		private void OnDirectionalInputUpdate (Vector2 input) {
			if(!_isDead) {
				_directionalInput = input;
			}
		}

		private void OnJumpPressed()
		{
			if (_isDead) return;
		
			if (_isWallSliding) {
				HandleWallJump();
				FireEvent(JumpEvent);
			} else if (_playerCollisionChecker.CollisionData.Below) {
				HandleJump();
				FireEvent(JumpEvent);
			}
		}

		private void OnJumpReleased()
		{
			if (_isDead) return;
		
			if (_velocity.y > _minJumpVelocity) {
				_velocity.y = _minJumpVelocity;
			}
		}

		private void OnDashPressed()
		{
			//todo particles + doublejump + 1 dash/ground
			if (_isDashing || _isDead) return;
		
			StartCoroutine(HandleDash());
			FireEvent(DashEvent);
		}

		private void OnDeath() {
			_isDead = true;
			_directionalInput = new Vector2();
			Invoke(nameof(ResetDeath), deathDelay);
			FireEvent(DeathEvent);
		}

		private void OnTriggerEnter2D(Collider2D other) {
			UnityEngine.Debug.Log(transform.position.x-other.gameObject.transform.position.x);
			if(other.CompareTag($"Trap")) {
				OnDeath();
			}
		}

		private void OnTriggerExit2D(Collider2D other) {
			//FIXME bug si player passe pas gate et fait marche arriere
			if(other.CompareTag($"Gate") && GateEvent != null) {
				GateEvent(other.gameObject.GetComponent<GateTrigger>().Gate);
			}
		}

		private void ResetDeath() {
			_isDead = false;
		}

		private IEnumerator HandleDash() {
			_isDashing = true;
			var normalizedInput = _directionalInput.normalized;
			var dashVelocity = dashDistance / dashTime;

			if(!_isWallSliding){
				_velocity.x = (normalizedInput == Vector2.zero) ? dashVelocity * _playerCollisionChecker.FaceDirection : normalizedInput.x * dashVelocity;
			} else {
				_velocity.x = dashVelocity * -_playerCollisionChecker.FaceDirection;
			}
		
			_velocity.y = normalizedInput.y * dashVelocity;
		
			_impulseSource.GenerateImpulse(new Vector3(10, 10));
			_animator.UpdateAnimation(_velocity, _playerCollisionChecker);

			yield return new WaitForSeconds(dashTime);
			_velocity.x = _playerCollisionChecker.FaceDirection;
			_velocity.y = 0;
			_isDashing = false;
		}

		private void HandleWallSliding() {
			var wallDirX = (_playerCollisionChecker.CollisionData.Left) ? -1 : 1;
		
			if (_velocity.y < -wallSlideSpeedMax) {
				_velocity.y = -wallSlideSpeedMax;
			}

			if (_timeToWallUnstick > 0) {
				_velocityXSmoothing = 0;
				_velocity.x = 0;

				if (_directionalInput.x != wallDirX && _directionalInput.x != 0) {
					_timeToWallUnstick -= Time.deltaTime;
				} else {
					_timeToWallUnstick = wallStickTime;
				}
			} else {
				_timeToWallUnstick = wallStickTime;
			}
		}

		private void HandleWallJump() {
			var wallDirX = (_playerCollisionChecker.CollisionData.Left) ? -1 : 1;

			if (Mathf.Sign(wallDirX) == Mathf.Sign(_directionalInput.x) && _directionalInput.x != 0) { // wall climb
				_velocity.x = -wallDirX * wallJumpClimb.x;
				_velocity.y = wallJumpClimb.y;
			} else { // wall jump
				_velocity.x = -wallDirX * wallJump.x;
				_velocity.y = wallJump.y;
				_playerCollisionChecker.FaceDirection *= -1;
			}
		}

		private void HandleJump() {
			if (_playerCollisionChecker.CollisionData.IsSlidingDownSlope) { // jump from sliding slope
				_velocity.y = _maxJumpVelocity * _playerCollisionChecker.CollisionData.SlopeNormal.y;
				_velocity.x = _maxJumpVelocity * _playerCollisionChecker.CollisionData.SlopeNormal.x;
			} else { // base jump
				_velocity.y = _maxJumpVelocity;
			}
		}

		private void HandleMove() {
			_playerCollisionChecker.Move (_velocity * Time.deltaTime, _directionalInput);
			UpdateVerticalVelocityAfterMove();
		}

		private void CalculateVelocity()
		{
			if (_isDashing) return;

			_velocity.y = Mathf.Clamp(_velocity.y, -16f, 16f);
		
			var targetVelocityX = _directionalInput.x * moveSpeed;
			_velocity.x = Mathf.SmoothDamp (_velocity.x, targetVelocityX, ref _velocityXSmoothing, (_playerCollisionChecker.CollisionData.Below)?accelGrounded:accelAirborne);
			_velocity.y += _gravity * Time.deltaTime;
		}

		private void UpdateVerticalVelocityAfterMove()
		{
			if (!_playerCollisionChecker.CollisionData.Above && !_playerCollisionChecker.CollisionData.Below) return;
		
			if (_playerCollisionChecker.CollisionData.IsSlidingDownSlope) { 
				_velocity.y += _playerCollisionChecker.CollisionData.SlopeNormal.y * -_gravity * Time.deltaTime; // big angle = fast fall
			} else {
				_velocity.y = 0; // is grounded or cannot go through smthinh
			}
		}

		private static void FireEvent(PlayerDelegate playerEvent)
		{
			playerEvent?.Invoke();
		}
	}
}