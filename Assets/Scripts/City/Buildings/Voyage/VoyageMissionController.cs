using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoyageMissionController : MonoBehaviour
{
	[SerializeField] private StatusMission status = StatusMission.NotOpen;
	private Mission mission;
	private int numMission = 0;

	public void SetData(Mission mission, int numMission, StatusMission newStatus)
	{
		this.mission    = mission;
		this.numMission = numMission;
		this.status     = newStatus;
	}

	public void SetStatus(StatusMission newStatus)
	{
		this.status = newStatus;
	}
	
	public void OpenPanelInfo()
	{
		VoyageControllerSctipt.Instance.ShowInfo(this, this.mission.WinReward, this.status);
	}

	public void OpenMission()
	{
		if(status == StatusMission.Open)
		{
			VoyageControllerSctipt.Instance.OpenMission(this.mission);
		}
	}
}