# Autodesk Reality Capture API -- .NET Sample + Toolkit
--------------------
[![language](https://img.shields.io/badge/language-C%23-blue.svg)](https://www.visualstudio.com/)
[![ReCap](https://img.shields.io/badge/Reality%20Capture%20API-v3.1%20-green.svg)](http://developer-recap-autodesk.github.io/)
![Platforms](https://img.shields.io/badge/platform-windows-lightgray.svg)
[![License](http://img.shields.io/:license-mit-blue.svg)](http://opensource.org/licenses/MIT)

<b>Note:</b> For using these samples you need a valid oAuth credential and a ReCap client ID. Visit this [page](http://developer-recap-autodesk.github.io/) for instructions to get on-board.

## Motivation

The Reality Capture API Beta provides a web service to create textured mesh from a set of photos, and can request an automatic 3D calibration. The REST API provides a similar service as the [Autodesk ReCap 360](http://www.autodesk.com/products/recap-360/overview) web application. The purpose of this sample is to show an application that can provide a Reality Capture work flow using photographic images.

Description
--------------------
This sample uses the .NET API with RestSharp library to demonstarte how to use the Reality Capture API.

Dependencies
--------------------
This sample is dependent on the following 3rd party assemblies:

1. The RestSharp assembly
	* RestSharp - Simple REST and HTTP Client for .NET you need at least version 104.3.3. 
	* You can get this component source code here: [http://restsharp.org/](http://restsharp.org/)

2. The Xceed WPF Toolkit Community Edition
	* this assembly is only used if you want to display the Reaity Capture photoscene properties in a Property window. Properties are also dumped into a text window. 
	* You can get the binaries and documentation from [https://wpftoolkit.codeplex.com/](https://wpftoolkit.codeplex.com/)


Building the sample
---------------------------

The sample was created using Visual Studio 2012 Service Pack 1, but should be compatible with other .NET Frameworks and C# compilers.

You must first create the UserSettings.cs file and provide your oAuth & Reality Capture API credentials. An example of that file is provided as UserSettings_.cs. The easiest thing to do is to make a copy of that file, and modify it with your credentials, and then add it to the project.
	 
Setup/Usage Instructions
-------------------------

* When you launch the sample, the application will try to connect to the Reality Capture API server and verifies that you are properly authorized on the Autodesk oAuth server. Once the credentials are verified, the sample will refresh your access token immediately. If not, it will ask you to be authorized. 

* The sample displays a treeview that contains existing Reality Capture API photoscenes. You can right-click on the root node to display a context menu that allows you to create a new scene. Right-click on each scene node to display different options.


--------

## License

This sample is licensed under the terms of the [MIT License](http://opensource.org/licenses/MIT). Please see the [LICENSE](LICENSE) file for full details.


## Credits

This sample code Written by Philippe Leefsma (Forge Partner Development)  
http://www.autodesk.com/adn  
