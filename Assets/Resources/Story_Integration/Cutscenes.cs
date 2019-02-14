using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class Cutscenes
{
	public static List<Cutscene> ParseCutscenes(string json)
	{
		List<Cutscene> cutscenes = new List<Cutscene>();
		
		Dictionary<string, object> jsonRoot = MiniJSON.Json.Deserialize(json) as Dictionary<string, object>;
		List<object> jsonCutscenes = jsonRoot["cutscenes"] as List<object>;
		foreach(Dictionary<string, object> jsonCutscene in jsonCutscenes)
		{
			Cutscene cutscene = new Cutscene();
			cutscene.Name = (string) jsonCutscene["name"];
			cutscene.Place = (string) jsonCutscene["place"];
			cutscene.Priority = (long) jsonCutscene["priority"];
			
			List<object> jsonTriggers = jsonCutscene["triggers"] as List<object>;
			foreach(Dictionary<string, object> jsonTrigger in jsonTriggers)
			{
				Triggerer trigger = new Triggerer();
				
				List<object> jsonConstraints = jsonTrigger["constraints"] as List<object>;
				foreach(Dictionary<string, object> jsonConstraint in jsonConstraints)
				{
					Constraint constraint = new Constraint();
					
					constraint.Parameter = (string) jsonConstraint["parameter"];
					string condition = (string) jsonConstraint["condition"];
					if (condition == "equalTo")
						constraint.Condition = Condition.EqualTo;
					else if (condition == "greaterThan")
						constraint.Condition = Condition.GreaterThan;
					else if (condition == "lessThan")
						constraint.Condition = Condition.LessThan;
					else if (condition == "greaterThanOrEqualTo")
						constraint.Condition = Condition.GreaterThanOrEqualTo;
					else if (condition == "lessThanOrEqualTo")
						constraint.Condition = Condition.LessThanOrEqualTo;
					constraint.Value = jsonConstraint["value"];
					
					trigger.AddConstraint(constraint);
				}
				cutscene.AddTrigger(trigger);
			}
			
			List<object> jsonDialogues = jsonCutscene["dialogues"] as List<object>;
			foreach(Dictionary<string, object> jsonDialogue in jsonDialogues)
			{
				Dialogue dialogue = new Dialogue();
				dialogue.Actor = (string) jsonDialogue["actor"];
				dialogue.Script = (string) jsonDialogue["dialogue"];
				cutscene.AddDialogue(dialogue);
			}
			cutscenes.Add(cutscene);
		}
		
		return cutscenes;
	}

	public static void SupplyValues(List<Cutscene> cutscenes, Dictionary<string, object> values)
	{
		foreach(Cutscene cutscene in cutscenes)
			cutscene.Supply(values);
	}

	public static void LogCutscenes(List<Cutscene> cutscenes)
	{
		foreach(Cutscene cutscene in cutscenes)
		{
			if(GameManager.PRINT_LOGS) 
			{
				Debug.Log(cutscene.Name);
			 	Debug.Log(cutscene.Place);
			 	Debug.Log(cutscene.Priority);
			}
			int index = 1;
			foreach(Triggerer trigger in cutscene.Triggers)
			{
				if(GameManager.PRINT_LOGS) Debug.Log("Trigger " + index++);
				foreach(Constraint constraint in trigger.Constraints)
					if(GameManager.PRINT_LOGS) Debug.Log(constraint.Parameter + " " + constraint.Condition.ToString() + " " + constraint.Value);
			}
			foreach(Dialogue dialogue in cutscene.Dialogues)
			{
				if(GameManager.PRINT_LOGS) Debug.Log(dialogue.Actor + ": " + dialogue.Script);
			}
		}
	}

	public static Cutscene SelectCutscene(List<Cutscene> cutscenes)
	{
		List<Cutscene> playable = new List<Cutscene>();
		foreach(Cutscene cutscene in cutscenes)
			if (!GameManager._gameState.DisplayedCutscenes.Contains(cutscene.Name))
				if (cutscene.Evaluate())
					playable.Add(cutscene);
		Cutscene bestScene = null;
		if (playable.Count > 0)
			bestScene = playable.Aggregate((i1, i2) => i1.Priority > i2.Priority ? i1 : i2);
		return bestScene;
	}
}
