using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Typewriter : MonoBehaviour 
{
	[Header("Typewriter Parameters")]
	[SerializeField] float m_CharacterWait;
	[SerializeField] float m_PunctuationWait;
	[SerializeField] AudioClip m_DefaultDialogueChirp;

	[Header("Name Tag")]
	[SerializeField] GameObject m_NameTag;
	[SerializeField] Text m_NameTagText;

	[Header("Text Panel")]
	[SerializeField] GameObject m_TextPanel;
	[SerializeField] Text m_TextPanelText;

	[Header("Target Audio Source")]
	[SerializeField] AudioSource m_AudioSource;

	//string m_CurrentText;
	List<string> m_Dialogue;
	string m_Name;
	AudioClip m_DialogueChirp;
	UnityAction m_Callback;
	bool m_UseNameTag;
	bool m_Playing;
	bool m_Skip;
	bool m_Finished;
	bool m_AxisFree;
	int m_Counter;

	private static readonly List<char> m_WaitPunctuation = new List<char>() {
		'.', ',', '?', '!', '(', ')'
	};

	void Start() 
	{ 
		DisableTextBox();
		InitBooleans();
		m_Dialogue = null;
		m_Callback = null;
	}

	void Update()
	{
		if (m_Playing)
		{
			if (Input.GetAxisRaw("Submit") != 0f && m_AxisFree)
			{
				m_AxisFree = false;
				Next();
			}
			if (Input.GetAxisRaw("Submit") == 0f && !m_AxisFree)
			{
				m_AxisFree = true;
			}
		}
	}

	public void StartTypewriter(List<string> dialogue,
		string name = "",
		AudioClip dialogueChirp = null,
		UnityAction callback = null)
	{
		InitBooleans();
		m_Dialogue = dialogue;
		m_UseNameTag = (name != "");
		m_Name = name;
		m_DialogueChirp = (dialogueChirp != null) ? dialogueChirp : 
			((m_DefaultDialogueChirp != null) ? m_DefaultDialogueChirp : null);
		m_AudioSource.clip = m_DialogueChirp;
		m_Callback = callback;
		m_Counter = 0;
		EnableTextBox();
		Next();
		m_Playing = true;
	}

	public void StopTypewriter()
	{
		StopAllCoroutines();
		DisableTextBox();
		InitBooleans();
		m_Dialogue = null;
		m_Callback = null;
	}

	void InitBooleans()
	{
		m_Playing = false;
		m_Skip = false; 
		m_Finished = true;
		m_AxisFree = true;
		m_UseNameTag = false;
	}

	void EnableTextBox()
	{
		m_TextPanel.SetActive(true);
		m_TextPanelText.enabled = true;
		if (m_NameTag != null) { m_NameTag.SetActive(m_UseNameTag); } 
		if (m_NameTagText != null) { m_NameTagText.text = m_Name; }
	}

	void DisableTextBox()
	{
		m_TextPanel.SetActive(false);
		m_TextPanelText.enabled = false;
		if (m_NameTag != null) { m_NameTag.SetActive(false); }
	}

	void FinishDialogue()
	{
		DisableTextBox();
		m_Playing = false;
		if (m_Callback != null) m_Callback();
	}

	void Next()
	{
		/* If we're done displaying text */
		if (m_Finished) 
		{ 
			/* If we've gone through all the text */
			if (m_Counter == m_Dialogue.Count) 
			{ 
				/* Finish displaying */
				FinishDialogue();
				return;
			}
			/* Display the next line */
			TypeText(m_Dialogue[m_Counter++]); 
		}
		/* Otherwise, skip to the end of the text we're displaying */
		else 
		{ 
			SkipToEnd(); 
		}
	}

	void TypeText(string text)
	{
		m_Skip = false;
		StopAllCoroutines();
		StartCoroutine(TypeRoutine(text));
	}

	void SkipToEnd()
	{
		m_Skip = !m_Finished ? true : false;
	}

	private IEnumerator TypeRoutine(string text)
	{
		m_Finished = false;
		m_TextPanelText.text = "";
		for (int i = 0; i < text.Length; i++)
		{
			// If we have to skip through the text, print the rest of the string
			if (m_Skip) { PrintRemainingCharacters(text, i); break; }

			// Otherwise, print a single character
			PrintCharacter(text[i]);

			/* If it was the last character, break
			 * this is so the coroutine ends even if the string ends in punctuation
			 */
			if (i == text.Length - 1) break;

			/* Wait for a set amound of time */
			yield return new WaitForSeconds(IsWaitPunctuation(text[i])
				? m_PunctuationWait
				: m_CharacterWait);
		}
		m_Finished = true;
	}

	void PrintCharacter(char c)
	{
		m_AudioSource.Play();
		m_TextPanelText.text += c;
	}

	void PrintRemainingCharacters(string text, int counter)
	{
		m_TextPanelText.text += text.Substring(counter);
	}

	bool IsWaitPunctuation(char c)
	{
		return m_WaitPunctuation.Contains(c);

	}
	
}
