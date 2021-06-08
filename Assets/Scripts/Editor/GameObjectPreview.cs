using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Strungerhulder.EditorTools
{
    internal class GameObjectPreview
    {
        public RenderTexture outputTexture;


        private static Type s_GameObjectInspectorType;
        private static MethodInfo s_GetPreviewDataMethod;
        private static FieldInfo s_RenderUtilityField;

        private Rect m_RenderRect;
        private Color m_Light0Color;
        private Color m_Light1Color;
        private PreviewRenderUtility m_RenderUtility;

        private Editor m_CachedEditor;


        public void CreatePreviewForTarget(GameObject target)
        {
            if (!m_CachedEditor || m_CachedEditor.target != target)
            {
                m_RenderUtility = null;
                // There is a bug that breaks previews and Prefab mode after creating too many editors.
                // Simply using CreateCachedEditor is fixing that problem.
                Editor.CreateCachedEditor(target, s_GameObjectInspectorType, ref m_CachedEditor);
            }
        }

        public void RenderInteractivePreview(Rect rect)
        {
            if (!m_CachedEditor)
                return;

            if (m_RenderUtility == null || m_RenderUtility.lights[0] == null)
            {
                var previewData = s_GetPreviewDataMethod.Invoke(m_CachedEditor, null);
                m_RenderUtility = s_RenderUtilityField.GetValue(previewData) as PreviewRenderUtility;

                m_Light0Color = m_RenderUtility.lights[0].color;
                m_Light1Color = m_RenderUtility.lights[1].color;
            }

            m_RenderUtility.lights[0].color = m_Light0Color * 1.6f;
            m_RenderUtility.lights[1].color = m_Light1Color * 6f;
            var backColor = m_RenderUtility.camera.backgroundColor;
            m_RenderUtility.camera.backgroundColor = new Color(backColor.r, backColor.g, backColor.b, 0);
            m_RenderUtility.camera.clearFlags = CameraClearFlags.Depth;

            var color = GUI.color;

            // Hide default preview texture, since it has no alpha blending
            GUI.color = new Color(1, 1, 1, 0);

            m_CachedEditor.OnPreviewGUI(rect, null);

            GUI.color = color;

            outputTexture = m_RenderUtility.camera.targetTexture;
        }

        public void DrawPreviewTexture(Rect rect)
        {
            GUI.DrawTexture(rect, outputTexture, ScaleMode.ScaleToFit, true, 0);
        }

        public static bool HasRenderableParts(GameObject go)
        {
            var renderers = go.GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                switch (renderer)
                {
                    case MeshRenderer _:
                        var filter = renderer.gameObject.GetComponent<MeshFilter>();
                        if (filter && filter.sharedMesh)
                            return true;
                        break;

                    case SkinnedMeshRenderer skinnedMesh:
                        if (skinnedMesh.sharedMesh)
                            return true;
                        break;

                    case SpriteRenderer sprite:
                        if (sprite.sprite)
                            return true;
                        break;

                    case BillboardRenderer billboard:
                        if (billboard.billboard && billboard.sharedMaterial)
                            return true;
                        break;
                }
            }

            return false;
        }


        [InitializeOnLoadMethod]
        private static void OnInitialize()
        {
            s_GameObjectInspectorType = typeof(Editor).Assembly.GetType("UnityEditor.GameObjectInspector");

            var previewDataType = s_GameObjectInspectorType.GetNestedType("PreviewData", BindingFlags.NonPublic);

            s_GetPreviewDataMethod = s_GameObjectInspectorType.GetMethod("GetPreviewData", BindingFlags.NonPublic | BindingFlags.Instance);
            s_RenderUtilityField = previewDataType.GetField("m_RenderUtility", BindingFlags.Public | BindingFlags.Instance);
        }
    }
}
