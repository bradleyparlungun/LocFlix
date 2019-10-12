# LocFlix
A UWP/WPF application for organizing and viewing your local movie collection.

To use this application you need a TMDB api key, you can get this from https://www.themoviedb.org.

If you would like to fiddle around with the same dataset as I'm using, then go to https://github.com/alxbrn/LocFlix/releases and download the movies.json data set.

## UWP Application
![UWP Application](https://i.imgur.com/KHAyjc4.jpg)

## WPF Application
![WPF Application](https://i.imgur.com/KLMSbcF.png)

## Uwp File System Permission
In UWP you can't by default access the local system file system, but there is a way around this by adding the following code to your manifest file.

```
<Package
  xmlns:rescap="http://schemas.microsoft.com/appx/manifest/foundation/windows10/restrictedcapabilities"
  IgnorableNamespaces="uap mp rescap">

<Capabilities>
    <rescap:Capability Name="broadFileSystemAccess"/>
  </Capabilities>
```

But the system is not ok with this, so the user has to go into their windows settings under privacy settings and change the following settings.

![windows privacy settings](https://i.gyazo.com/05d60dbf784edebf8c710e826b3a81bc.png)

If you still have issues with getting access to the file system, then find the specific app in "Apps and features" and make sure the specific application has access to the file system.

![app settings](https://i.gyazo.com/abe27b2ac5a9f86606d148a43e222cd7.png)

Enjoy!
