using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace baodeag
{
    public class StealthObject : MonoBehaviour
    {
        private List<CharacterManager> charactersStandingInStealthObject = new List<CharacterManager>();

        private void OnTriggerEnter(Collider other)
        {
            CharacterManager character = other.GetComponent<CharacterManager>();

            if (character == null)
                return;

            AddCharacterToStealthObject(character);
        }

        private void OnTriggerExit(Collider other)
        {
            CharacterManager character = other.GetComponent<CharacterManager>();

            if (character == null)
                return;

            RemoveCharacterFromStealthObject(character);
        }

        private void OnDisable()
        {
            for (int i = 0; i < charactersStandingInStealthObject.Count; i++)
            {
                if (charactersStandingInStealthObject[i] == null)
                    continue;

                RemoveCharacterFromStealthObject(charactersStandingInStealthObject[i]);
            }
        }

        private void AddCharacterToStealthObject(CharacterManager character)
        {
            for (int i = 0; i < charactersStandingInStealthObject.Count; i++)
            {
                if (charactersStandingInStealthObject[i] == null)
                    charactersStandingInStealthObject.RemoveAt(i);
            }

            if (character == null)
                return;

            if (charactersStandingInStealthObject.Contains(character))
                return;

            charactersStandingInStealthObject.Add(character);
            character.characterCombatManager.AddStealthObject(this);
        }

        private void RemoveCharacterFromStealthObject(CharacterManager character)
        {
            if (character == null)
                return;

            if (!charactersStandingInStealthObject.Contains(character))
                return;

            charactersStandingInStealthObject.Remove(character);
            character.characterCombatManager.RemoveStealthObject(this);

            for (int i = 0; i < charactersStandingInStealthObject.Count; i++)
            {
                if (charactersStandingInStealthObject[i] == null)
                    charactersStandingInStealthObject.RemoveAt(i);
            }
        }
    }
}
