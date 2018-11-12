using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DialogueNode
{
	public List<string> text;
	public List<Option> options;
}

[System.Serializable]
public class Option
{
	public string text;
	public string next;
}

[System.Serializable]
public class NiceButton
{
	public Button button;
	public Text text;
	
	public void SetActive(bool active)
	{
		button.gameObject.SetActive(active);
	}
}

[System.Serializable]
public class InspectorNode
{
	public string id;
	public DialogueNode node;
}

public class DialogueSystem : MonoBehaviour 
{
	[SerializeField] string m_CharacterName;
	[SerializeField] List<InspectorNode> m_InspectorDialogue;
	[SerializeField] string m_StartNode;
	[SerializeField] Typewriter m_Typewriter;
	[SerializeField] List<NiceButton> m_OptionButtons;

	Dictionary<string, DialogueNode> m_Dialogue;
	string current;

	void Start()
	{
		current = m_StartNode;
		m_Dialogue = new Dictionary<string, DialogueNode>();
		foreach (InspectorNode node in m_InspectorDialogue)
		{
			m_Dialogue.Add(node.id, node.node);
		}
		SetButtonsActive(false);
	}

	[ContextMenu("Do Dialogue")]
	void StartDialogue()
	{
		current = m_StartNode;
		SendCurrentDialogue();
	}

	void SendCurrentDialogue()
	{
		SetButtonsActive(false);
		m_Typewriter.StartTypewriter(
			m_Dialogue[current].text, 
			m_CharacterName, 
			null, 
			() => {
				ShowOptions(current);
			}
		);
	}

	void ShowOptions(string node)
	{
		if (m_Dialogue[node].options.Count == 0) return;
		Debug.Assert(m_Dialogue[node].options.Count <= m_OptionButtons.Count);
		List<Option> options = m_Dialogue[node].options;
		for (int i = 0; i < options.Count; i++)
		{
			int index = i;
			m_OptionButtons[i].SetActive(true);
			m_OptionButtons[i].text.text = options[i].text;
			m_OptionButtons[i].button.onClick.RemoveAllListeners();
			m_OptionButtons[i].button.onClick.AddListener(
				() => {
					current = m_Dialogue[node].options[index].next;
					SendCurrentDialogue();
				}
			);
		}	
	}

	void SetButtonsActive(bool active)
	{
		foreach (NiceButton b in m_OptionButtons)
		{
			b.SetActive(active);
		}
	}
		
}
