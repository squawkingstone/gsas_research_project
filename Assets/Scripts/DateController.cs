using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*

	Let's go through what needs to happen flow chart style, and go from there

	So, for each date, we go through the following process:

	introduction();
	while (!questions.asked && time.available)
	{
		wait for user to select a question
		disable that question button
		hide questions
		show npc answer
		show questions
	}
	goodbye();
	next_date();

 */

public class DateController : MonoBehaviour 
{
	[System.Serializable]
	public class Response
	{
		[SerializeField] public string id;
		[SerializeField] public List<string> response;
	}

	[SerializeField] List<Response> _responses_list;

	Dictionary<string, List<string>> _responses;

	[SerializeField] Typewriter _typewriter;
	[SerializeField] GameObject _question_container;
	[SerializeField] List<Button> _question_buttons;

	bool shouldLoadNextDate;
	bool shouldStartNextDate;
	bool loadingNextDate;

	void Awake() {}

	void Start() 
	{
		int i = 0;
		/*
		_responses = new Dictionary<string, List<string>>();
		foreach (Response r in _responses_list)
		{
			_responses.Add(r.id, r.response);
		}
		*/
		_responses = XMLParser.xmlParse("Cindy");
		foreach (string k in _responses.Keys)
		{
			int index = i;
			_question_buttons[i].transform.GetChild(0).GetComponent<Text>().text = k;
			_question_buttons[i].onClick.AddListener(
				() => {
					_question_buttons[index].interactable = false;
					HideQuestions();
					ShowResponse(k);
				}
			);
			i++;
		}
		shouldLoadNextDate = false;
		shouldStartNextDate = false;
		loadingNextDate = false;
		HideQuestions();
	}

	void Update()
	{
		// load the date 
		if (shouldLoadNextDate)
		{
			LoadDate("Cindy");
		}
		
		if (shouldStartNextDate && !loadingNextDate)
		{
			StartDate();
			shouldStartNextDate = false;
		}
	}

	[ContextMenu("Start Date")]
	public void LoadTestDate() { LoadDate("Cindy"); }

	public void LoadDate(string npc) { StartCoroutine(LoadDateCoroutine(npc)); }

	public void StartDate()
	{
		foreach (Button b in _question_buttons)
		{
			b.interactable = true;	
		}
		HideQuestions();
		Introduction();
	}

	void Introduction()
	{
		_typewriter.StartTypewriter(
			new List<string>(new string[] { "Hi! My name's Cindy!", "Let's get started!" }),
			"Cindy",
			null,
			() => { ShowQuestions(); }
		);
	}

	void Goodbye()
	{
		_typewriter.StartTypewriter(
			new List<string>(new string[] { "Well, our time's up", "Nice to meet you!" }),
			"Cindy",
			null,
			() => { Debug.Log("DONE!"); }
		);
	}

	public void ShowQuestions()	{ _question_container.SetActive(true);  }
	public void HideQuestions() { _question_container.SetActive(false); }

	public void ShowResponse(string response)
	{
		_typewriter.StartTypewriter(
			_responses[response], 
			"Cindy", 
			null,
			() => { EndResponse(); } 
		);
		// we can do the whole back and forth thing here
	}

	void EndResponse() 
	{ 
		if (AreRemainingQuestions())
		{
			ShowQuestions();
		}
		else
		{
			Goodbye();
		}
	}

	bool AreRemainingQuestions()
	{
		foreach (Button b in _question_buttons)
		{
			if (b.interactable) { return true; }
		}
		return false;
	}

	private IEnumerator LoadDateCoroutine(string npc)
	{
		shouldLoadNextDate = false;
		loadingNextDate = true;
		yield return null;
		loadingNextDate = false;
		shouldStartNextDate = true;
	}

}
