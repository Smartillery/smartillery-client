using UnityEngine;
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

public class ActionQueue {

	private List<Action> _actionQueue;

	private ActionThread _managerThread;
	private Exception _currentException;
	private bool _error;
	private AutoResetEvent _added;
	private static object _actionQueueLock = new object();

	private bool debug = false;

	public ActionQueue()
	{
		_added = new AutoResetEvent(false);
		_actionQueue = new List<Action>();

		_managerThread = new ActionThread(() =>
		  {
			while(true)
			{
				Action action = null;
				lock(_actionQueueLock)
				{
					if(HasException)
					{
						DebugMessage ("ActionQueue: Waiting for Exception to be cleared");
						_added.WaitOne();
					}

					if(!Empty)
					{
						DebugMessage ("ActionQueue: Dequeuing Action");

						action = _actionQueue[0];
						_actionQueue.RemoveAt(0);
					}
					else
					{
						DebugMessage ("ActionQueue: Waiting for new action");
						_added.WaitOne();
					}
				}
				
				if(action != null)
				{
					BuildAction(action);
				}
			}
		});
	}

	public void AddAction(Action action, bool priority = false)
	{
		DebugMessage ("ActionQueue: Action Added");

		if(priority)
		{
			_actionQueue.Insert(0, action);
		}
		else
		{
			_actionQueue.Add(action);
		}
		_added.Set();
	}

	public bool Empty 
	{
		get
		{
			return _actionQueue.Count == 0;
		}
	}

	public bool HasException
	{
		get
		{
			return _currentException != null;
		}
	}

	public void GetException()
	{
		if(HasException)
			throw _currentException;
	}

	public void ClearException()
	{
		_currentException = null;
		_added.Set();
	}

	private void BuildAction(Action action)
	{
		DebugMessage("ActionQueue: Running Action");
		try
		{
			action();
		}
		catch(Exception ex)
		{
			_currentException = ex;
		}
		DebugMessage("ActionQueue: Action Complete");

	}

	private void DebugMessage(string str, params object[] args)
	{
		if(debug)
			Debug.Log(string.Format(str, args));
	}
}
