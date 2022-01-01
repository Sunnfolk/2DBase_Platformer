using UnityEngine;

namespace Examples.Celeste.Player
{
	[RequireComponent (typeof (global::Examples.Celeste.Player.Player))]
	public class PlayerInput : MonoBehaviour
	{
		private InputActions _inputActions;
		public delegate void PlayerAxisInputDelegate(Vector2 input);	
		public delegate void PlayerKeyInputDelegate();

		public event PlayerAxisInputDelegate directionalInputEvent;
		public event PlayerKeyInputDelegate jumpPressedEvent;
		public event PlayerKeyInputDelegate jumpReleasedEvent;
		public event PlayerKeyInputDelegate dashPressedEvent;

		private Vector2 _moveVector;
		private bool _dash;
		private bool _jump;

		private void Awake() => _inputActions = new InputActions();

		private void Update ()
		{
			_moveVector = _inputActions.Player.Move.ReadValue<Vector2>();
			UnityEngine.Debug.Log(_moveVector);
		
			_dash = _inputActions.Player.Move.ReadValue<float>() > 0.2f;
			_jump = _inputActions.Player.Jump.ReadValue<float>() > 0.2f;
			UnityEngine.Debug.Log(_jump);
		
			directionalInputEvent?.Invoke(_moveVector);

			if (_inputActions.Player.Jump.IsPressed() && jumpPressedEvent != null){
				jumpPressedEvent();
				UnityEngine.Debug.Log("Jumped");
			}
		
			if (_jump && jumpReleasedEvent != null){
				jumpReleasedEvent();
				UnityEngine.Debug.Log("Jumping");
			}

			if (_dash && dashPressedEvent != null){
				dashPressedEvent();
			}
		}
	}
}