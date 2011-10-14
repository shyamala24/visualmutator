namespace CommonUtilityInfrastructure.Threading
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using CommonUtilityInfrastructure.WpfUtils;
    using CommonUtilityInfrastructure.WpfUtils.Messages;

    using log4net;
    public interface IThreading
    {
        Task ScheduleAsync<T>(Func<T> onBackground, Action<T> onGui, Action onException = null, Action onFinally = null);

        void InvokeOnGui(Action onGui);

        Action<T> ActionOnGui<T>(Action<T> onGui);

        Task ScheduleAsync(Action onBackground, Action onGui = null, Action onException = null, Action onFinally = null);

        void ContinueOnGuiWhenAll(ICollection<Task> tasks, Action onGui);

        Task ScheduleAsyncSequential(Action onBackground, Action onGui = null, 
                                                     Action onException = null, Action onFinally = null);

        Task ScheduleAsyncSequential<T>(Func<T> onBackground, 
                                                        Action<T> onGui, Action onException = null, Action onFinally = null);
    }
    public class Threading : IThreading
    {
        private readonly IExecute _execute;

        private readonly IMessageService _messageService;
        private ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Threading(
            IExecute execute,
            IMessageService messageService
            )
        {
            _execute = execute;
            _messageService = messageService;
        }
        public void InvokeOnGui( Action onGui)
        {
            _execute.OnUIThread(onGui);
        }
        public Action<T> ActionOnGui<T>(Action<T> onGui)
        {
            return param =>
            {
                _execute.OnUIThread(() => onGui(param));
            };
        }
        public void ContinueOnGuiWhenAll(ICollection<Task> tasks, Action onGui)
        {
            if (tasks.Count != 0)
            {
                new TaskFactory(_execute.GuiScheduler).ContinueWhenAll(tasks.ToArray(), t2 => onGui());
            }
            
        }

        public Task ScheduleAsync(Action onBackground, Action onGui = null,
            Action onException = null, Action onFinally = null )
        {
            return new TaskFactory(_execute.ThreadPoolScheduler)
                .StartNew(onBackground)
                .ContinueWith(prev =>
                {
                    ContinueWithMethod(prev.Exception, onGui, onException, onFinally);

                }, _execute.GuiScheduler);

        }

        public Task ScheduleAsync<T>(Func<T> onBackground, Action<T> onGui, 
            Action onException = null, Action onFinally = null)
        {
            return new TaskFactory(_execute.ThreadPoolScheduler)
                .StartNew(onBackground)
                .ContinueWith(prev =>
                {
                    ContinueWithMethod(prev.Exception, () => onGui(prev.Result), onException, onFinally);

                }, _execute.GuiScheduler);
        
        }

        public Task ScheduleAsyncSequential(Action onBackground, Action onGui = null, 
            Action onException = null, Action onFinally = null)
        {
            return new TaskFactory(_execute.LimitedThreadPoolScheduler(1))
                .StartNew(onBackground)
                .ContinueWith(prev =>
                {
                    ContinueWithMethod(prev.Exception, onGui, onException, onFinally);

                }, _execute.GuiScheduler);

        }

        public Task ScheduleAsyncSequential<T>(Func<T> onBackground, 
            Action<T> onGui, Action onException = null, Action onFinally = null)
        {
            return new TaskFactory(_execute.LimitedThreadPoolScheduler(1))
                .StartNew(onBackground)
                .ContinueWith(prev =>
                {
                    ContinueWithMethod(prev.Exception, () => onGui(prev.Result), onException, onFinally);

                }, _execute.GuiScheduler);

        }


        public void ContinueWithMethod(AggregateException aggregateException, Action onGuiPacked, Action onException, Action onFinally)
        {
            
            try
            {
                if (aggregateException != null)
                {
                    var prevException = aggregateException.InnerException;
                    if (onException != null)
                    {
                        onException();
                    }
                    var nonFatalWrapped = prevException as NonFatalWrappedException;
                    if (nonFatalWrapped != null)
                    {
                        _messageService.ShowError(nonFatalWrapped.InnerException, _log);
                    }
                    else
                    {
                        _messageService.ShowFatalError(prevException, _log);
                    }
                    
                }
                else
                {
                    if (onGuiPacked != null)
                    {
                        onGuiPacked();
                    }
                    
                }
            }
            catch (Exception e)
            {
                _messageService.ShowFatalError(e, _log);
            }
            finally
            {
                try
                {
                    if (onFinally != null)
                    {
                        onFinally();
                    }
                }
                catch (Exception e)
                {
                    _messageService.ShowFatalError(e, _log);
                }
            }

        }
    }
}