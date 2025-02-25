﻿using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.IO;
using WinDurango.UI.Utils;

namespace WinDurango.UI.Dialogs
{
    public sealed partial class InstallConfirmationDialog : ContentDialog
    {
        public InstallConfirmationDialog(string manifest)
        {
            var properties = Packages.GetPropertiesFromManifest(manifest);
            string manifestFolder = manifest.Substring(0, manifest.Length - "AppxManifest.xml".Length);
            this.InitializeComponent();
            this.XamlRoot = App.MainWindow.Content.XamlRoot;
            packageName.Text = properties.DisplayName ?? "Unknown";
            packagePublisher.Text = properties.PublisherDisplayName ?? "Unknown";
            packageVersion.Text = "Unknown";
            packageLocation.Text = manifest;
            packageLogo.Source = new BitmapImage(new Uri(Path.Combine(manifestFolder, properties.Logo)));
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
