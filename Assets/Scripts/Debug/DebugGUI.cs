using UnityEngine;

namespace Debug
{
    public class DebugGUI : MonoBehaviour
    {
        public static DebugGUI Instance { get; private set; }

        private float _baseTopOffset;
        private float _baseBottomOffset;
        private float _baseLeftOffset;
        private float _baseRightOffset;

        private float _baseHeight;
        private float _baseWidth;
    
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                //DontDestroyOnLoad(gameObject);
            }
        }

        #region Top Right
    
        public void RightTopText(float topOffset = 0f, float width = 80f, float height = 25f, string text = "Top Right Label Text")
        {
            GUI.Label(new Rect(20,10+topOffset, width, height), text);
        }
        public void RightTopBox(float topOffset = 0f, float width = 80f, float height = 25f, string text = "Top Right Box Text")
        {
            GUI.Box(new Rect(20,10+topOffset, width, height), text);
        }
    
        #endregion

        #region Top Left
    
        public void LeftTopText(float leftOffset = 0f, float topOffset = 0f, float width = 80f, float height = 25f, string text = "Top Left Label Text")
        {
            GUI.Label(new Rect(Screen.width-50-leftOffset,10+topOffset, width, height), text);
        }
        public void LeftTopBox(float leftOffset = 0f, float topOffset = 0f, float width = 80f, float height = 25f, string text = "Top Left Label Text")
        {
            GUI.Label(new Rect(Screen.width-50-leftOffset,10+topOffset, width, height), text);
        }
    

        #endregion

        #region Bottom Right
    
        public void RightBottomText(float bottomOffset = 0f, float width = 80f, float height = 25f, string text = "Bottom Right Label Text")
        {
            GUI.Label(new Rect(20,Screen.height-30-bottomOffset, width, height), text);
        }
        public void RightBottomBox(float bottomOffset = 0f, float width = 80f, float height = 25f, string text = "Bottom Right Box Text")
        {
            GUI.Box(new Rect(20,Screen.height-30-bottomOffset, width, height), text);
        }

        public void RightBottomBox(float bottomOffset = 0f, string text = "Bottom Right Box Text")
        {
            GUI.Box(new Rect(20, Screen.height - 30 - bottomOffset, 80, 25f), text);
        }
        public void RightBottomBox(string text = "Bottom Right Box Text")
        {
            GUI.Box(new Rect(20, Screen.height - 30 , 80, 25f), text);
        }

        #endregion

        #region Bottom Left

        public void LeftBottomText(float leftOffset = 0f, float bottomOffset = 0f, float width = 80f, float height = 25f, string text = "Bottom Right Label Text")
        {
            GUI.Label(new Rect(Screen.width-50-leftOffset,Screen.height-30-bottomOffset, width, height), text);
        }
        public void LeftBottomBox(float leftOffset = 0f, float bottomOffset = 0f, float width = 80f, float height = 25f, string text = "Bottom Right Box Text")
        {
            GUI.Box(new Rect(Screen.width-50-leftOffset,Screen.height-30-bottomOffset, width, height), text);
        }

        public void LeftBottomBox(float leftOffset = 0f, float bottomOffset = 0f, string text = "Bottom Right Box Text")
        {
            GUI.Box(new Rect(Screen.width-50-leftOffset, Screen.height - 30 - bottomOffset, 80, 25f), text);
        }
        public void LeftBottomBox(string text = "Bottom Right Box Text")
        {
            GUI.Box(new Rect(Screen.width-50, Screen.height - 30 , 80, 25f), text);
        }
        #endregion
   
    }
}
