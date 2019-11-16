using UnityEngine;
using System.Collections;

public class CoroutineManager : MonoBehaviour {

	static CoroutineManager _instance = null;
	
	public static CoroutineManager instance
	{
		get
		{
			if(!_instance)
			{
				_instance = FindObjectOfType(typeof (CoroutineManager)) as CoroutineManager;
			}
			
			if(!_instance)
			{
				var obj = new GameObject("CoroutineManager");
				_instance = obj.AddComponent<CoroutineManager>();
			}
			
			return _instance;
		}
	}
	
	void onApplicationQuit()
	{
		_instance = null;
	}
	
}

public class Job
{
	public event System.Action<bool> onComplete;
	
	private bool _running;
	
	public bool running{ get{return _running;}}

	private IEnumerator _coroutine;
	
	private bool _jobWasKilled = false;
	
	public Job(IEnumerator coroutine): this(coroutine, true)
	{}

	public Job(IEnumerator coroutine, bool startNow)
	{
		_coroutine = coroutine;
		
		if(startNow) Start ();
	}
	
	public static Job Make(IEnumerator jobToStart)
	{
		return new Job(jobToStart);		
	}
	
	public static Job Make(IEnumerator jobToStart, bool startNow)
	{
		return new Job(jobToStart, startNow);
	}
	
	public void Kill()
	{
		_jobWasKilled = true;
		_running = false;
	}
	
	public void Start()
	{
		_running = true;
		CoroutineManager.instance.StartCoroutine(DoJob());
	}
	
	public IEnumerator StartAsCoroutine()
	{
		_running = true;
		yield return CoroutineManager.instance.StartCoroutine(DoJob ());
	}
	
	public IEnumerator DoJob()
	{
		yield return null;
		
		while(_running)
		{
			if(_coroutine.MoveNext())
			{
				yield return _coroutine.Current;
			}
			else
			{
				_running = false;
			}
		}
		
		if(onComplete != null) onComplete(_jobWasKilled);
	}
}