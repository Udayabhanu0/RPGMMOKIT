using LiteNetLibManager;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using LiteNetLib.Utils;
using Cysharp.Threading.Tasks;

namespace MultiplayerARPG.MMO
{
    public class UIMmoCharacterCreateWithRace : UIMmoCharacterCreateExtension
    {
        public CharacterGenderTogglePair[] genderToggles;
        public CharacterRace srace = null;
        public CharacterGender sGender = null;
     
        private Dictionary<CharacterGender, Toggle> cacheGenderToggles;
        public Dictionary<CharacterGender, Toggle> CacheGenderToggles
        {
            get
            {
                if (cacheGenderToggles == null)
                {
                    cacheGenderToggles = new Dictionary<CharacterGender, Toggle>();
                    foreach (CharacterGenderTogglePair genderToggle in genderToggles)
                    {
                        cacheGenderToggles[genderToggle.Gender] = genderToggle.toggle;
                    }
                }
                return cacheGenderToggles;
            }
        }

        protected override void OnClickCreate()
        {
            PlayerCharacterData characterData = new PlayerCharacterData();
            characterData.Id = GenericUtils.GetUniqueId();
            characterData.SetNewPlayerCharacterData(inputCharacterName.text.Trim(), SelectedDataId, SelectedEntityId);
            characterData.FactionId = SelectedFactionId;
            MMOClientInstance.Singleton.RequestCreateCharacter(characterData, OnRequestedCreateCharacter);
            }

        private readonly HashSet<CharacterRace> selectedRaces = new HashSet<CharacterRace>();
        private readonly HashSet<CharacterGender> selectedGenders = new HashSet<CharacterGender>();

        protected override void OnRaceToggleUpdate(CharacterRace race, bool isOn)
        {
            if (isOn)
            {
                selectedRaces.Add(race);
                srace = race;
                LoadCharacters();

                int index = 0;
                foreach (CharacterRaceTogglePair toggle in raceToggles)
                {
                    if (toggle.race == srace)
                        SelectedFactionId = GetSelectableFactions()[index].DataId;

                    index++;
                }
            }
            else
                selectedRaces.Remove(race);
        }

        private void OnGenderToggleUpdate(CharacterGender gender, bool isOn)
        {
            if (isOn)
            {
                selectedGenders.Add(gender);
                sGender = gender;
                LoadCharacters();
                GetCreatableCharacters();
                //Debug.Log(gender);
            }
            else
                selectedGenders.Remove(gender);
        }
        //------------------------------

        public override void Show()
        {
            foreach (KeyValuePair<CharacterRace, Toggle> raceToggle in CacheRaceToggles)
            {
                raceToggle.Value.onValueChanged.RemoveAllListeners();
                raceToggle.Value.onValueChanged.AddListener((isOn) =>
                {
                    OnRaceToggleUpdate(raceToggle.Key, isOn);
                });
                OnRaceToggleUpdate(raceToggle.Key, raceToggle.Value.isOn);
            }

            foreach (KeyValuePair<CharacterGender, Toggle> genderToggle in CacheGenderToggles)
            {
                genderToggle.Value.onValueChanged.RemoveAllListeners();
                genderToggle.Value.onValueChanged.AddListener((isOn) =>
                {
                    OnGenderToggleUpdate(genderToggle.Key, isOn);
                });
                OnGenderToggleUpdate(genderToggle.Key, genderToggle.Value.isOn);
            }
            base.Show();

        }

        protected override List<BasePlayerCharacterEntity> GetCreatableCharacters()
        {
            if (srace == null || sGender == null)
            {
                var character = GameInstance.PlayerCharacterEntities.Values.Where((a) => a.Race.Equals(raceToggles[0].race)).ToList();
                var gendr = character.Where((a) => a.gender.Equals(genderToggles[0].Gender)).ToList();
                return gendr;
            }
            else
            {
                var character = GameInstance.PlayerCharacterEntities.Values.Where((a) => a.Race.Equals(srace)).ToList();
                var gendr = character.Where((a) => a.gender.Equals(sGender)).ToList();
                return gendr;
            }

        }

        private void OnRequestedCreateCharacter(ResponseHandlerData responseHandler, AckResponseCode responseCode, ResponseCreateCharacterMessage response)
        {
            if (responseCode.ShowUnhandledResponseMessageDialog(response.message)) return;
            if (eventOnCreateCharacter != null)
                eventOnCreateCharacter.Invoke();
        }
    }
}
