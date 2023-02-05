﻿using UnityEngine;
using UnityEngine.EventSystems;

namespace MultiplayerARPG
{
    public abstract partial class BasePlayerCharacterController : MonoBehaviour
    {
        public struct UsingSkillData
        {
            public AimPosition aimPosition;
            public BaseSkill skill;
            public int level;
            public int itemIndex;
            public UsingSkillData(AimPosition aimPosition, BaseSkill skill, int level, int itemIndex)
            {
                this.aimPosition = aimPosition;
                this.skill = skill;
                this.level = level;
                this.itemIndex = itemIndex;
            }

            public UsingSkillData(AimPosition aimPosition, BaseSkill skill, int level)
            {
                this.aimPosition = aimPosition;
                this.skill = skill;
                this.level = level;
                this.itemIndex = -1;
            }
        }

        public static BasePlayerCharacterController Singleton { get; protected set; }
        /// <summary>
        /// Controlled character, can use `GameInstance.PlayingCharacter` or `GameInstance.PlayingCharacterEntity` instead.
        /// </summary>
        public static BasePlayerCharacterEntity OwningCharacter { get { return Singleton == null ? null : Singleton.PlayingCharacterEntity; } }
        public System.Action<BasePlayerCharacterController> onSetup;
        public System.Action<BasePlayerCharacterController> onDesetup;

        public BasePlayerCharacterEntity PlayingCharacterEntity
        {
            get { return GameInstance.PlayingCharacterEntity; }
            set
            {
                if (value.IsOwnerClient)
                {
                    Desetup(GameInstance.PlayingCharacterEntity);
                    GameInstance.PlayingCharacter = value;
                    GameInstance.OpenedStorageType = StorageType.None;
                    GameInstance.OpenedStorageOwnerId = string.Empty;
                    Setup(GameInstance.PlayingCharacterEntity);
                }
            }
        }

        public Transform CameraTargetTransform
        {
            get { return PlayingCharacterEntity.CameraTargetTransform; }
        }

        public Transform EntityTransform
        {
            get { return PlayingCharacterEntity.EntityTransform; }
        }

        public Transform MovementTransform
        {
            get { return PlayingCharacterEntity.MovementTransform; }
        }

        public float StoppingDistance
        {
            get { return PlayingCharacterEntity.StoppingDistance; }
        }

        public BaseUISceneGameplay CacheUISceneGameplay { get; protected set; }
        public GameInstance CurrentGameInstance { get { return GameInstance.Singleton; } }
        public ITargetableEntity SelectedEntity { get; protected set; }
        public BaseGameEntity SelectedGameEntity
        {
            get
            {
                if (SelectedEntity is IGameEntity)
                    return (SelectedEntity as IGameEntity).Entity;
                return null;
            }
        }
        public uint SelectedGameEntityObjectId { get { return SelectedGameEntity != null ? SelectedGameEntity.ObjectId : 0; } }
        public ITargetableEntity TargetEntity { get; protected set; }
        public BaseGameEntity TargetGameEntity
        {
            get
            {
                if (TargetEntity is IGameEntity)
                    return (TargetEntity as IGameEntity).Entity;
                return null;
            }
        }
        public uint TargetGameEntityObjectId { get { return TargetGameEntity != null ? TargetGameEntity.ObjectId : 0; } }
        public BuildingEntity ConstructingBuildingEntity { get; protected set; }
        public BuildingEntity TargetBuildingEntity { get { return TargetGameEntity as BuildingEntity; } }
        protected int buildingItemIndex;
        protected UsingSkillData queueUsingSkill;

        protected virtual void Awake()
        {
            Singleton = this;
            this.InvokeInstanceDevExtMethods("Awake");
        }

        protected virtual void Update()
        {
        }

        protected virtual void Setup(BasePlayerCharacterEntity characterEntity)
        {
            // Initial UI Scene gameplay
            if (CurrentGameInstance.UISceneGameplayPrefab != null)
                CacheUISceneGameplay = Instantiate(CurrentGameInstance.UISceneGameplayPrefab);
            if (CacheUISceneGameplay != null)
                CacheUISceneGameplay.OnControllerSetup(characterEntity);
            // Don't send navigation events
            EventSystem eventSystem = FindObjectOfType<EventSystem>();
            if (eventSystem != null)
                eventSystem.sendNavigationEvents = false;
            if (onSetup != null)
                onSetup.Invoke(this);
        }

        protected virtual void Desetup(BasePlayerCharacterEntity characterEntity)
        {
            if (CacheUISceneGameplay != null)
                Destroy(CacheUISceneGameplay.gameObject);
            if (onDesetup != null)
                onDesetup.Invoke(this);
        }

        protected virtual void OnDestroy()
        {
            Desetup(PlayingCharacterEntity);
            this.InvokeInstanceDevExtMethods("OnDestroy");
        }

        public virtual void ConfirmBuild()
        {
            if (ConstructingBuildingEntity == null)
                return;
            if (ConstructingBuildingEntity.CanBuild())
            {
                uint parentObjectId = 0;
                if (ConstructingBuildingEntity.BuildingArea != null)
                    parentObjectId = ConstructingBuildingEntity.BuildingArea.GetEntityObjectId();
                PlayingCharacterEntity.Building.CallServerConstructBuilding(buildingItemIndex, ConstructingBuildingEntity.EntityTransform.position, ConstructingBuildingEntity.EntityTransform.rotation, parentObjectId);
            }
            DestroyConstructingBuilding();
        }

