using System;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UsefulScripts
{
    public class AsyncTestClass
    {
        private CancellationTokenSource cts;

        //Methods
        //Try Catch async task
        private async void LoadNewScene(string sceneName)
        {
            if (cts == null)
            {
                cts = new CancellationTokenSource();
                try
                {
                    await PerformSceneLoading(cts.Token, sceneName);
                }
                catch (OperationCanceledException ex)
                {
                    if (ex.CancellationToken == cts.Token)
                    {
                        //Perform operation after cancelling
                        Debug.Log("Task cancelled");
                    }
                }
                finally
                {
                    cts.Cancel();
                    cts = null;
                }
            }
            else
            {
                //Cancel Previous token
                cts.Cancel();
                cts = null;
            }
        }
        //Actual Scene loading
        private async Task PerformSceneLoading(CancellationToken token, string sceneName)
        {
            token.ThrowIfCancellationRequested();
            if (token.IsCancellationRequested)
                return;

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;
            while (true)
            {
                token.ThrowIfCancellationRequested();
                if (token.IsCancellationRequested)
                    return;
                if (asyncOperation.progress >= 0.9f)
                    break;
            }
            asyncOperation.allowSceneActivation = true;
            cts.Cancel();
            token.ThrowIfCancellationRequested();
            if (token.IsCancellationRequested)
                return;
        }
    }
}