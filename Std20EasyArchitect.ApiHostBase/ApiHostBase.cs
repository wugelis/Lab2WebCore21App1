using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Std20EasyArchitect.ApiHostBase.DTO;
using System;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using System.Text;

namespace Std20EasyArchitect.ApiHostBase
{
    [ApiController]
    [Route("api/[controller]/{fileName}/{nameSpace}/{className}/{methodName}/{*pathInfo}")]
    [Route("api/[controller]/{fileName}/{nameSpace}/{methodName}/{*pathInfo}")]
    [Route("api/[controller]/{fileName}/{methodName}/{*pathInfo}")]
    [Route("api/[controller]/{fileName}/{*pathInfo}")]
    [Route("api/[controller]/{*pathInfo}")]
    public class ApiHostBase: ControllerBase
    {
        /// <summary>
        /// 取得 HTTP Body 參數內容方法
        /// </summary>
        /// <returns></returns>
        private object GetParameter()
        {
            object inputStramStr = null;
            var ContentType = HttpContext.Request.ContentType;

            if (!string.IsNullOrEmpty(ContentType)
                && (ContentType.IndexOf("application/json") >= 0 || ContentType.IndexOf("text/plain") >= 0))
            {
                MemoryStream ms = new MemoryStream();
                HttpContext.Request.Body.CopyTo(ms);

                using (StreamReader inputStream = new StreamReader(ms, Encoding.UTF8))
                {
                    inputStramStr = Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            else
            {
                inputStramStr = HttpContext.Request.Body;
            }

            return inputStramStr;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private string ReleaseStartEndQuotes(string parameter)
        {
            parameter = parameter.StartsWith("\"") ? parameter.Substring(1, parameter.Length - 1) : parameter;
            parameter = parameter.EndsWith("\"") ? parameter.Substring(0, parameter.Length - 1) : parameter;
            return parameter;
        }

        #region The binary read method.
        /// <summary>
        /// The binary read method.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private byte[] BinaryReadToEnd(Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
        #endregion

        /// <summary>
        /// ApiHostBase 核心所提供的共用的 Post 方法
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="nameSpace"></param>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<object> Post(
            string fileName,
            string nameSpace,
            string className,
            string methodName)
        {
            object result = null;

            if (string.IsNullOrEmpty(fileName) ||
                string.IsNullOrEmpty(nameSpace) ||
                string.IsNullOrEmpty(className) ||
                string.IsNullOrEmpty(methodName))
            {
                return new string[] { "傳入的 Url 有誤！請確認呼叫 Api 的 Url 的格式！" }
                    .Select(c => new
                    {
                        Err = c
                    }).ToList();
            }

            object parameter = GetParameter();

            Assembly assem = Assembly.Load($"{fileName}, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            if(assem != null)
            {
                Type runtimeType = assem.GetType($"{nameSpace}.{className}");
                object targetObj = Activator.CreateInstance(runtimeType);
                object invokeObj = null;
                MethodInfo[] methods = runtimeType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Default);

                bool found = false;
                foreach(var method in methods)
                {
                    if (method.Name.ToLower() == methodName.ToLower())
                    {
                        ParameterInfo[] parames = method.GetParameters();
                        if (parames.Length > 0)
                        {
                            string paramName = parames[0].Name;
                            Type propertyType = parames[0].GetType();

                            //parames[0].ParameterType.Name.Dump();

                            Type parameType = parames[0].ParameterType;
                            //ConstructorInfo constructor = parameType.GetConstructor(Type.EmptyTypes);
                            //object parameObj = constructor.Invoke(null);

                            switch (parameType.ToString())
                            {
                                case "System.Int16":
                                    invokeObj = Int16.Parse(ReleaseStartEndQuotes(parameter.ToString()));
                                    break;
                                case "System.Int32":
                                    invokeObj = int.Parse(ReleaseStartEndQuotes(parameter.ToString()));
                                    break;
                                case "System.Int64":
                                    invokeObj = Int64.Parse(ReleaseStartEndQuotes(parameter.ToString()));
                                    break;
                                case "System.Single":
                                    invokeObj = Single.Parse(ReleaseStartEndQuotes(parameter.ToString()));
                                    break;
                                case "System.DateTime":
                                    invokeObj = DateTime.Parse(ReleaseStartEndQuotes(parameter.ToString()));
                                    break;
                                case "System.Double":
                                    invokeObj = double.Parse(ReleaseStartEndQuotes(parameter.ToString()));
                                    break;
                                case "System.Decimal":
                                    invokeObj = decimal.Parse(ReleaseStartEndQuotes(parameter.ToString()));
                                    break;
                                case "System.Boolean":
                                    invokeObj = bool.Parse(ReleaseStartEndQuotes(parameter.ToString()));
                                    break;
                                case "System.String":
                                    invokeObj = ReleaseStartEndQuotes(parameter.ToString());
                                    break;
                                case "System.Byte[]":
                                    if (parameter is Stream)
                                    {
                                        Stream content = parameter as Stream;
                                        //BinaryReader br = new BinaryReader(content);
                                        invokeObj = BinaryReadToEnd(content); //br.ReadBytes(content.Length);
                                    }
                                    else
                                    {
                                        invokeObj = JsonConvert.DeserializeObject(parameter.ToString(), parameType);
                                    }
                                    break;
                                default:    //如果都不是以上的物件，才進行 JSON DeserializeObject.
                                    invokeObj = JsonConvert.DeserializeObject(parameter.ToString(), parameType);
                                    break;
                            }

                            result = method.Invoke(targetObj, new object[] { invokeObj });
                        }
                    }
                }
            }

            return result;
        }
        /// <summary>
        /// ApiHostBase 核心所提供的共用的 Get 方法
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="nameSpace"></param>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public ActionResult<object> Get(
            string fileName, 
            string nameSpace, 
            string className, 
            string methodName)
        {
            if(string.IsNullOrEmpty(fileName) ||
                string.IsNullOrEmpty(nameSpace) ||
                string.IsNullOrEmpty(className) ||
                string.IsNullOrEmpty(methodName))
            {
                return new string[] { "傳入的 Url 有誤！請確認呼叫 Api 的 Url 的格式！" }
                    .Select(c => new
                    {
                        Err = c
                    }).ToList();
            }

            object result = null;
            object targetObj = null;
            Assembly assem = null;
            try
            {
                assem = Assembly.Load($"{fileName}, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            }
            catch(Exception ex)
            {
                assem = Assembly.LoadFrom(Path.Combine(Directory.GetCurrentDirectory(), $"bin\\Debug\\netcoreapp2.1\\{fileName}.dll"));
            }
            
            if(assem != null)
            {
                Type targetType = assem.GetType($"{nameSpace}.{className}");
                targetObj = Activator.CreateInstance(targetType);
                MethodInfo[] methods = targetType.GetMethods(
                    BindingFlags.Public |
                    BindingFlags.Instance |
                    BindingFlags.Default);

                bool found = false;
                foreach(var method in methods)
                {
                    if(method.Name.ToLower() == methodName.ToLower())
                    {
                        found = true;
                        result = method.Invoke(targetObj, null);
                        break;
                    }
                }
                if(!found)
                {
                    result = new string[] { $"methodName:{methodName} 不存在" }
                        .Select(c => new
                        {
                            Err = c
                        }).ToList();
                }
            }

            return result;

            /*
            return new Employee[] {
                new Employee() {
                    Id = "1000",
                    ChtName = "Gelis Wu"
                },
                new Employee() {
                    Id = "1001",
                    ChtName = "Mary Lee"
                }
            };
            */
        }
    }
}
