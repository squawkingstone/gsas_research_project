using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDialogue : MonoBehaviour 
{
	[SerializeField] List<string> m_Text;
	[SerializeField] Typewriter m_Typewriter;

	[ContextMenu("Display Dialogue")]
	void SendDialogue()
	{
		m_Typewriter.StartTypewriter(m_Text);
	}

}
