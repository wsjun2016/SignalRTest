using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace SignalRTest.Base {

    #region BaseClass

    public abstract class BaseClass { }

    #endregion

    #region EmptyClass

    public class EmptyClass : BaseClass { }

    #endregion

    #region BaseResponseResult

    /// <summary>
    /// 返回结果
    /// </summary>
    public class BaseResponseResult {
        public BaseResponseResult() { }

        public BaseResponseResult(int returnValue, string returnMsg) {
            _code = returnValue;
            _message = returnMsg;
        }

        private int _code = 0;
        private string _message = "";
        /// <summary>
        /// 错误码，0表示成功，其他表示失败
        /// </summary>
        public virtual int returnValue { get { return _code; } }
        /// <summary>
        /// 错误码，0表示成功，其他表示失败
        /// </summary>
        public virtual string returnMsg { get { return _message; } }
        /// <summary>
        /// 返回的数据，json格式
        /// </summary>
        public virtual object returnData { get; set; }

        /// <summary>
        /// 设置返回状态码
        /// </summary>
        /// <param name="code"></param>
        public virtual void SetCode(int code) {
            _code = code;
        }
        /// <summary>
        /// 设置返回消息
        /// </summary>
        /// <param name="message"></param>
        public virtual void SetMessage(string message) {
            _message = message;
        }
        /// <summary>
        /// 设置返回状态码和消息
        /// </summary>
        /// <param name="returnValue"></param>
        /// <param name="returnMsg"></param>
        public virtual void SetResult(int returnValue, string returnMsg) {
            _code = returnValue;
            _message = returnMsg;
        }

        /// <summary>
        /// 设置返回消息体
        /// </summary>
        /// <param name="obj"></param>
        public virtual void SetReturnData(params object[] obj) { }

        /// <summary>
        /// 将当前结果转化为JSON字符串
        /// </summary>
        /// <returns></returns>
        public virtual string ToJson() {
            return new JavaScriptSerializer().Serialize(this);
        }

        public virtual void Response() {
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            HttpContext.Current.Response.Write(this.ToJson());
        }
    }

    #endregion
    
    #region ModelBinder

    /// <summary>
    /// 模型绑定抽象类，需要继承此抽象类。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ModelBinder<T> where T : class {
        protected ModelState _modelState;

        /// <summary>
        /// 模型绑定状态
        /// </summary>
        public ModelState ModelState {
            get {
                if (_modelState == null)
                    _modelState = new ModelState();
                return _modelState;
            }
        }

        /// <summary>
        /// 绑定操作
        /// </summary>
        /// <returns></returns>
        public abstract T Binder();

        /// <summary>
        /// 验证实体数据合法性。如果有错误，请在ModelState参数中获取。
        /// </summary>
        /// <param name="entity"></param>
        protected void Valide(object entity) {
            //获取T类型的所有公共属性
            Type type = entity.GetType();
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            if (properties != null && properties.Count() > 0) {
                //针对每一个公共属性，获取其特性
                foreach (var property in properties) {
                    //如果当前属性为一个自定义类型
                    if (property.PropertyType != typeof(object) && Type.GetTypeCode(property.PropertyType) == TypeCode.Object) {
                        this.Valide(property.GetValue(entity, null));
                    }
                    else
                        ValideProperty(entity, property);

                    if (!_modelState.IsValid)
                        break;
                }
            }
        }

        /// <summary>
        /// 验证属性的每一个特性约束
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="property"></param>
        private void ValideProperty(object entity, PropertyInfo property) {
            if (entity != null && property != null) {
                var attributes = property.GetCustomAttributes(typeof(ValidationAttribute), false);
                foreach (ValidationAttribute attribute in attributes)
                    ValidatePropertyAttribute(entity, property, attribute);
            }
        }

        /// <summary>
        /// 使用特性对属性进行验证
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="property"></param>
        /// <param name="attribute"></param>
        private void ValidatePropertyAttribute(object entity, PropertyInfo property, ValidationAttribute attribute) {
            if (entity != null && property != null && attribute != null) {
                //找到该属性
                //注明：每一个函数都应当具有独立性.
                PropertyInfo currentProperty = entity.GetType().GetProperties().Where(p => p.Name == property.Name).FirstOrDefault();
                if (currentProperty != null) {
                    var value = currentProperty.GetValue(entity, null);
                    if (!attribute.IsValid(value))
                        _modelState.Errors.Add(attribute.ErrorMessage);
                }
            }
        }

    }

    #endregion

    #region BaseHandler

    #region BaseHandler的旧版本（已弃用）。该版本实现了Binder和Process方法。新版本没有实现Binder和Process方法，并让派生类自己实现。新版本扩展性较强。

    //public abstract class BaseHandler<T> : ModelBinder<T> where T : class {
    //    protected readonly ILog log;
    //    public BaseHandler() {
    //        log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    //    }
    //    public BaseHandler(string typeName) {
    //        if ((typeName ?? "").Trim() != "")
    //            log = LogManager.GetLogger(typeName);
    //        else
    //            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    //    }

    //    /// <summary>
    //    /// 对实体模型进行绑定和参数的特性验证
    //    /// </summary>
    //    /// <returns></returns>
    //    public override T Binder() {
    //        //初始化模型
    //        T rc = default(T);

    //        try {
    //            //初始化ModelState
    //            if (_modelState == null)
    //                _modelState = new ModelState();
    //            else
    //                _modelState.Errors.Clear();

    //            //获取数据
    //            Stream stream = HttpContext.Current.Request.InputStream;
    //            stream.Seek(0, SeekOrigin.Begin);
    //            byte[] buffer = new byte[stream.Length];
    //            int count = stream.Read(buffer, 0, buffer.Length);
    //            if (count > 0) {
    //                string requestParam = Encoding.UTF8.GetString(buffer);
    //                //绑定数据
    //                rc = new JavaScriptSerializer().Deserialize<T>(requestParam);
    //                if (rc != null) {
    //                    //验证数据合法性
    //                    base.Valide(rc);
    //                }
    //                else
    //                    _modelState.Errors.Add("绑定数据失败！");
    //            }
    //            else
    //                _modelState.Errors.Add("请求参数为空！");
    //        }
    //        catch (Exception ex) {
    //            _modelState.Errors.Add("绑定数据出现错误！");
    //        }

    //        return rc;
    //    }

    //    /// <summary>
    //    /// 处理接口API消息
    //    /// </summary>      
    //    /// <returns></returns>
    //    public BaseResponseResult Process() {
    //        BaseResponseResult rc = new BaseResponseResult((int)Live.Model.ErrorCode.OperationError, "操作失败！");

    //        try {
    //            //绑定请求参数
    //            T requestParam = Binder();
    //            //开启逻辑操作
    //            if (_modelState.IsValid)
    //                rc = DoWork(requestParam);
    //            else {
    //                StringBuilder sbuilder = new StringBuilder();
    //                foreach (var error in _modelState.Errors)
    //                    sbuilder.Append(error.ErrorMessage);

    //                rc.SetResult((int)Live.Model.ErrorCode.InvalideParameter, sbuilder.ToString());
    //            }
    //        }
    //        catch (Exception ex) {
    //            rc.SetCode((int)Live.Model.ErrorCode.SystemError);
    //            rc.SetMessage("系统错误！");
    //            rc.returnData = null;
    //            log.Error("Process", ex);
    //        }

    //        return rc;
    //    }

    //    /// <summary>
    //    /// 真正需要处理的接口逻辑
    //    /// </summary>
    //    /// <param name="param">客户端传过来的请求参数</param>
    //    /// <returns></returns>
    //    protected abstract BaseResponseResult DoWork(T param);
    //}

    #endregion

    #region BaseHandler的新版本

    /// <summary>
    /// 一般处理逻辑的基类。
    /// </summary>
    /// <typeparam name="T">请求参数模型</typeparam>
    /// <typeparam name="R">返回参数模型</typeparam>
    public abstract class BaseHandler<T, R> : ModelBinder<T> where T : class where R : class {
        protected readonly ILog log;
        public BaseHandler() {
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }
        public BaseHandler(string typeName) {
            if ((typeName ?? "").Trim() != "")
                log = LogManager.GetLogger(typeName);
            else
                log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        /// <summary>
        /// 处理接口API消息
        /// </summary>      
        /// <returns></returns>
        public abstract R Process();

        /// <summary>
        /// 真正需要处理的接口逻辑
        /// </summary>
        /// <param name="param">客户端传过来的请求参数</param>
        /// <returns></returns>
        protected abstract R DoWork(T param);
    }

    #endregion

    /// <summary>
    /// 直播业务接口的模板方法处理器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class LiveHandler<T> : BaseHandler<T, BaseResponseResult> where T : BaseClass {        
        public LiveHandler():base() {  }
        public LiveHandler(string typeName):base(typeName) {  }

        /// <summary>
        /// 对实体模型进行绑定和参数的特性验证
        /// </summary>
        /// <returns></returns>
        public override T Binder() {
            //初始化模型
            T rc = default(T);

            try {
                //初始化ModelState
                if (_modelState == null)
                    _modelState = new ModelState();
                else
                    _modelState.Errors.Clear();

                //获取数据
                Stream stream = HttpContext.Current.Request.InputStream;
                stream.Seek(0, SeekOrigin.Begin);
                byte[] buffer = new byte[stream.Length];
                int count = stream.Read(buffer, 0, buffer.Length);
                if (count > 0) {
                    string requestParam = Encoding.UTF8.GetString(buffer);
                    //绑定数据
                    rc = new JavaScriptSerializer().Deserialize<T>(requestParam);
                    if (rc != null) {
                        //验证数据合法性
                        base.Valide(rc);
                    }
                    else
                        _modelState.Errors.Add("绑定数据失败！");
                }
                else
                    _modelState.Errors.Add("请求参数为空！");
            }
            catch (Exception ex) {
                _modelState.Errors.Add("绑定数据出现错误！");
            }

            return rc;
        }

        /// <summary>
        /// 处理接口API消息
        /// </summary>      
        /// <returns></returns>
        public override BaseResponseResult Process() {
            BaseResponseResult rc = new BaseResponseResult((int)ErrorCode.OperationError, "操作失败！");

            try {
                //绑定请求参数
                T requestParam = Binder();
                //开启逻辑操作
                if (_modelState.IsValid)
                    rc = DoWork(requestParam);
                else {
                    StringBuilder sbuilder = new StringBuilder();
                    foreach (var error in _modelState.Errors)
                        sbuilder.Append(error.ErrorMessage);

                    rc.SetResult((int)ErrorCode.InvalideParameter, sbuilder.ToString());
                }
            }
            catch (Exception ex) {
                rc.SetCode((int)ErrorCode.SystemError);
                rc.SetMessage("系统错误！");
                rc.returnData = null;
                log.Error("Process", ex);
            }

            return rc;
        }        
    }

    #endregion

    #region ModelState

    /// <summary>
    /// 自定义的模型绑定状态
    /// </summary>
    [Serializable]
    public class ModelState {
        private ModelErrorCollection _errors = new ModelErrorCollection();
        public bool IsValid {
            get {
                return _errors.Count == 0;
            }
        }
        public ModelErrorCollection Errors {
            get {
                return _errors;
            }
        }
    }

    [Serializable]
    public class ModelErrorCollection : Collection<ModelError> {
        public void Add(string errorMessage) {
            base.Add(new ModelError(errorMessage));
        }

        public void Add(Exception exception) {
            base.Add(new ModelError(exception));
        }
    }

    [Serializable]
    public class ModelError {
        public ModelError(Exception exception) : this(exception, null) { }

        public ModelError(string errorMessage) {
            this.ErrorMessage = errorMessage ?? string.Empty;
        }

        public ModelError(Exception exception, string errorMessage) : this(errorMessage) {
            if (exception == null)
                throw new ArgumentNullException("exception");
            this.Exception = exception;
        }

        public Exception Exception { get; private set; }
        public string ErrorMessage { get; private set; }
    }

    #endregion

    public enum ErrorCode {
        /// <summary>
        /// 操作错误
        /// </summary>
        OperationError = -1,
        /// <summary>
        /// 成功
        /// </summary>
        Success = 0,
        /// <summary>
        /// 失败
        /// </summary>
        Failed = 1,
        /// <summary>
        /// 请求参数格式不符合要求
        /// </summary>
        InvalideParameter = 98,
        /// <summary>
        /// 无此接口
        /// </summary>
        NoAction = 99,
        /// <summary>
        /// 系统错误
        /// </summary>
        SystemError = 100,
    }

}