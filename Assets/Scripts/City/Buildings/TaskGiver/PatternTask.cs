using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[CreateAssetMenu(fileName = "PatternTasks", menuName = "Custom ScriptableObject/PatternTasks", order = 55)]
public class PatternTask : ScriptableObject{
	public List<Task> tasks = new List<Task>();

	public Task GetSimpleTask(){
		List<Task> workTasks = tasks.FindAll(x => (x.rating <= 4));
		Task result = (Task) (workTasks[Random.Range(0, workTasks.Count)]).Clone();
		result.Reward = GetRandomReward(result.rating); 
		return result;
	}  
	public Task GetSpecialTask(){
		List<Task> workTasks = tasks.FindAll(x => (x.rating > 4));
		Task result = (Task) (workTasks[Random.Range(0, workTasks.Count)]).Clone();
		result.Reward = GetRandomReward(result.rating); 
		return result;
	} 
	public Task GetRandomTask(){
		Task result = (Task) (tasks[Random.Range(0, tasks.Count)]).Clone();
		result.Reward = GetRandomReward(result.rating); 
		return result;
	}
	[ContextMenu("Check equals ID")]
	private void CheckEquealsID(){
		for(int i = 0; i < tasks.Count - 1; i++){
			for(int j = i + 1; j < tasks.Count; j++){
				if(tasks[i].ID == tasks[j].ID)
					Debug.Log("Tasks have equals ID = " + tasks[i].ID.ToString() + ", name task = '" + tasks[i].name + "' and task = '" + tasks[j].name+ "'");
			}
		}
	}
	public List<RewardForTaskFromRating> rewardForTaskFromRating = new List<RewardForTaskFromRating>();
	public Resource GetRandomReward(int rating){
		List<RewardForTaskFromRating> listReward = rewardForTaskFromRating.FindAll(x => (x.rating == rating));
		return (listReward[Random.Range(0, listReward.Count)]).GetReward();
	}
}

[System.Serializable]
public class RewardForTaskFromRating{
	public int rating;
	public Resource res;
	public float delta = 0.25f;
	public Resource GetReward(){
		return (new Resource(res.Name, res.Amount) * UnityEngine.Random.Range(1 - delta, 1 + delta));
	}
}