        public virtual void CancelBuild()
        {
            DestroyConstructingBuilding();
        }

        public virtual BuildingEntity InstantiateConstructingBuilding(BuildingEntity prefab)
        {
            ConstructingBuildingEntity = Instantiate(prefab);
            ConstructingBuildingEntity.SetupAsBuildMode(PlayingCharacterEntity);
            ConstructingBuildingEntity.EntityTransform.parent = null;
            return ConstructingBuildingEntity;
        }

        public virtual void DestroyConstructingBuilding()
        {
            if (ConstructingBuildingEntity == null)
                return;
            Destroy(ConstructingBuildingEntity.gameObject);
            ConstructingBuildingEntity = null;
        }

        public virtual void DeselectBuilding()
        {
            TargetEntity = null;
        }

        public virtual void DestroyBuilding(BuildingEntity targetBuildingEntity)
        {
            if (targetBuildingEntity == null)
                return;
            PlayingCharacterEntity.Building.CallServerDestroyBuilding(targetBuildingEntity.ObjectId);
            DeselectBuilding();
        }

        public virtual void SetBuildingPassword(BuildingEntity targetBuildingEntity)
        {
            if (targetBuildingEntity == null)
                return;
            uint objectId = targetBuildingEntity.ObjectId;
            UISceneGlobal.Singleton.ShowPasswordDialog(
                LanguageManager.GetText(UITextKeys.UI_SET_BUILDING_PASSWORD.ToString()),
                LanguageManager.GetText(UITextKeys.UI_SET_BUILDING_PASSWORD_DESCRIPTION.ToString()),
                (password) =>
                {
                    PlayingCharacterEntity.Building.CallServerSetBuildingPassword(objectId, password);
                }, string.Empty, targetBuildingEntity.PasswordContentType, targetBuildingEntity.PasswordLength,
                LanguageManager.GetText(UITextKeys.UI_SET_BUILDING_PASSWORD.ToString()));
            DeselectBuilding();
        }

        public virtual void LockBuilding(BuildingEntity targetBuildingEntity)
        {
            if (targetBuildingEntity == null)
                return;
            PlayingCharacterEntity.Building.CallServerLockBuilding(targetBuildingEntity.ObjectId);
            DeselectBuilding();
        }

        public virtual void UnlockBuilding(BuildingEntity targetBuildingEntity)
        {
            if (targetBuildingEntity == null)
                return;
            PlayingCharacterEntity.Building.CallServerUnlockBuilding(targetBuildingEntity.ObjectId);
            DeselectBuilding();
        }

        public virtual void ActivateBuilding(BuildingEntity targetBuildingEntity)
        {
            if (targetBuildingEntity == null)
                return;
            targetBuildingEntity.OnActivate();
            DeselectBuilding();
        }

        protected void ShowConstructBuildingDialog()
        {
            if (!ConstructingBuildingEntity.CanBuild())
            {
                DestroyConstructingBuilding();
                CacheUISceneGameplay.HideConstructBuildingDialog();
                return;
            }
            CacheUISceneGameplay.ShowConstructBuildingDialog(ConstructingBuildingEntity);
        }

        protected void HideConstructBuildingDialog()
        {
            CacheUISceneGameplay.HideConstructBuildingDialog();
        }

        protected void ShowCurrentBuildingDialog()
        {
            CacheUISceneGameplay.ShowCurrentBuildingDialog(TargetBuildingEntity);
        }

        protected void HideCurrentBuildingDialog()
        {
            CacheUISceneGameplay.HideCurrentBuildingDialog();
        }

        protected void ShowItemsContainerDialog(ItemsContainerEntity itemsContainerEntity)
        {
            CacheUISceneGameplay.ShowItemsContainerDialog(itemsContainerEntity);
        }

        protected void HideItemsContainerDialog()
        {
            CacheUISceneGameplay.HideItemsContainerDialog();
        }

        protected void HideNpcDialog()
        {
            CacheUISceneGameplay.HideNpcDialog();
        }

        public void SetQueueUsingSkill(AimPosition aimPosition, BaseSkill skill, int level)
        {
            queueUsingSkill = new UsingSkillData(aimPosition, skill, level);
        }

        public void SetQueueUsingSkill(AimPosition aimPosition, BaseSkill skill, int level, int itemIndex)
        {
            queueUsingSkill = new UsingSkillData(aimPosition, skill, level, itemIndex);
        }

        public void ClearQueueUsingSkill()
        {
            queueUsingSkill = new UsingSkillData();
            queueUsingSkill.aimPosition = default;
            queueUsingSkill.skill = null;
            queueUsingSkill.level = 0;
            queueUsingSkill.itemIndex = -1;
        }

        public abstract void UseHotkey(HotkeyType type, string relateId, AimPosition aimPosition);
        public abstract AimPosition UpdateBuildAimControls(Vector2 aimAxes, BuildingEntity prefab);
        public abstract void FinishBuildAimControls(bool isCancel);
    }
}
