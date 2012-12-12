using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace Practic.Framework.Support
{
    public class BackgroundOpStack 
    {

        private readonly Stack<EncapsulatedAction> _runnableActions = new Stack<EncapsulatedAction>();
        private readonly Thread _workerThread;

        private volatile bool _isRunning;

        public bool IsRunning
        {
            get { return _isRunning; }
            set { _isRunning = value; }
        }

        private static BackgroundOpStack _instance;
        public static BackgroundOpStack Instance
        {
            get { return _instance ?? (_instance = new BackgroundOpStack()); }
        }

        private BackgroundOpStack()
        {
            _workerThread = new Thread(ImYourWorkerBitchYo);
            Application.Current.Exit += ShutDown;
            _workerThread.Start();
        }

        private void ShutDown(object sender, EventArgs e)
        {
            lock (_instance)
            {
                IsRunning = false;
                _runnableActions.Clear();
                Monitor.Pulse(_instance);
            }
        }

        public void Enqueue(Action<object> action, object myObj)
        {
            if (action != null)
            lock (_instance)
            {
                _runnableActions.Push(new EncapsulatedAction(action,myObj));
                Monitor.Pulse(_instance);
            }
        }

        private void ImYourWorkerBitchYo()
        {
            Thread.CurrentThread.Name = "WorkerThread_" + DateTime.Now.ToString("{0:f}");
            IsRunning = true;
            var privateStack = new Stack<EncapsulatedAction>();

            while (IsRunning)
            {
                lock (_instance)
                {

                    if (_runnableActions.Count > 0)
                    {
                        privateStack.Push(_runnableActions.Pop());
                    }
                    else
                    {
                        Monitor.Wait(_instance);                        
                    }
                    
                    if(!IsRunning)
                        return;
                }
                
                while (privateStack.Count > 0)
                {
                    var op = privateStack.Pop();
                    op.Run();
                    Thread.Sleep(0); // OK that yields
                }
            }
        }


        private sealed class EncapsulatedAction
        {
            private readonly Action<object> _action;
            private readonly object _state;

            public EncapsulatedAction(Action<object> action, object state)
            {
                _action = action;
                _state = state;
            }
            
            public void Run()
            {
                _action(_state);
            }
        }
    }
}
