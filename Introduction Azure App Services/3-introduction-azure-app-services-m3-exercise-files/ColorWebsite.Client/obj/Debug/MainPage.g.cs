﻿

#pragma checksum "D:\Demo\ColorWebsite\ColorWebsite.Client\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "086601A2695D4C2C82577FF09F64DCE3"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace colorwebsitemobile
{
    partial class MainPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 60 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)(target)).Checked += this.CheckBoxRemove_Checked;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 51 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.ButtonRefresh_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 38 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.ButtonSave_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}

