﻿using UnityEngine;

namespace MultiplayerARPG
{
    [CreateAssetMenu(fileName = GameDataMenuConsts.PLAYER_FRAME_FILE, menuName = GameDataMenuConsts.PLAYER_FRAME_MENU, order = GameDataMenuConsts.PLAYER_FRAME_ORDER)]
    public partial class PlayerFrame : BaseGameData
    {
        public bool isLocked;
    }
}
