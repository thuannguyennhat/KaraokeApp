
**FloatingSearchView** is an implementation of a floating search box with search suggestions.

## Usage
    
Add a FloatingSearchView to your view hierarchy, and make sure that it takes 
up the full width and height of the screen.

    <com.arlib.floatingsearchview.FloatingSearchView
        xmlns:app="http://schemas.android.com/apk/res-auto"
        android:id="@+id/floating_search_view"
        android:layout_width="match_parent"
        android:layout_height="match_parent"
        app:floatingSearch_searchBarMarginLeft="@dimen/search_view_inset"
        app:floatingSearch_searchBarMarginTop="@dimen/search_view_inset"
        app:floatingSearch_searchBarMarginRight="@dimen/search_view_inset"
        app:floatingSearch_showMenuAction="true"
        app:floatingSearch_searchHint="Search..."
        app:floatingSearch_voiceRecHint="Say something..."
        app:floatingSearch_showSearchHintWhenNotFocused="true"
        app:floatingSearch_showVoiceInput="true"
        app:floatingSearch_showOverFlowMenu="true"
        app:floatingSearch_hideOverflowMenuWhenFocused="true"
        app:floatingSearch_showSearchKey="false"
        app:floatingSearch_dismissOnOutsideTouch="true"
        app:floatingSearch_menu="@menu/menu_main" />

Then, listen to query changes and provide suggestion items that implement 
`ISearchSuggestion`:

    searchView.QueryChange += async (sender, e) => {
        if (!string.IsNullOrEmpty (e.OldQuery) && string.IsNullOrEmpty (e.NewQuery)) {
            searchView.ClearSuggestions ();
        } else {
            // show the top left circular progress
            searchView.ShowProgress ();
            
            // simulates a query call to a data source with a new query.
            var results = await GetDataAsync (e.NewQuery);
            
            // swap the data and collapse/expand the dropdown
            searchView.SwapSuggestions (results);
            
            // complete the progress
            searchView.HideProgress ();
        }
    };

### Configuration

### Adding Overflow Menu Items

A menu can be added to the floating search view:

    <com.arlib.floatingsearchview.FloatingSearchView
        xmlns:app="http://schemas.android.com/apk/res-auto"
        ...
        app:floatingSearch_menu="@menu/menu_main" />

Then, listen for item selections: 

    searchView.MenuItemClick += (sender, e) => {
        var item = e.MenuItem
    };

### Managing Suggestions


First, implement `ISearchSuggestion`:

    public class MySuggestion : Java.Lang.Object, ISearchSuggestion
    {
        // ISearchSuggestion interface
        
        public string GetBody () {
            // return the text of the suggestion 
        }
        public void SetBodyText (TextView textView) {
            // customize the appearace of the search TextView
        }
        public bool SetLeftIcon (ImageView imageView) {
            // set the image for the suggestion ImageView
        }
        public IParcelableCreator GetCreator () {
            return CREATOR ();
        }

        // IParcelable interface
        
        public MySuggestion (Parcel source) {
            // read the saved values from source
        }

        [ExportField ("CREATOR")]
        public static IParcelableCreator CREATOR () {
            return new MySuggestionCreator ();
        }
        public int DescribeContents () {
            return 0;
        }
        public void WriteToParcel (Parcel dest, ParcelableWriteFlags flags) {
            // write data to dest
        }

        public class MySuggestionCreator : Java.Lang.Object, IParcelableCreator
        {
            public Java.Lang.Object CreateFromParcel (Parcel source) {
                return new MySuggestion (source);
            }
            public Java.Lang.Object[] NewArray (int size) {
                return new MySuggestion[size];
            }
        }
    }
### Enable Voice Recognition

In your xml, add:

    <com.arlib.floatingsearchview.FloatingSearchView
        xmlns:app="http://schemas.android.com/apk/res-auto"
        ...
        app:floatingSearch_showVoiceInput="true" />

Then, in your activity, add `OnHostActivityResult` to the `OnActivityResult` method:

    protected override void OnActivityResult (int requestCode, Result resultCode, Android.Content.Intent data)
    {
        base.OnActivityResult (requestCode, resultCode, data);
        
        searchView.OnHostActivityResult (requestCode, resultCode, data);
    }
