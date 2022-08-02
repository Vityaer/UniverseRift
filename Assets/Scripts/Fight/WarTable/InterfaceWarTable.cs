using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IWorkWithWarTable{
	void RegisterOnOpenCloseWarTable();
	void UnregisterOnOpenCloseWarTable();
	void Change(bool isOpen);
}
