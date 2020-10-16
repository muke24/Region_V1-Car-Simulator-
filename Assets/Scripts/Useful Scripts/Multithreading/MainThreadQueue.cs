using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// A queue of commands to execute on the main thread. Each command function (e.g. NewGameObject)
/// takes a Result parameter that initially has no value but gets a value after the command
/// executes.
/// </summary>
/// <author>http://JacksonDunstan.com/articles/3930</author>
/// <license>MIT</license>
public class MainThreadQueue
{
	/// <summary>
	/// Result of a queued command. Will have a Value when it IsReady.
	/// </summary>
	public class Result<T>
	{
		private T value;
		private bool hasValue;
		private AutoResetEvent readyEvent;

		public Result()
		{
			readyEvent = new AutoResetEvent(false);
		}

		/// <summary>
		/// Result value. Blocks until IsReady is true.
		/// </summary>
		public T Value
		{
			get
			{
				readyEvent.WaitOne();
				return value;
			}
		}

		/// <summary>
		/// Check if the result value is ready.
		/// </summary>
		public bool IsReady
		{
			get
			{
				return hasValue;
			}
		}

		/// <summary>
		/// Set the result value and flag it as ready.
		/// This is meant to be called by MainThreadQueue only.
		/// </summary>
		/// <param name="value">
		/// The result value
		/// </param>
		public void Ready(T value)
		{
			this.value = value;
			hasValue = true;
			readyEvent.Set();
		}

		/// <summary>
		/// Reset the result so it can be used again.
		/// </summary>
		public void Reset()
		{
			value = default(T);
			hasValue = false;
		}
	}

	/// <summary>
	/// A result with no value (i.e. for a function returning "void")
	/// </summary>
	public class Result
	{
		private bool hasValue;
		private AutoResetEvent readyEvent;

		public Result()
		{
			readyEvent = new AutoResetEvent(false);
		}

		/// <summary>
		/// If the command has been executed
		/// </summary>
		public bool IsReady
		{
			get
			{
				return hasValue;
			}
		}

		/// <summary>
		/// Mark the result as ready to indicate that the command has been executed.
		/// </summary>
		public void Ready()
		{
			hasValue = true;
			readyEvent.Set();
		}

		/// <summary>
		/// Blocks until IsReady is true
		/// </summary>
		public void Wait()
		{
			readyEvent.WaitOne();
		}

		/// <summary>
		/// Reset the result so it can be used again.
		/// </summary>
		public void Reset()
		{
			hasValue = false;
		}
	}

	/// <summary>
	/// Types of commands
	/// </summary>
	private enum CommandType
	{
		/// <summary>
		/// Instantiate a new GameObject
		/// </summary>
		NewGameObject,

		/// <summary>
		/// Get a GameObject's transform
		/// </summary>
		GetTransform,

		/// <summary>
		/// Set a Transform's position
		/// </summary>
		SetPosition
	}

	/// <summary>
	/// Base class of all command types
	/// </summary>
	private abstract class BaseCommand
	{
		/// <summary>
		/// Type of the command
		/// </summary>
		public CommandType Type;
	}

	/// <summary>
	/// Command object for instantiating a GameObject
	/// </summary>
	private class NewGameObjectCommand : BaseCommand
	{
		/// <summary>
		/// Name of the GameObject
		/// </summary>
		public string Name;

		/// <summary>
		/// Result of the command: the newly-instantiated GameObject
		/// </summary>
		public Result<GameObject> Result;

		public NewGameObjectCommand()
		{
			Type = CommandType.NewGameObject;
		}
	}

	/// <summary>
	/// Command object for getting a GameObject's transform
	/// </summary>
	private class GetTransformCommand : BaseCommand
	{
		/// <summary>
		/// GameObject to get the Transform for
		/// </summary>
		public GameObject GameObject;

		/// <summary>
		/// Result of the command: the GameObject's transform.
		/// </summary>
		public Result<Transform> Result;

		public GetTransformCommand()
		{
			Type = CommandType.GetTransform;
		}
	}

	/// <summary>
	/// Set a Transform's position
	/// </summary>
	private class SetPositionCommand : BaseCommand
	{
		/// <summary>
		/// Transform to set the position of
		/// </summary>
		public Transform Transform;

		/// <summary>
		/// Position to set to the Transform
		/// </summary>
		public Vector3 Position;

		/// <summary>
		/// Result of the command: no value
		/// </summary>
		public Result Result;

		public SetPositionCommand()
		{
			Type = CommandType.SetPosition;
		}
	}

	// Pools of command objects used to avoid creating more than we need
	private Stack<NewGameObjectCommand> newGameObjectPool;
	private Stack<GetTransformCommand> getTransformPool;
	private Stack<SetPositionCommand> setPositionPool;

	// Queue of commands to execute
	private Queue<BaseCommand> commandQueue;

	// Stopwatch for limiting the time spent by Execute
	private Stopwatch executeLimitStopwatch;

	/// <summary>
	/// Create the queue. It initially has no commands.
	/// </summary>
	public MainThreadQueue()
	{
		newGameObjectPool = new Stack<NewGameObjectCommand>();
		getTransformPool = new Stack<GetTransformCommand>();
		setPositionPool = new Stack<SetPositionCommand>();
		commandQueue = new Queue<BaseCommand>();
		executeLimitStopwatch = new Stopwatch();
	}

	/// <summary>
	/// Get an object from a pool or create a new one if none are available.
	/// This function is thread-safe.
	/// </summary>
	/// <returns>
	/// An object from the pool or a new instance
	/// </returns>
	/// <param name="pool">
	/// Pool to get from
	/// </param>
	/// <typeparam name="T">
	/// Type of pooled object
	/// </typeparam>
	private static T GetFromPool<T>(Stack<T> pool)
		where T : new()
	{
		lock (pool)
		{
			if (pool.Count > 0)
			{
				return pool.Pop();
			}
		}
		return new T();
	}

