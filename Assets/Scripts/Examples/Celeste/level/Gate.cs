using UnityEngine;

namespace Examples.Celeste.level
{
    [CreateAssetMenu(fileName = "Gate", menuName = "2DLab/Gate")]
    public class Gate : ScriptableObject {
        public int Id;
        public Board BoardA;
        public Board BoardB;
    }
}
