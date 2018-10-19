using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Doh18.Base;
using FreshMvvm;
using PropertyChanged;

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

        protected async Task<bool> Do(Func<Task> func, Func<string, Task> onError = null, string loadingMessage = null, [CallerMemberName]string caller = "")
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

            ex.TrackError(null, caller);

            if (onError != null)
                await onError(error);

            return false;
        }

        protected async Task<Result> Do(Func<Task<Result>> func, string loadingMessage = null, [CallerMemberName]string caller = "")
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

            ex.TrackError(null, caller);

            return Result.Fail(error);
        }

        protected async Task<Result<T>> Do<T>(Func<Task<Result<T>>> func, string loadingMessage = null, [CallerMemberName]string caller = "")
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

            ex.TrackError(null, caller);

            return Result.Fail<T>(error);
        }

        #endregion 
    }
}
