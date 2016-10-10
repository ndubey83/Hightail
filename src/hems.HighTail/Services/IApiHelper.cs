using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using hems.HighTail.Models;
using log4net;
using YouSendIt;

namespace hems.HighTail.Services {
    public interface IApiHelper {
        /// <summary>
        /// Helper to make api call, it handles API exception and 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="function"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        ApiResult<T> MakeApiCall<T>(Delegate function, params object[] args);
        /// <summary>
        /// Helper to make api call, it handles API exception and 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="function"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        ApiResult MakeApiCall(Delegate function, params object[] args);
         /// <summary>
        /// Pages through API method call, and returns the full set of results as a single list. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="batchSize"></param>
        /// <param name="fnGetPage"></param>
        /// <returns></returns>
        List<T> PagedApiCalls<T>(int batchSize, Func<int, int, List<T>> fnGetPage);
    }

    public class ApiHelper : IApiHelper {
        public readonly IYouSendItAPI HighTailApi;
        public readonly ILog Log;
        public int MaxTryCount = 25;

        public ApiHelper(
            IYouSendItAPI highTailApi
            , ILog log
            , int? maxTryCount = null) {
            HighTailApi = highTailApi;
            Log = log;
            if (string.IsNullOrEmpty(HighTailApi.AuthToken)) {
                HighTailApi.Login(ConfigurationManager.AppSettings["HighTailEmail"], ConfigurationManager.AppSettings["HighTailPassword"]);
            }
            else {
                HighTailApi = highTailApi;
            }
            if (maxTryCount.HasValue && maxTryCount.Value > 0) {
                MaxTryCount = maxTryCount.Value;
            }
        }

        public ApiResult MakeApiCall(Delegate function, params object[] args) {
            bool success = false;
            var tryCount = 0;
            ApiResult  result = new ApiResult();
            while (tryCount < MaxTryCount && !success) {
                try {
                    Log.DebugFormat(function.Method.Name);
                    function.DynamicInvoke(args);
                    result.Error = false;
                    result.ErrorMessage = "";
                    success = true;
                }
                catch (Exception aex) {
                    result.Error = true;
                    result.ErrorMessage = aex.InnerException.Message;
                    Log.WarnFormat(@"  Failed. Sleeping for {2}ms, and then trying again. # tries: {0:N0}. Details: {1}", tryCount, aex.ToString(), (tryCount + 1) * 500);
                    Thread.Sleep(new TimeSpan(0, 0, 0, 0, (tryCount + 1) * 500));
                }
                if (result.Error && ValidError(result.ErrorMessage)) {
                    success = true;
                }
                tryCount++;
            }
            //adding 1 sec delay so as API call rate limit is maintained
            Thread.Sleep(1000);
            return result;
        }

        public ApiResult<T> MakeApiCall<T>(Delegate function, params object[] args) {
            bool success = false;
            var tryCount = 0;
            T returnValue = default(T);
            ApiResult<T> result = new ApiResult<T>();
            while (tryCount < MaxTryCount && !success) {
                try {
                    Log.DebugFormat(function.Method.Name);
                    returnValue = (T)function.DynamicInvoke(args);
                    result.Response = returnValue;
                    result.Error = false;
                    result.ErrorMessage = "";
                    success = true;
                }
                catch (Exception aex) {
                    result.Error = true;
                    result.ErrorMessage = aex.InnerException.Message;
                    Log.WarnFormat(@"  Failed. Sleeping for {2}ms, and then trying again. # tries: {0:N0}. Details: {1}", tryCount, aex.ToString(), (tryCount + 1) * 500);
                    Thread.Sleep(new TimeSpan(0, 0, 0, 0, (tryCount + 1) * 500));
                }
                if (result.Error && ValidError(result.ErrorMessage)) {
                    success = true;
                }
                tryCount++;
            }
            //adding 1 sec delay so as API call rate limit is maintained
            Thread.Sleep(1000);
            return result;
        }


        public List<T> PagedApiCalls<T>(int batchSize, Func<int, int, List<T>> fnGetPage) {
            var results = new List<T>();

            int lastBatchCount = 0;

            if (Log != null)
                Log.DebugFormat(
                    "Loading all pages of ({0})items from API. Expected count: unknown, pageSize: {1:N0}..."
                    , typeof(T).Name, batchSize
                );

            do {
                var itemsBatch = fnGetPage(results.Count, batchSize);
                lastBatchCount = itemsBatch.Count;
                results.AddRange(itemsBatch);
                if (Log != null)
                    Log.DebugFormat(
                        "Loaded page of {0:N0} ({2})items from API. Loaded so far: {1:N0}, total expected: unkown."
                        , lastBatchCount, results.Count, typeof(T).Name
                    );
            } while (lastBatchCount > 0 && lastBatchCount >= batchSize );


            return results;
        }

       public bool ValidError(string errorMessage) {
           string[] validError = { "This file has been deleted."};
           return validError.Contains(errorMessage);
       }
    }

}
