﻿using System.Collections.Generic;
using UnityEngine;

namespace MultiplayerARPG
{
    [DisallowMultipleComponent]
    public class CharacterSkillAndBuffComponent : BaseGameEntityComponent<BaseCharacterEntity>
    {
        public const float SKILL_BUFF_UPDATE_DURATION = 1f;
        public const string KEY_VEHICLE_BUFF = "<VEHICLE_BUFF>";

        private float updatingTime;
        private float deltaTime;
        private Dictionary<string, CharacterRecoveryData> recoveryBuffs;

        public override void EntityStart()
        {
            recoveryBuffs = new Dictionary<string, CharacterRecoveryData>();
        }

        public override sealed void EntityUpdate()
        {
            if (!Entity.IsServer)
                return;

            deltaTime = Time.unscaledDeltaTime;
            updatingTime += deltaTime;

            if (Entity.IsRecaching || Entity.IsDead())
                return;

            if (updatingTime >= SKILL_BUFF_UPDATE_DURATION)
            {
                float tempDuration;
                int tempCount;
                if (Entity.PassengingVehicleEntity != null)
                {
                    tempDuration = Entity.PassengingVehicleEntity.GetBuff().GetDuration();
                    if (tempDuration > 0f)
                    {
                        CharacterRecoveryData recoveryData;
                        if (!recoveryBuffs.TryGetValue(KEY_VEHICLE_BUFF, out recoveryData))
                        {
                            recoveryData = new CharacterRecoveryData(Entity);
                            recoveryData.SetupByBuff(null, Entity.PassengingVehicleEntity.GetBuff());
                            recoveryBuffs.Add(KEY_VEHICLE_BUFF, recoveryData);
                        }
                        recoveryData.Apply(1 / tempDuration * updatingTime);
                    }
                }
                // Removing summons if it should
                if (!Entity.IsDead())
                {
                    tempCount = Entity.Summons.Count;
                    CharacterSummon summon;
                    for (int i = tempCount - 1; i >= 0; --i)
                    {
                        summon = Entity.Summons[i];
                        tempDuration = summon.GetBuff().GetDuration();
                        if (summon.ShouldRemove())
                        {
                            recoveryBuffs.Remove(summon.id);
                            Entity.Summons.RemoveAt(i);
                            summon.UnSummon(Entity);
                        }
                        else
                        {
                            summon.Update(updatingTime);
                            Entity.Summons[i] = summon;
                        }
                        // If duration is 0, damages / recoveries will applied immediately, so don't apply it here
                        if (tempDuration > 0f)
                        {
                            CharacterRecoveryData recoveryData;
                            if (!recoveryBuffs.TryGetValue(summon.id, out recoveryData))
                            {
                                recoveryData = new CharacterRecoveryData(Entity);
                                recoveryData.SetupByBuff(null, summon.GetBuff());
                                recoveryBuffs.Add(summon.id, recoveryData);
                            }
                            recoveryData.Apply(1 / tempDuration * updatingTime);
                        }
                        // Don't update next buffs if character dead
                        if (Entity.IsDead())
                            break;
                    }
                }
                // Removing buffs if it should
                if (!Entity.IsDead())
                {
                    tempCount = Entity.Buffs.Count;
                    CharacterBuff buff;
                    for (int i = tempCount - 1; i >= 0; --i)
                    {
                        buff = Entity.Buffs[i];
                        tempDuration = buff.GetBuff().GetDuration();
                        if (buff.ShouldRemove())
                        {
                            recoveryBuffs.Remove(buff.id);
                            Entity.Buffs.RemoveAt(i);
                        }
                        else
                        {
                            buff.Update(updatingTime);
                            Entity.Buffs[i] = buff;
                        }
                        // If duration is 0, damages / recoveries will applied immediately, so don't apply it here
                        if (tempDuration > 0f)
                        {
                            CharacterRecoveryData recoveryData;
                            if (!recoveryBuffs.TryGetValue(buff.id, out recoveryData))
                            {
                                recoveryData = new CharacterRecoveryData(Entity);
                                recoveryData.SetupByBuff(buff, buff.GetBuff());
                                recoveryBuffs.Add(buff.id, recoveryData);
                            }
                            recoveryData.Apply(1 / tempDuration * updatingTime);
                        }
                        // Don't update next buffs if character dead
                        if (Entity.IsDead())
                            break;
                    }
                }
                // Removing skill usages if it should
                if (!Entity.IsDead())
                {
                    tempCount = Entity.SkillUsages.Count;
                    CharacterSkillUsage skillUsage;
                    for (int i = tempCount - 1; i >= 0; --i)
                    {
                        skillUsage = Entity.SkillUsages[i];
                        if (skillUsage.ShouldRemove())
                        {
                            Entity.SkillUsages.RemoveAt(i);
                        }
                        else
                        {
                            skillUsage.Update(updatingTime);
                            Entity.SkillUsages[i] = skillUsage;
                        }
                    }
                }
                updatingTime = 0;
            }
        }
    }
}
