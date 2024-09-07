namespace SimpleTaskManagerApp.Models;

public class BlackListController
{
	public List<string> BlackList { get; init; }

	public BlackListController()
	{
		BlackList = new List<string>();
	}

	public bool CheckProcess(string ProcessName)
	{
		foreach (var p in BlackList)
			if (p == ProcessName)
				return true;

		return false;
	}

	public void AddProcess(string ProcessName)
	{
		if (!CheckProcess(ProcessName))
			BlackList.Add(ProcessName);
	}

	public void RemoveProcess(string ProcessName)
	{
		if (CheckProcess(ProcessName))
			BlackList.Remove(ProcessName);
	}

	public string GetLast(string FullProcessName)
	{
		var res = FullProcessName.Split('\\').Last();

		if (res.EndsWith(".exe"))
			return res.Split('.').First();

		return res;
	}

}
