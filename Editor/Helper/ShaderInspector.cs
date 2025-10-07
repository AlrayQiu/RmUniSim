using UnityEditor;
using UnityEngine;


namespace com.alray.rmunisim.Applications.Helper
{
    public static class ShaderInspectorHelper
    {
        public static void Draw(Shader shader, Material material)
        {
            int count = ShaderUtil.GetPropertyCount(shader);

            for (int i = 0; i < count; i++)
            {
                var name = ShaderUtil.GetPropertyName(shader, i);
                var desc = ShaderUtil.GetPropertyDescription(shader, i);
                var type = ShaderUtil.GetPropertyType(shader, i);

                switch (type)
                {
                    case ShaderUtil.ShaderPropertyType.Color:
                        Color c = material.GetColor(name);
                        Color newC = EditorGUILayout.ColorField(desc, c);
                        if (newC != c) material.SetColor(name, newC);
                        break;

                    case ShaderUtil.ShaderPropertyType.Vector:
                        Vector4 v = material.GetVector(name);
                        Vector4 newV = EditorGUILayout.Vector4Field(desc, v);
                        if (newV != v) material.SetVector(name, newV);
                        break;

                    case ShaderUtil.ShaderPropertyType.Float:
                    case ShaderUtil.ShaderPropertyType.Range:
                        float f = material.GetFloat(name);
                        float newF = EditorGUILayout.FloatField(desc, f);
                        if (newF != f) material.SetFloat(name, newF);
                        break;

                    case ShaderUtil.ShaderPropertyType.TexEnv:
                        Texture t = material.GetTexture(name);
                        Texture newT = EditorGUILayout.ObjectField(desc, t, typeof(Texture), false) as Texture;
                        if (newT != t) material.SetTexture(name, newT);
                        break;
                }
            }
        }
    }
}