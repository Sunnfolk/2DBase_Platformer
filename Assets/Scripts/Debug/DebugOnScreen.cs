using Examples.Kinematic;
using UnityEngine;

namespace Debug
{
    [ExecuteInEditMode]
    public class DebugOnScreen : DebugGUI
    {
        public float Score = 0f;
        public float Health = 100f;

        [SerializeField] private Input _input;
        [SerializeField]
        private PlayerPlatformController _controller;
        
        [SerializeField] private float velocity;
        private RectOffset rectOff;
        
        private void OnGUI()
        {
            // GUI.skin.box.wordWrap = true;
            rectOff = GUI.skin.box.overflow;

            RightTopBox(0f, 80, 25, $"Health: {Health}");
            RightTopText(25f, 80, 25, $"Score:{Score} ");
        
            RightBottomBox($"Velocity: {_controller.velocityTest}" );
        }
    }
}