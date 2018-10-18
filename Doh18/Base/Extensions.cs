using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Doh18.Base
{
    public static class StringExtensions
    {
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }
    }

    [ContentProperty("Source")]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }
            // Do your translation lookup here, using whatever method you require
            var imageSource = ImageSource.FromResource(Source);

            return imageSource;
        }
    }

    public static class PropertyChangedExtensions
    {
        public static void WhenPropertyChanged<T>(this T obj, string property,
            Action<T> action) where T : INotifyPropertyChanged
        {
            obj.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == property)
                    action((T)sender);
            };
        }

        public static void WhenPropertyChanged<T>(this T obj, string property,
            Predicate<T> predicate, Action<T> action) where T : INotifyPropertyChanged
        {
            obj.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == property && predicate((T)sender))
                    action((T)sender);
            };
        }

        public static void WhenCollectionChanged<T>(this T obj,
            Func<T, NotifyCollectionChangedEventArgs, bool> predicate, Action<T> action)
            where T : INotifyCollectionChanged
        {
            obj.CollectionChanged += (sender, e) =>
            {
                if (predicate((T)sender, e))
                    action((T)sender);
            };
        }
    }

    public static class DictioanaryExtensions
    {
        public static IDictionary<T1, T2> Merge<T1, T2>(this IDictionary<T1, T2> dictionary, IDictionary<T1, T2> newElements)
        {
            if (newElements == null || newElements.Count == 0)
                return dictionary;

            foreach (var e in newElements)
            {
                dictionary.Remove(e.Key);
                dictionary.Add(e);
            }

            return dictionary;
        }
    }
}
