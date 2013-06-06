using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace PlanningDairyTemplate.Data
{
    /// <summary>
    /// Base class for <see cref="SampleDataItem"/> and <see cref="SampleDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : PlanningDairyTemplate.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, SampleDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private SampleDataGroup _group;
        public SampleDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
        private string _createdon = string.Empty;
        public string CreatedOn
        {
            get { return this._createdon; }
            set { this.SetProperty(ref this._createdon, value); }
        }
        private string _createdtxt = string.Empty;
        public string CreatedTxt
        {
            get { return this._createdtxt; }
            set { this.SetProperty(ref this._createdtxt, value); }
        }

        private string _Colour = string.Empty;
        public string Colour
        {
            get { return this._Colour; }
            set { this.SetProperty(ref this._Colour, value); }
        }
        private string _bgColour = string.Empty;
        public string bgColour
        {
            get { return this._bgColour; }
            set { this.SetProperty(ref this._bgColour, value); }
        }
        private string _createdontwo = string.Empty;
        public string CreatedOnTwo
        {
            get { return this._createdontwo; }
            set { this.SetProperty(ref this._createdontwo, value); }
        }
        private string _createdtxttwo = string.Empty;
        public string CreatedTxtTwo
        {
            get { return this._createdtxttwo; }
            set { this.SetProperty(ref this._createdtxttwo, value); }
        }

        private string _currentStatus = string.Empty;
        public string CurrentStatus
        {
            get { return this._currentStatus; }
            set { this.SetProperty(ref this._currentStatus, value); }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class SampleDataGroup : SampleDataCommon
    {
        public SampleDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
        }

        private ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> Items
        {
            get { return this._items; }
        }
        
        public IEnumerable<SampleDataItem> TopItems
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed
            get { return this._items.Take(12); }
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();
        public ObservableCollection<SampleDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            
            return _sampleDataSource.AllGroups;
        }

        public static SampleDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public SampleDataSource()
        {
           // String ITEM_CONTENT = String.Format("");

            var group1 = new SampleDataGroup("Group-1",
                    "Ways & Directions",
                    "Ways & Directions",
                    "Assets/Images/10.jpg",
                    "Affiliate programs can be a big source of revenue. The key to maximizing your earnings is engaging your readers. Unlike traditional ads where you are paid for impressions or clicks, affiliates are only paid when/if a specific action is performed. The action might be a purchase or signing up for a newsletter, but regardless, you are not paid until you've compelled your readers to act.");
            group1.Items.Add(new SampleDataItem("Group-1-Item-1",
                    "Know Your Audience",
                    "The most successful way to use affiliate programs is to anticipate and meet the needs of your readers. Consider why they are coming to your site. What are you providing that they are looking for? Make sure the affiliate products you are promoting provide a solution to your audience's problems.",
                    "Assets/DarkGray.png",
					"",            
                    "Details:\n\nThe most successful way to use affiliate programs is to anticipate and meet the needs of your readers. Consider why they are coming to your site. What are you providing that they are looking for? Make sure the affiliate products you are promoting provide a solution to your audience's problems.\n\nIf you are writing about sports, don't put up affiliate ads for printer toner just because everyone has a printer and those programs have a high payout. The people who are coming to read commentary or get stats for their favorite teams aren't thinking about those things when they're on your site.",
                    group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Know Your Audience", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/11.jpg")), CurrentStatus = "Affiliate Marketing" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-2",
                     "Be Trustworthy",
                     "Readers are savvy. They know an affiliate link when they see one. If you break their trust by promoting a product you don't believe in or take advantage of their visit with too many ads, they will leave and never come back.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nReaders are savvy. They know an affiliate link when they see one. If you break their trust by promoting a product you don't believe in or take advantage of their visit with too many ads, they will leave and never come back. It is your repeat visitors that will drive traffic. They are the ones who will give you linkbacks, spread the word, and recommend your site as the go-to place for valuable content. You need to build a relationship based on genuine content.If your visitors don't think you're being honest, they won't read anything else you have to say.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Be Trustworthy", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/12.jpg")), CurrentStatus = "Affiliate Marketing" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-3",
                     "Select Carefully",
                     "Take the time to go through all the different options for products or services available through the programs. Put some thought into which products or services your readers may need or like.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nTake the time to go through all the different options for products or services available through the programs. Put some thought into which products or services your readers may need or like. Also, change the ads around often, try different ones, and use different graphics and text to see which are the most effective. It may take some time before you figure out the best formula, and you may also find that you need to continually rotate ads to attract more attention.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Select Carefully", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/13.jpg")), CurrentStatus = "Affiliate Marketing" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-4",
                     "Be Helpful",
                     "Think of affiliate ads as additional resources that complement your content. Give value to your content by making it helpful, useful, and informative.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nThink of affiliate ads as additional resources that complement your content. Give value to your content by making it helpful, useful, and informative. Don't put up a list of your favorite books, hoping people will click on the affiliate link, purchase the books (just because you listed them), so you can cash in on a sale. Take some time to write a detailed review, and use affiliate ads to point them in the right direction if they decide to act on your information. That's what affiliate ads are for. If you write a great review recommending a book and readers buy the book because of it, you should get something for that. But just throwing out links to products with no rhyme or reason will result in a quick exit by visitors.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Be Helpful", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/14.jpg")), CurrentStatus = "Affiliate Marketing" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-5",
                     "Be Transparent",
                     "Always disclose your affiliations. Your readers will appreciate your honesty, and will feel better about contributing to your earnings. If they sense that you are being less than honest about your affiliations, they are savvy enough to bypass your link and go directly to the vendor just to avoid giving you referral credit.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nAlways disclose your affiliations. Your readers will appreciate your honesty, and will feel better about contributing to your earnings. If they sense that you are being less than honest about your affiliations, they are savvy enough to bypass your link and go directly to the vendor just to avoid giving you referral credit.\n\nHonesty and full disclosure is a necessary part to building a loyal reader base. They know they are supporting you by using your referral links. Make them happy and eager to do so.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Be Transparent", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/15.jpg")), CurrentStatus = "Affiliate Marketing" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-6",
                     "Try Different Programs",
                     "If one particular program doesn't seem to be working for you, try another one. Affiliate programs don't look the same. They offer different products, services, and payment structures.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nIf one particular program doesn't seem to be working for you, try another one. Affiliate programs don't look the same. They offer different products, services, and payment structures. Some programs will have a lifetime payout on sales while others will limit it to 30-90 days. Some programs allow much more flexibility in the types of ad units available, as well as colors and design so it fits better on your site's layout.\n\nAlso, check your favorite vendors to see if they run their own affiliate program. Sometimes you can go directly to the source. You're not limited to big affiliate networks. Integrate systematic ad testing into your strategy to maximize your profits.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Try Different Programs", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/16.jpg")), CurrentStatus = "Affiliate Marketing" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-7",
                     "Write Timeless Content",
                     "Your old content can still be valuable even though it's no longer on your front page. Take advantage of the long term opportunities by making sure you provide timeless content.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nYour old content can still be valuable even though it's no longer on your front page. Take advantage of the long term opportunities by making sure you provide timeless content. If visitors come across your older content first, and find that it offers dated information, they will leave right away. Of course, information moves forward, so relevant content changes quickly. You can make your content timeless simply by adding links to your updated articles on your old ones. Many platforms allow you to show most recent or most popular or related articles on every page, so no matter how old the article is, it will always show access to your new ones. Your old content can make money for you indefinitely.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Write Timeless Content", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/17.jpg")), CurrentStatus = "Affiliate Marketing" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-8",
                     "Be Patient",
                     "Affiliate revenue grows and builds up with time. Remember that some programs offer lifetime payouts. If you refer a visitor, you may continue to make money from that one visitor even if he doesn't come back to your site.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nAffiliate revenue grows and builds up with time. Remember that some programs offer lifetime payouts. If you refer a visitor, you may continue to make money from that one visitor even if he doesn't come back to your site. Also, as long as you have referral links still active in your old posts, they may still payout for you. Affiliate programs aren't a get rich quick plan, but it provides opportunity to make passive income in the future.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Be Patient", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/18.jpg")), CurrentStatus = "Affiliate Marketing" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-9",
                     "Stay Relevant",
                     "Keep up to date on the latest offerings of your affiliate programs. New ad units, advertisers, and tools are constantly being added to improve usability and be more visually appealing.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nKeep up to date on the latest offerings of your affiliate programs. New ad units, advertisers, and tools are constantly being added to improve usability and be more visually appealing. Small changes go a long way in motivating action by readers. You may be left out in the dust by being complacent with your strategy. Don't get lazy about monitoring trends and exploring new opportunities.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Stay Relevant", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/19.jpg")), CurrentStatus = "Affiliate Marketing" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-10",
                     "Content Comes First",
                     "Above all else, your content must be your highest priority. Your content is your foundation, the life blood on which the site exists. Without valuable and helpful content, readers won't come.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nAbove all else, your content must be your highest priority. Your content is your foundation, the life blood on which the site exists. Without valuable and helpful content, readers won't come. Focus on providing excellent content, and the monetizing strategies will work out. Once you start compromising your content to cater to the affiliate programs or any other money making venture, you will lose your readers. Once that happens, you will lose the opportunity to receive any earnings from any of your ads, be they CPM, CPC, or referral based.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Content Comes First", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/20.jpg")), CurrentStatus = "Affiliate Marketing" });
					 
            this.AllGroups.Add(group1);


			
			
         
        }
    }
}
