using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRoyale
{
    public class RagdollController : MonoBehaviour
    {
        Animator anim_Ragdoll;
        Rigidbody[] rb_Bones;
        Rigidbody rb_Body;
        CharacterController characterController;

        public bool bl_Debug;
        
    }

    public class HitMultiplier
    {
        public string srt_BoneName = "head";
        public float fl_Value;
    }
}
