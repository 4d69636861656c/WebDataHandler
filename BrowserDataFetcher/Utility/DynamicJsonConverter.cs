namespace BrowserDataFetcher
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Dynamic;
    using System.Linq;
    using System.Text;
    using System.Web.Script.Serialization;

    /// <summary>
    /// The <see cref="DynamicJsonConverter"/>.
    /// </summary>
    public sealed class DynamicJsonConverter : JavaScriptConverter
    {
        #region Public Methods

        /// <summary>
        /// Deserializes an object.
        /// </summary>
        /// <param name="dictionary">
        /// The dictionary.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="serializer">
        /// The serializer.
        /// </param>
        /// <returns>
        /// The deserialized object.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <see cref="ArgumentNullException"/>.
        /// </exception>
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            return type == typeof(object) ? new DynamicJsonObject(dictionary) : null;
        }

        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <param name="obj">
        /// The object.
        /// </param>
        /// <param name="serializer">
        /// The serializer.
        /// </param>
        /// <returns>
        /// The serialized object.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// The <see cref="NotImplementedException"/>.
        /// </exception>
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the supported types.
        /// </summary>
        public override IEnumerable<Type> SupportedTypes
        {
            get
            {
                return new ReadOnlyCollection<Type>(new List<Type>(new[] { typeof(object) }));
            }
        }

        #endregion Public Methods

        #region Nested classes

        /// <summary>
        /// The <see cref="DynamicJsonConverter"/>.
        /// </summary>
        private sealed class DynamicJsonObject : DynamicObject
        {
            /// <summary>
            /// The <see cref="object"/> dictionary.
            /// </summary>
            private readonly IDictionary<string, object> _dictionary;

            /// <summary>
            /// Initializes a new instance of the <see cref="DynamicJsonObject"/> class.
            /// </summary>
            /// <param name="dictionary">
            /// The dictionary.
            /// </param>
            /// <exception cref="ArgumentNullException">
            /// The <see cref="ArgumentNullException"/>.
            /// </exception>
            public DynamicJsonObject(IDictionary<string, object> dictionary)
            {
                _dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
            }

            /// <summary>
            /// Returns a string that represents the current object.
            /// </summary>
            /// <returns>
            /// A string that represents the current object.
            /// </returns>
            public override string ToString()
            {
                StringBuilder sb = new StringBuilder("{ ");
                ToString(sb);
                sb.Append("}");

                return sb.ToString();
            }

            /// <summary>
            /// Returns a string that represents the current object.
            /// </summary>
            /// <returns>
            /// A string that represents the current object.
            /// </returns>
            private void ToString(StringBuilder sb)
            {
                bool firstDict = true;
                foreach (var pair in _dictionary)
                {
                    if (!firstDict)
                    {
                        sb.Append(",");
                    }

                    firstDict = false;
                    sb.Append("\"" + pair.Key + "\": ");

                    if (pair.Value is string)
                    {
                        sb.Append("\"" + pair.Value.ToString() + "\"");
                    }
                    else if (pair.Value is Dictionary<string, object>)
                    {
                        sb.Append((new DynamicJsonObject(pair.Value as Dictionary<string, object>).ToString()));
                    }
                    else if (pair.Value is ArrayList)
                    {
                        if ((pair.Value as ArrayList).Count > 1)
                        {
                            sb.Append("[");
                        }

                        bool firstAL = true;
                        foreach (var item in pair.Value as ArrayList)
                        {
                            if (!firstAL)
                            {
                                sb.Append(",");
                            }

                            firstAL = false;
                            if (item is string)
                            {
                                sb.Append("\"" + item + "\"");
                            }
                            else if (item is IDictionary<string, object>)
                            {
                                sb.Append((new DynamicJsonObject(item as Dictionary<string, object>).ToString()));
                            }
                            else
                            {
                                sb.Append("ERROR");
                            }
                        }

                        if ((pair.Value as ArrayList).Count > 1)
                        {
                            sb.Append("]");
                        }
                    }
                    else
                    {
                        sb.Append("ERROR");
                    }
                }
            }

            /// <summary>
            /// Provides the implementation for operations that get member values.
            /// Classes derived from the <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to specify dynamic behavior for operations such as getting a value for a property.
            /// </summary>
            /// <param name="binder">
            /// Provides information about the object that called the dynamic operation.
            /// The binder.Name property provides the name of the member on which the dynamic operation is performed.
            /// For example, for the Console.WriteLine(sampleObject.SampleProperty) statement, where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, binder.Name returns "SampleProperty".
            /// The binder.IgnoreCase property specifies whether the member name is case-sensitive.
            /// </param>
            /// <param name="result">
            /// The result of the get operation.
            /// For example, if the method is called for a property, you can assign the property value to <paramref name="result" />.
            /// </param>
            /// <returns>
            /// <see langword="true" /> if the operation is successful; otherwise, <see langword="false" />.
            /// If this method returns <see langword="false" />, the run-time binder of the language determines the behavior.
            /// (In most cases, a run-time exception is thrown.)
            /// </returns>
            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                if (!_dictionary.TryGetValue(binder.Name, out result))
                {
                    result = null;

                    return true;
                }

                result = WrapResultObject(result);

                return true;
            }

            /// <summary>
            /// Provides the implementation for operations that get a value by index.
            /// Classes derived from the <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to specify dynamic behavior for indexing operations.
            /// </summary>
            /// <param name="binder">
            /// Provides information about the operation.
            /// </param>
            /// <param name="indexes">
            /// The indexes that are used in the operation.
            /// For example, for the sampleObject[3] operation in C# (sampleObject(3) in Visual Basic), where sampleObject is derived from the <see langword="DynamicObject" /> class, <paramref name="indexes[0]" /> is equal to 3.
            /// </param>
            /// <param name="result">
            /// The result of the index operation.
            /// </param>
            /// <returns>
            /// <see langword="true" /> if the operation is successful; otherwise, <see langword="false" />.
            /// If this method returns <see langword="false" />, the run-time binder of the language determines the behavior.
            /// (In most cases, a run-time exception is thrown.)
            /// </returns>
            public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
            {
                if (indexes.Length == 1 && indexes[0] != null)
                {
                    if (!_dictionary.TryGetValue(indexes[0].ToString(), out result))
                    {
                        result = null;

                        return true;
                    }

                    result = WrapResultObject(result);

                    return true;
                }

                return base.TryGetIndex(binder, indexes, out result);
            }

            /// <summary>
            /// Wraps the resulting object.
            /// </summary>
            /// <param name="result">
            /// The result.
            /// </param>
            /// <returns>
            /// The wrapped object.
            /// </returns>
            private static object WrapResultObject(object result)
            {
                IDictionary<string, object> dictionary = result as IDictionary<string, object>;
                if (dictionary != null)
                {
                    return new DynamicJsonObject(dictionary);
                }

                ArrayList arrayList = result as ArrayList;
                if (arrayList != null && arrayList.Count > 0)
                {
                    return arrayList[0] is IDictionary<string, object>
                        ? new List<object>(arrayList.Cast<IDictionary<string, object>>().Select(x => new DynamicJsonObject(x)))
                        : new List<object>(arrayList.Cast<object>());
                }

                return result;
            }

            /// <summary>
            /// Wraps the resulting object.
            /// </summary>
            /// <param name="result">
            /// The result.
            /// </param>
            /// <returns>
            /// The wrapped object.
            /// </returns>
            private static object _WrapResultObject(object result)
            {
                var dictionary = result as IDictionary<string, object>;
                if (dictionary != null)
                {
                    return new DynamicJsonObject(dictionary);
                }

                var arrayList = result as ArrayList;
                if (arrayList != null && arrayList.Count > 0)
                {
                    return arrayList[0] is IDictionary<string, object>
                        ? new List<object>(arrayList.Cast<IDictionary<string, object>>().Select(x => new DynamicJsonObject(x)))
                        : new List<object>(arrayList.Cast<object>());
                }

                return result;
            }
        }

        #endregion Nested classes
    }
}