	/// <summary>
	/// Return an object to a pool.
	/// This function is thread-safe.
	/// </summary>
	/// <param name="pool">
	/// Pool to return to
	/// </param>
	/// <param name="obj">
	/// Object to return
	/// </param>
	/// <typeparam name="T">
	/// Type of pooled object
	/// </typeparam>
	private static void ReturnToPool<T>(Stack<T> pool, T obj)
	{
		lock (pool)
		{
			pool.Push(obj);
		}
	}

	/// <summary>
	/// Queue a command. This function is thread-safe.
	/// </summary>
	/// <param name="cmd">
	/// Command to queue
	/// </param>
	private void QueueCommand(BaseCommand cmd)
	{
		lock (commandQueue)
		{
			commandQueue.Enqueue(cmd);
		}
	}

	/// <summary>
	/// Queue a command to instantiate a GameObject
	/// </summary>
	/// <param name="name">
	/// Name of the GameObject. Must not be null.
	/// </param>
	/// <param name="result">
	/// Result to be filled in when the command executes. Must not be null.
	/// </param>
	public void NewGameObject(
		string name,
		Result<GameObject> result)
	{
		Assert.IsTrue(name != null);
		Assert.IsTrue(result != null);

		result.Reset();
		NewGameObjectCommand cmd = GetFromPool(newGameObjectPool);
		cmd.Name = name;
		cmd.Result = result;
		QueueCommand(cmd);
	}

	/// <summary>
	/// Queue a command to get a GameObject's transform
	/// </summary>
	/// <param name="go">
	/// GameObject to get the transform from. Must not be null.
	/// </param>
	/// <param name="result">
	/// Result to be filled in when the command executes. Must not be null.
	/// </param>
	public void GetTransform(
		GameObject go,
		Result<Transform> result)
	{
		Assert.IsTrue(go != null);
		Assert.IsTrue(result != null);

		result.Reset();
		GetTransformCommand cmd = GetFromPool(getTransformPool);
		cmd.GameObject = go;
		cmd.Result = result;
		QueueCommand(cmd);
	}

	/// <summary>
	/// Queue a command to set a Transform's position
	/// </summary>
	/// <param name="transform">
	/// Transform to set the position of
	/// </param>
	/// <param name="position">
	/// Position to set to the transform
	/// </param>
	/// <param name="result">
	/// Result to be filled in when the command executes. Must not be null.
	/// </param>
	/// <param name="result">
	/// Result to be filled in when the command executes. Must not be null.
	/// </param>
	public void SetPosition(
		Transform transform,
		Vector3 position,
		Result result)
	{
		Assert.IsTrue(transform != null);
		Assert.IsTrue(result != null);

		result.Reset();
		SetPositionCommand cmd = GetFromPool(setPositionPool);
		cmd.Transform = transform;
		cmd.Position = position;
		cmd.Result = result;
		QueueCommand(cmd);
	}

	/// <summary>
	/// Execute commands until there are none left or a maximum time is used
	/// </summary>
	/// <param name="maxMilliseconds">
	/// Maximum number of milliseconds to execute for. Must be positive.
	/// </param>
	public void Execute(int maxMilliseconds = int.MaxValue)
	{
		Assert.IsTrue(maxMilliseconds > 0);

		// Process commands until we run out of time
		executeLimitStopwatch.Reset();
		executeLimitStopwatch.Start();
		while (executeLimitStopwatch.ElapsedMilliseconds < maxMilliseconds)
		{
			// Get the next queued command, but stop if the queue is empty
			BaseCommand baseCmd;
			lock (commandQueue)
			{
				if (commandQueue.Count == 0)
				{
					break;
				}
				baseCmd = commandQueue.Dequeue();
			}

			// Process the command. These steps are followed for each command:
			// 1. Extract the command's fields
			// 2. Reset the command's fields
			// 3. Do the work
			// 4. Return the command to its pool
			// 5. Make the result ready
			switch (baseCmd.Type)
			{
				case CommandType.NewGameObject:
					{
						// Extract the command's fields
						NewGameObjectCommand cmd = (NewGameObjectCommand)baseCmd;
						string name = cmd.Name;
						Result<GameObject> result = cmd.Result;

						// Reset the command's fields
						cmd.Name = null;
						cmd.Result = null;

						// Return the command to its pool
						ReturnToPool(newGameObjectPool, cmd);

						// Do the work
						GameObject go = new GameObject(name);

						// Make the result ready
						result.Ready(go);
						break;
					}
				case CommandType.GetTransform:
					{
						// Extract the command's fields
						GetTransformCommand cmd = (GetTransformCommand)baseCmd;
						GameObject go = cmd.GameObject;
						Result<Transform> result = cmd.Result;

						// Reset the command's fields
						cmd.GameObject = null;
						cmd.Result = null;

						// Return the command to its pool
						ReturnToPool(getTransformPool, cmd);

						// Do the work
						Transform transform = go.transform;

						// Make the result ready
						result.Ready(transform);
						break;
					}
				case CommandType.SetPosition:
					{
						// Extract the command's fields
						SetPositionCommand cmd = (SetPositionCommand)baseCmd;
						Transform transform = cmd.Transform;
						Vector3 position = cmd.Position;
						Result result = cmd.Result;

						// Reset the command's fields
						cmd.Transform = null;
						cmd.Position = Vector3.zero;
						cmd.Result = null;

						// Return the command to its pool
						ReturnToPool(setPositionPool, cmd);

						// Do the work
						transform.position = position;

						// Make the result ready
						result.Ready();
						break;
					}
			}
		}
	}
}