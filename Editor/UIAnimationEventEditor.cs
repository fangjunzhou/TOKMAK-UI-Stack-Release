﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FinTOKMAK.UIStackSystem.Editor
{
    public class UIAnimationEventEditor : EditorWindow
    {
        #region Private Field

        /// <summary>
        /// The current working AnimationWindow.
        /// </summary>
        private AnimationWindow _window;

        /// <summary>
        /// The current working clip.
        /// </summary>
        private AnimationClip _clip;

        /// <summary>
        /// The current play frame.
        /// </summary>
        private int _frame;

        /// <summary>
        /// The current play time.
        /// </summary>
        private float _time;

        /// <summary>
        /// All the anim event table, key is the event display name, value is the AnimationEvent Function name.
        /// </summary>
        private Dictionary<string, string> _animEventTable;

        #endregion
        
        [MenuItem("FinTOKMAK/UI Stack System/UI Animation Event Editor")]
        private static void ShowWindow()
        {
            var window = GetWindow<UIAnimationEventEditor>();
            window.titleContent = new GUIContent("UI Animation Event Editor");
            window.minSize = new Vector2(200, 250);
            window.Show();
        }

        private void OnEnable()
        {
            // Add all the events to the table
            _animEventTable = new Dictionary<string, string>();
            _animEventTable.Add("Finish Active to Background", "FinishActive2Background");
            _animEventTable.Add("Finish Active to Inactive", "FinishActive2Inactive");
            _animEventTable.Add("Finish Background to Inactive", "FinishBackground2Inactive");
        }

        private void OnGUI()
        {
            if (_window == null)
            {
                UpdateWorkingTarget();
            }

            if (_clip == null)
            {
                EditorGUILayout.HelpBox("No clip!", MessageType.Warning);
                if (GUILayout.Button("Update Clip"))
                {
                    UpdateWorkingTarget();
                }
                return;
            }
            
            GUILayout.Label($"Working Clip: {_clip.name}", EditorStyles.boldLabel);

            if (GUILayout.Button("Update"))
            {
                UpdateWorkingTarget();
            }
            
            EditorGUILayout.Space();
            
            EditorGUILayout.BeginVertical("Box");
            
            foreach (string displayName in _animEventTable.Keys)
            {
                if (GUILayout.Button(displayName))
                {
                    UpdateWorkingTarget();

                    if (_window == null || _clip == null)
                    {
                        return;
                    }
                    
                    List<AnimationEvent> events = AnimationUtility.GetAnimationEvents(_clip).ToList();
                    events.Add(new AnimationEvent()
                    {
                        time = _time,
                        functionName = _animEventTable[displayName]
                    });
                    AnimationUtility.SetAnimationEvents(_clip, events.ToArray());
                }
            }
            
            EditorGUILayout.EndVertical();
        }

        #region Private Methods

        /// <summary>
        /// Call this method to get the current working AnimationWindow, anim clip and other info.
        /// </summary>
        private void UpdateWorkingTarget()
        {
            // Get window
            _window = AnimationWindow.GetWindow<AnimationWindow>();
            // Get clip
            _clip = _window.animationClip;
            // Get frame
            _frame = _window.frame;
            // Get time
            _time = _window.time;
        }

        #endregion
    }
}