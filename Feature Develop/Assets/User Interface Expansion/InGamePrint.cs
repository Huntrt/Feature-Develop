using System.Collections;
using UnityEngine;

public class InGamePrint : MonoBehaviour
{
	//Turn this script into singleton
	public static InGamePrint i; void Awake() {i = this;}
	//Current log data
	[SerializeField][TextArea] string log;
	[SerializeField][Tooltip("How many line the log could has")] int lineLimit;
	[SerializeField][Tooltip("How long until auto clear all line (-1 for never)")] float fadeDuration; 
	int lineCount;
	//UI to display log
	public TMPro.TextMeshProUGUI display; 

	void Start()
	{
		//Send an error if there is no text display
		if(display == null) UnityEngine.Debug.LogError("The In Game Print don't has any text display");
	}

	public void Print(object info)
	{
		//Adding info to log text
		log += info + "\n";
		//Increase one line
		lineCount++;
		//Refresh fading count down when ever print an new line
		if(fadeDuration >= 0) {StopCoroutine("Fading"); StartCoroutine("Fading");}
		//When reached line limit then remove the last line
		if(lineCount > lineLimit) RemoveLog();
		UpdateLog();
	}

	void RemoveLog()
	{
		//Delete the first line
		log = log.Substring(log.IndexOf("\n")+1);
		//Decrease one line
		lineCount--;
		UpdateLog();
	}

	public void ClearLog()
	{
		//Remove all the log data
		log = "";
		//Remove all the line count
		lineCount -= lineCount;
		UpdateLog();
	}

	IEnumerator Fading()
	{
		//Wait for the time to fade
		yield return new WaitForSeconds(fadeDuration);
		//Remov the line
		RemoveLog();
		//Repeat if still has line to remove
		if(lineCount > 0) StartCoroutine("Fading");
	}

	//Update log text UI
	void UpdateLog() {display.text = log;}
}