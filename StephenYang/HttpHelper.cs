using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace StephenYang
{
    public class HttpHelper
    {
        public static string URL { set; get; }

        /// <summary>
        /// 超时时间间隔 
        /// </summary>
        private static string _Timeout = WebConfigurationManager.AppSettings["Timeout"];

        /// <summary>
        /// 访问api接口密钥
        /// </summary>
        private static string _validationKey = WebConfigurationManager.AppSettings["ValidationKey"];


        /// <summary>
        /// 根据枚举初始化
        /// </summary>
        /// <param name="serverUrlType"></param>
        /// <returns></returns>
        private static HttpClient PrepareHttpClient()
        {
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
            var _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(GetBaseUrl())
            };
            if (!string.IsNullOrEmpty(_Timeout))
            {
                long ticks = 0;
                if (long.TryParse(_Timeout, out ticks))
                {
                    _httpClient.Timeout = new TimeSpan(ticks);
                }
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _validationKey);
            return _httpClient;
        }

        /// <summary>
        /// 获取基地址
        /// </summary>
        /// <returns></returns>
        private static string GetBaseUrl()
        {
            return URL;
        }

        /// <summary>
        /// 显示BadRequest错误信息
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static async Task HandlerBadRequest(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var s = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!string.IsNullOrEmpty(s))
                {
                    JToken jToken = null;
                    if (JObject.Parse(s).TryGetValue("Message", out jToken))
                    {
                        s = jToken.ToObject<string>();
                    }
                }
                var message = string.Format("{0} {1} : {2}", response.ReasonPhrase, (int)response.StatusCode, s);
                response.Dispose();
                throw new HttpRequestException(message);
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError) //接收服务器的异常并处理，此种异常暂定为要完整接收服务端的异常
            {
                var ServerException = await response.Content.ReadAsAsync<Exception>().ConfigureAwait(false); //获取服务端的异常在客户端进行处理，后续处理待完善。
                response.Dispose();
                throw ServerException; //服务端的异常完整的抛出
            }
            else if (!response.IsSuccessStatusCode)
            {
                response.EnsureSuccessStatusCode();
            }
        }

        /// <summary>
        /// 查询对象转换为get请求的url参数
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private static string ObjectToUrlParms(Object o)
        {
            StringBuilder sb = new StringBuilder("/?");
            Type t = o.GetType();
            PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in pi)
            {
                MethodInfo mi = p.GetGetMethod();
                if (mi != null && mi.IsPublic)
                {
                    object obj = mi.Invoke(o, new Object[] { });
                    //增加时间格式的处理
                    if (p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(Nullable<DateTime>))
                        sb.AppendFormat("&{0}={1}", p.Name, obj == null ? "" : string.Format("{0:yyyy-MM-dd HH:mm:ss}", obj));
                    else
                        sb.AppendFormat("&{0}={1}", p.Name, obj == null ? "" : obj.ToString());
                }
            }
            return sb.ToString();
        }

        #region GET

        public static async Task<string> doGet(string urlParas)
        {
            try
            {
                var _httpClient = PrepareHttpClient();
                var response = await _httpClient.GetAsync(urlParas).ConfigureAwait(false);
                await HandlerBadRequest(response).ConfigureAwait(false);
                string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// get请求传人Input参数对象、调用的服务器url枚举返回字符串
        /// </summary>
        /// <typeparam name="Input">传人的参数对象类型</typeparam>
        /// <param name="serverUrlType">服务器url枚举</param>
        /// <param name="input">传人的参数对象实例</param>
        /// <param name="urlParms">地址</param>
        /// <returns>返回字符串</returns>
        public static async Task<string> doGet<Input>(Input input, string urlParms) where Input : new()
        {
            try
            {
                string url = urlParms + ObjectToUrlParms(input);
                var _httpClient = PrepareHttpClient();
                var response = await _httpClient.GetAsync(_httpClient.BaseAddress + url).ConfigureAwait(false);
                await HandlerBadRequest(response).ConfigureAwait(false);
                string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// get请求传人查询对象、服务器地址枚举返回单个输出对象
        /// </summary>
        /// <typeparam name="Input">输入参数对象类型</typeparam>
        /// <typeparam name="Output">输出对象类型</typeparam>
        /// <param name="serverUrlType">服务器地址枚举</param>
        /// <param name="input">输入参数对象实例</param>
        /// <param name="urlParms">地址</param>
        /// <returns>返回输出对象实例</returns>
        public static async Task<Output> doGet<Input, Output>(Input input, string urlParms)
            where Input : new()
            where Output : new()
        {
            try
            {
                string url = urlParms + ObjectToUrlParms(input);
                var _httpClient = PrepareHttpClient();
                var response = await _httpClient.GetAsync(url).ConfigureAwait(false);
                await HandlerBadRequest(response).ConfigureAwait(false);
                Output output = await response.Content.ReadAsAsync<Output>().ConfigureAwait(false);
                return output;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<T> doGet<T>(string urlParms) where T : new()
        {
            try
            {
                var _httpClient = PrepareHttpClient();
                var response = await _httpClient.GetAsync(urlParms).ConfigureAwait(false);
                await HandlerBadRequest(response).ConfigureAwait(false);
                T t = await response.Content.ReadAsAsync<T>().ConfigureAwait(false);
                return t;
            }
            catch (Exception ex)
            {
                throw ex;
                //return default(T);
            }
        }

        /// <summary>
        /// 指定api服务查询
        /// </summary>
        /// <typeparam name="T">查询对象类型</typeparam>
        /// <param name="serverUrlType">api服务地址枚举</param>
        /// <param name="urlParms">地址(参数拼接好)</param>
        /// <returns>返回集合</returns>
        public static async Task<IEnumerable<T>> doGetList<T>(string urlParms) where T : new()
        {
            try
            {
                var _httpClient = PrepareHttpClient();
                var response = await _httpClient.GetAsync(urlParms).ConfigureAwait(false);
                await HandlerBadRequest(response).ConfigureAwait(false);
                IEnumerable<T> lst = await response.Content.ReadAsAsync<IEnumerable<T>>().ConfigureAwait(false);
                return lst;
            }
            catch (Exception ex)
            {
                throw ex;
                //return default(IEnumerable<T>);
            }
        }

        /// <summary>
        /// 指定api服务器查询
        /// </summary>
        /// <typeparam name="T">查询的对象类型</typeparam>
        /// <typeparam name="S">查询条件对象类型</typeparam>
        /// <param name="serverUrlType">api服务枚举</param>
        /// <param name="s">查询对象实例</param>
        /// <param name="urlParms">地址</param>
        /// <returns>返回集合</returns>
        public static async Task<IEnumerable<T>> doGetList<T, S>(S s, string urlParms)
            where S : new()
            where T : new()
        {
            try
            {
                var _httpClient = PrepareHttpClient();
                string url = urlParms + ObjectToUrlParms(s);
                var response = await _httpClient.GetAsync(url).ConfigureAwait(false);
                await HandlerBadRequest(response).ConfigureAwait(false);
                IEnumerable<T> lst = await response.Content.ReadAsAsync<IEnumerable<T>>().ConfigureAwait(false);
                return lst;
            }
            catch (Exception ex)
            {
                throw ex;
                //return default(IEnumerable<T>);
            }
        }

        public static async Task<T> doGetPageList<T, S>( S s, string urlParms)
            where S : new()
            where T : new()
        {
            try
            {
                var _httpClient = PrepareHttpClient();
                string url = urlParms + ObjectToUrlParms(s);
                var response = await _httpClient.GetAsync(url).ConfigureAwait(false);
                await HandlerBadRequest(response).ConfigureAwait(false);
                T lst = await response.Content.ReadAsAsync<T>().ConfigureAwait(false);
                return lst;
            }
            catch (Exception ex)
            {
                throw ex;
                //return default(IEnumerable<T>);
            }
        }

        #endregion


        #region POST

        /// <summary>
        /// 调用指定api服务器新增
        /// </summary>
        /// <typeparam name="T">新增对象类型</typeparam>
        /// <param name="serverUrlType">api服务枚举</param>
        /// <param name="t">新增对象实例</param>
        /// <param name="url">地址</param>
        /// <returns></returns>
        public static async Task<string> doPost<T>( T t, string url) where T : new()
        {
            try
            {
                var _httpClient = PrepareHttpClient();
                var response = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + url, t).ConfigureAwait(false);
                await HandlerBadRequest(response).ConfigureAwait(false);
                string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 调用指定api服务器新增并返回新增后对象
        /// </summary>
        /// <typeparam name="T">新增对象类型</typeparam>
        /// <param name="serverUrlType">api服务枚举</param>
        /// <param name="t">新增对象实例</param>
        /// <param name="actionName">地址</param>
        /// <returns></returns>
        public static async Task<TOut> doPostOut<TOut, T>( T t, string actionName) where T : new()
        {
            try
            {
                var _httpClient = PrepareHttpClient();
                var response = await _httpClient.PostAsJsonAsync(actionName, t).ConfigureAwait(false);
                await HandlerBadRequest(response).ConfigureAwait(false);
                TOut output = await response.Content.ReadAsAsync<TOut>().ConfigureAwait(false);
                return output;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据string类型参数获取集合对象（与get方式类似，考虑到get方式url请求参数长度的限制）
        /// </summary>
        /// <typeparam name="T">反序列化实体类型</typeparam>
        /// <param name="serverUrlType">api服务枚举</param>
        /// <param name="input">stirng类型参数（长）</param>
        /// <param name="url">地址</param>
        /// <returns></returns>
        public static async Task<IEnumerable<T>> doPostOut<T>( string input, string url) where T : new()
        {
            return await doPostOut<T>(input, url, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// 根据string类型参数获取集合对象（与get方式类似，考虑到get方式url请求参数长度的限制）
        /// </summary>
        /// <typeparam name="T">反序列化实体类型</typeparam>
        /// <param name="serverUrlType">api服务枚举</param>
        /// <param name="input">stirng类型参数（长）</param>
        /// <param name="url">地址</param>
        /// <returns></returns>
        public static async Task<IEnumerable<T>> doPostOut<T>(string input, string url, CancellationToken token) where T : new()
        {
            try
            {
                var _httpClient = PrepareHttpClient();
                var response = await _httpClient.PostAsJsonAsync(url, input, token).ConfigureAwait(false);
                await HandlerBadRequest(response).ConfigureAwait(false);
                IEnumerable<T> outputs = await response.Content.ReadAsAsync<IEnumerable<T>>().ConfigureAwait(false);
                return outputs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Put

        public static async Task<string> doPut<T>( T t, string url) where T : new()
        {
            return await doPutAsJsonAsync<T>(t, url).ConfigureAwait(false);
        }

        /// <summary>
        /// 调用指定api服务器新增并返回新增后对象
        /// </summary>
        /// <typeparam name="T">新增对象类型</typeparam>
        /// <param name="serverUrlType">api服务枚举</param>
        /// <param name="t">新增对象实例</param>
        /// <param name="url">地址</param>
        /// <returns></returns>
        public static async Task<T> doPutOut<T>(T t, string url) where T : new()
        {
            try
            {
                var _httpClient = PrepareHttpClient();
                var response = await _httpClient.PutAsJsonAsync(url, t).ConfigureAwait(false);
                await HandlerBadRequest(response).ConfigureAwait(false);
                T output = await response.Content.ReadAsAsync<T>().ConfigureAwait(false);
                return output;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<string> doPutPost<T>(T t, string url)
        {
            return await doPostAsJsonAsync<T>(t, url).ConfigureAwait(false);
        }

        #endregion

        #region Delete

        public static async Task<string> doDelete(string url)
        {
            try
            {
                var _httpClient = PrepareHttpClient();
                var response = await _httpClient.DeleteAsync(url).ConfigureAwait(false);
                await HandlerBadRequest(response).ConfigureAwait(false);
                string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<string> doDelete<T>(IEnumerable<T> ids, string url)
        {
            return await doPostAsJsonAsync<IEnumerable<T>>(ids, url).ConfigureAwait(false);
        }

        #endregion

        private static async Task<string> doPutAsJsonAsync<T>(T t, string url)
        {
            try
            {
                var _httpClient = PrepareHttpClient();
                var response = await _httpClient.PutAsJsonAsync<T>(url, t).ConfigureAwait(false);
                await HandlerBadRequest(response).ConfigureAwait(false);
                string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static async Task<string> doPostAsJsonAsync<T>(T t, string url)
        {
            try
            {
                var _httpClient = PrepareHttpClient();
                var response = await _httpClient.PostAsJsonAsync<T>(url, t).ConfigureAwait(false);
                await HandlerBadRequest(response).ConfigureAwait(false);
                string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
