// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using System.Linq;
using System.Threading;

public class ActionThread
{
	public Exception Exception;

	private Thread _thread;
	private Action _action;

	public ActionThread(Action action)
	{
		_action = action;

		_thread = new Thread(Run);
		_thread.Start();
	}

	private void Run()
	{
		try
		{
			_action();
		}
		catch(Exception ex)
		{
			Exception = ex;
		}
	}

	public bool Complete
	{
		get {
			return _thread.ThreadState == ThreadState.Aborted || _thread.ThreadState == ThreadState.Stopped;
		}
	}

	public void Wait()
	{
		if(!Complete)
			_thread.Join ();

		if(Exception != null)
			throw Exception;
	}
}

