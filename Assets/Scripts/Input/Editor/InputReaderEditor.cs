using UnityEditor;
using UnityEngine;
using Strungerhulder.Editors;

namespace Strungerhulder.Input
{
    [CustomEditor(typeof(InputReader))]
    public class InputReaderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (!Application.isPlaying)
                return;

            ScriptableObjectHelper.GenerateButtonsForEvents<InputReader>(target);
        }
    }
}
