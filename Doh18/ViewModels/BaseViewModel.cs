using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Doh18.Base;
using FreshMvvm;
using PropertyChanged;
using Constants = Doh18.Base.Constants;

namespace Doh18.ViewModels
{
    public class BaseViewModel : FreshBasePageModel
    {
        #region Properties

        public bool IsBusy { get; set; }
        [DependsOn(nameof(IsBusy))]
        public bool IsNotBusy => !IsBusy;

        #endregion

        #region Lifecycle

        public BaseViewModel()
        {
            
        }

        #endregion

        #region Methods

        protected async Task<bool> Do(Func<Task> func, Func<string, Task> onError = null, string loadingMessage = null, string caller = null)
        {
            string error = null;
            Exception ex = null;

            try
            {
                if (IsBusy)
                    return false;

                IsBusy = true;
                if (loadingMessage != null)
                    UserDialogs.Instance.ShowLoading(loadingMessage);

                await func();
            }
            catch (OperationCanceledException e)
            {
                ex = e;
                error = "Time out!";
            }        
            catch (Exception e)
            {
                ex = e;
                error = e.Message;
            }
            finally
            {
                IsBusy = false;
                if (loadingMessage != null)
                    UserDialogs.Instance.HideLoading();
            }

            if (ex == null)
                return true;

            Ach.TrackError(ex, new Dictionary<string, string>
            {
                { Constants.Where, $"{GetType().Name}: {caller ?? "unknown"}" }
            });

            if (onError != null)
                await onError(error);

            return false;
        }

        protected async Task<Result> Do(Func<Task<Result>> func, string loadingMessage = null, string caller = null)
        {
            string error = null;
            Exception ex = null;
            var result = new Result();

            try
            {
                if (IsBusy)
                    return Result.Fail(nameof(IsBusy));

                IsBusy = true;
                if (loadingMessage != null)
                    UserDialogs.Instance.ShowLoading(loadingMessage);

                result = await func();
            }
            catch (OperationCanceledException e)
            {
                ex = e;
                error = "Time out!";
            }           
            catch (Exception e)
            {
                ex = e;
                error = e.Message;
            }
            finally
            {
                IsBusy = false;
                if (loadingMessage != null)
                    UserDialogs.Instance.HideLoading();
            }

            if (ex == null)
                return result.IsFailure ? Result.Fail(result.Error) : result;

            Ach.TrackError(ex, new Dictionary<string, string>
            {
                { Constants.Where, $"{GetType().Name}: {caller ?? "unknown"}" }
            });

            return Result.Fail(error);
        }

        protected async Task<Result<T>> Do<T>(Func<Task<Result<T>>> func, string loadingMessage = null, string caller = null)
        {
            string error = null;
            Exception ex = null;
            var result = new Result<T>();

            try
            {
                if (IsBusy)
                    return Result.Fail<T>(nameof(IsBusy));

                IsBusy = true;
                if (loadingMessage != null)
                    UserDialogs.Instance.ShowLoading(loadingMessage);

                result = await func();
            }
            catch (OperationCanceledException e)
            {
                ex = e;
                error = "Time out";
            }            
            catch (Exception e)
            {
                ex = e;
                error = e.Message;
            }
            finally
            {
                IsBusy = false;
                if (loadingMessage != null)
                    UserDialogs.Instance.HideLoading();
            }

            if (ex == null)
                return result.IsFailure ? Result.Fail<T>(result.Error) : result;

            Ach.TrackError(ex, new Dictionary<string, string>
            {
                { Constants.Where, $"{GetType().Name}: {caller ?? "unknown"}" }
            });

            return Result.Fail<T>(error);
        }

        #endregion 
    }
}
