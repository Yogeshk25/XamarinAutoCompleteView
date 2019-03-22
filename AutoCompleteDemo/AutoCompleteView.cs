using System;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AutoCompleteDemo
{
    public enum AutoCompleteTextChangeReason
    {
        /// <summary>The user edited the text.</summary>
        UserInput = 0,

        /// <summary>The text was changed via code.</summary>
        ProgrammaticChange = 1,

        /// <summary>The user selected one of the items in the auto-suggestion box.</summary>
        SuggestionChosen = 2
    }

    public sealed class AutoCompleteTextChangedEventArgs : EventArgs
    {
        public AutoCompleteTextChangedEventArgs(AutoCompleteTextChangeReason reason)
        {
            Reason = reason;
        }

        /// <summary>
        /// Returns a Boolean value indicating if the current value of the TextBox is unchanged from the point in time when the TextChanged event was raised.
        /// </summary>
        /// <returns>Indicates if the current value of the TextBox is unchanged from the point in time when the TextChanged event was raised.</returns>
        public bool CheckCurrent() => true; //TODO

        /// <summary>
        /// Gets or sets a value that indicates the reason for the text changing in the AutoSuggestBox.
        /// </summary>
        /// <value>The reason for the text changing in the AutoSuggestBox.</value>
        public AutoCompleteTextChangeReason Reason { get; }
    }

    public sealed class AutoCompleteQuerySubmittedEventArgs : EventArgs
    {
        public AutoCompleteQuerySubmittedEventArgs(string queryText, object chosenSuggestion)
        {
            QueryText = queryText;
            ChosenSuggestion = chosenSuggestion;
        }

        /// <summary>
        /// Gets the suggested result that the use chose.
        /// </summary>
        /// <value>The suggested result that the use chose.</value>
        public object ChosenSuggestion { get; }

        /// <summary>
        /// The query text of the current search.
        /// </summary>
        /// <value>Gets the query text of the current search.</value>
        public string QueryText { get; }
    }

    public sealed class AutoCompleteViewEventArgs : EventArgs
    {
        public object SelectedItem { get; }

        public AutoCompleteViewEventArgs(object selectedItem)
        {
            SelectedItem = selectedItem;
        }
    }

    public class AutoCompleteView : View
    {
        private bool suppressTextChangedEvent;

        /// <summary>
        /// Gets or sets the Text property
        /// </summary>
        /// <seealso cref="TextColor"/>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Text"/> bindable property.
        /// </summary>
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(AutoCompleteView), "", BindingMode.OneWay, null, OnTextPropertyChanged);

        private static void OnTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var box = (AutoCompleteView)bindable;
            if (!box.suppressTextChangedEvent) //Ensure this property changed didn't get call because we were updating it from the native text property
                box.TextChanged?.Invoke(box, new AutoCompleteTextChangedEventArgs(AutoCompleteTextChangeReason.ProgrammaticChange));
        }

        /// <summary>
        /// Gets or sets the foreground color of the control
        /// </summary>
        /// <seealso cref="Text"/>
        public global::Xamarin.Forms.Color TextColor
        {
            get { return (global::Xamarin.Forms.Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="TextColor"/> bindable property.
        /// </summary>
        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(global::Xamarin.Forms.Color), typeof(AutoCompleteView), global::Xamarin.Forms.Color.Gray, BindingMode.OneWay, null, null);

        /// <summary>
        /// Gets or sets the PlaceholderText
        /// </summary>
        /// <seealso cref="PlaceholderTextColor"/>
        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="PlaceholderText"/> bindable property.
        /// </summary>
        public static readonly BindableProperty PlaceholderTextProperty =
            BindableProperty.Create(nameof(PlaceholderText), typeof(string), typeof(AutoCompleteView), string.Empty, BindingMode.OneWay, null, null);

        /// <summary>
        /// Gets or sets the foreground color of the control
        /// </summary>
        /// <seealso cref="PlaceholderText"/>
        public global::Xamarin.Forms.Color PlaceholderTextColor
        {
            get { return (global::Xamarin.Forms.Color)GetValue(PlaceholderTextColorProperty); }
            set { SetValue(PlaceholderTextColorProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="PlaceholderTextColor"/> bindable property.
        /// </summary>
        public static readonly BindableProperty PlaceholderTextColorProperty =
            BindableProperty.Create(nameof(PlaceholderTextColor), typeof(global::Xamarin.Forms.Color), typeof(AutoCompleteView), global::Xamarin.Forms.Color.Gray, BindingMode.OneWay, null, null);

        /// <summary>
        /// Gets or sets the property path that is used to get the value for display in the
        /// text box portion of the AutoSuggestBox control, when an item is selected.
        /// </summary>
        /// <value>
        /// The property path that is used to get the value for display in the text box portion
        /// of the AutoSuggestBox control, when an item is selected.
        /// </value>
        public string TextMemberPath
        {
            get { return (string)GetValue(TextMemberPathProperty); }
            set { SetValue(TextMemberPathProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="TextMemberPath"/> bindable property.
        /// </summary>
        public static readonly BindableProperty TextMemberPathProperty =
            BindableProperty.Create(nameof(TextMemberPath), typeof(string), typeof(AutoCompleteView), string.Empty, BindingMode.OneWay, null, null);

        /// <summary>
        /// Gets or sets the name or path of the property that is displayed for each data item.
        /// </summary>
        /// <value>
        /// The name or path of the property that is displayed for each the data item in
        /// the control. The default is an empty string ("").
        /// </value>
        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="DisplayMemberPath"/> bindable property.
        /// </summary>
        public static readonly BindableProperty DisplayMemberPathProperty =
            BindableProperty.Create(nameof(DisplayMemberPath), typeof(string), typeof(AutoCompleteView), string.Empty, BindingMode.OneWay, null, null);

        /// <summary>
        /// Gets or sets a Boolean value indicating whether the drop-down portion of the AutoSuggestBox is open.
        /// </summary>
        /// <value>A Boolean value indicating whether the drop-down portion of the AutoSuggestBox is open.</value>
        public bool IsSuggestionListOpen
        {
            get { return (bool)GetValue(IsSuggestionListOpenProperty); }
            set { SetValue(IsSuggestionListOpenProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="IsSuggestionListOpen"/> bindable property.
        /// </summary>
        public static readonly BindableProperty IsSuggestionListOpenProperty =
            BindableProperty.Create(nameof(IsSuggestionListOpen), typeof(bool), typeof(AutoCompleteView), false, BindingMode.OneWay, null, null);


        /// <summary>
        /// Used in conjunction with <see cref="TextMemberPath"/>, gets or sets a value indicating whether items in the view will trigger an update 
        /// of the editable text part of the <see cref="AutoCompleteView"/> when clicked.
        /// </summary>
        /// <value>A value indicating whether items in the view will trigger an update of the editable text part of the <see cref="AutoCompleteView"/> when clicked.</value>
        public bool UpdateTextOnSelect
        {
            get { return (bool)GetValue(UpdateTextOnSelectProperty); }
            set { SetValue(UpdateTextOnSelectProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="UpdateTextOnSelect"/> bindable property.
        /// </summary>
        public static readonly BindableProperty UpdateTextOnSelectProperty =
            BindableProperty.Create(nameof(UpdateTextOnSelect), typeof(bool), typeof(AutoCompleteView), true, BindingMode.OneWay, null, null);

        /// <summary>
        /// Gets or sets the header object for the text box portion of this control.
        /// </summary>
        /// <value>The header object for the text box portion of this control.</value>
        public System.Collections.IList ItemsSource
        {
            get { return GetValue(ItemsSourceProperty) as System.Collections.IList; }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ItemsSource"/> bindable property.
        /// </summary>
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(System.Collections.IList), typeof(AutoCompleteView), null, BindingMode.OneWay, null, null);

        internal void RaiseSuggestionChosen(object selectedItem)
        {
            SuggestionChosen?.Invoke(this, new AutoCompleteViewEventArgs(selectedItem));
        }

        /// <summary>
        /// Raised before the text content of the editable control component is updated.
        /// </summary>
        public event EventHandler<AutoCompleteViewEventArgs> SuggestionChosen;

        // Called by the native control when users enter text
        internal void NativeControlTextChanged(string text, AutoCompleteTextChangeReason reason)
        {
            suppressTextChangedEvent = true; //prevent loop of events raising, as setting this property will make it back into the native control
            Text = text;
            suppressTextChangedEvent = false;
            TextChanged?.Invoke(this, new AutoCompleteTextChangedEventArgs(reason));
        }

        /// <summary>
        /// Raised after the text content of the editable control component is updated.
        /// </summary>
        public event EventHandler<AutoCompleteTextChangedEventArgs> TextChanged;

        internal void RaiseQuerySubmitted(string queryText, object chosenSuggestion)
        {
            QuerySubmitted?.Invoke(this, new AutoCompleteQuerySubmittedEventArgs(queryText, chosenSuggestion));
        }

        /// <summary>
        /// Occurs when the user submits a search query.
        /// </summary>
        public event EventHandler<AutoCompleteQuerySubmittedEventArgs> QuerySubmitted;

    }
}

