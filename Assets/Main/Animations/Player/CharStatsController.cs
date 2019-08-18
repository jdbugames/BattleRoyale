using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattleRoyale
{
    [CreateAssetMenu(fileName = "Stats", menuName = "BattleRoyale/Char Stats")]
    public class CharStatsController : ScriptableObject
    {
        public float fl_Speed = 6;
        public float fl_RotationSpeed = 25;
        public float fl_JumpForce = 25;
        public float fl_MinAngle = -70;
        public float fl_MaxAngle = 90;
        public float fl_CameraSpeed = 24;

        public float fl_CrouchHeightOffSet;
        public float fl_CrouchPosOffSet;
    }
}
