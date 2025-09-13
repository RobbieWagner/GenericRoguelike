using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using RobbieWagnerGames.Utilities;
using RobbieWagnerGames.Dialogue;
using RobbieWagnerGames.Managers;
using UnityEngine.InputSystem;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using DG.Tweening;

namespace RobbieWagnerGames.RoguelikeCYOA
{
	public partial class DialogueManager : MonoBehaviourSingleton<DialogueManager>
	{
		private Story currentStory = null;
		private string currentSentence;
		private Coroutine typingCoroutine;
		[SerializeField] private float typeSpeed = 0.05f;

		protected override void Awake()
		{
			base.Awake();
			dialogueCanvas.enabled = false;
			InputManager.Instance.GetAction(ActionMapName.DIALOGUE, "Select").performed += OnSelect;
			diceRollParent.gameObject.SetActive(false);
		}

		#region story
		public bool StartStory(TextAsset storyText)
		{
			if (currentStory == null)
			{
				currentStory = DialogueConfigurer.CreateStory(storyText);
				//characterInfoButton.InitializeUI(CharacterManager.Instance.currentCharacter.characterSprite, CharacterManager.Instance.currentCharacter.characterName);
				dialogueCanvas.enabled = true;
				ContinueStory();
				InputManager.Instance.EnableActionMap(ActionMapName.DIALOGUE);
				return true;
			}
			return false;
		}

		private void ContinueStory()
		{
			if (currentStory.canContinue)
			{
				currentSentence = FormatGameText(currentStory.Continue());

				// Stop any existing typing coroutine
				if (typingCoroutine != null)
				{
					StopCoroutine(typingCoroutine);
				}

				// Start typing out the new sentence
				typingCoroutine = StartCoroutine(TypeSentence(currentSentence));
				StartCoroutine(FinishTypingSentence());
			}
			else
			{
				EndStory();
			}
		}

		public void EndStory()
		{
			currentStory = null;
			dialogueText.text = "";

			InputManager.Instance.DisableActionMap(ActionMapName.DIALOGUE);
			ClearChoices();
			dialogueCanvas.enabled = false;
		}
		#endregion

		#region choices
		public void MakeChoice(int choiceIndex, List<string> tags)
		{
			ClearChoices();
			StartCoroutine(MakeChoiceCo(choiceIndex, tags));
		}

		private IEnumerator MakeChoiceCo(int choiceIndex, List<string> tags)
		{
			if (tags != null && tags.Any())
			{
				foreach (string tag in tags)
				{
					// Process tag before making choice
					if (!string.IsNullOrEmpty(tag))
					{
						if(tag.StartsWith("DEBUG"))
						{
							yield return null;
							Debug.Log(tag);
						}
						//if (tag.StartsWith("ROLL"))
						//	yield return StartCoroutine(ParseRollTag(tag));
					}
				}
			}

			currentStory.ChooseChoiceIndex(choiceIndex);

			ContinueStory();
		}

		//private IEnumerator ParseRollTag(string tagContent)
		//{
		//	// Extract parameters from "ROLL(STAT,value)" format
		//	var match = Regex.Match(tagContent, @"ROLL\((\w+),(\d+)\)");
		//	if (match.Success && match.Groups.Count == 3)
		//	{
		//		string stat = match.Groups[1].Value;
		//		int threshold = int.Parse(match.Groups[2].Value);
		//		yield return StartCoroutine(HandleStatCheck(stat, threshold));
		//	}
		//}

		//private IEnumerator HandleStatCheck(string stat, int threshold)
		//{
		//	CharacterStat statType = Character.ConvertStringToCharacterStat(stat);
		//	if(!CharacterManager.Instance.currentCharacter.stats.TryGetValue(statType, out int playerStatValue))
		//		yield break;

		//	// Roll 2d6 (PbtA-style)
		//	int die1 = Random.Range(1, 7);
		//	int die2 = Random.Range(1, 7);
		//	int roll = die1 + die2;
		//	int result = roll + playerStatValue;
		//	bool success = (result >= threshold);

		//	// Display roll results to user
		//	yield return StartCoroutine(DisplayDiceRoll(die1, die2, roll, statType, playerStatValue, success));

		//	// Pass result back to Ink (using a global variable)
		//	Debug.Log($"{stat}, {roll}, {success}");
		//	currentStory.variablesState["lastRollSuccess"] = success ? 1 : 0;
		//}
		#endregion

		#region controls
		private void OnSelect(InputAction.CallbackContext context)
		{
			if (typingCoroutine == null)
			{
				if (currentStory.currentChoices.Count == 0)
					ContinueStory();
			}
			else
			{
				StopCoroutine(typingCoroutine);
				typingCoroutine = null;
			}
		}

		#endregion
	}
}