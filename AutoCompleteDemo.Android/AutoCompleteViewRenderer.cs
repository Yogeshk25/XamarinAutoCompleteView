using System;
using Android.Content;
using Android.Widget;
using AutoCompleteDemo;
using AutoCompleteDemo.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
[assembly: ExportRenderer(typeof(AutoCompleteView), typeof(AutoCompleteViewRenderer))]
namespace AutoCompleteDemo.Droid
{
    public class AutoCompleteViewRenderer : ViewRenderer<AutoCompleteView, AutoCompleteTextView>
    {
        public AutoCompleteViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<AutoCompleteView> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
            {
                SetNativeControl(new AutoCompleteTextView(this.Context));
            }
        }
    }
}
