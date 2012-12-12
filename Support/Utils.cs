using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Practic.Framework.Support;

namespace Practic
{
    public static class Utils
    {

        private static readonly IDictionary<String, Object> DataBin = new Dictionary<string, object>();

        public static void HandlePropertyChanged(this INotifyPropertyChanged clazz, PropertyChangedEventHandler handler, String propertyName)
        {
            if (handler != null)
            {
                handler(clazz, new PropertyChangedEventArgs(propertyName));
            }
        }

        public static void HandlePropertyChanging(this INotifyPropertyChanging clazz, PropertyChangingEventHandler handler, String propertyName)
        {
            if (handler != null)
            {
                handler(clazz, new PropertyChangingEventArgs(propertyName));
            }
        }

        public static DateTime JavaTimeStampToDateTime(double javaTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(Math.Round(javaTimeStamp / 1000)).ToLocalTime();
            return dtDateTime;
        }

        public static BackgroundOpStack Worker
        {
            get { return BackgroundOpStack.Instance; }
        }

        public static void PropDpChanged<T, TArg>(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs, Action<T, TypeSafePropertyChangedEventArgs<TArg>> realOnChangedHandler) where TArg : class where T:class 
        {
            var caller = dependencyObject as T;

            if (caller == null)
            {
                throw new InvalidOperationException("Dependency object is null.");
            }


            realOnChangedHandler(caller, TypeSafePropertyChangedEventArgs<TArg>.FromEventArgs<TArg>(eventArgs));
        }
        public class TypeSafePropertyChangedEventArgs<TArg> : EventArgs
        {
            private TypeSafePropertyChangedEventArgs(TArg oldValue, TArg newValue, DependencyProperty property)
            {
                _oldValue = oldValue;
                _newValue = newValue;
                _property = property;
            }

            public static TypeSafePropertyChangedEventArgs<T> FromEventArgs<T>(DependencyPropertyChangedEventArgs eventArgs) where T:class 
            {

                var oldValue = eventArgs.OldValue as T;
                var newValue = eventArgs.NewValue as T;
                var propName = eventArgs.Property;

                return new TypeSafePropertyChangedEventArgs<T>(oldValue, newValue, propName);
            }

            public DependencyProperty Property
            {
                get { return _property; }
            }

            public TArg OldValue
            {
                get { return _oldValue; }
            }

            public TArg NewValue
            {
                get { return _newValue; }

            }

            private readonly TArg _oldValue;
            private readonly TArg _newValue;
            private readonly DependencyProperty _property;

        }

        public static bool IsInDesignMode
        {
            get
            {
                return DesignerProperties.IsInDesignTool;
            }
        }

        public static void AddStateForTransfer(String key, Object value)
        {
            if (DataBin.ContainsKey(key))
            {
                DataBin.Remove(key);
            }

            DataBin.Add(key, value);
        }

        public static T RetrieveStateForTransfer<T>(String key, T defaultValue = default(T))
        {
            if(!DataBin.ContainsKey(key))
            {
                return defaultValue;
            }

            var result = DataBin[key];

            if(result is T)
            {
                return (T)result;
            }
            
            return defaultValue;
        }

        public static Uri UriFromImageUriString(string uriString)
        {
            DebugUtils.GenericAssert(!String.IsNullOrEmpty(uriString), "There must be a valid string here instead of {0}!", uriString);
            return new Uri(uriString, UriKind.Relative);
        }

        // Color helpers

        public static float Lerp(this float start, float end, float amount)
        {
            float difference = end - start;
            float adjusted = difference * amount;
            return start + adjusted;
        }

        public static Color Lerp(this Color colour, Color to, float amount)
        {
            // start colours as lerp-able floats
            float sr = colour.R, sg = colour.G, sb = colour.B;

            // end colours as lerp-able floats
            float er = to.R, eg = to.G, eb = to.B;

            // lerp the colours to get the difference
            byte r = (byte)sr.Lerp(er, amount),
                 g = (byte)sg.Lerp(eg, amount),
                 b = (byte)sb.Lerp(eb, amount);

            // return the new colour
            return Color.FromArgb(colour.A, r, g, b);
        }

    }
}
