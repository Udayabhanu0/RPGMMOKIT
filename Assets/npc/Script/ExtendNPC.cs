using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiplayerARPG
{
    public class ExtendNPC : NpcEntity
    {
        [Header("Movement Tool")]
        public float walkSpeed = 3.5f;
        public float RunSpeed = 5.5f;

        public override sealed bool CanMove()
        {
            return true;
        }
        public  override sealed float GetMoveSpeed()
        {
            return walkSpeed;
        }
        public override sealed bool CanSprint()
        {
            return true;
        }
        public override sealed bool CanCrouch()
        {
            return true;
        }
        public override sealed bool CanCrawl()
        {
            return true;
        }

    }

}

