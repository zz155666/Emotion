﻿#pragma checksum "C:\Users\user\Documents\Visual Studio 2015\Projects\Emotion\Emotion\MainPagePCHot.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A0A296B1FE7CFCC97BDE4F3D9D23DEF6"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Emotion
{
    partial class MainPagePCHot : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.mainSplitView = (global::Windows.UI.Xaml.Controls.SplitView)(target);
                }
                break;
            case 2:
                {
                    this.mainNavigationList = (global::Windows.UI.Xaml.Controls.ListBox)(target);
                    #line 15 "..\..\..\MainPagePCHot.xaml"
                    ((global::Windows.UI.Xaml.Controls.ListBox)this.mainNavigationList).SelectionChanged += this.mainNavigationList_SelectionChanged;
                    #line default
                }
                break;
            case 3:
                {
                    this.menuItem = (global::Windows.UI.Xaml.Controls.ListBoxItem)(target);
                    #line 16 "..\..\..\MainPagePCHot.xaml"
                    ((global::Windows.UI.Xaml.Controls.ListBoxItem)this.menuItem).Tapped += this.ListBoxItem_Tapped;
                    #line default
                }
                break;
            case 4:
                {
                    this.homeItem = (global::Windows.UI.Xaml.Controls.ListBoxItem)(target);
                    #line 23 "..\..\..\MainPagePCHot.xaml"
                    ((global::Windows.UI.Xaml.Controls.ListBoxItem)this.homeItem).Tapped += this.ListBoxItem_Tapped;
                    #line default
                }
                break;
            case 5:
                {
                    this.newsItem = (global::Windows.UI.Xaml.Controls.ListBoxItem)(target);
                    #line 35 "..\..\..\MainPagePCHot.xaml"
                    ((global::Windows.UI.Xaml.Controls.ListBoxItem)this.newsItem).Tapped += this.ListBoxItem_Tapped;
                    #line default
                }
                break;
            case 6:
                {
                    this.collectionItem = (global::Windows.UI.Xaml.Controls.ListBoxItem)(target);
                    #line 49 "..\..\..\MainPagePCHot.xaml"
                    ((global::Windows.UI.Xaml.Controls.ListBoxItem)this.collectionItem).Tapped += this.ListBoxItem_Tapped;
                    #line default
                }
                break;
            case 7:
                {
                    this.searchItem = (global::Windows.UI.Xaml.Controls.ListBoxItem)(target);
                    #line 62 "..\..\..\MainPagePCHot.xaml"
                    ((global::Windows.UI.Xaml.Controls.ListBoxItem)this.searchItem).Tapped += this.ListBoxItem_Tapped;
                    #line default
                }
                break;
            case 8:
                {
                    this.settingItem = (global::Windows.UI.Xaml.Controls.ListBoxItem)(target);
                    #line 83 "..\..\..\MainPagePCHot.xaml"
                    ((global::Windows.UI.Xaml.Controls.ListBoxItem)this.settingItem).Tapped += this.ListBoxItem_Tapped;
                    #line default
                }
                break;
            case 9:
                {
                    this.mainFrame = (global::Windows.UI.Xaml.Controls.Frame)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}
