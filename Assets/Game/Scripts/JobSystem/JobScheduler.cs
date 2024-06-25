using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Scripts.Utils;
using UnityEngine;
using TaskStatus = System.Threading.Tasks.TaskStatus;

namespace Game.Scripts.JobSystem {
    // It can be make alot of better but i don't have to much time to do it :(
    [DefaultExecutionOrder(-1001)]
    public class JobScheduler : Singleton<JobScheduler>, IPullQueue {
        private Queue<IJobData> _jobQueue = new();
        private Coroutine _queueCoroutine;
        
        public IShareJobData Current { private set; get; }

        public JobBuilder QueueTask(Func<Task> task) {
            return new JobBuilder().
                WithScheduler(this).
                WithTask(task);
        }
        
        public JobBuilder QueueTask(Func<Action> task) {
            return new JobBuilder().
                WithScheduler(this).
                WithTask(() => Task.Run(task));
        }
        
        public JobBuilder QueueTasks(Func<Task>[] operations) {
            var builder = new JobBuilder().WithScheduler(this);
            foreach (var operation in operations) 
                builder.WithTask(operation);

            return builder;
        }

        public void PullInQueue(IJobData data) {
            if (IsProcessJob(data.ShareData.Id)) return;
            
            _jobQueue.Enqueue(data);

            _queueCoroutine ??= StartCoroutine(ProcessScheduler());
        }

        public void PullNoneQueue(IJobData data) {
            StartCoroutine(NoQueueProcess(data));
        }
    
        IEnumerator NoQueueProcess(IJobData job) {
            yield return StartCoroutine(CheckConditions(job));
            yield return StartCoroutine(ProcessTasks(job));
        }

        IEnumerator ProcessScheduler() {
            while (_jobQueue.Count > 0) {
                var job = _jobQueue.Dequeue();
                Current = job.ShareData;
                job.ShareData.Callback.OnExecute?.Invoke();
                yield return StartCoroutine(CheckConditions(job));
                yield return StartCoroutine(ProcessTasks(job));
            }

            Current = null;
            _queueCoroutine = null;
        }

        IEnumerator CheckConditions(IJobData job) {
            if (job.Conditions == null) {
                job.ShareData.Callback.OnFailed?.Invoke();
                yield break;
            }
            
            foreach (var condition in job.Conditions) {
                var startTime = Time.time;
        
                while (!condition.Invoke()) {
                    if (Time.time - startTime < job.ExceptionTime) {
                        job.ShareData.Callback.OnFailed?.Invoke();
                        yield break;
                    }
                        
                    yield return null;
                }
            }
        }
        
        IEnumerator ProcessTasks(IJobData job) {
            if (job.Tasks == null) {
                job.ShareData.Callback.OnFailed?.Invoke();
                yield break;
            }

            var isFailed = false;
            foreach (var task in job.Tasks) {
                var processTask = task.Invoke();
                yield return new WaitUntil(() => processTask.IsCompleted);
                if(!isFailed) isFailed = processTask.IsFaulted;
                
                if (job.Conditions == null || !job.Conditions.TrueForAll((condition) => condition())) {
                    job.ShareData.Callback.OnFailed?.Invoke();
                    yield break;
                }
            }
            
            if(!isFailed)
                job.ShareData.Callback.OnSuccess?.Invoke();
            else 
                job.ShareData.Callback.OnFailed?.Invoke();
        }

        public bool IsProcessJob(Guid id) {
            foreach (var data in _jobQueue) {
                if (data.ShareData.Id == id)
                    return true;
            }

            return false;
        }
    }
    
    public struct JobBuilder  {
            
        private JobScheduler _core;
        public JobBuilder WithScheduler(JobScheduler processor) {
            _core = processor;
            return this;
        }
            
        
        private string _name;
        public JobBuilder WithName(string name) {
            this._name = name;
            return this;
        }

        
        private List<Func<bool>> _conditions;
        public JobBuilder WithCondition(Func<bool> condition) {
            _conditions ??= new List<Func<bool>>();
            
            this._conditions.Add(condition);
            return this;
        }
        
            
        private List<Func<Task>> _tasks;
        public JobBuilder WithTask(Func<Task> task) {
            _tasks ??= new List<Func<Task>>();
            
            this._tasks.Add(task);
            return this;
        }

        private int _exceptionTime;
        public JobBuilder WithExceptionTime(int time) {
            _exceptionTime = time;
            return this;
        }

        private JobCallback _callback;

        public JobBuilder WithCallback(Action onExecute = null, Action onSuccess = null, Action onFailed = null) {
            if(onExecute != null)
                this._callback.OnExecute = onExecute;
            if(onSuccess != null)
                this._callback.OnSuccess = onSuccess;
            if(onFailed != null)
                this._callback.OnFailed = onFailed;
            return this;
        }
        

        public void PushInQueue(Guid id) => _core.PullInQueue(new JobData(new ShareJobData(_name, id, _callback),_conditions, _tasks, _exceptionTime));
        public void PushNoneQueue(Guid id) => _core.PullNoneQueue(new JobData(new ShareJobData(_name, id, _callback),_conditions, _tasks, _exceptionTime));
    }

    public interface IJobCallback {
        Action OnExecute { set; get; }
        Action OnSuccess { set; get; }
        Action OnFailed { set;get; }
    }

    public struct JobCallback : IJobCallback {
        public Action OnExecute { set; get; }
        public Action OnSuccess { set;get; }
        public Action OnFailed { set; get; }
    }
    
    public struct JobData : IJobData {
        public ShareJobData ShareData { get; }
        public List<Func<bool>> Conditions { get; }
        public List<Func<Task>> Tasks { get; }
        public int ExceptionTime { get; }

        public JobData(ShareJobData shareData, List<Func<bool>> conditions, List<Func<Task>> tasks, int exceptionTime) {
            ShareData = shareData;
            Conditions = conditions;
            Tasks = tasks;
            ExceptionTime = exceptionTime;
        }
    }
        
    public struct ShareJobData : IShareJobData {
        public string Name { private set; get; }
        public Guid Id { get; }
        public IJobCallback Callback { get; }

        public ShareJobData(string name, Guid id, IJobCallback callback) {
            Name = name;
            Id = id;
            Callback = callback;
        }
    }
    
    public interface IShareJobData {
        public string Name { get; }
        public Guid Id { get; }
        public IJobCallback Callback { get; }
    }
    
    public interface IJobData {
        public ShareJobData ShareData { get; }

        public List<Func<bool>> Conditions { get; }
        public List<Func<Task>> Tasks { get; }
        public int ExceptionTime { get; }
    }
    
    public interface IPullQueue {
        void PullInQueue(IJobData data);
        void PullNoneQueue(IJobData data);
    }
}