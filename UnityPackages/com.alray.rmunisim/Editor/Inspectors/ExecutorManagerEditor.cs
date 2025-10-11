using UnityEngine;
using UnityEditor;

namespace com.alray.rmunisim.Applications
{
    public class ExecutorSettingsWindow : EditorWindow
    {
        [MenuItem("RmUniSim/Executor Settings")]
        public static void OpenWindow()
        {
            // 打开窗口，如果已打开则聚焦
            GetWindow<ExecutorSettingsWindow>("Executor Settings");
        }
        private void OnGUI()
        {
            GUILayout.Label("执行器设置", EditorStyles.boldLabel);
            ExecutorManager.ExecutorType = (ExecutorManager.ExecutorTypes)EditorGUILayout.EnumPopup("执行器类型", ExecutorManager.ExecutorType);
        }
    }
}